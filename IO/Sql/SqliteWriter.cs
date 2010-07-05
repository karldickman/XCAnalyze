using System;
using System.Collections.Generic;
using System.Data;

using Mono.Data.Sqlite;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// A <see cref="IWriter"/> used to write the model to an SQLite database.
    /// </summary>
    public class SqliteWriter : Writer
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
        /// Create a new SqliteDatabaseWriter using an in-memory database.
        /// </summary>
        public SqliteWriter () : this(":memory:") {}

        /// <summary>
        /// Create a new SqliteDatabaseWriter using a specific database file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to connect to.
        /// </param>
        public SqliteWriter (string fileName) 
            : this(new SqliteConnection ("Data Source=" + fileName), fileName) {}

        /// <summary>
        /// Create a new writer using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        public SqliteWriter (IDbConnection connection,
            string database) : base(connection, database) {}
            
        /// <summary>
        /// Create a new writer using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="oepn">
        /// Should the database be opened.
        /// </param>
        protected internal SqliteWriter(IDbConnection connection,
            string database, IDbCommand command)
        : base(connection, database, command) {}
        
        override protected internal AbstractReader CreateReader()
        {
            return new Reader(Connection, Command, Database);
        }
        
        override protected internal void Open()
        {
            Connection.Open();
            Command = Connection.CreateCommand();
            Command.CommandText = GET_TABLES_COMMAND;
            IDataReader reader = Command.ExecuteReader();
            IList<string> tables = new List<string>();
            while(reader.Read())
            {
                tables.Add(reader[GET_TABLES_COLUMN].ToString());
            }
            reader.Close();
            foreach(string table in tables)
            {
                Command.CommandText = "DROP TABLE " + table;
                Command.ExecuteNonQuery();
            }
            InitializeDatabase();
        }
    }
}