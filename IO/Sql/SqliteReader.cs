using System;
using System.Data;

using Mono.Data.Sqlite;

namespace XCAnalyze.IO.Sql
{
    public partial class SqliteReader : Reader
    {
        #region Constructors
        
        /// <summary>
        /// Create a new <see cref="SqliteConnection" to the given file.
        /// </summary>
        protected static IDbConnection CreateConnection(string fileName)
        {
            IDbConnection connection = new SqliteConnection("Data Source=" + fileName);
            connection.Open();
            return connection;
        }
        
        /// <summary>
        /// Create a new reader using an in-memory database.
        /// </summary>
        public SqliteReader() : this(":memory:")
        {
        }

        /// <summary>
        /// Create a new reader that reads from a file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file from which to read.
        /// </param>
        public SqliteReader(string fileName) : this(CreateConnection(fileName), fileName)
        {
        }

        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public SqliteReader(IDbConnection connection, string database) : base(connection, database)
        {
        }
        
        #endregion
    }
}
