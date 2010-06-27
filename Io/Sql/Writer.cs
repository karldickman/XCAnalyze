using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using XCAnalyze.Collections;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{   
    /// <summary>
    /// A writer to write all the data in the model to a database.
    /// </summary>
    abstract public class Writer : AbstractWriter
    {
        /// <summary>
        /// The creation script for the database.
        /// </summary>
        public string CREATION_SCRIPT
        {
            get
            {
                return SupportFiles.GetPath ("xca_create." +
                    CREATION_SCRIPT_EXTENSION);
            }
        }
        
        /// <summary>
        /// The file extension of the creation script.
        /// </summary>
        abstract public string CREATION_SCRIPT_EXTENSION { get; }
        
        /// <summary>
        /// The title of the column that has the names of all the tables.
        /// </summary>
        abstract public string GET_TABLES_COLUMN { get; }

        /// <summary>
        /// The script used to get the list of tables in the database.
        /// </summary>
        abstract public string GET_TABLES_COMMAND { get; }
        
        public Writer(IDbConnection connection, string database)
            : base(connection, database) {}
        
        public Writer(IDbConnection connection, string database,
            IDbCommand command) : base(connection, database, command) {}
        
        override public IList<string> CreationScript()
        {
            IList<string> commands;
            ScriptReader reader;
            reader = new ScriptReader(CREATION_SCRIPT);
            commands = reader.Read();
            reader.Dispose();
            return commands;
        }
        
        /// <summary>
        /// Format a particular date for insertion in an SQL query.
        /// </summary>
        /// <param name="value_">
        /// The <see cref="Date"/> to format.
        /// </param>
        public string Format(Date value_)
        {
            string result = value_.Year + "-";
            if(value_.Month < 10)
            {
                result += "0";
            }
            result += value_.Month + "-";
            if(value_.Day < 10)
            {
                result += "0";
            }
            return Format(result + value_.Day);
        }
        
        /// <summary>
        /// Format a particular boolean value for insertion in an SQL query.
        /// </summary>
        public string Format (bool value_)
        {
            if (value_)
            {
                return "1";
            }
            return "0";
        }
        
        /// <summary>
        /// Format the given gender for insertion in an SQL query.
        /// </summary>
        public string Format(Gender value_)
        {
            return Format(value_.ToString());
        }
        
        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        public string Format(int? value_)
        {
            if(value_ == null)
            {
                return "NULL";
            }
            return value_.ToString();
        }
        
        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        public string Format (string value_)
        {
            if (value_ == null)
            {
                return "NULL";
            }
            return "\"" + value_ + "\"";
        }

        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        public string Format(string[] value_)
        {
            if(value_ == null)
            {
                return "NULL";
            }
            return Format(String.Join(", ", value_));
        }
        
        /// <summary>
        /// Format a particular race time for insertion via an SQL query.
        /// </summary>
        /// <param name="time">
        /// The <see cref="Time"/> to format.
        /// </param>
        public string Format (Time time)
        {
            return time.Seconds.ToString ();
        }
        
        override protected internal void InitializeDatabase ()
        {
            IList<string> creationCommands = CreationScript();
            foreach(string command in creationCommands)
            {
                Command.CommandText = command;
                Command.ExecuteNonQuery();
            }
        }
        
        override protected internal bool IsDatabaseInitialized ()
        {
            IList<string> foundTables = new List<string> ();
            Command.CommandText = GET_TABLES_COMMAND;
            ResultsReader = Command.ExecuteReader ();
            while (ResultsReader.Read ())
            {
                foundTables.Add ((string)ResultsReader[GET_TABLES_COLUMN]);
            }
            ResultsReader.Dispose ();
            if (foundTables.Count < TABLES.Length)
            {
                return false;
            }
            foreach (string table in TABLES)
            {
                if (!foundTables.Contains (table))
                {
                    return false;
                }
            }
            if (foundTables.Count == TABLES.Length)
            {
                return true;
            }
            if (foundTables.Count != TABLES.Length + VIEWS.Length)
            {
                return false;
            }
            foreach (string table in VIEWS)
            {
                if (!foundTables.Contains (table))
                {
                    return false;
                }
            }
            return true;
        }
        
        override public void WriteAffiliations(IList<Affiliation> affiliations,
            IList<Runner> runners, IList<School> schools)
        {
            for(int i = 0; i < affiliations.Count; i++)
            {
                Command.CommandText = "INSERT INTO affiliations (id, runner_id, school_id, year) VALUES (" + (i + 1) + ", " + (runners.IndexOf(affiliations[i].Runner) + 1) + ", " + (schools.IndexOf(affiliations[i].School) + 1) + ", " + affiliations[i].Year + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteConferences(IList<string> conferences)
        {
            for(int i = 0; i < conferences.Count; i++)
            {
                Command.CommandText = "INSERT INTO conferences (id, name) VALUES (" + (i + 1) + ", " + Format(conferences[i]) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteMeetNames(IList<string> meetNames)
        {
            for(int i = 0; i < meetNames.Count; i++)
            {
                Command.CommandText = "INSERT INTO meet_names (id, name) VALUES (" + (i + 1) + ", " + Format(meetNames[i]) + ")";
                Command.ExecuteNonQuery();
            }
        }
       
        override public void WriteMeets(IList<Meet> meets,
            IList<string> meetNames, IList<Race> races, IList<Venue> venues)
        {
            int? meetNameId, venueId, mensRaceId, womensRaceId;
            for(int i = 0; i < meets.Count; i++)
            {
                meetNameId = venueId = mensRaceId = womensRaceId = null;
                if(meets[i].Name != null)
                {
                    meetNameId = meetNames.IndexOf(meets[i].Name) + 1;
                }
                if(meets[i].Venue != null)
                {
                    venueId = venues.IndexOf(meets[i].Venue) + 1;
                }
                if(meets[i].MensRace != null)
                {
                    mensRaceId = races.IndexOf(meets[i].MensRace) + 1;
                }
                if(meets[i].WomensRace != null)
                {
                    womensRaceId = races.IndexOf(meets[i].WomensRace) + 1;
                }
                Command.CommandText = "SELECT id FROM races";
                ResultsReader = Command.ExecuteReader();
                ResultsReader.Close();
                Command.CommandText = "INSERT INTO meets (id, meet_name_id, date, venue_id, mens_race_id, womens_race_id) VALUES (" + (i + 1) + ", " + Format(meetNameId) + ", " + Format(meets[i].Date) + ", " + Format(venueId) + ", " + Format(mensRaceId) + ", " + Format(womensRaceId) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WritePerformances(IList<Performance> performances,
            IList<Race> races, IList<Runner> runners)
        {
            for(int i = 0; i < performances.Count; i++)
            {                
                Command.CommandText = "INSERT INTO results (id, runner_id, race_id, time) VALUES (" + (i + 1) + ", " + Format(runners.IndexOf(performances[i].Runner) + 1) + ", " + Format(races.IndexOf(performances[i].Race) + 1) + ", " + Format(performances[i].Time) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteRaces(IList<Race> races)
        {
            for(int i = 0; i < races.Count; i++)
            {
                Command.CommandText = "INSERT INTO races (id, distance) VALUES (" + (i + 1) + ", " + races[i].Distance + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteRunners(IList<Runner> runners)
        {
            for(int i = 0; i < runners.Count; i++)
            {
                Command.CommandText = "INSERT INTO runners (id, surname, given_name, gender, year) VALUES (" + (i + 1) + ", " + Format(runners[i].Surname) + ", " + Format(runners[i].GivenName) + ", " + Format(runners[i].Gender) + ", " + Format(runners[i].Year) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteSchools(IList<School> schools,
            IList<string> conferences)
        {
            int? conferenceId;
            foreach(School school in schools)
            {
                conferenceId = null;
                if(school.Conference != null)
                {
                    conferenceId = conferences.IndexOf(school.Conference) + 1;
                }
                Command.CommandText = "INSERT INTO schools (name, type, name_first, conference_id) VALUES (" + Format(school.Name) + ", " + Format(school.Type) + ", " + Format(school.NameFirst) + ", " + Format(conferenceId) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteVenues(IList<Venue> venues)
        {
            foreach(Venue venue in venues)
            {
                Command.CommandText = "INSERT INTO venues (name, city, state) VALUES (" + Format(venue.Name) + ", " + Format(venue.City) + ", " + Format(venue.State) + ")";                
                Command.ExecuteNonQuery();
            }
        }
    }
    
    abstract public class TestDatabaseWriter
    {
        /// <summary>
        /// A delegate for test methods.
        /// </summary>
        protected internal delegate T TestMethod<T>(T value_);
        
        public const string EXAMPLE_DATABASE = "xca_example";
        
        /// <summary>
        /// The name of the test database.
        /// </summary>
        public const string TEST_DATABASE = "xca_test";

        /// <summary>
        /// The username used to connect to the test database.
        /// </summary>
        public const string TEST_ACCOUNT = "xcanalyze";
        
        /// <summary>
        /// A sample list of affiliations.
        /// </summary>
        protected internal IXList<Affiliation> Affiliations { get; set; }
        
        /// <summary>
        /// A sample list of conferences.
        /// </summary>
        protected internal IXList<string> Conferences { get; set; }
        
        /// <summary>
        /// A sample global state.
        /// </summary>
        protected internal XcData GlobalState { get; set; }
        
        /// <summary>
        /// A sample list of meet names.
        /// </summary>
        protected internal IXList<string> MeetNames { get; set; }
        
        /// <summary>
        /// A sample list of meets.
        /// </summary>
        protected internal IXList<Meet> Meets { get; set; }
        
        /// <summary>
        /// A sample list of performances.
        /// </summary>
        protected internal IXList<Performance> Performances { get; set; }
        
        /// <summary>
        /// A sample list of races.
        /// </summary>
        protected internal IXList<Race> Races { get; set; }
        
        /// <summary>
        /// The reader for the database.
        /// </summary>
        protected internal AbstractReader Reader { get; set; }
        
        /// <summary>
        /// A sample list of runners.
        /// </summary>
        protected internal IXList<Runner> Runners { get; set; }
        
        /// <summary>
        /// A sample list of schools.
        /// </summary>
        protected internal IXList<School> Schools { get; set; }
        
        /// <summary>
        /// A sample list of venues.
        /// </summary>
        protected internal IXList<Venue> Venues { get; set; }
        
        /// <summary>
        /// The writer for the database.
        /// </summary>
        protected internal AbstractWriter Writer { get; set; }
        
        [SetUp]
        virtual public void SetUp()
        {
            string nwc, sciac;
            int y;
            Runner karl, hannah, richie, keith, leo, francis, florian;
            nwc = "Northwest Conference";
            sciac = "Southern California Intercollegiate Athletic Conference";
            Conferences = new XList<string> ();
            Conferences.Add (nwc);
            Conferences.Add (sciac);
            Conferences.Add ("Southern Collegiate Athletic Conference");
            Runners = new XList<Runner>();
            karl = new Runner("Dickman", "Karl", Gender.MALE, 2010);
            hannah = new Runner("Palmer", "Hannah", Gender.FEMALE, 2010);
            richie = new Runner("LeDonne", "Richie", Gender.MALE, 2011);
            keith = new Runner("Woodard", "Keith", Gender.MALE, null);
            leo = new Runner("Castillo", "Leo", Gender.MALE, 2012);
            francis = new Runner("Reynolds", "Francis", Gender.MALE, 2010);
            florian = new Runner("Scheulen", "Florian", Gender.MALE, 2010);
            Runners.Add(karl);
            Runners.Add(hannah);
            Runners.Add(richie);
            Runners.Add(keith);
            Runners.Add(leo);
            Runners.Add(francis);
            Runners.Add(florian);
            Schools = new XList<School>();
            Schools.Add(new School("Lewis & Clark", "College", nwc));
            Schools.Add(new School("Willamette", "University", nwc));
            Schools.Add(new School("Puget Sound", "University", false, nwc));
            Schools.Add(new School("Claremont-Mudd-Scripps", null, sciac));
            Schools.Add(new School("Pomona-Pitzer", null, sciac));
            Schools.Add(new School("California", "Institute of Technology", sciac));
            Schools.Add(new School("California, Santa Cruz", "University", false));
            Affiliations = new XList<Affiliation>();
            for(y = 2006; y < 2010; y++)
            {
                Affiliations.Add(new Affiliation(karl, Schools[0], y));
            }
            for(y = 2007; y < 2009; y++)
            {
                Affiliations.Add(new Affiliation(richie, Schools[0], y));
            }
            Affiliations.Add(new Affiliation(hannah, Schools[0], 2006));
            Affiliations.Add(new Affiliation(hannah, Schools[0], 2008));
            Affiliations.Add(new Affiliation(hannah, Schools[0], 2009));
            Affiliations.Add(new Affiliation(keith, Schools[0], 1969));
            Affiliations.Add(new Affiliation(keith, Schools[0], 1970));
            Affiliations.Add(new Affiliation(keith, Schools[0], 1971));
            Affiliations.Add(new Affiliation(keith, Schools[0], 1989));
            for(y = 2008; y < 2010; y++) {
                Affiliations.Add(new Affiliation(leo, Schools[1], y));
            }
            for(y = 2006; y < 2010; y++)
            {
                Affiliations.Add(new Affiliation(francis, Schools[2], y));
            }
            for(y = 2006; y < 2010; y++)
            {
                Affiliations.Add(new Affiliation(florian, Schools[3], y));
            }
            MeetNames = new XList<string>();
            MeetNames.Add("Lewis & Clark Invitational");
            MeetNames.Add("Charles Bowles Invitational");
            MeetNames.Add("Northwest Conference Championship");
            MeetNames.Add("SCIAC Multi-Duals");
            MeetNames.Add("Sundodger Invitational");
            MeetNames.Add("NCAA West Region Championship");
            Venues = new XList<Venue>();
            Venues.Add(new Venue("Milo McIver State Park", "Estacada", "OR"));
            Venues.Add(new Venue("Bush Pasture Park", "Salem", "OR"));
            Venues.Add(new Venue("Veteran's Memorial Golf Course", "Walla Walla", "WA"));
            Venues.Add(new Venue("Pomona College Campus", "Claremont", "CA"));
            Venues.Add(new Venue("Lincoln Park", "Seattle", "WA"));
            Meets = new XList<Meet>();
            Meets.Add(new Meet(MeetNames[0], new Date(2009, 9, 5), Venues[0],
                    new Race(null, 8000), new Race(null, 6000)));
            Meets.Add(new Meet(MeetNames[1], new Date(2009, 10, 1), Venues[1],
                    new Race(null, 8000), new Race(null, 5000)));
            Meets.Add(new Meet(MeetNames[2], new Date(2008, 11, 1), Venues[2],
                    new Race(null, 8000), new Race(null, 6000)));
            Meets.Add(new Meet(MeetNames[3], new Date(2009, 10, 15), Venues[3],
                    new Race(null, 8000), new Race(null, 6000)));
            Meets.Add(new Meet(MeetNames[4], new Date(2009, 9, 14), Venues[4],
                    new Race(null, 8000), new Race(null, 6000)));
            Meets.Add(new Meet(MeetNames[5], new Date(2008, 11, 15), Venues[1],
                    new Race(null, 8000), new Race(null, 6000)));
            Performances = new XList<Performance>();
            Performances.Add(new Performance(karl, Meets[4].MensRace, new Time(24*60+55)));
            Performances.Add(new Performance(karl, Meets[1].MensRace, new Time(24*60+44)));
            Performances.Add(new Performance(hannah, Meets[5].WomensRace, new Time(22*60+3)));
            GlobalState = new XcData(Affiliations, Meets, Performances, Runners, Schools);
            Races = GlobalState.Races;
        }
        
        abstract protected internal void SetUpPartial();        
        
        [TearDown]
        virtual public void TearDown ()
        {
            Writer.Dispose ();
            Reader.Dispose ();
        }
        
        abstract protected internal AbstractReader CreateExampleReader();
        
        abstract protected internal AbstractWriter CreateWriter();

        protected internal void RepeatTest<T> (TestMethod<T> Test, T original)
        {
            RepeatTest (3, Test, original);
        }
        
        protected internal void RepeatTest<T>(int number, TestMethod<T> Test, T original)
        {
            for (int i = 0; i < number; i++)
            {
                original = Test<T> (original);
                Writer.Dispose();
                Writer = CreateWriter();
            }
        }        
        
        [Test]
        virtual public void TestInitializeDatabase ()
        {
            SetUpPartial ();
            Writer.InitializeDatabase ();
        }
        
        [Test]
        virtual public void TestIsDatabaseInitialized()
        {
            SetUpPartial ();
            Assert.That (!Writer.IsDatabaseInitialized ());
            Writer.InitializeDatabase ();
            Assert.That (Writer.IsDatabaseInitialized ());
        }
        
        [Test]
        public void TestWrite ()
        {
            RepeatTest (Write, GlobalState);
        }
        
        [Test]
        public void TestWriteExample ()
        {
            AbstractReader exampleReader = CreateExampleReader ();
            XcData expected = exampleReader.Read ();
            Writer.Write (expected);
            XcData actual = Reader.Read ();
            Assert.That (TestXcaWriter.AreDataEqual (expected, actual));
        }
        
        virtual public XcData Write(XcData expected)
        {
            Writer.Write(expected);
            XcData actual = Reader.Read();
            Assert.IsNotEmpty((ICollection)actual.Affiliations);
            Assert.IsNotEmpty((ICollection)actual.Conferences);
            Assert.IsNotEmpty((ICollection)actual.Performances);
            Assert.IsNotEmpty((ICollection)actual.Meets);
            Assert.IsNotEmpty((ICollection)actual.Races);
            Assert.IsNotEmpty((ICollection)actual.Runners);
            Assert.IsNotEmpty((ICollection)actual.Schools);
            Assert.IsNotEmpty((ICollection)actual.Venues);
            Assert.That(TestXcaWriter.AreDataEqual(expected, actual));
            return actual;
        }
        
        [Test]
        virtual public void TestWriteAffiliations()
        {
            RepeatTest(WriteAffiliations, Affiliations);
        }
        
        public IXList<Affiliation> WriteAffiliations (
            IList<Affiliation> expected)
        {
            Writer.WriteConferences (Conferences);
            IDictionary<int, string> conferenceIds =
                new Dictionary<int, string> (Reader.ReadConferences ());
            Writer.WriteRunners (Runners);
            IDictionary<int, Runner> runnerIds =
                new Dictionary<int, Runner> (Reader.ReadRunners ());
            Writer.WriteSchools (Schools, Conferences);
            IDictionary<int, School> schoolIds = 
                new Dictionary<int, School> (Reader.ReadSchools (conferenceIds));
            Writer.WriteAffiliations (expected, Runners, Schools);
            IXList<Affiliation> actual =
                new XList<Affiliation>(
                    Reader.ReadAffiliations (runnerIds, schoolIds).Values);
            Assert.AreEqual (Affiliations.Count, actual.Count);
            foreach (Affiliation affiliation in Affiliations)
            {
                Assert.That (actual.Contains (affiliation));
            }
            return actual;      
        }
        
        [Test]
        virtual public void TestWriteConferences()
        {
            RepeatTest(WriteConferences, Conferences);
        }
        
        public IXList<string> WriteConferences (IXList<string> expected)
        {
            Writer.WriteConferences (expected);
            IXList<string> actual =
                new XList<string>(Reader.ReadConferences ().Values);
            Assert.AreEqual (Conferences.Count, actual.Count);
            foreach (string conference in Conferences) 
            {
                Assert.That (actual.Contains (conference));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWritePerformances()
        {
            RepeatTest(WritePerformances, Performances);
        }
        
        public IXList<Performance> WritePerformances (
            IXList<Performance> expected)
        {
            Writer.WriteRaces (Races);
            IDictionary<int, Race> raceIds = new Dictionary<int, Race> ();
            for (int i = 0; i < Races.Count; i++)
            {
                raceIds[i + 1] = Races[i];
            }
            Writer.WriteRunners (Runners);
            IDictionary<int, Runner> runnerIds = Reader.ReadRunners ();
            Writer.WritePerformances (expected, Races, Runners);
            IXList<Performance> actual =
                new XList<Performance> (Reader.ReadPerformances (raceIds, runnerIds).Values);
            Assert.AreEqual (Performances.Count, actual.Count);
            foreach (Performance performance in Performances)
            {
                Assert.That (actual.Contains (performance));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteMeets()
        {
            RepeatTest(WriteMeets, Meets);
        }
        
        public IXList<Meet> WriteMeets (IXList<Meet> expected)
        {
            Writer.WriteMeetNames (MeetNames);
            IDictionary<int, string> meetNameIds =
                new Dictionary<int, string> (Reader.ReadMeetNames ());
            Writer.WriteRaces (Races);
            IDictionary<int, Race> raceIds =
                new Dictionary<int, Race> (Reader.ReadRaces ());
            Writer.WriteVenues (Venues);
            IDictionary<int, Venue> venueIds =
                new Dictionary<int, Venue> (Reader.ReadVenues ());
            Writer.WriteMeets (expected, MeetNames, Races, Venues);
            IXList<Meet> actual =
                new XList<Meet> (
                    Reader.ReadMeets (meetNameIds, raceIds, venueIds).Values);
            Assert.AreEqual (Meets.Count, actual.Count);
            foreach (Meet meet in Meets)
            {
                Assert.That (actual.Contains (meet));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteMeetNames()
        {
            RepeatTest(WriteMeetNames, MeetNames);
        }
        
        public IXList<string> WriteMeetNames (IXList<string> expected)
        {
            Writer.WriteMeetNames (MeetNames);
            IXList<string> actual =
                new XList<string> (Reader.ReadMeetNames().Values);
            Assert.AreEqual(MeetNames.Count, actual.Count);
            foreach(string meet in MeetNames)
            {
                Assert.That(actual.Contains(meet));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteRaces()
        {
            RepeatTest(WriteRaces, Races);
        }
        
        public IXList<Race> WriteRaces (IXList<Race> expected)
        {
            Writer.WriteRaces (expected);
            IXList<Race> actual = new XList<Race> (Reader.ReadRaces ().Values);
            Assert.AreEqual (Races.Count, actual.Count);
            foreach (Race race in Races)
            {
                bool found = false;
                foreach (Race candidate in actual)
                {
                    if (race.Distance == candidate.Distance) 
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) 
                {
                    Assert.Fail ("Could not find " + race + " in the written results.");
                }
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteRunners()
        {
            RepeatTest(WriteRunners, Runners);
        }
        
        public IXList<Runner> WriteRunners (IXList<Runner> runners)
        {
            Writer.WriteRunners (Runners);
            IXList<Runner> actual =
                new XList<Runner> (Reader.ReadRunners ().Values);
            Assert.AreEqual (Runners.Count, actual.Count);
            foreach (Runner runner in Runners)
            {
                Assert.That (actual.Contains (runner));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteSchools()
        {
            RepeatTest(WriteSchools, Schools);
        }
        
        public IXList<School> WriteSchools (IXList<School> expected)
        {
            Writer.WriteConferences (Conferences);
            IDictionary<int, string> conferenceIds =
                new Dictionary<int, string> (Reader.ReadConferences ());
            Writer.WriteSchools (expected, Conferences);
            IXList<School> actual =
                new XList<School>(Reader.ReadSchools (conferenceIds).Values);
            Assert.AreEqual (Schools.Count, actual.Count);
            foreach (School school in Schools)
            {
                Assert.That (actual.Contains (school));
                foreach (string conference in Conferences)
                {
                    if (school.Conference != null && school.Conference.Equals (conference))
                    {
                        Assert.AreEqual (conference, ((School)actual[actual.IndexOf (school)]).Conference);
                        break;
                    }
                }
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteVenues()
        {
            RepeatTest(WriteVenues, Venues);
        }
        
        public IXList<Venue> WriteVenues (IXList<Venue> Venues)
        {
            Writer.WriteVenues (Venues);
            IXList<Venue> actual =
                new XList<Venue>(Reader.ReadVenues ().Values);
            Assert.AreEqual (Venues.Count, actual.Count);
            foreach (Venue venue in actual)
            {
                Assert.That (actual.Contains (venue));
            }
            return actual;
        }
    }
}