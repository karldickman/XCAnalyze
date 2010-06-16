using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{    
    /// <summary>
    /// A <see cref="IReader"/> that reads all the required data for the model out of a
    /// database.
    /// </summary>
    public class DatabaseReader : AbstractDatabaseReader
    {
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public DatabaseReader (IDbConnection connection, string database)
        : base(connection, database) {}
        
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
        public DatabaseReader(IDbConnection connection, IDbCommand command,
            string database) : base(connection, command, database) {}
        
        override public IDictionary<int, Affiliation> ReadAffiliations (
            IDictionary<int, Runner> runners, IDictionary<int, School> schools)
        {
            IDictionary<int, Affiliation> affiliations =
                new Dictionary<int, Affiliation>();
            int id, year;
            Runner runner;
            School school;
            Command.CommandText = "SELECT * FROM affiliations";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                runner = runners[int.Parse (Reader["runner_id"].ToString ())];
                school = schools[int.Parse (Reader["school_id"].ToString ())];
                year = int.Parse(Reader["year"].ToString());
                affiliations.Add(id, new Affiliation(runner, school, year));
            }
            Reader.Close ();
            return affiliations;
        }
        
        override public IDictionary<int, string> ReadConferences ()
        {
            IDictionary<int, string> conferences = new Dictionary<int, string>();
            int id;
            Command.CommandText = "SELECT * FROM conferences";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                conferences.Add(id, (string)Reader["name"]);
            }
            Reader.Close ();
            return conferences;
        }
        
        /// <exception>
        /// A <see cref="ArgumentNullException"/> is thrown when a meet with two
        /// null races is encountered.
        /// </exception>
        override public IDictionary<int, Meet> ReadMeets(
            IDictionary<int, string> meetNames, IDictionary<int, Race> races,
            IDictionary<int, Venue> venues)
        {
            IDictionary<int, Meet> meets = new Dictionary<int, Meet>();
            int id;
            Date date;
            Race mensRace, womensRace;
            string meetName;
            Venue venue;
            Command.CommandText = "SELECT * FROM meets";
            Reader = Command.ExecuteReader();
            while(Reader.Read())
            {
                id = int.Parse(Reader["id"].ToString());
                if(Reader["meet_name_id"] is DBNull)
                {
                    meetName = null;
                }
                else
                {
                    meetName = meetNames[int.Parse(Reader["meet_name_id"].ToString())];
                }
                date = new Date((DateTime)Reader["date"]);
                if(Reader["venue_id"] is DBNull)
                {
                    venue = null;
                }
                else
                {
                    venue = venues[int.Parse(Reader["venue_id"].ToString())];
                }
                if(Reader["mens_race_id"] is DBNull)
                {
                    mensRace = null;
                }
                else
                {
                    mensRace = races[int.Parse(Reader["mens_race_id"].ToString())];
                }
                if(Reader["womens_race_id"] is DBNull)
                {
                    womensRace = null;
                }
                else
                {
                    womensRace = races[int.Parse(Reader["womens_race_id"].ToString())];
                }
                meets.Add(id,
                    new Meet(meetName, date, venue, mensRace, womensRace));
            }
            Reader.Close();
            return meets;
        }
        
        override public IDictionary<int, string> ReadMeetNames ()
        {
            IDictionary<int, string> meetNames = new Dictionary<int, string>();
            int id;
            Command.CommandText = "SELECT * FROM meet_names";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                meetNames.Add(id, (string)Reader["name"]);
            }
            Reader.Close ();
            return meetNames;
        }

        override public IDictionary<int, Performance> ReadPerformances (
            IDictionary<int, Race> races, IDictionary<int, Runner> runners)
        {
            IDictionary<int, Performance> performances =
                new Dictionary<int, Performance>();
            int id;
            Race race;
            Runner runner;
            Time time;
            Command.CommandText = "SELECT * FROM results";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                runner = runners[int.Parse(Reader["runner_id"].ToString())];
                race = races[int.Parse(Reader["race_id"].ToString())];
                time = new Time((double)Reader["time"]);
                performances.Add(id, new Performance(runner, race, time));
            }
            Reader.Close ();
            return performances;
        }

        override public IDictionary<int, Race> ReadRaces ()
        {
            IDictionary<int, Race> races = new Dictionary<int, Race>();
            int id, distance;
            Command.CommandText = "SELECT * FROM races";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                distance = int.Parse(Reader["distance"].ToString());
                races.Add(id, new Race(null, distance));
            }
            Reader.Close ();
            return races;
        }

        override public IDictionary<int, Runner> ReadRunners ()
        {
            IDictionary<int, Runner> runners = new Dictionary<int, Runner>();
            Gender gender;
            int id;
            int? year;
            IList<string> nicknames;
            string givenName, surname;
            Command.CommandText = "SELECT * FROM runners";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ()) {
                id = int.Parse (Reader["id"].ToString ());
                gender = Gender.FromString(Reader["gender"].ToString());
                givenName = (string)Reader["given_name"];
                surname = (string)Reader["surname"];
                nicknames = new List<string>();
                if (!(Reader["nicknames"] is DBNull))
                {
                    foreach(string nickname
                        in ((string)Reader["nicknames"]).Split(','))
                    {
                        nickname.Trim();
                        nicknames.Add(nickname);
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
                runners.Add(id,
                    new Runner(surname, givenName, nicknames, gender, year));
            }
            Reader.Close ();
            return runners;
        }

        override public IDictionary<int, School> ReadSchools (
            IDictionary<int, string> conferences)
        {
            IDictionary<int, School> schools = new Dictionary<int, School>();
            int id;
            bool nameFirst;
            IList<string> nicknames;
            string conference, name, type;
            Command.CommandText = "SELECT * FROM schools";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                name = (string)Reader["name"];
                nameFirst = (bool)Reader["name_first"];
                nicknames = new List<string>();
                if (!(Reader["nicknames"] is DBNull))
                {
                    foreach(string nickname
                        in ((string)Reader["nicknames"]).Split(','))
                    {
                        nicknames.Add(nickname.Trim());
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
                    conference = null;
                }
                else
                {
                    conference = conferences[int.Parse (Reader["conference_id"].ToString ())];
                }
                schools.Add(id, new School(name, type, nameFirst, nicknames, conference));
            }
            Reader.Close ();
            return schools;
        }

        override public IDictionary<int, Venue> ReadVenues ()
        {
            IDictionary<int, Venue> venues = new Dictionary<int, Venue>();
            int id;
            string name, city, state;
            Command.CommandText = "SELECT * FROM venues";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                name = (Reader["name"] is DBNull) ? null : (string)Reader["name"];       
                city = (string)Reader["city"];    
                state = (string)Reader["state"];             
                venues.Add(id, new Venue(name, city, state));
            }
            Reader.Close ();
            return venues;
        }
    }
    
    abstract public class TestDatabaseReader
    {
        public const string EXAMPLE_DATABASE = "xca_example";
        
        protected internal AbstractDatabaseReader Reader { get; set; }
        
        abstract public void SetUp();
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Dispose ();
        }
        
        [Test]
        virtual public void TestRead ()
        {
            Reader.Read ();
        }
    }
}