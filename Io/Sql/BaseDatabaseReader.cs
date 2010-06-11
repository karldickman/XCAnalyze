using System;
using System.Data;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// The interface which all database readers must implement.
    /// </summary>
    abstract public class BaseDatabaseReader : IReader<Model.XcData>
    {        
        /// <summary>
        /// The reader for the resultset.
        /// </summary>
        protected internal IDataReader Reader { get; set; }
        
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
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public BaseDatabaseReader (IDbConnection connection, string database)
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
        public BaseDatabaseReader (IDbConnection connection, IDbCommand command,
            string database)
        {
            Connection = connection;
            Command = command;
            Database = database;
        }
        
        public void Dispose ()
        {
            if (Reader != null)
            {
                Reader.Dispose ();
            }
            Command.Dispose ();
            Connection.Dispose ();
        }

        /// <summary>
        /// Read the data out of the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Model.GlobalState"/> containing all the data in the
        /// database.
        /// </returns>
        public Model.XcData Read ()
        {
            ReadConferences ();
            ReadRunners ();
            ReadSchools ();
            ReadAffiliations ();
            ReadMeetNames ();
            ReadVenues ();
            ReadRaces ();
            ReadPerformances ();
            return new Tables.XcData (Tables.Affiliation.List,
                Tables.Conference.List, Tables.MeetName.List,
                Tables.Performance.List, Tables.Race.List, Tables.Runner.List,
                Tables.School.List, Tables.Venue.List);
        }
                
        /// <summary>
        /// Read the affiliations table of the database.
        /// </summary>
        abstract public void ReadAffiliations();
        
        /// <summary>
        /// Read the conferences table of the database.
        /// </summary>
        abstract public void ReadConferences();        
        
        /// <summary>
        /// Read the meets table of the database.
        /// </summary>
        abstract public void ReadMeetNames();
             
        /// <summary>
        /// Read the performances table of the database.
        /// </summary>           
        abstract public void ReadPerformances();
        
        /// <summary>
        /// Read the races table of the database.
        /// </summary>
        abstract public void ReadRaces();
                
        /// <summary>
        /// Read the runners table of the database.
        /// </summary>
        abstract public void ReadRunners();
        
        /// <summary>
        /// Read the schools table of the database.
        /// </summary>
        abstract public void ReadSchools();
                
        /// <summary>
        /// Read the venues table of the database.
        /// </summary>
        abstract public void ReadVenues();
    }
}