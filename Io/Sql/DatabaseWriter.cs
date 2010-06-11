using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{   
    /// <summary>
    /// A writer to write all the data in the model to a database.
    /// </summary>
    abstract public class DatabaseWriter : BaseDatabaseWriter
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
        
        public DatabaseWriter(IDbConnection connection, string database)
            : base(connection, database) {}
        
        public DatabaseWriter(IDbConnection connection, string database,
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
        /// The <see cref="Model.Date"/> to format.
        /// </param>
        public string Format(Model.Date value_)
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
        public string Format(Model.Gender value_)
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
        /// The <see cref="Model.Time"/> to format.
        /// </param>
        public string Format (Model.Time time)
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
        
        override public void WriteAffiliations(IList<Model.Affiliation> affiliations)
        {
            foreach(Model.Affiliation affiliation in affiliations)
            {
                Command.CommandText = "INSERT INTO affiliations (runner_id, school_id, year) VALUES (" + Tables.Runner.GetId(affiliation.Runner) + ", " + Tables.School.GetId(affiliation.School) + ", " + affiliation.Year + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteConferences(IList<string> conferences)
        {
            foreach(string conference in conferences)
            {
                Command.CommandText = "INSERT INTO conferences (name) VALUES (" + Format(conference) + ")";
                Command.ExecuteNonQuery();
            }
            Reader.ReadConferences();
        }
        
        override public void WriteConferences (IList<Tables.Conference> conferences)
        {
            foreach (Tables.Conference conference in conferences)
            {
                Command.CommandText = "INSERT INTO conferences (name, abbreviation) VALUES (" + Format(conference.Name) + ", " + Format(conference.Abbreviation) + ")";               
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteMeetNames(IList<string> meetNames)
        {
            foreach(string meet in meetNames)
            {
                Command.CommandText = "INSERT INTO meets (name) VALUES (" + Format(meet) + ")";
                Command.ExecuteNonQuery();
            }
            Reader.ReadMeetNames();
        }
        
        override public void WriteMeetNames(IList<Tables.MeetName> meetNames)
        {
            foreach(Tables.MeetName meet in meetNames)
            {
                Command.CommandText = "INSERT INTO meets (name) VALUES (" + Format(meet.Name) + ")";
                Command.ExecuteNonQuery();
            }
        }
       
        override public void WritePerformances(IList<Model.Performance> performances)
        {
            foreach(Model.Performance performance in performances)
            {                
                Command.CommandText = "INSERT INTO results (runner_id, race_id, time) VALUES (" + Format(Tables.Runner.GetId(performance.Runner)) + ", " + Format(Tables.Race.GetId(performance.Race)) + ", " + Format(performance.Time) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteRaces(IList<Model.Race> races)
        {
            foreach(Model.Race race in races)
            {
                Command.CommandText = "INSERT INTO races (meet_id, venue_id, date, gender, distance) VALUES (" + Format(Tables.MeetName.GetId(race.Name)) + ", " + Format(Tables.Venue.GetId(race.Venue)) + ", " + Format(race.Date) + ", " + Format(race.Gender) + ", " + race.Distance + ")";
                Command.ExecuteNonQuery();
            }
            Reader.ReadRaces();
        }
        
        override public void WriteRunners(IList<Model.Runner> runners)
        {
            foreach(Model.Runner runner in runners)
            {
                Command.CommandText = "INSERT INTO runners (surname, given_name, gender, year) VALUES (" + Format(runner.Surname) + ", " + Format(runner.GivenName) + ", " + Format(runner.Gender) + ", " + Format(runner.Year) + ")";
                Command.ExecuteNonQuery();
            }
            Reader.ReadRunners();
        }
        
        override public void WriteSchools(IList<Model.School> schools)
        {
            foreach(Model.School school in schools)
            {
                Command.CommandText = "INSERT INTO schools (name, type, name_first, conference_id) VALUES (" + Format(school.Name) + ", " + Format(school.Type) + ", " + Format(school.NameFirst) + ", " + Format(Tables.Conference.GetId(school.Conference)) + ")";
                Command.ExecuteNonQuery();
            }
            Reader.ReadSchools();
        }
        
        override public void WriteVenues(IList<Model.Venue> venues)
        {
            foreach(Model.Venue venue in venues)
            {
                Command.CommandText = "INSERT INTO venues (name, city, state) VALUES (" + Format(venue.Name) + ", " + Format(venue.City) + ", " + Format(venue.State) + ")";                
                Command.ExecuteNonQuery();
            }
            Reader.ReadVenues();
        }
    }
    
    abstract public class TestDatabaseWriter
    {
        /// <summary>
        /// A delegate for test methods.
        /// </summary>
        protected internal delegate T TestMethod<T>(T value_);
        
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
        protected internal IList<Model.Affiliation> Affiliations { get; set; }
        
        /// <summary>
        /// A sample list of conferences.
        /// </summary>
        protected internal IList<Tables.Conference> Conferences { get; set; }
        
        /// <summary>
        /// A sample global state.
        /// </summary>
        protected internal Model.XcData GlobalState { get; set; }
        
        /// <summary>
        /// A sample list of meet names.
        /// </summary>
        protected internal IList<Tables.MeetName> MeetNames { get; set; }
        
        /// <summary>
        /// A sample list of meets.
        /// </summary>
        protected internal IList<Meet> Meets { get; set; }
        
        /// <summary>
        /// A sample list of performances.
        /// </summary>
        protected internal IList<Model.Performance> Performances { get; set; }
        
        /// <summary>
        /// A sample list of races.
        /// </summary>
        protected internal IList<Race> Races { get; set; }
        
        /// <summary>
        /// The reader for the database.
        /// </summary>
        protected internal BaseDatabaseReader Reader { get; set; }
        
        /// <summary>
        /// A sample list of runners.
        /// </summary>
        protected internal IList<Model.Runner> Runners { get; set; }
        
        /// <summary>
        /// A sample list of schools.
        /// </summary>
        protected internal IList<Model.School> Schools { get; set; }
        
        /// <summary>
        /// A sample list of venues.
        /// </summary>
        protected internal IList<Model.Venue> Venues { get; set; }
        
        /// <summary>
        /// The writer for the database.
        /// </summary>
        protected internal BaseDatabaseWriter Writer { get; set; }
        
        [SetUp]
        virtual public void SetUp()
        {
            Tables.Conference nwc, sciac;
            int y;
            Model.Runner karl, hannah, richie, keith, leo, francis, florian;
            nwc = new Tables.Conference (-1, "Northwest Conference", "NWC");
            sciac = new Tables.Conference (-1, 
                "Southern California Intercollegiate Athletic Conference",
                "SCIAC");
            Conferences = new List<Tables.Conference> ();
            Conferences.Add (nwc);
            Conferences.Add (sciac);
            Conferences.Add (new Tables.Conference (-1, 
                "Southern Collegiate Athletic Conference", "SCAC"));
            Runners = new List<Model.Runner>();
            karl = new Model.Runner("Dickman", "Karl", Model.Gender.MALE, 2010);
            hannah = new Model.Runner("Palmer", "Hannah", Model.Gender.FEMALE, 2010);
            richie = new Model.Runner("LeDonne", "Richie", Model.Gender.MALE, 2011);
            keith = new Model.Runner("Woodard", "Keith", Model.Gender.MALE, null);
            leo = new Model.Runner("Castillo", "Leo", Model.Gender.MALE, 2012);
            francis = new Model.Runner("Reynolds", "Francis", Model.Gender.MALE, 2010);
            florian = new Model.Runner("Scheulen", "Florian", Model.Gender.MALE, 2010);
            Runners.Add(karl);
            Runners.Add(hannah);
            Runners.Add(richie);
            Runners.Add(keith);
            Runners.Add(leo);
            Runners.Add(francis);
            Runners.Add(florian);
            Schools = new List<Model.School>();
            Schools.Add(new Model.School("Lewis & Clark", "College", nwc.Name));
            Schools.Add(new Model.School("Willamette", "University", nwc.Name));
            Schools.Add(new Model.School("Puget Sound", "University", false, nwc.Name));
            Schools.Add(new Model.School("Claremont-Mudd-Scripps", null, sciac.Name));
            Schools.Add(new Model.School("Pomona-Pitzer", null, sciac.Name));
            Schools.Add(new Model.School("California", "Institute of Technology", sciac.Name));
            Schools.Add(new Model.School("California, Santa Cruz", "University", false));
            Affiliations = new List<Model.Affiliation>();
            for(y = 2006; y < 2010; y++)
            {
                Affiliations.Add(new Model.Affiliation(karl, Schools[0], y));
            }
            for(y = 2007; y < 2009; y++)
            {
                Affiliations.Add(new Model.Affiliation(richie, Schools[0], y));
            }
            Affiliations.Add(new Model.Affiliation(hannah, Schools[0], 2006));
            Affiliations.Add(new Model.Affiliation(hannah, Schools[0], 2008));
            Affiliations.Add(new Model.Affiliation(hannah, Schools[0], 2009));
            Affiliations.Add(new Model.Affiliation(keith, Schools[0], 1969));
            Affiliations.Add(new Model.Affiliation(keith, Schools[0], 1970));
            Affiliations.Add(new Model.Affiliation(keith, Schools[0], 1971));
            Affiliations.Add(new Model.Affiliation(keith, Schools[0], 1989));
            for(y = 2008; y < 2010; y++) {
                Affiliations.Add(new Model.Affiliation(leo, Schools[1], y));
            }
            for(y = 2006; y < 2010; y++)
            {
                Affiliations.Add(new Model.Affiliation(francis, Schools[2], y));
            }
            for(y = 2006; y < 2010; y++)
            {
                Affiliations.Add(new Model.Affiliation(florian, Schools[3], y));
            }
            MeetNames = new List<Tables.MeetName>();
            MeetNames.Add(new Tables.MeetName(-1, "Lewis & Clark Invitational"));
            MeetNames.Add(new Tables.MeetName(-1, "Charles Bowles Invitational"));
            MeetNames.Add(new Tables.MeetName(-1, "Northwest Conference Championship"));
            MeetNames.Add(new Tables.MeetName(-1, "SCIAC Multi-Duals"));
            MeetNames.Add(new Tables.MeetName(-1, "Sundodger Invitational"));
            MeetNames.Add(new Tables.MeetName(-1, "NCAA West Region Championship"));
            Venues = new List<Venue>();
            Venues.Add(new Venue("Milo McIver State Park", "Estacada", "OR"));
            Venues.Add(new Venue("Bush Pasture Park", "Salem", "OR"));
            Venues.Add(new Venue("Veteran's Memorial Golf Course", "Walla Walla", "WA"));
            Venues.Add(new Venue("Pomona College Campus", "Claremont", "CA"));
            Venues.Add(new Venue("Lincoln Park", "Seattle", "WA"));
            Meets = new List<Model.Meet>();
            Meets.Add(new Meet(MeetNames[0].Name, new Model.Date(2009, 9, 5), Venues[0],
                    new Race(8000), new Race(6000)));
            Meets.Add(new Meet(MeetNames[1].Name, new Model.Date(2009, 10, 1), Venues[1],
                    new Race(8000), new Race(5000)));
            Meets.Add(new Meet(MeetNames[2].Name, new Model.Date(2008, 11, 1), Venues[2],
                    new Race(8000), new Race(6000)));
            Meets.Add(new Meet(MeetNames[3].Name, new Model.Date(2009, 10, 15), Venues[3],
                    new Race(8000), new Race(6000)));
            Meets.Add(new Meet(MeetNames[4].Name, new Model.Date(2009, 9, 14), Venues[4],
                    new Race(8000), new Race(6000)));
            Meets.Add(new Meet(MeetNames[5].Name, new Model.Date(2008, 11, 15), Venues[1],
                    new Race(8000), new Race(6000)));
            Performances = new List<Model.Performance>();
            Performances.Add(new Model.Performance(karl, Meets[4].MensRace, new Model.Time(24*60+55)));
            Performances.Add(new Model.Performance(karl, Meets[1].MensRace, new Model.Time(24*60+44)));
            Performances.Add(new Model.Performance(hannah, Meets[5].WomensRace, new Model.Time(22*60+3)));
            GlobalState = new Model.XcData(Affiliations, Meets, Performances, Runners, Schools, Venues);
            Races = GlobalState.Races;
        }
        
        abstract protected internal void SetUpPartial();        
        
        [TearDown]
        virtual public void TearDown ()
        {
            Writer.Dispose ();
            Reader.Dispose ();
        }
        
        abstract protected internal BaseDatabaseWriter CreateWriter();

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
        
        virtual public XcData Write(XcData expected)
        {
            Writer.Write(expected);
            Model.XcData actual = Reader.Read();
            Assert.That(actual is Tables.XcData);
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
        
        public IList<Affiliation> WriteAffiliations (IList<Affiliation> expected)
        {
            Writer.WriteConferences (Conferences);
            Reader.ReadConferences ();
            Writer.WriteRunners (Runners);
            Reader.ReadRunners ();
            Writer.WriteSchools (Schools);
            Reader.ReadSchools ();
            Writer.WriteAffiliations (expected);
            Reader.ReadAffiliations ();
            IList<Affiliation> actual = Tables.Affiliation.List;
            Assert.AreEqual (Affiliations.Count, actual.Count);
            foreach (Model.Affiliation affiliation in actual)
            {
                Assert.That (affiliation is Tables.Affiliation);
            }
            foreach (Model.Affiliation affiliation in Affiliations)
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
        
        public IList<Tables.Conference> WriteConferences (IList<Tables.Conference> expected)
        {
            Writer.WriteConferences (expected);
            Reader.ReadConferences ();
            IList<Tables.Conference> actual = Tables.Conference.List;
            Assert.AreEqual (Conferences.Count, actual.Count);
            foreach (Tables.Conference conference in Conferences) 
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
        
        public IList<Performance> WritePerformances(IList<Performance> expected)
        {
            Writer.WriteMeetNames(MeetNames);
            Reader.ReadMeetNames();
            Writer.WriteVenues(Venues);
            Reader.ReadVenues();
            Writer.WriteRaces(Races);
            Reader.ReadRaces();
            Writer.WriteRunners(Runners);
            Reader.ReadRunners();
            Writer.WritePerformances(expected);
            Reader.ReadPerformances();
            IList<Model.Performance> actual = Tables.Performance.List;
            Assert.AreEqual(Performances.Count, actual.Count);
            foreach(Model.Performance performance in actual)
            {
                Assert.That(performance is Tables.Performance);
            }
            foreach(Model.Performance performance in Performances)
            {
                Assert.That(actual.Contains(performance));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteMeetNames()
        {
            RepeatTest(WriteMeetNames, MeetNames);
        }
        
        public IList<Tables.MeetName> WriteMeetNames(IList<Tables.MeetName> expected)
        {
            Writer.WriteMeetNames(MeetNames);
            Reader.ReadMeetNames();
            IList<Tables.MeetName>actual = Tables.MeetName.List;
            Assert.AreEqual(MeetNames.Count, actual.Count);
            foreach(Tables.MeetName meet in MeetNames)
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
        
        public IList<Race> WriteRaces (IList<Race> expected)
        {
            Writer.WriteMeetNames (MeetNames);
            Reader.ReadMeetNames ();
            Writer.WriteVenues (Venues);
            Reader.ReadVenues ();
            Writer.WriteRaces (expected);
            Reader.ReadRaces ();
            IList<Race> actual = Tables.Race.List;
            Assert.AreEqual (Races.Count, actual.Count);
            foreach (Race race in actual)
            {
                Assert.That (race is Tables.Race);
            }
            foreach (Model.Race race in Races)
            {
                Assert.That (actual.Contains (race));
            }
            return actual;
        }
        
        [Test]
        virtual public void TestWriteRunners()
        {
            RepeatTest(WriteRunners, Runners);
        }
        
        public IList<Runner> WriteRunners (IList<Runner> runners)
        {
            Writer.WriteRunners (Runners);
            Reader.ReadRunners ();
            IList<Runner> actual = Tables.Runner.List;
            Assert.AreEqual (Runners.Count, actual.Count);
            foreach (Model.Runner runner in actual)
            {
                Assert.That (runner is Tables.Runner);
            }
            foreach (Model.Runner runner in Runners)
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
        
        public IList<School> WriteSchools (IList<School> expected)
        {
            Writer.WriteConferences (Conferences);
            Reader.ReadConferences ();
            Writer.WriteSchools (expected);
            Reader.ReadSchools ();
            IList<School> actual = Tables.School.List;
            Assert.AreEqual (Schools.Count, actual.Count);
            foreach (Model.School school in actual)
            {
                Assert.That (school is Tables.School);
            }
            foreach (Model.School school in Schools)
            {
                Assert.That (actual.Contains (school));
                foreach (Tables.Conference conference in Conferences)
                {
                    if (school.Conference != null && school.Conference.Equals (conference.Name))
                    {
                        Assert.AreEqual (conference.Name, ((School)actual[actual.IndexOf (school)]).Conference);
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
        
        public IList<Venue> WriteVenues (IList<Venue> Venues)
        {
            Writer.WriteVenues (Venues);
            Reader.ReadVenues ();
            IList<Venue> actual = Tables.Venue.List;
            Assert.AreEqual (Venues.Count, actual.Count);
            foreach (Venue venue in actual)
            {
                Assert.That (venue is Tables.Venue);
            }
            foreach (Venue venue in actual)
            {
                Assert.That (actual.Contains (venue));
            }
            return actual;
        }
    }
}