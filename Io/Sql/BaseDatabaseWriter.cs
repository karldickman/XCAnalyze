using System;
using System.Collections.Generic;
using System.Data;
using XCAnalyze.Collections;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// The interface that all database writers must conform to.
    /// </summary>
    abstract public class BaseDatabaseWriter
        : IWriter<Model.XcData>, IDisposable
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
        protected internal BaseDatabaseReader Reader { get; set; }
        
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
        public BaseDatabaseWriter (IDbConnection connection, string database)
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
        protected internal BaseDatabaseWriter (IDbConnection connection,
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
        abstract protected internal BaseDatabaseReader CreateReader();
        
        public void Dispose ()
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
        
        /// <summary>
        /// Write data to the database.
        /// </summary>
        /// <param name="data">
        /// The <see cref="Data"/> to be written.
        /// </param>
        public void Write (Model.XcData data)
        {
            WriteConferences (data.Conferences);
            WriteRunners (data.Runners);
            WriteSchools (data.Schools, data.Conferences);
            WriteAffiliations (data.Affiliations, data.Runners, data.Schools);
            WriteMeetNames (data.MeetNames);
            WriteRaces (data.Races);
            WriteVenues (data.Venues);
            WriteMeets (data.Meets, data.MeetNames, data.Races, data.Venues);
            WritePerformances (data.Performances, data.Races, data.Runners);
        }

        abstract public void WriteAffiliations(IList<Affiliation> affiliations,
            IList<Runner> runnerIds, IList<School> schoolIds);                 
                
        abstract public void WriteConferences(IList<string> conferences);
                
        abstract public void WriteMeetNames(IList<string> meetNames);
        
        abstract public void WriteMeets(IList<Meet> meets,
            IList<string> meetNames, IList<Race> races, IList<Venue> venues);
        
        abstract public void WritePerformances(IList<Performance> performances,
            IList<Race> races, IList<Runner> runners);
        
        abstract public void WriteRaces(IList<Race> races);
                
        abstract public void WriteRunners(IList<Runner> runners);
                
        abstract public void WriteSchools(IList<School> schoolIds,
            IList<string> conferences);
                
        abstract public void WriteVenues(IList<Venue> venues);
    }
}
