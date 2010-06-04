using System;
using System.Data;
using XCAnalyze.Io.Sql.Tables;

namespace XCAnalyze.Io.Sql
{
    public class DatabaseReader : IReader<Model.GlobalState>
    {
        protected internal IDataReader Reader { get; set; }
        protected internal IDbCommand Command { get; set; }
        protected internal IDbConnection Connection { get; set; }
        
        protected internal DatabaseReader (IDbConnection connection)
        {
            Connection = connection;
        }
        
        /// <summary>
        /// Create a new database reader using the provided connection;
        /// </summary>
        public static DatabaseReader NewInstance (IDbConnection connection)
        {
            DatabaseReader reader = new DatabaseReader (connection);
            reader.Connection.Open ();
            reader.Command = reader.Connection.CreateCommand ();
            return reader;
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

        public Model.GlobalState Read ()
        {
            ReadConferences ();
            ReadRunners ();
            ReadSchools ();
            ReadAffiliations ();
            ReadMeets ();
            ReadVenues ();
            ReadPerformances ();
            return SqlGlobalState.NewInstance (SqlAffiliation.List, SqlConference.List,
                SqlMeet.List, SqlPerformance.List, SqlRace.List, SqlRunner.List,
                SqlSchool.List, SqlVenue.List);
        }
        
        public void ReadAffiliations ()
        {
            int id, runnerId, schoolId;
            Command.CommandText = "SELECT * FROM affiliations";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = (int)(uint)Reader["id"];
                runnerId = (int)(uint)Reader["runner_id"];
                schoolId = (int)(uint)Reader["school_id"];
                new SqlAffiliation ((int)id, runnerId,
                        schoolId, (int)Reader["year"]);
            }
            Reader.Close ();
        }
        
        public void ReadConferences ()
        {
            SqlConference.Clear ();
            int id;
            Command.CommandText = "SELECT * FROM conferences";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                new SqlConference (id, (string)Reader["name"],
                    (string)Reader["abbreviation"]);
            }
            Reader.Close ();
        }
        
        public void ReadMeets ()
        {
            int id;
            Command.CommandText = "SELECT * FROM meets";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = (int)(uint)Reader["id"];
                new SqlMeet (id, (string)Reader["name"]);
            }
            Reader.Close ();
        }
        
        public void ReadPerformances ()
        {
            int id, runnerId, raceId;
            Command.CommandText = "SELECT * FROM results";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = (int)(uint)Reader["id"];
                runnerId = (int)(uint)Reader["runner_id"];
                raceId = (int)(uint)Reader["race_id"];
                new SqlPerformance ((int)id, runnerId,
                        raceId, new Model.Time((double)Reader["time"]));
            }
            Reader.Close ();
        }
        
        public void ReadRaces ()
        {
            int id;
            int? meetId, venueId;
            Command.CommandText = "SELECT * FROM races";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ()) {
                id = (int)(uint)Reader["id"];
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
                new SqlRace (id, meetId, venueId,
                    new Model.Date((DateTime)Reader["date"]),
                    Model.Gender.FromString ((string)Reader["gender"]),
                    (int)Reader["distance"]);
            }
            Reader.Close ();
        }
        
        public void ReadRunners ()
        {
            SqlRunner.Clear ();
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
                new SqlRunner ((int)id, (string)Reader["surname"],
                    (string)Reader["given_name"], nicknames,
                    Model.Gender.FromString ((string)Reader["gender"]), year);
            }
            Reader.Close ();
        }

        public void ReadSchools ()
        {
            SqlSchool.Clear ();
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
                Console.WriteLine ("conferenceId: " + conferenceId);
                new SqlSchool (id, (string)Reader["name"], nicknames, type,
                    (bool)Reader["name_first"], conferenceId);
            }
            Reader.Close ();
        }
        
        public void ReadVenues ()
        {
            int id;
            int? elevation;
            string name;
            Command.CommandText = "SELECT * FROM venues";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = (int)(uint)Reader["id"];
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
                new SqlVenue (id, name, (string)Reader["city"],
                    (string)Reader["state"], elevation);
            }
            Reader.Close ();
        }
    }
}