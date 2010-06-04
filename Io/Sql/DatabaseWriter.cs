using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{
    abstract public class DatabaseWriter : IWriter<Data>
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
        virtual public void Write (Data data)
        {
            throw new NotImplementedException ();
        }
        
        /// <summary>
        /// Write the list of conferences to the database.
        /// </summary>
        virtual public void WriteConferences (IList<SqlConference> conferences)
        {
            foreach (SqlConference conference in conferences)
            {
                if(conference.Id >= 0)
                {
                    Command.CommandText = "UPDATE conferences SET name = " + Format(conference.Name) + ", abbreviation = " + Format(conference.Abbreviation) + " WHERE id = " + ((SqlConference)conference).Id;
                }
                else
                {
                    Command.CommandText = "INSERT INTO conferences (name, abbreviation) VALUES (" + Format(conference.Name) + ", " + Format(conference.Abbreviation) + ")";
                }
                Command.ExecuteNonQuery();
            }
        }
        
        virtual public void WriteRunners(IList<Runner> runners)
        {
            string nicknames, year;
            foreach(Runner runner in runners)
            {
                if(runner.Year == null)
                {
                    year = "NULL";
                }
                else
                {
                    year = runner.Year.ToString();
                }
                if(runner is SqlRunner)
                {
                    if(((SqlRunner)runner).Nicknames == null)
                    {
                        nicknames = "NULL";
                    }
                    else
                    {
                        nicknames = "\"" + String.Join(", ", ((SqlRunner)runner).Nicknames) + "\"";
                    }
                    Command.CommandText = "UPDATE runners SET surname = \"" + runner.Surname + "\", given_name = \"" + runner.GivenName + "\", nicknames = " + nicknames + ", gender = \"" + runner.Gender + "\", year = " + year + " WHERE id = " + ((SqlRunner)runner).Id;   
                }
                else
                {
                    Command.CommandText = "INSERT INTO runners (surname, given_name, gender, year) VALUES (\"" + runner.Surname + "\", \"" + runner.GivenName + "\", \"" + runner.Gender + "\", " + year + ")";
                }
                Command.ExecuteNonQuery();
            }
        }
        
        virtual public void WriteSchools(IList<School> schools)
        {
            int? conferenceId;
            SqlSchool sqlSchool;
            foreach(School school in schools)
            {
                if(school is SqlSchool)
                {
                    sqlSchool = (SqlSchool) school;
                    Command.CommandText = "UPDATE schools SET name = " + Format(sqlSchool.Name) + ", type = " + Format(sqlSchool.Type) + ", name_first = " + Format(sqlSchool.NameFirst) + ", nicknames = " + Format(sqlSchool.Nicknames) + ", conference_id = " + Format(sqlSchool.ConferenceId) + " WHERE id = " + sqlSchool.Id;
                }
                else
                {
                    conferenceId = SqlConference.GetId(school.Conference);
                    Command.CommandText = "INSERT INTO schools (name, type, name_first, conference_id) VALUES (" + Format(school.Name) + ", " + Format(school.Type) + ", " + Format(school.NameFirst) + ", " + Format(conferenceId) + ")";
                }
                Command.ExecuteNonQuery();
            }
        }
    }
    
    public class MySqlDatabaseWriter : DatabaseWriter
    {        
        override public string CREATION_SCRIPT_EXTENSION
        {
            get { return "mysql"; }
        }
        
        override public string GET_TABLES_COLUMN
        {
            get { return "Tables_in_" + Database; }
        }
        
        override public string GET_TABLES_COMMAND
        {
            get { return "SHOW TABLES"; }
        }
        
        protected internal string Database { get; set; }
        
        protected internal MySqlDatabaseWriter(IDbConnection connection,
            string database) : base(connection)
        {
            Database = database;
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user)
        {
            return NewInstance (host, database, user, user);
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password)
        {
            return NewInstance (host, database, user, password, 3306);
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password, int port)
        {
            return NewInstance (host, database, user, password, port, false);
        }
        
        public static MySqlDatabaseWriter NewInstance (string host,
            string database, string user, string password, int port,
            bool pooling)
        {
            string connectionString = "Server=" + host + "; User ID=" + user + "; Password=" + password + "; Pooling=" + pooling + ";";
            return NewInstance (new MySqlConnection(connectionString), database);
        }
        
        public static MySqlDatabaseWriter NewInstance (IDbConnection connection,
            string database)
        {
            string fullConnectionString = connection.ConnectionString + "Database=" + database;
            MySqlDatabaseWriter writer = new MySqlDatabaseWriter (connection, database);
            connection.Open ();
            writer.Command = connection.CreateCommand ();
            writer.Command.CommandText = "USE " + database;
            try
            {
                writer.Command.ExecuteNonQuery ();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine (exception.Message);
                Console.WriteLine ("Creating database " + database);
                writer.Command.CommandText = "CREATE DATABASE " + database;
                writer.Command.ExecuteNonQuery ();
            }
            writer.Close ();
            writer = new MySqlDatabaseWriter (new MySqlConnection (fullConnectionString), database);
            writer.Connection.Open ();
            writer.Command = writer.Connection.CreateCommand ();
            if (!writer.IsDatabaseInitialized ()) 
            {
                writer.InitializeDatabase ();
            }
            return writer;
        }
        
        override public void InitializeDatabase()
        {
            throw new NotImplementedException();
        }
    }  
   
    public class SqliteDatabaseWriter : DatabaseWriter
    {
        override public string CREATION_SCRIPT_EXTENSION
        {
            get
            {
                return "sqlite";
            }
        }
        
        override public string GET_TABLES_COLUMN
        {
             get { return "name"; }
        }

        override public string GET_TABLES_COMMAND
        {
            get { return "SELECT name FROM sqlite_master WHERE type=\"table\""; }
        }

        protected internal SqliteDatabaseWriter(IDbConnection connection)
            : base(connection) {}

        /// <summary>
        /// Create a new SqliteDatabaseWriter using an in-memory database.
        /// </summary>
        public static SqliteDatabaseWriter NewInstance ()
        {
            return NewInstance (":memory:");
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using a specific database file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to connect to.
        /// </param>
        public static SqliteDatabaseWriter NewInstance (string fileName)
        {
            return NewInstance (
                new SqliteConnection ("Data Source=" + fileName));
        }

        /// <summary>
        /// Create a new SqliteDatabaseWriter using the given connection.
        /// </summary>
        public static SqliteDatabaseWriter NewInstance (IDbConnection connection)
        {
            SqliteDatabaseWriter writer = new SqliteDatabaseWriter (connection);
            writer.Connection.Open ();
            writer.Command = writer.Connection.CreateCommand ();
            if (!writer.IsDatabaseInitialized ())
            {
                writer.InitializeDatabase ();
            }
            return writer;
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
        protected internal IList<SqlConference> Conferences { get; set; }
        
        /// <summary>
        /// The reader for the database
        /// </summary>
        protected internal DatabaseReader Reader { get; set; }
        
        /// <summary>
        /// A sample list of runners
        /// </summary>
        protected internal IList<Runner> Runners { get; set; }
        
        /// <summary>
        /// A sample list of schools
        /// </summary>
        protected internal IList<School> Schools { get; set; }
        
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
            Conferences = new List<SqlConference> ();
            Conferences.Add (nwc);
            Conferences.Add (sciac);
            Conferences.Add (new Conference (
                "Southern Collegiate Athletic Conference", "SCAC"));
            Runners = new List<Runner>();
            Runners.Add(new Runner("Dickman", "Karl", Gender.MALE, 2010));
            Runners.Add(new Runner("Palmer", "Hannah", Gender.FEMALE, 2010));
            Runners.Add(new Runner("LeDonne", "Richie", Gender.MALE, 2010));
            Runners.Add(new Runner("Woodard", "Keith", Gender.MALE, null));
            Schools = new List<School>();
            Schools.Add(new School("Lewis & Clark", "College", nwc.Name));
            Schools.Add(new School("Willamette", "University", nwc.Name));
            Schools.Add(new School("Puget Sound", "University", false, nwc.Name));
            Schools.Add(new School("Claremont-Mudd-Scripps", null, sciac.Name));
            Schools.Add(new School("Pomona-Pitzer", null, sciac.Name));
            Schools.Add(new School("California", "Institute of Technology", sciac.Name));
            Schools.Add(new School("California, Santa Cruz", "University", false));
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
            IList<SqlConference> actual;
            Writer.WriteConferences (Conferences);
            Reader.ReadConferences();
            actual = SqlConference.List;
            Assert.AreEqual(Conferences.Count, actual.Count);
            foreach (SqlConference conference in actual)
            {
                Assert.That (conference is SqlConference);
            }
            foreach (SqlConference conference in Conferences) 
            {
                Assert.That (actual.Contains (conference));
            }
            Conferences = actual;
            Conferences[0].Name = "XKCD";
            Conferences[1].Name = "SUCKS";
            Conferences[2].Name = "BALLS";
            Conferences[2].Abbreviation = "BLZ";
            /*
            Writer.WriteConferences (Conferences);
            Reader.ReadConferences();
            actual = SqlConference.List;
            Assert.AreEqual (Conferences.Count, actual.Count);
            foreach (SqlConference conference in Conferences) 
            {
                Assert.That (actual.Contains (conference));
            }*/
        }
        
        virtual public void TestWriteRunners()
        {
            IList<Runner> actual;
            Writer.WriteRunners(Runners);
            Reader.ReadRunners();
            actual = SqlRunner.List;
            Assert.AreEqual(Runners.Count, actual.Count);
            foreach(Runner runner in actual)
            {
                Assert.That(runner is SqlRunner);
            }
            foreach(Runner runner in Runners)
            {
                Assert.That(actual.Contains(runner));
            }
            Runners = actual;
            ((SqlRunner)Runners[3]).Nicknames = new string[] {"Beast"};
            Writer.WriteRunners(Runners);
            Reader.ReadRunners();
            actual = SqlRunner.List;
            Assert.AreEqual(Runners.Count, actual.Count);
            Assert.AreEqual("Beast", ((SqlRunner)actual[3]).Nicknames[0]);
            Assert.AreEqual(Runners.Count, actual.Count);
            foreach(Runner runner in Runners)
            {
                Assert.That(actual.Contains(runner));
            }
        }
        
        virtual public void TestWriteSchools()
        {
            IList<School> actual;
            Writer.WriteConferences(Conferences);
            Reader.ReadConferences();
            Writer.WriteSchools(Schools);
            Reader.ReadSchools();
            actual = SqlSchool.List;
            Assert.AreEqual(Schools.Count, actual.Count);
            foreach(School school in actual)
            {
                Assert.That(school is SqlSchool);
            }
            foreach(School school in Schools)
            {
                Console.WriteLine(actual[0].Conference);
                Console.WriteLine((bool)(actual[0] is SqlSchool));
                Assert.That(actual.Contains(school));
                foreach(SqlConference conference in Conferences)
                {
                    if(school.Conference != null && school.Conference.Equals(conference.Name))
                    {
                        Assert.AreEqual(conference.Name, ((SqlSchool)actual[actual.IndexOf(school)]).Conference);
                        break;
                    }
                }
            }
            Schools = actual;
            ((SqlSchool)Schools[5]).Nicknames = new string[] {"Caltech"};
            ((SqlSchool)Schools[6]).Nicknames = new string[] {"UCSC"};
            Writer.WriteSchools(Schools);
            Reader.ReadSchools();
            actual = SqlSchool.List;
            Assert.AreEqual(Schools.Count, actual.Count);
            Assert.AreEqual("Caltech", ((SqlSchool)actual[5]).Nicknames[0]);
            Assert.AreEqual("UCSC", ((SqlSchool)actual[6]).Nicknames[0]);
            foreach(School school in Schools)
            {
                Assert.That(actual.Contains(school));
            }
        }
        
        protected internal class Conference : SqlConference
        {
            public Conference (string name, string abbreviation)
            : base(-1, name, abbreviation) {}
        }
    }
    
    [TestFixture]
    public class TestMySqlDatabaseWriter : TestDatabaseWriter
    {       
        [SetUp]
        override public void SetUp ()
        {
            base.SetUp();
            Writer = MySqlDatabaseWriter.NewInstance ("localhost",
                TEST_DATABASE, "xcanalyze");
            Reader = MySqlDatabaseReader.NewInstance ("localhost",
                TEST_DATABASE, "xcanalyze");
        }   
        
        override public void SetUpWriters()
        {
            throw new NotImplementedException();
        }
        
        [TearDown]
        override public void TearDown()
        {
            for(int i = DatabaseWriter.TABLES.Length - 1; i >= 0; i--)
            {
                Writer.Command.CommandText = "DELETE FROM " + DatabaseWriter.TABLES[i];
                Writer.Command.ExecuteNonQuery();
            }
            base.TearDown();
        }
        
        [Test]
        override public void TestIsDatabaseInitialized ()
        {
            Assert.That (Writer.IsDatabaseInitialized ());
        }
        
        [Test]
        override public void TestInitializeDatabase ()
        {
            Writer.InitializeDatabase ();
        }
        
        [Test]
        override public void TestWrite ()
        {
            base.TestWrite ();
        }
        
        [Test]
        override public void TestWriteConferences ()
        {
            base.TestWriteConferences ();
        }
        
        [Test]
        override public void TestWriteRunners()
        {
            base.TestWriteRunners();
        }
        
        [Test]
        override public void TestWriteSchools()
        {
            base.TestWriteSchools();
        }
    }

    [TestFixture]
    public class TestSqliteDatabaseWriter : TestDatabaseWriter
    {      
        /// <summary>
        /// The name of the test database file.
        /// </summary>
        override public string TEST_DATABASE { get { return "xca_test.db"; } }

        [SetUp]
        override public void SetUp ()
        {
            base.SetUp();
            Writer = SqliteDatabaseWriter.NewInstance (TEST_DATABASE);
            Reader = DatabaseReader.NewInstance (new SqliteConnection (
                    Writer.Connection.ConnectionString));
        }
        
        override public void SetUpWriters ()
        {
            TearDown ();
            Writer = new SqliteDatabaseWriter (new SqliteConnection (
                    "Data Source=" + TEST_DATABASE));
            Writer.Connection.Open ();
            Writer.Command = Writer.Connection.CreateCommand ();
        }

        [TearDown]
        override public void TearDown ()
        {
            base.TearDown();
            File.Delete (TEST_DATABASE);
        }

        [Test]
        override public void TestInitializeDatabase ()
        {
            base.TestInitializeDatabase();
        }

        [Test]
        override public void TestIsDatabaseInitialized ()
        {
            base.TestIsDatabaseInitialized();
        }

        [Test]
        override public void TestWrite ()
        {
            base.TestWrite();
        }
        
        [Test]
        override public void TestWriteConferences ()
        {
            base.TestWriteConferences();
        }
        
        [Test]
        override public void TestWriteRunners()
        {
            base.TestWriteRunners();
        }
        
        [Test]
        override public void TestWriteSchools()
        {
            base.TestWriteSchools();
        }
    }
}