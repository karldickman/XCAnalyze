using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using XCAnalyze.Io.Sql.Tables;

namespace XCAnalyze.Io.Sql
{
    /// <summary>
    /// A writer to write all the data in the model to a database.
    /// </summary>
    abstract public class DatabaseWriter : IWriter<Model.GlobalState>
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
        protected internal DatabaseWriter (IDbConnection connection)
        {
            Connection = connection;
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
        /// Format the given boolean for insertion in an SQL query.
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
        /// Write data to the database.
        /// </summary>
        /// <param name="data">
        /// The <see cref="Data"/> to be written.
        /// </param>
        virtual public void Write (Model.GlobalState data)
        {
            throw new NotImplementedException ();
        }
        
        /// <summary>
        /// Write the list of conferences to the database.
        /// <param name="conferences">
        /// The <see cref="IList<Table.Conferences>"/> to write.
        /// </param>
        /// </summary>
        virtual public void WriteConferences (IList<Tables.Conference> conferences)
        {
            foreach (Conference conference in conferences)
            {
                if(conference.Id >= 0)
                {
                    Command.CommandText = "UPDATE conferences SET name = " + Format(conference.Name) + ", abbreviation = " + Format(conference.Abbreviation) + " WHERE id = " + ((Conference)conference).Id;
                }
                else
                {
                    Command.CommandText = "INSERT INTO conferences (name, abbreviation) VALUES (" + Format(conference.Name) + ", " + Format(conference.Abbreviation) + ")";
                }
                Command.ExecuteNonQuery();
            }
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
                if(runner is Runner)
                {
                    Command.CommandText = "UPDATE runners SET surname = " + Format(runner.Surname) + ", given_name = " + Format(runner.GivenName) + ", nicknames = " + Format(((Runner)runner).Nicknames) + ", gender = " + Format(runner.Gender) + ", year = " + Format(runner.Year) + " WHERE id = " + ((Runner)runner).Id;   
                }
                else
                {
                    Command.CommandText = "INSERT INTO runners (surname, given_name, gender, year) VALUES (" + Format(runner.Surname) + ", " + Format(runner.GivenName) + ", " + Format(runner.Gender) + ", " + Format(runner.Year) + ")";
                }
                Command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Write a list of schools to the database.
        /// </summary>
        /// <param name="schools">
        /// The <see cref="IList<Model.School>"/> to write.
        /// </param>
        virtual public void WriteSchools(IList<Model.School> schools)
        {
            int? conferenceId;
            School sqlSchool;
            foreach(Model.School school in schools)
            {
                if(school is School)
                {
                    sqlSchool = (School) school;
                    Command.CommandText = "UPDATE schools SET name = " + Format(sqlSchool.Name) + ", type = " + Format(sqlSchool.Type) + ", name_first = " + Format(sqlSchool.NameFirst) + ", nicknames = " + Format(sqlSchool.Nicknames) + ", conference_id = " + Format(sqlSchool.ConferenceId) + " WHERE id = " + sqlSchool.Id;
                }
                else
                {
                    conferenceId = Conference.GetId(school.Conference);
                    Command.CommandText = "INSERT INTO schools (name, type, name_first, conference_id) VALUES (" + Format(school.Name) + ", " + Format(school.Type) + ", " + Format(school.NameFirst) + ", " + Format(conferenceId) + ")";
                }
                Command.ExecuteNonQuery();
            }
        }
    }
    
    abstract public class TestDatabaseWriter
    {        
        /// <summary>
        /// The name of the test database.
        /// </summary>
        virtual public string TEST_DATABASE { get { return "xca_test"; } }

        /// <summary>
        /// A sample list of conferences.
        /// </summary>
        protected internal IList<Tables.Conference> Conferences { get; set; }
        
        /// <summary>
        /// The reader for the database
        /// </summary>
        protected internal DatabaseReader Reader { get; set; }
        
        /// <summary>
        /// A sample list of runners
        /// </summary>
        protected internal IList<Model.Runner> Runners { get; set; }
        
        /// <summary>
        /// A sample list of schools
        /// </summary>
        protected internal IList<Model.School> Schools { get; set; }
        
        /// <summary>
        /// The writer for the database.
        /// </summary>
        protected internal DatabaseWriter Writer { get; set; }
        
        [SetUp]
        virtual public void SetUp()
        {
            Conference nwc, sciac;
            nwc = new Conference ("Northwest Conference", "NWC");
            sciac = new Conference (
                "Southern California Intercollegiate Athletic Conference",
                "SCIAC");
            Conferences = new List<Tables.Conference> ();
            Conferences.Add (nwc);
            Conferences.Add (sciac);
            Conferences.Add (new Conference (
                "Southern Collegiate Athletic Conference", "SCAC"));
            Runners = new List<Model.Runner>();
            Runners.Add(new Model.Runner("Dickman", "Karl", Model.Gender.MALE, 2010));
            Runners.Add(new Model.Runner("Palmer", "Hannah", Model.Gender.FEMALE, 2010));
            Runners.Add(new Model.Runner("LeDonne", "Richie", Model.Gender.MALE, 2010));
            Runners.Add(new Model.Runner("Woodard", "Keith", Model.Gender.MALE, null));
            Schools = new List<Model.School>();
            Schools.Add(new Model.School("Lewis & Clark", "College", nwc.Name));
            Schools.Add(new Model.School("Willamette", "University", nwc.Name));
            Schools.Add(new Model.School("Puget Sound", "University", false, nwc.Name));
            Schools.Add(new Model.School("Claremont-Mudd-Scripps", null, sciac.Name));
            Schools.Add(new Model.School("Pomona-Pitzer", null, sciac.Name));
            Schools.Add(new Model.School("California", "Institute of Technology", sciac.Name));
            Schools.Add(new Model.School("California, Santa Cruz", "University", false));
        }
        
        abstract public void SetUpWriters();
        
        [TearDown]
        virtual public void TearDown ()
        {
            Writer.Close ();
            Reader.Close ();
        }  

        virtual public void TestInitializeDatabase ()
        {
            SetUpWriters ();
            Writer.InitializeDatabase ();
        }
        
        virtual public void TestIsDatabaseInitialized()
        {
            SetUpWriters ();
            Assert.That (!Writer.IsDatabaseInitialized ());
            Writer.InitializeDatabase ();
            Assert.That (Writer.IsDatabaseInitialized ());
        }
        
        virtual public void TestWrite()
        {
            Assert.Fail("Not yet implemented.");
        }
        
        virtual public void TestWriteConferences()
        {
            IList<Tables.Conference> actual;
            Writer.WriteConferences (Conferences);
            Reader.ReadConferences();
            actual = Conference.List;
            Assert.AreEqual(Conferences.Count, actual.Count);
            foreach (Tables.Conference conference in actual)
            {
                Assert.That (conference is Tables.Conference);
            }
            foreach (Conference conference in Conferences) 
            {
                Assert.That (actual.Contains (conference));
            }
            Conferences = actual;
            Conferences[0].Name = "XKCD";
            Conferences[1].Name = "SUCKS";
            Conferences[2].Name = "BALLS";
            Conferences[2].Abbreviation = "BLZ";
            Writer.WriteConferences (Conferences);
            Reader.ReadConferences();
            actual = Conference.List;
            Assert.AreEqual (Conferences.Count, actual.Count);
            foreach (Tables.Conference conference in Conferences) 
            {
                Assert.That (actual.Contains (conference));
            }
        }
        
        virtual public void TestWriteRunners()
        {
            IList<Model.Runner> actual;
            Writer.WriteRunners(Runners);
            Reader.ReadRunners();
            actual = Runner.List;
            Assert.AreEqual(Runners.Count, actual.Count);
            foreach(Model.Runner runner in actual)
            {
                Assert.That(runner is Runner);
            }
            foreach(Model.Runner runner in Runners)
            {
                Assert.That(actual.Contains(runner));
            }
            Runners = actual;
            ((Runner)Runners[3]).Nicknames = new string[] {"Beast"};
            Writer.WriteRunners(Runners);
            Reader.ReadRunners();
            actual = Runner.List;
            Assert.AreEqual(Runners.Count, actual.Count);
            Assert.AreEqual("Beast", ((Runner)actual[3]).Nicknames[0]);
            Assert.AreEqual(Runners.Count, actual.Count);
            foreach(Model.Runner runner in Runners)
            {
                Assert.That(actual.Contains(runner));
            }
        }
        
        virtual public void TestWriteSchools()
        {
            IList<Model.School> actual;
            Writer.WriteConferences(Conferences);
            Reader.ReadConferences();
            Writer.WriteSchools(Schools);
            Reader.ReadSchools();
            actual = School.List;
            Assert.AreEqual(Schools.Count, actual.Count);
            foreach(Model.School school in actual)
            {
                Assert.That(school is School);
            }
            foreach(Model.School school in Schools)
            {
                Console.WriteLine(actual[0].Conference);
                Console.WriteLine((bool)(actual[0] is School));
                Assert.That(actual.Contains(school));
                foreach(Conference conference in Conferences)
                {
                    if(school.Conference != null && school.Conference.Equals(conference.Name))
                    {
                        Assert.AreEqual(conference.Name, ((School)actual[actual.IndexOf(school)]).Conference);
                        break;
                    }
                }
            }
            Schools = actual;
            ((School)Schools[5]).Nicknames = new string[] {"Caltech"};
            ((School)Schools[6]).Nicknames = new string[] {"UCSC"};
            Writer.WriteSchools(Schools);
            Reader.ReadSchools();
            actual = School.List;
            Assert.AreEqual(Schools.Count, actual.Count);
            Assert.AreEqual("Caltech", ((School)actual[5]).Nicknames[0]);
            Assert.AreEqual("UCSC", ((School)actual[6]).Nicknames[0]);
            foreach(Model.School school in Schools)
            {
                Assert.That(actual.Contains(school));
            }
        }
        
        protected internal class Conference : Tables.Conference
        {
            public Conference (string name, string abbreviation)
            : base(-1, name, abbreviation) {}
        }
    }
}