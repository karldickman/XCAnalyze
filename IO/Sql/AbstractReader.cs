using System;
using System.Data;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// The interface which all database readers must implement.
    /// </summary>
    abstract public partial class AbstractReader
    : AbstractXcDataReader, IDisposable
    {
        #region Properties
        
        /// <summary>
        /// The command used to query the database.
        /// </summary>
        protected IDbCommand Command { get; set; }
        
        /// <summary>
        /// The connection to the database.
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// The name of the database from which this reader reads.
        /// </summary>
        protected string Database { get; set; }
        
        /// <summary>
        /// The reader for the resultset.
        /// </summary>
        protected IDataReader Reader { get; set; }
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the connection is not open.
        /// </exception>
        public AbstractReader (IDbConnection connection, string database)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new ArgumentException ("Argument connection must be open.");
            }
            Connection = connection;
            Command = Connection.CreateCommand ();
            Database = database;
        }
        
        #endregion
        
        #region IDisposable implementation
        
        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        void IDisposable.Dispose ()
        {
            Dispose (true);
        }
        
        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        /// <param name="disposing">
        /// True if called from the Dispose() method, false if called from the
        /// deconstructor.
        /// </param>
        protected void Dispose (bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize (this);
            }
            if (Reader != null)
            {
                Reader.Dispose ();
            }
            Command.Dispose ();
            Connection.Dispose ();
        }
        
        ~AbstractReader ()
        {
            Dispose (false);
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Close the connection to the database.
        /// </summary>
        public void Close ()
        {
            ((IDisposable)this).Dispose ();
        }
        
        #endregion
    }
}