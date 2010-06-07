using Mono.Data.Sqlite;
using NUnit.Framework;
using System;
using System.Data;
using System.IO;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// A <see cref="IWriter"/> used to write the model to an SQLite database.
    /// </summary>
    public class SqliteDatabaseWriter : DatabaseWriter
    {
        override public string CREATION_SCRIPT_EXTENSION
        {
            get
            {
                return "sqlite";
            }
        }
        
        override public string GET_TABLES_COLUMN
        {
             get { return "name"; }
        }

        override public string GET_TABLES_COMMAND
        {
            get { return "SELECT name FROM sqlite_master WHERE type=\"table\""; }
        }

        /// <summary>
        /// Create a new writer using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        protected internal SqliteDatabaseWriter(IDbConnection connection)
            : base(connection)
        {
            DatabaseReader = SqliteDatabaseReader.NewInstance(
                new SqliteConnection(connection.ConnectionString));
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using an in-memory database.
        /// </summary>
        public static SqliteDatabaseWriter NewInstance ()
        {
            return NewInstance (":memory:");
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using a specific database file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to connect to.
        /// </param>
        public static SqliteDatabaseWriter NewInstance (string fileName)
        {
            return NewInstance (
                new SqliteConnection ("Data Source=" + fileName));
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using the given connection.
        /// </summary>
        public static SqliteDatabaseWriter NewInstance (IDbConnection connection)
        {
            SqliteDatabaseWriter writer = new SqliteDatabaseWriter (connection);
            writer.Connection.Open ();
            writer.Command = writer.Connection.CreateCommand ();
            if (!writer.IsDatabaseInitialized ())
            {
                writer.InitializeDatabase ();
            }
            return writer;
        }
    }
    
    [TestFixture]
    public class TestSqliteDatabaseWriter : TestDatabaseWriter
    {      
        /// <summary>
        /// The name of the test database file.
        /// </summary>
        override public string TEST_DATABASE { get { return "xca_test.db"; } }

        [SetUp]
        override public void SetUp ()
        {
            base.SetUp();
            Writer = SqliteDatabaseWriter.NewInstance (TEST_DATABASE);
            Reader = DatabaseReader.NewInstance (new SqliteConnection (
                    Writer.Connection.ConnectionString));
        }
        
        override public void SetUpWriters ()
        {
            TearDown ();
            Writer = new SqliteDatabaseWriter (new SqliteConnection (
                    "Data Source=" + TEST_DATABASE));
            Writer.Connection.Open ();
            Writer.Command = Writer.Connection.CreateCommand ();
        }

        [TearDown]
        override public void TearDown ()
        {
            base.TearDown();
            File.Delete (TEST_DATABASE);
        }
    }
}
