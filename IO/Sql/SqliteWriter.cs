using System;
using System.Collections.Generic;
using System.Data;

using Mono.Data.Sqlite;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// A <see cref="IWriter"/> used to write the model to an SQLite database.
    /// </summary>
    public partial class SqliteWriter : Writer
    {
        #region Constructors

        /// <summary>
        /// Create an open <see cref="SqliteConnection" /> to the specified file.
        /// </summary>
        protected static IDbConnection CreateConnection(string fileName)
        {
            IDbConnection connection = new SqliteConnection("Data Source=" + fileName);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using an in-memory database.
        /// </summary>
        public SqliteWriter() : this(":memory:")
        {
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using a specific database file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to connect to.
        /// </param>
        public SqliteWriter(string fileName) : this(CreateConnection(fileName), fileName)
        {
        }

        /// <summary>
        /// Create a new writer using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        public SqliteWriter(IDbConnection connection, string database) : base(connection, database)
        {
        }

        /// <summary>
        /// Create a new writer using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="initializeDatabase">
        /// Should the database be initialized.
        /// </param>
        protected SqliteWriter(IDbConnection connection, string database, bool initializeDatabase) : base(connection, database, initializeDatabase)
        {
        }
        
        #endregion
        
        #region Writer implementation
        
        protected override string CreationScriptExtension {
            get { return "sqlite"; }
        }

        protected override string GetTablesColumn {
            get { return "name"; }
        }

        protected override string GetTablesCommand {
            get { return "SELECT name FROM sqlite_master WHERE type=\"table\""; }
        }
        
        #endregion
    }
}
