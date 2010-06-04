using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;

namespace XCAnalyze.Io.Sql
{
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
        
        protected internal string Database { get; set; }
        
        protected internal MySqlDatabaseWriter(IDbConnection connection,
            string database) : base(connection)
        {
            Database = database;
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user)
        {
            return NewInstance (host, database, user, user);
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password)
        {
            return NewInstance (host, database, user, password, 3306);
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password, int port)
        {
            return NewInstance (host, database, user, password, port, false);
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password, int port,
            bool pooling)
        {
            string connectionString = "Server=" + host + "; User ID=" + user + "; Password=" + password + "; Pooling=" + pooling + ";";
            return NewInstance (new MySqlConnection(connectionString), database);
        }
        
        public static MySqlDatabaseWriter NewInstance (IDbConnection connection,
            string database)
        {
            string fullConnectionString = connection.ConnectionString + "Database=" + database;
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
            writer = new MySqlDatabaseWriter (new MySqlConnection (fullConnectionString), database);
            writer.Connection.Open ();
            writer.Command = writer.Connection.CreateCommand ();
            if (!writer.IsDatabaseInitialized ()) 
            {
                writer.InitializeDatabase ();
            }
            return writer;
        }
        
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
            Writer = MySqlDatabaseWriter.NewInstance ("localhost",
                TEST_DATABASE, "xcanalyze");
            Reader = MySqlDatabaseReader.NewInstance ("localhost",
                TEST_DATABASE, "xcanalyze");
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
        
        [Test]
        override public void TestInitializeDatabase ()
        {
            Writer.InitializeDatabase ();
        }
        
        [Test]
        override public void TestWrite ()
        {
            base.TestWrite ();
        }
        
        [Test]
        override public void TestWriteConferences ()
        {
            base.TestWriteConferences ();
        }
        
        [Test]
        override public void TestWriteRunners()
        {
            base.TestWriteRunners();
        }
        
        [Test]
        override public void TestWriteSchools()
        {
            base.TestWriteSchools();
        }
    }
}
