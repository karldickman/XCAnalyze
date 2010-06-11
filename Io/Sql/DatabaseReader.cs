using System;
using System.Data;
using XCAnalyze.Io.Sql.Tables;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// A <see cref="IReader"/> that reads all the required data for the model out of a
    /// database.
    /// </summary>
    public class DatabaseReader : IReader<Model.XcData>
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
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        protected internal DatabaseReader (IDbConnection connection)
        {
            Connection = connection;
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
        public DatabaseReader(IDbConnection connection,
            IDbCommand command) : this(connection)
        {
            Command = command;
        }
        
        /// <summary>
        /// Create a new database reader using the provided connection;
        /// </summary>
        public static DatabaseReader NewInstance (IDbConnection connection)
        {
            connection.Open ();
            return new DatabaseReader (connection, connection.CreateCommand());
        }

        /// <summary>
        /// Close this database reader.
        /// </summary>
        public void Close ()
        {
            if (Reader != null)
            {
                Reader.Close ();
            }
            if (Command != null)
            {
                Command.Dispose ();
            }
            Connection.Close ();
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
            ReadRaces();
            ReadPerformances ();
            return new Tables.XcData (Tables.Affiliation.List, Tables.Conference.List,
                Tables.MeetName.List, Tables.Performance.List, Tables.Race.List,
                Tables.Runner.List, Tables.School.List, Tables.Venue.List);
        }
        
        /// <summary>
        /// Read the affiliations table of the database.
        /// </summary>
        public void ReadAffiliations ()
        {
            Affiliation.Clear();
            int id, runnerId, schoolId;
            Command.CommandText = "SELECT * FROM affiliations";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                runnerId = int.Parse(Reader["runner_id"].ToString());
                schoolId = int.Parse(Reader["school_id"].ToString());
                new Affiliation (id, runnerId, schoolId,
                    int.Parse(Reader["year"].ToString()));
            }
            Reader.Close ();
        }
        
        /// <summary>
        /// Read the conferences table of the database.
        /// </summary>
        public void ReadConferences ()
        {
            Conference.Clear ();
            int id;
            string abbreviation;
            Command.CommandText = "SELECT * FROM conferences";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                if(Reader["abbreviation"] is DBNull)
                {
                    abbreviation = null;
                }
                else
                {
                    abbreviation = (string)Reader["abbreviation"];
                }
                new Conference (id, (string)Reader["name"], abbreviation);
            }
            Reader.Close ();
        }
        
        /// <summary>
        /// Read the meets table of the database.
        /// </summary>
        public void ReadMeetNames ()
        {
            MeetName.Clear();
            int id;
            Command.CommandText = "SELECT * FROM meets";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                new MeetName (id, (string)Reader["name"]);
            }
            Reader.Close ();
        }
        
        /// <summary>
        /// Read the performances table of the database.
        /// </summary>
        public void ReadPerformances ()
        {
            Performance.Clear();
            int id, runnerId, raceId;
            Command.CommandText = "SELECT * FROM results";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                runnerId = int.Parse(Reader["runner_id"].ToString());
                raceId = int.Parse(Reader["race_id"].ToString());
                new Performance ((int)id, runnerId,
                        raceId, new Model.Time((double)Reader["time"]));
            }
            Reader.Close ();
        }
        
        /// <summary>
        /// Read the races table of the database.
        /// </summary>
        public void ReadRaces ()
        {
            Race.Clear();
            int id;
            int? meetId, venueId;
            Command.CommandText = "SELECT * FROM races";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ()) {
                id = int.Parse(Reader["id"].ToString());
                if (Reader["meet_id"] is DBNull)
                {
                    meetId = null;
                }
                else
                {
                    meetId = new int?(int.Parse(Reader["meet_id"].ToString()));
                }
                if (Reader["venue_id"] is DBNull)
                {
                    venueId = null;
                }
                else
                {
                    venueId = new int?(int.Parse(Reader["venue_id"].ToString()));
                }
                new Race (id, meetId, new Model.Date((DateTime)Reader["date"]),
                    venueId, Model.Gender.FromString ((string)Reader["gender"]),
                    (int)Reader["distance"]);
            }
            Reader.Close ();
        }
        
        /// <summary>
        /// Read the runners table of the database.
        /// </summary>
        public void ReadRunners ()
        {
            Runner.Clear ();
            int id;
            int? year;
            string[] nicknames;
            Command.CommandText = "SELECT * FROM runners";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ()) {
                id = int.Parse (Reader["id"].ToString ());
                if (Reader["nicknames"] is DBNull)
                {
                    nicknames = null;
                }
                else
                {
                    nicknames = ((string)Reader["nicknames"]).Split (',');
                    foreach (string nickname in nicknames)
                    {
                        nickname.Trim ();
                    }
                }
                if (Reader["year"] is DBNull)
                {
                    year = null;
                }
                else
                {
                    year = new Nullable<int>(int.Parse(Reader["year"].ToString()));
                }
                new Runner ((int)id, (string)Reader["surname"],
                    (string)Reader["given_name"], nicknames,
                    Model.Gender.FromString ((string)Reader["gender"]), year);
            }
            Reader.Close ();
        }

        /// <summary>
        /// Read the schools table of the database.
        /// </summary>
        public void ReadSchools ()
        {
            School.Clear ();
            int id;
            int? conferenceId;
            string[] nicknames;
            string type;
            Command.CommandText = "SELECT * FROM schools";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                if (Reader["nicknames"] is DBNull)
                {
                    nicknames = null;
                }
                else
                {
                    nicknames = ((string)Reader["nicknames"]).Split (',');
                    foreach (string nickname in nicknames)
                    {
                        nickname.Trim ();
                    }
                }
                if (Reader["type"] is DBNull)
                {
                    type = null;
                }
                else
                {
                    type = (string)Reader["type"];
                }
                if (Reader["conference_id"] is DBNull)
                {
                    conferenceId = null;
                }
                else
                {
                    conferenceId = int.Parse (Reader["conference_id"].ToString ());
                }
                new School (id, (string)Reader["name"], nicknames, type,
                    (bool)Reader["name_first"], conferenceId);
            }
            Reader.Close ();
        }
        
        /// <summary>
        /// Read the venues table of the database.
        /// </summary>
        public void ReadVenues ()
        {
            Venue.Clear();
            int id;
            int? elevation;
            string name;
            Command.CommandText = "SELECT * FROM venues";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                if (Reader["name"] is DBNull)
                {
                    name = null;
                }
                else
                {
                    name = (string)Reader["name"];
                }
                if (Reader["elevation"] is DBNull)
                {
                    elevation = null;
                }
                else
                {
                    elevation = (int)Reader["elevation"];
                }
                new Venue (id, name, (string)Reader["city"],
                    (string)Reader["state"], elevation);
            }
            Reader.Close ();
        }
    }
}