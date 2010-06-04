using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;

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
        /// The name of the database to which we are connected.
        /// </summary>
        protected internal string Database { get; set; }
        
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
            string database) : base(connection)
        {
            Database = database;
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
                "; Password=" + password + "; Pooling=" + pooling + ";";
            return NewInstance (new MySqlConnection(connectionString), database);
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
            string fullConnectionString = connection.ConnectionString +
                "Database=" + database;
            MySqlDatabaseWriter writer = new MySqlDatabaseWriter (connection, database);
            connection.Open ();
            writer.Command = connection.CreateCommand ();
            writer.Command.CommandText = "USE " + database;
            try
            {
                writer.Command.ExecuteNonQuery ();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine (exception.Message);
                Console.WriteLine ("Creating database " + database);
                writer.Command.CommandText = "CREATE DATABASE " + database;
                writer.Command.ExecuteNonQuery ();
            }
            writer.Close ();
            writer = new MySqlDatabaseWriter (
                new MySqlConnection (fullConnectionString), database);
            writer.Connection.Open ();
            writer.Command = writer.Connection.CreateCommand ();
            if (!writer.IsDatabaseInitialized ()) 
            {
                writer.InitializeDatabase ();
            }
            return writer;
        }
        
        /// <summary>
        /// Create all the required tables in the databae.
        /// </summary>
        override public void InitializeDatabase()
        {
            throw new NotImplementedException();
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
        
        override public void SetUpWriters()
        {
            throw new NotImplementedException();
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
        
        [Test]
        override public void TestIsDatabaseInitialized ()
        {
            Assert.That (Writer.IsDatabaseInitialized ());
        }
    }
}
