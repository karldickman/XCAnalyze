using System;
using System.Collections.Generic;
using System.Data;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// The interface that all database writers must conform to.
    /// </summary>
    abstract public class AbstractWriter : AbstractXcDataWriter
    {
        /// <summary>
        /// The tables that should be in the database.  These are in dependency
        /// order: tables later in the list have foreign keys referencing tables
        /// earlier in the list.
        /// </summary>
        public static readonly string[] TABLES = {"runners", "conferences",
            "schools", "affiliations", "meet_names", "races", "venues", "meets",
            "results"};      
        
        /// <summary>
        /// The views that should be in the database.
        /// </summary>
        public static readonly string[] VIEWS = {"performances"};
        
        /// <summary>
        /// The <see cref="IDbCommand"/> used to query the database.
        /// </summary>
        protected internal IDbCommand Command { get; set; }
    
        /// <summary>
        /// The <see cref="IDbConnection"/> to the database.
        /// </summary>
        protected internal IDbConnection Connection { get; set; }
    
        /// <summary>
        /// The name of the database to which this reader is connected.
        /// </summary>
        protected internal string Database { get; set; }
        
        /// <summary>
        /// The <see cref="DatabaseReader" /> used to read things back out of
        /// the database.
        /// </summary>
        protected internal AbstractReader Reader { get; set; }
        
        /// <summary>
        /// The <see cref="IDataReader"/> used to read responses from the
        /// database.
        /// </summary>
        protected internal IDataReader ResultsReader { get; set; }
        
        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to which this writer writes.
        /// </param>
        public AbstractWriter (IDbConnection connection, string database)
        {
            Connection = connection;
            Database = database;
            Open ();
            Reader = CreateReader ();
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
        /// <param name="command">
        /// The <see cref="IDbCommand"/> to use.
        /// </param>
        protected internal AbstractWriter (IDbConnection connection,
            string database, IDbCommand command)
        {
            Connection = connection;
            Database = database;
            Command = command;
        } 
        
        /// <summary>
        /// Get the script to create the database.
        /// </summary>
        abstract public IList<string> CreationScript ();
        
        /// <summary>
        /// Get a database reader to use.
        /// </summary>
        /// <returns>
        /// A <see cref="BaseDatabaseReader"/> connected to the current
        /// database.
        /// </returns>
        abstract protected internal AbstractReader CreateReader();
        
        override public void Dispose ()
        {
            if (ResultsReader != null)
            {
                ResultsReader.Dispose ();
            }
            if (Command != null)
            {
                Command.Dispose ();
            }
            if (Reader != null)
            {
                Reader.Dispose ();
            }
            Connection.Close ();
            Connection.Dispose ();
        }
        
        /// <summary>
        /// Initialized the database for writing.
        /// </summary>
        abstract protected internal void InitializeDatabase();
        
        /// <summary>
        /// Is the database ready to be written to?
        /// </summary>
        abstract protected internal bool IsDatabaseInitialized();
        
        /// <summary>
        /// Do all the steps needed to open the connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to open.
        /// </param>
        abstract protected internal void Open();
    }
}
