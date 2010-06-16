using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// The interface which all database readers must implement.
    /// </summary>
    abstract public class AbstractDatabaseReader : AbstractReader
    {
        /// <summary>
        /// Has this instance been disposed of yet?
        /// </summary>
        private bool disposed;
        
        /// <summary>
        /// The command used to query the database.
        /// </summary>
        protected internal IDbCommand Command { get; set; }
        
        /// <summary>
        /// The connection to the database.
        /// </summary>
        protected internal IDbConnection Connection { get; set; }

        /// <summary>
        /// The name of the database from which this reader reads.
        /// </summary>
        protected internal string Database { get; set; }
        
        /// <summary>
        /// The reader for the resultset.
        /// </summary>
        protected internal IDataReader Reader { get; set; }
        
        protected internal AbstractDatabaseReader()
        {
            disposed = false;
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
        public AbstractDatabaseReader (IDbConnection connection, string database)
        : this()
        {
            Connection = connection;
            Connection.Open ();
            Command = Connection.CreateCommand ();
            Database = database;
        }
        
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="command">
        /// The <see cref="IDbCommand"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public AbstractDatabaseReader (IDbConnection connection, IDbCommand command,
            string database)
        : this()
        {
            Connection = connection;
            Command = command;
            Database = database;
        }
        
        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        override public void Dispose ()
        {
            if (!disposed)
            {
                if (Reader != null)
                {
                    Reader.Dispose ();
                }
                Command.Dispose ();
                Connection.Dispose ();
                GC.SuppressFinalize (this);
                disposed = true;
            }
        }
    }
}