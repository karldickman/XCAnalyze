using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// A <see cref="IWriter"/> to write the model to a MySQL database.
    /// </summary>
    public class MySqlDatabaseWriter : DatabaseWriter
    {
        override public string CREATION_SCRIPT_EXTENSION
        {
            get { return "mysql"; }
        }
        
        override public string GET_TABLES_COLUMN
        {
            get { return "Tables_in_" + Database; }
        }
        
        override public string GET_TABLES_COMMAND
        {
            get { return "SHOW TABLES"; }
        }
        
        /// <summary>
        /// Create a new database writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="command">
        /// The <see cref="IDbCommand"/> to use.
        /// </param>
        public MySqlDatabaseWriter(IDbConnection connection, string database,
            IDbCommand command) : base(connection, database, command) {}
        
        /// <summary>
        /// Create a new database writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database.
        /// </param>
        protected internal MySqlDatabaseWriter(IDbConnection connection,
            string database) : this(connection, database, true) {}
        
        /// <summary>
        /// Create a new database writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database.
        /// </param>
        /// <param name="open">
        /// Should the connection be opened?
        /// </param>
        protected internal MySqlDatabaseWriter(IDbConnection connection,
            string database, bool open)
            : base(connection, database, open)
        {
            DatabaseReader = new MySqlDatabaseReader(connection, Command);
        }
                
        /// <summary>
        /// Create a new writer connected to a local database.
        /// </summary>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        public static MySqlDatabaseWriter NewInstance (string database,
            string user)
        {
            return NewInstance ("localhost", database, user);
        }
           
        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user)
        {
            return NewInstance (host, database, user, user);
        }
               
        /// <summary>
        /// Create a new writer connected to a password-protected database.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password)
        {
            return NewInstance (host, database, user, password, 3306);
        }        
                
        /// <summary>
        /// Create a new writer connected to a password-protected database on a
        /// server listening at a particular port.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="port">
        /// The port number on which the server is listening.
        /// </param>
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password, int port)
        {
            return NewInstance (host, database, user, password, port, false);
        }
        
        /// <summary>
        /// Create a new writer connected to a password-protected database on a
        /// server listening at a particular port.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="port">
        /// The port number on which the server is listening.
        /// </param>
        /// <param name="pooling">
        /// Should pooling be turned on or off.
        /// </param>
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password, int port,
            bool pooling)
        {
            string connectionString = "Server=" + host + "; User ID=" + user +
                "; Database=" + database + "; Password=" + password +
                    "; Pooling=" + pooling + ";";
            return NewInstance (new MySqlConnection (connectionString), database);
        }
        
        /// <summary>
        /// Create a new writer using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database.
        /// </param>
        public static MySqlDatabaseWriter NewInstance (IDbConnection connection,
            string database)
        {
            return new MySqlDatabaseWriter(connection, database);
        }
        
        new public IList<string> CreationScript ()
        {
            IList<string> commands;
            MySqlScriptReader reader;
            reader = MySqlScriptReader.NewInstance(CREATION_SCRIPT);
            commands = reader.Read();
            reader.Close();
            return commands;
        }
        
        override public void InitializeDatabase()
        {
            IList<string> creationCommands = CreationScript();
            foreach(string command in creationCommands)
            {
                Command.CommandText = command;
                Command.ExecuteNonQuery();
            }
        }
        
        override public void Open ()
        {
            Connection.Open();
            Command = Connection.CreateCommand();
            Command.CommandText = "DROP DATABASE " + Database;
            try
            {
                Command.ExecuteNonQuery();
            }
            catch(MySqlException) {}
            Command.CommandText = "CREATE DATABASE " + Database;
            Command.ExecuteNonQuery();
            Command.CommandText = "USE " + Database;
            Command.ExecuteNonQuery();
            InitializeDatabase();
        }
    }
  
    [TestFixture]
    public class TestMySqlDatabaseWriter : TestDatabaseWriter
    {
        [SetUp]
        override public void SetUp ()
        {
            base.SetUp();
            Writer = MySqlDatabaseWriter.NewInstance (TEST_DATABASE, "xcanalyze");
            Reader = MySqlDatabaseReader.NewInstance (TEST_DATABASE, "xcanalyze");
        }  
        
        override public void SetUpPartial ()
        {
            IDbCommand command;
            Writer.Close();
            Writer.Connection.Open();
            command = Writer.Connection.CreateCommand();
            Writer = new MySqlDatabaseWriter (Writer.Connection, Writer.Database, command);
            command.CommandText = "DROP DATABASE " + TEST_DATABASE;
            command.ExecuteNonQuery();
            command.CommandText = "CREATE DATABASE " + TEST_DATABASE;
            command.ExecuteNonQuery();
            command.CommandText = "USE " + TEST_DATABASE;
            command.ExecuteNonQuery();
        }

        [TearDown]
        override public void TearDown()
        {
            for(int i = DatabaseWriter.TABLES.Length - 1; i >= 0; i--)
            {
                Writer.Command.CommandText = "DELETE FROM " + DatabaseWriter.TABLES[i];
                Writer.Command.ExecuteNonQuery();
            }
            base.TearDown();
        }
    }
}
