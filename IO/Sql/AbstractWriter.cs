using System;
using System.Collections.Generic;
using System.Data;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// The interface that all database writers must conform to.
    /// </summary>
    public abstract partial class AbstractWriter : AbstractXcDataWriter, IDisposable
    {
        #region Properties

        /// <summary>
        /// The <see cref="IDbCommand"/> used to query the database.
        /// </summary>
        protected IDbCommand Command { get; set; }

        /// <summary>
        /// The <see cref="IDbConnection"/> to the database.
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// The name of the database to which this reader is connected.
        /// </summary>
        protected string Database { get; set; }

        /// <summary>
        /// Has this instance been disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// The <see cref="IDataReader"/> used to read responses from the
        /// database.
        /// </summary>
        protected IDataReader Reader { get; set; }

        /// <summary>
        /// The tables that should be in the database.  These are in dependency
        /// order: tables later in the list have foreign keys referencing
        /// tables earlier in the list.
        /// </summary>
        public static readonly IList<string> Tables = new List<string> {
            "runners",
            "runner_nicknames",
            "college_enrollment_years",
            "results_unknown_race",
            "teams",
            "unaffiliated_teams",
            "team_nicknames",
            "conferences",
            "conference_affiliations",
            "states",
            "cities",
            "venues",
            "meets",
            "meet_hosts",
            "meet_instances",
            "meet_instance_hosts",
            "races",
            "results",
            "did_not_finish",
            "affiliations"
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to which this writer writes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the database connection is not open.
        /// </exception>
        public AbstractWriter(IDbConnection connection, string database) : this(connection, database, true)
        {
        }
        
        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to which this writer writes.
        /// </param>
        /// <param name="initializeDatabase">
        /// Should the database be initialized.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the database connection is not open.
        /// </exception>
        protected AbstractWriter(IDbConnection connection, string database, bool initializeDatabase)
        {
            if(connection.State != ConnectionState.Open) {
                throw new ArgumentException("Connection must be open.");
            }
            Connection = connection;
            Command = Connection.CreateCommand();
            Database = database;
            if(initializeDatabase && !IsDatabaseInitialized()) {
                InitializeDatabase();
            }
        }

        #endregion

        #region IDisposable implementation

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(Reader != null) {
                        Reader.Dispose();
                    }
                    if(Command != null) {
                        Command.Dispose();
                    }
                    Connection.Dispose();
                }
                IsDisposed = true;
            }
        }

        ~AbstractWriter()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Close this writer.
        /// </summary>
        public void Close()
        {
            ((IDisposable)this).Dispose();
        }

        /// <summary>
        /// Get the script to create the database.
        /// </summary>
        protected abstract IList<string> CreationScript();

        /// <summary>
        /// Initialized the database for writing.
        /// </summary>
        protected abstract void InitializeDatabase();

        /// <summary>
        /// Is the database ready to be written to?
        /// </summary>
        protected abstract bool IsDatabaseInitialized();
        
        #endregion
    }
}
