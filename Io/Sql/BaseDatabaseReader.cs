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
    abstract public class BaseDatabaseReader : IReader<XcData>
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
        
        protected internal BaseDatabaseReader()
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
        public BaseDatabaseReader (IDbConnection connection, string database)
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
        public BaseDatabaseReader (IDbConnection connection, IDbCommand command,
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
        public void Dispose ()
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

        /// <summary>
        /// Read the data out of the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Model.GlobalState"/> containing all the data in the
        /// database.
        /// </returns>
        public XcData Read ()
        {
            IDictionary<int, string> conferenceIds = ReadConferences ();
            IDictionary<int, Runner> runnerIds = ReadRunners ();
            IDictionary<int, School> schoolIds = ReadSchools (conferenceIds);
            IDictionary<int, Affiliation> affiliationIds =
                ReadAffiliations (runnerIds, schoolIds);
            IDictionary<int, string> meetNameIds = ReadMeetNames ();
            IDictionary<int, Race> raceIds = ReadRaces ();
            IDictionary<int, Venue> venueIds = ReadVenues ();
            IDictionary<int, Meet> meetIds = ReadMeets (meetNameIds, raceIds,
                venueIds);
            IDictionary<int, Performance> performanceIds = ReadPerformances (
                raceIds, runnerIds);
            IList<Affiliation> affiliations =
                new List<Affiliation> (affiliationIds.Values);
            IList<Meet> meets = new List<Meet> (meetIds.Values);
            IList<Performance> performances =
                new List<Performance> (performanceIds.Values);
            IList<Runner> runners = new List<Runner> (runnerIds.Values);
            IList<School> schools = new List<School> (schoolIds.Values);
            return new XcData (affiliations, meets, performances, runners,
                schools);
        }
                
        /// <summary>
        /// Read the affiliations.
        /// </summary>
        /// <param name="runners">
        /// A <see cref="IDictionary<System.Int32, Runner>"/> mapping id numbers
        /// to runners.
        /// </param>
        /// <param name="schools">
        /// A <see cref="IDictionary<System.Int32, School>"/> mapping id numbers
        /// to schools.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Affiliation>"/> mapping id
        /// numbers to affiliations.
        /// </returns>
        abstract public IDictionary<int, Affiliation> ReadAffiliations(
            IDictionary<int, Runner> runners, IDictionary<int, School> schools);
        
        /// <summary>
        /// Read the conferences.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping
        /// id numbers to conferences.
        /// </returns>
        abstract public IDictionary<int, string> ReadConferences();        
        
        /// <summary>
        /// Read the meets.
        /// </summary>
        /// <param name="meetNames">
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping id
        /// numbers to meet names.
        /// </param>
        /// <param name="races">
        /// A <see cref="IDictionary<System.Int32, Race>"/> mapping id numbers
        /// to races.
        /// </param>
        /// <param name="venues">
        /// A <see cref="IDictionary<System.Int32, Venue>"/> mapping id numbers
        /// to venues.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Meet>"/> mapping id numbers
        /// to meets.
        /// </returns>
        abstract public IDictionary<int, Meet> ReadMeets(
            IDictionary<int, string> meetNames, IDictionary<int, Race> races,
            IDictionary<int, Venue> venues);
        
        /// <summary>
        /// Read the meets.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping
        /// id numbers to meet names.
        /// </returns>
        abstract public IDictionary<int, string> ReadMeetNames();
        
        /// <summary>
        /// Read the performances.
        /// </summary>
        /// <param name="races">
        /// A <see cref="IDictionary<System.Int32, Race>"/> mapping id numbers
        /// to races.
        /// </param>
        /// <param name="runners">
        /// A <see cref="IDictionary<System.Int32, Runner>"/> mapping id numbers
        /// to runners.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Performance>"/> mapping id
        /// numbers to performances.
        /// </returns>
        abstract public IDictionary<int, Performance> ReadPerformances(
            IDictionary<int, Race> races, IDictionary<int, Runner> runners);
        
        /// <summary>
        /// Read the races.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Race>"/> mapping id nubmers
        /// to races.
        /// </returns>
        abstract public IDictionary<int, Race> ReadRaces();
                
        /// <summary>
        /// Read the runners.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Runner>"/> mapping id numbers
        /// to runners.
        /// </returns>
        abstract public IDictionary<int, Runner> ReadRunners();
        
        /// <summary>
        /// Read the schools.
        /// </summary>
        /// <param name="conferences">
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping id
        /// numbers to conferences.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, School>"/> mapping id numbers
        /// to schools.
        /// </returns>
        abstract public IDictionary<int, School> ReadSchools(
            IDictionary<int, string> conferences);
                
        /// <summary>
        /// Read the venues.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Venue>"/> mapping id numbers
        /// to venues.
        /// </returns>
        abstract public IDictionary<int, Venue> ReadVenues();
    }
}