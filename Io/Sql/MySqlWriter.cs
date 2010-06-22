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
    public class MySqlWriter : Writer
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
        public MySqlWriter(IDbConnection connection, string database,
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
        public MySqlWriter(IDbConnection connection, string database)
        : base(connection, database) {}
                
        /// <summary>
        /// Create a new writer connected to a local database.
        /// </summary>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        public MySqlWriter (string database, string user)
        : this ("localhost", database, user) {}
           
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
        public MySqlWriter (string host, string database, string user)
        : this (host, database, user, user) {}
               
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
        public MySqlWriter (string host, string database, string user,
            string password) : this (host, database, user, password, 3306) {}  
                
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
        public MySqlWriter (string host, string database, string user,
            string password, int port)
            : this(host, database, user, password, port, false) {}
        
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
        public MySqlWriter (string host, string database, string user,
            string password, int port, bool pooling)
            : this(new MySqlConnection (
                    "Server=" + host + "; User ID=" + user +
                    "; Database=" + database + "; Password=" + password +
                    "; Pooling=" + pooling + ";"), database) {}
        
        override protected internal AbstractReader CreateReader()
        {
            return new MySqlReader(Connection, Command, Database);
        }
        
        override protected internal void Open ()
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
            Writer = CreateWriter();
            Reader = new MySqlReader (TEST_DATABASE, TEST_ACCOUNT);
        }  
        
        override protected internal AbstractReader CreateExampleReader()
        {
            return new MySqlReader(EXAMPLE_DATABASE, TEST_ACCOUNT);
        }
        
        override protected internal AbstractWriter CreateWriter()
        {
            return new MySqlWriter(TEST_DATABASE, TEST_ACCOUNT);
        }
        
        override protected internal void SetUpPartial ()
        {
            IDbCommand command;
            Writer.Dispose();
            Writer.Connection.Open();
            command = Writer.Connection.CreateCommand();
            Writer = new MySqlWriter (Writer.Connection, Writer.Database, command);
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
            for(int i = Sql.Writer.TABLES.Length - 1; i >= 0; i--)
            {
                Writer.Command.CommandText = "DELETE FROM " + Sql.Writer.TABLES[i];
                Writer.Command.ExecuteNonQuery();
            }
            base.TearDown();
        }
    }
}
