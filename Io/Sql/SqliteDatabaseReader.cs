using Mono.Data.Sqlite;
using NUnit.Framework;
using System;
using System.Data;

namespace XCAnalyze.Io.Sql
{
    public class SqliteDatabaseReader : DatabaseReader
    {
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        protected internal SqliteDatabaseReader(IDbConnection connection)
            : base(connection) {}
    
        /// <summary>
        /// Create a new reader using an in-memory database.
        /// </summary>
        public static SqliteDatabaseReader NewInstance ()
        {
            return NewInstance (":memory:");
        }
        
        /// <summary>
        /// Create a new reader that reads from a file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file from which to read.
        /// </param>
        public static SqliteDatabaseReader NewInstance (string fileName)
        {
            return NewInstance (
                new SqliteConnection ("Data Source=" + fileName));
        }
        
        /// <summary>
        /// Create a new reader using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        new public static SqliteDatabaseReader NewInstance (IDbConnection connection)
        {
            SqliteDatabaseReader reader = new SqliteDatabaseReader (connection);
            reader.Connection.Open ();
            reader.Command = reader.Connection.CreateCommand ();
            return reader;
        }
    }
    
    [TestFixture]
    public class TestSqliteDatabaseReader
    {
        protected internal SqliteDatabaseReader Reader { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Reader = SqliteDatabaseReader.NewInstance (SupportFiles.GetPath ("example.db"));
        }
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Close ();
        }
        
        [Test]
        public void TestRead ()
        {
            Reader.Read ();
        }
    }
}
