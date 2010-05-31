using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{

    public class DatabaseReader : IReader<Data>
    {
        private IDataReader reader;
        private IDbCommand command;
        private IDbConnection connection;

        protected internal IDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }
        
        protected internal IDbCommand Command
        {
            get { return command; }
        }
        
        protected internal IDbConnection Connection
        {
            get { return connection; }
        }

        public DatabaseReader (IDbConnection connection)
        {
            this.connection = connection;
            command = connection.CreateCommand ();
        }

        public void Close ()
        {
            reader.Close ();
            command.Dispose ();
        }

        public Data Read ()
        {
            IDictionary<int, SqlConference> conferences = ReadConferences ();
            IDictionary<int, Runner> runners = ReadRunners ();
            IDictionary<int, School> schools = ReadSchools (conferences);
            IDictionary<int, Affiliation> affiliations = ReadAffiliations (runners, schools);
            IDictionary<int, SqlMeet> meets = ReadMeets ();
            IDictionary<int, SqlVenue> venues = ReadVenues ();
            IDictionary<int, Race> races = ReadRaces (meets, venues);
            IDictionary<int, Performance> performances = ReadPerformances (runners, races);
            return SqlData.NewInstance(new List<Affiliation>(affiliations.Values), new List<SqlConference>(conferences.Values), new List<SqlMeet>(meets.Values), new List<Performance>(performances.Values), new List<Race>(races.Values), new List<Runner>(runners.Values), new List<School>(schools.Values), new List<SqlVenue>(venues.Values));
        }
        
        IDictionary<int, Affiliation> ReadAffiliations (IDictionary<int, Runner> runners, IDictionary<int, School> schools)
        {
            IDictionary<int, Affiliation> affiliations = new Dictionary<int, Affiliation> ();
            int id, runnerId, schoolId;
            command.CommandText = "SELECT * FROM affiliations";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                runnerId = (int)(uint)reader["runner_id"];
                schoolId = (int)(uint)reader["school_id"];
                affiliations.Add (id, new SqlAffiliation ((int)id, runners[runnerId], schools[schoolId], (int)reader["year"]));
            }
            reader.Close ();
            return affiliations;
        }
        
        IDictionary<int, SqlConference> ReadConferences ()
        {
            IDictionary<int, SqlConference> conferences = new Dictionary<int, SqlConference> ();
            int id;
            command.CommandText = "SELECT * FROM conferences";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                conferences.Add (id, new SqlConference (id, (string)reader["name"], (string)reader["abbreviation"]));
            }
            reader.Close ();
            return conferences;
        }
        
        IDictionary<int, SqlMeet> ReadMeets ()
        {
            IDictionary<int, SqlMeet> meets = new Dictionary<int, SqlMeet> ();
            int id;
            command.CommandText = "SELECT * FROM meets";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                meets.Add (id, new SqlMeet (id, (string)reader["name"]));
            }
            reader.Close ();
            return meets;
        }
        
        IDictionary<int, Performance> ReadPerformances (IDictionary<int, Runner> runners, IDictionary<int, Race> races)
        {
            IDictionary<int, Performance> performances = new Dictionary<int, Performance> ();
            int id, runnerId, raceId;
            command.CommandText = "SELECT * FROM results";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                runnerId = (int)(uint)reader["runner_id"];
                raceId = (int)(uint)reader["race_id"];
                performances.Add (id, new SqlPerformance ((int)id, runners[runnerId], races[raceId], new Time((double)reader["time"])));
            }
            reader.Close ();
            return performances;
        }
        
        IDictionary<int, Race> ReadRaces (IDictionary<int, SqlMeet> meets, IDictionary<int, SqlVenue> venues)
        {
            IDictionary<int, Race> races = new Dictionary<int, Race> ();
            int id;
            SqlMeet meet;
            SqlVenue venue;
            command.CommandText = "SELECT * FROM races";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                if (reader["meet_id"] is DBNull) {
                    meet = null;
                } else {
                    meet = meets[(int)(uint)reader["meet_id"]];
                }
                if (reader["venue_id"] is DBNull) {
                    venue = null;
                } else {
                    venue = venues[(int)(uint)reader["venue_id"]];
                }
                races.Add (id, SqlRace.NewInstance (id, meet, venue, new Date((DateTime)reader["date"]), Gender.FromString ((string)reader["gender"]), (int)reader["distance"]));
            }
            reader.Close ();
            return races;
        }
        
        IDictionary<int, Runner> ReadRunners ()
        {
            IDictionary<int, Runner> runners = new Dictionary<int, Runner> ();
            int id;
            string[] nicknames;
            int? year;
            command.CommandText = "SELECT * FROM runners";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                if (reader["nicknames"] is DBNull) {
                    nicknames = null;
                } else {
                    nicknames = ((string)reader["nicknames"]).Split (',');
                    foreach (string nickname in nicknames) {
                        nickname.Trim ();
                    }
                }
                if (reader["year"] is DBNull) {
                    year = null;
                } else {
                    year = (int?)reader["year"];
                }
                runners.Add (id, new SqlRunner ((int)id, (string)reader["surname"], (string)reader["given_name"], nicknames, Gender.FromString ((string)reader["gender"]), year));
            }
            reader.Close ();
            return runners;
        }

        IDictionary<int, School> ReadSchools (IDictionary<int, SqlConference> conferences)
        {
            IDictionary<int, School> schools = new Dictionary<int, School> ();
            int id;
            SqlConference conference;
            string[] nicknames;
            string type;
            command.CommandText = "SELECT * FROM schools";
            reader = command.ExecuteReader ();
            while (reader.Read ()) {
                id = (int)(uint)reader["id"];
                if (reader["nicknames"] is DBNull) {
                    nicknames = null;
                } else {
                    nicknames = ((string)reader["nicknames"]).Split (',');
                    foreach (string nickname in nicknames) {
                        nickname.Trim ();
                    }
                }
                if (reader["type"] is DBNull) {
                    type = null;
                } else {
                    type = (string)reader["type"];
                }
                if (reader["conference_id"] is DBNull) {
                    conference = null;
                } else {
                    conference = conferences[(int)(uint)reader["conference_id"]];
                }
                schools.Add (id, SqlSchool.NewInstance (id, (string)reader["name"], nicknames, type, (bool)reader["name_first"], conference));
            }
            reader.Close ();
            return schools;
        }
        
        IDictionary<int, SqlVenue> ReadVenues ()
        {
            IDictionary<int, SqlVenue> venues = new Dictionary<int, SqlVenue> ();
            int id;
            int? elevation;
            string name;
            command.CommandText = "SELECT * FROM venues";
            reader = command.ExecuteReader ();
            while (reader.Read ())
            {
                id = (int)(uint)reader["id"];
                if (reader["name"] is DBNull) 
                {
                    name = null;
                } else {
                    name = (string)reader["name"];
                }
                if (reader["elevation"] is DBNull) 
                {
                    elevation = null;
                } else {
                    elevation = (int)reader["elevation"];
                }
                venues.Add (id, new SqlVenue (id, name, (string)reader["city"], (string)reader["state"], elevation));
            }
            reader.Close ();
            return venues;
        }
    }
    
    [TestFixture]
    public class TestDatabaseReader
    {
        private IDbConnection connection;
        private DatabaseReader reader;

        [SetUp]
        public void Setup ()
        {
            string connectionString = "Server=localhost;";
            connectionString += "Database=xcanalyze;";
            connectionString += "User ID=xcanalyze;";
            connectionString += "Pooling=false;";
            connection = new MySqlConnection (connectionString);
            connection.Open ();
            reader = new DatabaseReader (connection);
        }

        [Test]
        public void TestRead ()
        {
            reader.Read ();
            reader.Close ();
        }
    }
}
