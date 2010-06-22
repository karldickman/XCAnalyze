using Mono.Data.Sqlite;
using NUnit.Framework;
using System;
using System.Data;

namespace XCAnalyze.Io.Sql
{
    public class SqliteReader : Reader
    {
        /// <summary>
        /// Create a new reader using an in-memory database.
        /// </summary>
        public SqliteReader () : this(":memory:") {}

        /// <summary>
        /// Create a new reader that reads from a file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file from which to read.
        /// </param>
        public SqliteReader (string fileName)
            : this(new SqliteConnection ("Data Source=" + fileName), fileName) {}
        
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public SqliteReader (IDbConnection connection, string database)
            : base(connection, database) {}
    }
    
    [TestFixture]
    public class TestSqliteDatabaseReader : TestDatabaseReader
    {        
        [SetUp]
        override public void SetUp ()
        {
            Reader = new SqliteReader(SupportFiles.GetPath (EXAMPLE_DATABASE + ".db"));
        }
    }
}
