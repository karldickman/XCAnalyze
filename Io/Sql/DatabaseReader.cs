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
    public class DatabaseReader : BaseDatabaseReader
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
        
        override public void ReadAffiliations ()
        {
            Tables.Affiliation.Clear ();
            int id, runnerId, schoolId;
            Command.CommandText = "SELECT * FROM affiliations";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse (Reader["id"].ToString ());
                runnerId = int.Parse (Reader["runner_id"].ToString ());
                schoolId = int.Parse (Reader["school_id"].ToString ());
                new Tables.Affiliation (id, runnerId, schoolId,
                    int.Parse (Reader["year"].ToString ()));
            }
            Reader.Close ();
        }
        
        override public void ReadConferences ()
        {
            Tables.Conference.Clear ();
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
                new Tables.Conference (id, (string)Reader["name"], abbreviation);
            }
            Reader.Close ();
        }
        
        override public void ReadMeetNames ()
        {
            Tables.MeetName.Clear();
            int id;
            Command.CommandText = "SELECT * FROM meets";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                new Tables.MeetName (id, (string)Reader["name"]);
            }
            Reader.Close ();
        }

        override public void ReadPerformances ()
        {
            Tables.Performance.Clear();
            int id, runnerId, raceId;
            Command.CommandText = "SELECT * FROM results";
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                id = int.Parse(Reader["id"].ToString());
                runnerId = int.Parse(Reader["runner_id"].ToString());
                raceId = int.Parse(Reader["race_id"].ToString());
                new Tables.Performance ((int)id, runnerId,
                        raceId, new Model.Time((double)Reader["time"]));
            }
            Reader.Close ();
        }

        override public void ReadRaces ()
        {
            Tables.Race.Clear();
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
                new Tables.Race (id, meetId, new Model.Date((DateTime)Reader["date"]),
                    venueId, Model.Gender.FromString ((string)Reader["gender"]),
                    (int)Reader["distance"]);
            }
            Reader.Close ();
        }

        override public void ReadRunners ()
        {
            Tables.Runner.Clear ();
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
                new Tables.Runner ((int)id, (string)Reader["surname"],
                    (string)Reader["given_name"], nicknames,
                    Model.Gender.FromString ((string)Reader["gender"]), year);
            }
            Reader.Close ();
        }

        override public void ReadSchools ()
        {
            Tables.School.Clear ();
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
                new Tables.School (id, (string)Reader["name"], nicknames, type,
                    (bool)Reader["name_first"], conferenceId);
            }
            Reader.Close ();
        }

        override public void ReadVenues ()
        {
            Tables.Venue.Clear();
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
                new Tables.Venue (id, name, (string)Reader["city"],
                    (string)Reader["state"], elevation);
            }
            Reader.Close ();
        }
    }
    
    abstract public class TestDatabaseReader
    {
        public const string EXAMPLE_DATABASE = "xca_example";
        
        protected internal BaseDatabaseReader Reader { get; set; }
        
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