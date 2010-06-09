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
    abstract public class DatabaseWriter : IWriter<Model.XcData>
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
        virtual public string CREATION_SCRIPT_EXTENSION
        {
            get { return ".sql"; }
        }
        
        /// <summary>
        /// The title of the column that has the names of all the tables.
        /// </summary>
        abstract public string GET_TABLES_COLUMN { get; }

        /// <summary>
        /// The script used to get the list of tables in the database.
        /// </summary>
        abstract public string GET_TABLES_COMMAND { get; }

        /// <summary>
        /// The tables that should be in the database.  These are in dependency
        /// order: tables later in the list have foreign keys referencing tables
        /// earlier in the list.
        /// </summary>
        public static readonly string[] TABLES = {"conferences", "runners",
        "schools", "affiliations", "meets", "venues", "races", "results"};        
        
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
        protected internal DatabaseReader DatabaseReader { get; set; }
        
        /// <summary>
        /// The <see cref="IDataReader"/> used to read responses from the
        /// database.
        /// </summary>
        protected internal IDataReader Reader { get; set; }
        
        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="command">
        /// The <see cref="IDbCommand"/> to use.
        /// </param>
        public DatabaseWriter (IDbConnection connection, string database,
            IDbCommand command)
        {
            Connection = connection;
            Database = database;
            Command = command;
        }
        
        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        protected internal DatabaseWriter(IDbConnection connection,
            string database) : this(connection, database, true) {}
        
        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database.
        /// </param>
        /// <param name="open">
        /// Should the connection be opened?
        /// </param>
        protected internal DatabaseWriter(IDbConnection connection,
            string database, bool open)
        {
            Connection = connection;
            Database = database;
            if(open)
            {
                Open();
            }
        }

        /// <summary>
        /// Close the connection to the database.
        /// </summary>
        public void Close ()
        {
            if (Reader != null)
            {
                Reader.Close ();
                Reader = null;
            }
            if (Command != null)
            {
                Command.Dispose ();
                Command = null;
            }
            Connection.Close ();
        }
        
        /// <summary>
        /// Get the script to create the database.
        /// </summary>
        public string CreationScript ()
        {
            return String.Join(" ", File.ReadAllLines(CREATION_SCRIPT));
        }

        /// <summary>
        /// Initialize all the tables in the database.
        /// </summary>
        virtual public void InitializeDatabase ()
        {
            Command.CommandText = CreationScript ();
            Command.ExecuteNonQuery ();
        }

        /// <summary>
        /// Check that all the required tables in the database exist.
        /// </summary>
        virtual public bool IsDatabaseInitialized ()
        {
            IList<string> foundTables = new List<string> ();
            Command.CommandText = GET_TABLES_COMMAND;
            Reader = Command.ExecuteReader ();
            while (Reader.Read ())
            {
                foundTables.Add ((string)Reader[GET_TABLES_COLUMN]);
            }
            Reader.Dispose ();
            if (foundTables.Count < TABLES.Length)
            {
                return false;
            }
            foreach(string table in TABLES)
            {
                if(!foundTables.Contains(table))
                {
                    return false;
                }
            }
            if(foundTables.Count == TABLES.Length)
            {
                return true;
            }
            if(foundTables.Count != TABLES.Length + VIEWS.Length)
            {
                return false;
            }
            foreach(string table in VIEWS)
            {
                if(!foundTables.Contains(table))
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Format a particular date for insertion in an SQL query.
        /// </summary>
        /// <param name="value_">
        /// The <see cref="Model.Date"/> to format.
        /// </param>
        virtual public string Format(Model.Date value_)
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
        virtual public string Format (bool value_)
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
        virtual public string Format(Model.Gender value_)
        {
            return Format(value_.ToString());
        }
        
        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        virtual public string Format(int? value_)
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
        virtual public string Format (string value_)
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
        virtual public string Format(string[] value_)
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
        virtual public string Format(Model.Time time)
        {
            return time.Seconds.ToString();
        }
        
        /// <summary>
        /// Do all the steps needed to open the connection.
        /// </summary>
        abstract public void Open();
        
        /// <summary>
        /// Write data to the database.
        /// </summary>
        /// <param name="data">
        /// The <see cref="Data"/> to be written.
        /// </param>
        virtual public void Write (Model.XcData data)
        {
            Tables.XcData sqlData = null;
            if(data is Tables.XcData)
            {
                sqlData = (Tables.XcData)data;
            }
            if(sqlData != null)
            {
                WriteConferences(sqlData.SqlConferences);
            }
            else
            {
                WriteConferences(data.Conferences);
            }
            WriteRunners(data.Runners);
            WriteSchools(data.Schools);
            WriteAffiliations(data.Affiliations);
            if(sqlData != null)
            {
                WriteMeetNames(sqlData.SqlMeetNames);
            }
            else
            {
                WriteMeetNames(data.MeetNames);
            }
            WriteVenues(data.Venues);
            WriteRaces(data.Races);
            WritePerformances(data.Performances);
        }
        
        /// <summary>
        /// Write the list of affiliations to the database.
        /// </summary>
        /// <param name="affiliations">
        /// The <see cref="IList<Model.Affiliation>"/> to write.
        /// </param>
        virtual public void WriteAffiliations(IList<Model.Affiliation> affiliations)
        {
            foreach(Model.Affiliation affiliation in affiliations)
            {
                Command.CommandText = "INSERT INTO affiliations (runner_id, school_id, year) VALUES (" + Tables.Runner.GetId(affiliation.Runner) + ", " + Tables.School.GetId(affiliation.School) + ", " + affiliation.Year + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Write the list of conferences to the database.
        /// <param name="conferences">
        /// The <see cref="IList<string>"/> to write.
        /// </param>
        /// </summary>
        virtual public void WriteConferences(IList<string> conferences)
        {
            foreach(string conference in conferences)
            {
                Command.CommandText = "INSERT INTO conferences (name) VALUES (" + Format(conference) + ")";
                Command.ExecuteNonQuery();
            }
            DatabaseReader.ReadConferences();
        }
        
        /// <summary>
        /// Write the list of conferences to the database.
        /// <param name="conferences">
        /// The <see cref="IList<Tables.Conferences>"/> to write.
        /// </param>
        /// </summary>
        virtual public void WriteConferences (IList<Tables.Conference> conferences)
        {
            foreach (Tables.Conference conference in conferences)
            {
                Command.CommandText = "INSERT INTO conferences (name, abbreviation) VALUES (" + Format(conference.Name) + ", " + Format(conference.Abbreviation) + ")";               
                Command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Write a list of meets to the database.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IList<System.String>"/> to write.
        /// </param>
        virtual public void WriteMeetNames(IList<string> meetNames)
        {
            foreach(string meet in meetNames)
            {
                Command.CommandText = "INSERT INTO meets (name) VALUES (" + Format(meet) + ")";
                Command.ExecuteNonQuery();
            }
            DatabaseReader.ReadMeetNames();
        }
        
        /// <summary>
        /// Write a list of meets to the database.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IList<Meet>"/> to write.
        /// </param>
        virtual public void WriteMeetNames(IList<Tables.MeetName> meetNames)
        {
            foreach(Tables.MeetName meet in meetNames)
            {
                Command.CommandText = "INSERT INTO meets (name) VALUES (" + Format(meet.Name) + ")";
                Command.ExecuteNonQuery();
            }
        }
       
        /// <summary>
        /// Write a list of performances to the database.
        /// </summary>
        /// <param name="performances">
        /// The <see cref="IList<Model.Performance>"/> to write.
        /// </param>
        virtual public void WritePerformances(IList<Model.Performance> performances)
        {
            foreach(Model.Performance performance in performances)
            {                
                Command.CommandText = "INSERT INTO results (runner_id, race_id, time) VALUES (" + Format(Tables.Runner.GetId(performance.Runner)) + ", " + Format(Tables.Race.GetId(performance.Race)) + ", " + Format(performance.Time) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Write a list of races to the database.
        /// </summary>
        /// <param name="races">
        /// The <see cref="IList<Model.Race>"/> to write.
        /// </param>
        virtual public void WriteRaces(IList<Model.Race> races)
        {
            foreach(Model.Race race in races)
            {
                Command.CommandText = "INSERT INTO races (meet_id, venue_id, date, gender, distance) VALUES (" + Format(Tables.MeetName.GetId(race.Name)) + ", " + Format(Tables.Venue.GetId(race.Venue)) + ", " + Format(race.Date) + ", " + Format(race.Gender) + ", " + race.Distance + ")";
                Command.ExecuteNonQuery();
            }
            DatabaseReader.ReadRaces();
        }
        
        /// <summary>
        /// Write the list of runners to the database.
        /// </summary>
        /// <param name="runners">
        /// The <see cref="IList<Model.Runner>"/> to write.
        /// </param>
        virtual public void WriteRunners(IList<Model.Runner> runners)
        {
            foreach(Model.Runner runner in runners)
            {
                Command.CommandText = "INSERT INTO runners (surname, given_name, gender, year) VALUES (" + Format(runner.Surname) + ", " + Format(runner.GivenName) + ", " + Format(runner.Gender) + ", " + Format(runner.Year) + ")";
                Command.ExecuteNonQuery();
            }
            DatabaseReader.ReadRunners();
        }
        
        /// <summary>
        /// Write a list of schools to the database.
        /// </summary>
        /// <param name="schools">
        /// The <see cref="IList<Model.School>"/> to write.
        /// </param>
        virtual public void WriteSchools(IList<Model.School> schools)
        {
            foreach(Model.School school in schools)
            {
                Command.CommandText = "INSERT INTO schools (name, type, name_first, conference_id) VALUES (" + Format(school.Name) + ", " + Format(school.Type) + ", " + Format(school.NameFirst) + ", " + Format(Tables.Conference.GetId(school.Conference)) + ")";
                Command.ExecuteNonQuery();
            }
            DatabaseReader.ReadSchools();
        }
        
        /// <summary>
        /// Write a list of venues to the database.
        /// </summary>
        /// <param name="venues">
        /// The <see cref="IList<System.String[]>"/> to write.
        /// </param>
        virtual public void WriteVenues(IList<Model.Venue> venues)
        {
            foreach(Model.Venue venue in venues)
            {
                Command.CommandText = "INSERT INTO venues (name, city, state) VALUES (" + Format(venue.Name) + ", " + Format(venue.City) + ", " + Format(venue.State) + ")";                
                Command.ExecuteNonQuery();
            }
            DatabaseReader.ReadVenues();
        }
    }
    
    abstract public class TestDatabaseWriter
    {        
        /// <summary>
        /// The name of the test database.
        /// </summary>
        virtual public string TEST_DATABASE { get { return "xca_test"; } }

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
        protected internal DatabaseReader Reader { get; set; }
        
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
        protected internal DatabaseWriter Writer { get; set; }
        
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
                    new Race(Model.Gender.MALE, 8000), new Race(Model.Gender.FEMALE, 6000)));
            Meets.Add(new Meet(MeetNames[1].Name, new Model.Date(2009, 10, 1), Venues[1],
                    new Race(Model.Gender.MALE, 8000), new Race(Model.Gender.FEMALE, 5000)));
            Meets.Add(new Meet(MeetNames[2].Name, new Model.Date(2008, 11, 1), Venues[2],
                    new Race(Model.Gender.MALE, 8000), new Race(Model.Gender.FEMALE, 6000)));
            Meets.Add(new Meet(MeetNames[3].Name, new Model.Date(2009, 10, 15), Venues[3],
                    new Race(Model.Gender.MALE, 8000), new Race(Model.Gender.FEMALE, 6000)));
            Meets.Add(new Meet(MeetNames[4].Name, new Model.Date(2009, 9, 14), Venues[4],
                    new Race(Model.Gender.MALE, 8000), new Race(Model.Gender.FEMALE, 6000)));
            Meets.Add(new Meet(MeetNames[5].Name, new Model.Date(2008, 11, 15), Venues[1],
                    new Race(Model.Gender.MALE, 8000), new Race(Model.Gender.FEMALE, 6000)));
            Performances = new List<Model.Performance>();
            Performances.Add(new Model.Performance(karl, Meets[4].MensRace, new Model.Time(24*60+55)));
            Performances.Add(new Model.Performance(karl, Meets[1].MensRace, new Model.Time(24*60+44)));
            Performances.Add(new Model.Performance(hannah, Meets[5].WomensRace, new Model.Time(22*60+3)));
            GlobalState = new Model.XcData(Affiliations, Meets, Performances, Runners, Schools, Venues);
            Races = GlobalState.Races;
        }
        
        abstract public void SetUpPartial();        
        
        [TearDown]
        virtual public void TearDown ()
        {
            Writer.Close ();
            Reader.Close ();
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
        virtual public void TestWrite()
        {
            Model.XcData actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.Write(GlobalState);
                actual = Reader.Read();
                Assert.That(actual is XcData);
                Assert.IsNotEmpty((ICollection)actual.Affiliations);
                Assert.IsNotEmpty((ICollection)actual.Conferences);
                Assert.IsNotEmpty((ICollection)actual.Performances);
                Assert.IsNotEmpty((ICollection)actual.Meets);
                Assert.IsNotEmpty((ICollection)actual.Races);
                Assert.IsNotEmpty((ICollection)actual.Runners);
                Assert.IsNotEmpty((ICollection)actual.Schools);
                Assert.IsNotEmpty((ICollection)actual.Venues);
                Assert.That(TestXcaWriter.AreDataEqual(GlobalState, actual));
                Writer.Close();
                Writer.Open();
            }
        }
        
        [Test]
        virtual public void TestWriteAffiliations()
        {
            IList<Model.Affiliation> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteConferences(Conferences);
                Reader.ReadConferences();
                Writer.WriteRunners(Runners);
                Reader.ReadRunners();
                Writer.WriteSchools(Schools);
                Reader.ReadSchools();
                Writer.WriteAffiliations(Affiliations);
                Reader.ReadAffiliations();
                actual = Tables.Affiliation.List;
                Assert.AreEqual(Affiliations.Count, actual.Count);
                foreach(Model.Affiliation affiliation in actual)
                {
                    Assert.That(affiliation is Affiliation);
                }
                foreach(Model.Affiliation affiliation in Affiliations)
                {
                    Assert.That(actual.Contains(affiliation));
                }
                Writer.Close();
                Writer.Open();
                Affiliations = actual;
            }
        }
        
        [Test]
        virtual public void TestWriteConferences()
        {
            IList<Tables.Conference> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteConferences (Conferences);
                Reader.ReadConferences();
                actual = Tables.Conference.List;
                Assert.AreEqual(Conferences.Count, actual.Count);
                foreach (Tables.Conference conference in Conferences) 
                {
                    Assert.That (actual.Contains (conference));
                }
                Writer.Close();
                Writer.Open();
                Conferences = actual;
            }
        }
        
        [Test]
        virtual public void TestWritePerformances()
        {
            IList<Model.Performance> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteMeetNames(MeetNames);
                Reader.ReadMeetNames();
                Writer.WriteVenues(Venues);
                Reader.ReadVenues();
                Writer.WriteRaces(Races);
                Reader.ReadRaces();
                Writer.WriteRunners(Runners);
                Reader.ReadRunners();
                Writer.WritePerformances(Performances);
                Reader.ReadPerformances();
                actual = Tables.Performance.List;
                Assert.AreEqual(Performances.Count, actual.Count);
                foreach(Model.Performance performance in actual)
                {
                    Assert.That(performance is Performance);
                }
                foreach(Model.Performance performance in Performances)
                {
                    Assert.That(actual.Contains(performance));
                }
                Writer.Close();
                Writer.Open();
                Performances = actual;
            }
        }
        
        [Test]
        virtual public void TestWriteMeetNames()
        {
            IList<Tables.MeetName> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteMeetNames(MeetNames);
                Reader.ReadMeetNames();
                actual = Tables.MeetName.List;
                Assert.AreEqual(MeetNames.Count, actual.Count);
                foreach(Tables.MeetName meet in MeetNames)
                {
                    Assert.That(actual.Contains(meet));
                }
                Writer.Close();
                Writer.Open();
                MeetNames = actual;
            }
        }
        
        [Test]
        virtual public void TestWriteRaces()
        {
            IList<Model.Race> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteMeetNames(MeetNames);
                Reader.ReadMeetNames();
                Writer.WriteVenues(Venues);
                Reader.ReadVenues();
                Writer.WriteRaces(Races);
                Reader.ReadRaces();
                actual = Tables.Race.List;
                Assert.AreEqual(Races.Count, actual.Count);
                foreach(Race race in actual)
                {
                    Assert.That(race is Tables.Race);
                }
                foreach(Model.Race race in Races)
                {
                    Assert.That(actual.Contains(race));
                }
                Writer.Close();
                Writer.Open();
                Races = actual;
            }
        }
        
        [Test]
        virtual public void TestWriteRunners()
        {
            IList<Model.Runner> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteRunners(Runners);
                Reader.ReadRunners();
                actual = Tables.Runner.List;
                Assert.AreEqual(Runners.Count, actual.Count);
                foreach(Model.Runner runner in actual)
                {
                    Assert.That(runner is Tables.Runner);
                }
                foreach(Model.Runner runner in Runners)
                {
                    Assert.That(actual.Contains(runner));
                }
                Writer.Close();
                Writer.Open();
                Runners = actual;
            }
        }
        
        [Test]
        virtual public void TestWriteSchools()
        {
            IList<Model.School> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteConferences(Conferences);
                Reader.ReadConferences();
                Writer.WriteSchools(Schools);
                Reader.ReadSchools();
                actual = Tables.School.List;
                Assert.AreEqual(Schools.Count, actual.Count);
                foreach(Model.School school in actual)
                {
                    Assert.That(school is School);
                }
                foreach(Model.School school in Schools)
                {
                    Assert.That(actual.Contains(school));
                    foreach(Tables.Conference conference in Conferences)
                    {
                        if(school.Conference != null && school.Conference.Equals(conference.Name))
                        {
                            Assert.AreEqual(conference.Name, ((School)actual[actual.IndexOf(school)]).Conference);
                            break;
                        }
                    }
                }
                Writer.Close();
                Writer.Open();
                Schools = actual;
            }
        }
        
        [Test]
        virtual public void TestWriteVenues()
        {
            IList<Venue> actual;
            for(int i = 0; i < 3; i++)
            {
                Writer.WriteVenues(Venues);
                Reader.ReadVenues();
                actual = Tables.Venue.List;
                Assert.AreEqual(Venues.Count, actual.Count);
                foreach(Venue venue in Venues)
                {
                    Assert.That(actual.Contains(venue));
                }
                Writer.Close();
                Writer.Open();
                Venues = actual;
            }
        }
    }
}