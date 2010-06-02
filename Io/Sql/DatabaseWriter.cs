using Mono.Data.Sqlite;
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
                return SupportFiles.GetPath ("xca_create." + CREATION_SCRIPT_EXTENSION);
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
        /// The script used to get the list of tables in the database.
        /// </summary>
        abstract public string GET_TABLES_COMMAND { get; }

        /// <summary>
        /// The tables that should be in the database.
        /// </summary>
        public static readonly string[] TABLES = {"affiliations", "conferences",
            "meets", "races", "results", "runners", "schools",
            "venues"};

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
            return File.ReadAllText (CREATION_SCRIPT);
        }

        /// <summary>
        /// Initialize all the tables in the database.
        /// </summary>
        public void InitializeDatabase ()
        {
            Command.CommandText = CreationScript();
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
                foundTables.Add ((string)Reader["name"]);
            }
            Reader.Dispose ();
            if (foundTables.Count != TABLES.Length)
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
            return true;
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
        /// Update the database to reflect the changes to the conference.
        /// </summary>
        virtual public void WriteConference(SqlConference conference)
        {
            Command.CommandText = "UPDATE conferences SET name = \"" + conference.Name + "\", abbreviation = \"" + conference.Abbreviation + "\" WHERE id = " + conference.Id;
            Command.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Write a new conference to the database.
        /// </summary>
        virtual public void WriteConference(Conference conference)
        {
            Command.CommandText = "INSERT INTO conferences (name, abbreviation) VALUES (\"" + conference.Name + "\", \"" + conference.Abbreviation + "\")";
            Command.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Write the list of conferences to the database.
        /// </summary>
        virtual public void WriteConferences (IList<Conference> conferences)
        {
            foreach (Conference conference in conferences)
            {
                if(conference is SqlConference)
                {
                    WriteConference((SqlConference)conference);
                }
                else
                {
                    WriteConference(conference);
                }
            }
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

    [TestFixture]
    public class TestSqliteDatabaseWriter
    {
        /// <summary>
        /// The name of the database file.
        /// </summary>
        public const string FILE_NAME = "nunit.db";
        
        /// <summary>
        /// The database reader used to help in testing.
        /// </summary>
        protected internal DatabaseReader Reader { get; set; }
        
        /// <summary>
        /// The database writer that will be tested.
        /// </summary>
        protected internal SqliteDatabaseWriter Writer { get; set; }

        [SetUp]
        public void SetUp ()
        {
            Writer = SqliteDatabaseWriter.NewInstance (FILE_NAME);
            Reader = DatabaseReader.NewInstance (new SqliteConnection(Writer.Connection.ConnectionString));
        }

        [TearDown]
        public void TearDown ()
        {
            Writer.Close ();
            Reader.Close ();
            File.Delete (FILE_NAME);
        }

        [Test]
        public void TestCreateDatabase ()
        {
            TearDown ();
            Writer = new SqliteDatabaseWriter (new SqliteConnection ("Data Source=" + FILE_NAME));
            Writer.Connection.Open ();
            Writer.Command = Writer.Connection.CreateCommand ();
            Writer.InitializeDatabase ();
        }

        [Test]
        public void TestIsDatabaseInitialized ()
        {
            TearDown ();
            Writer = new SqliteDatabaseWriter (new SqliteConnection ("Data Source=" + FILE_NAME));
            Writer.Connection.Open ();
            Writer.Command = Writer.Connection.CreateCommand ();
            Assert.That (!Writer.IsDatabaseInitialized ());
            Writer.InitializeDatabase ();
            Assert.That (Writer.IsDatabaseInitialized ());
        }

        [Test]
        public void TestWrite ()
        {
            Assert.Fail ("Not yet implemented.");
        }
        
        [Test]
        public void TestWriteConferences ()
        {
            IList<Conference> actual;
            IList<Conference> expected = new List<Conference> ();
            expected.Add (new Conference ("Northwest Conference",
                "NWC"));
            expected.Add (new Conference (
                "Southern California Intercollegiate Athletic Conference",
                "SCIAC"));
            expected.Add (new Conference (
                "Southern Collegiate Athletic Conference", "SCAC"));
            Writer.WriteConferences (expected);
            actual = new List<Conference> (Reader.ReadConferences ().Values);
            foreach (Conference conference in actual)
            {
                Assert.That (conference is SqlConference);
            }
            foreach (Conference conference in expected) 
            {
                Assert.That (actual.Contains (conference));
            }
            expected = actual;
            expected[0].Name = "XKCD";
            expected[1].Name = "SUCKS";
            expected[2].Name = "BALLS";
            expected[2].Abbreviation = "BLZ";
            Writer.WriteConferences (expected);
            actual = new List<Conference> (Reader.ReadConferences ().Values);
            Assert.AreEqual (expected.Count, actual.Count);
            foreach (Conference conference in expected) 
            {
                Assert.That (actual.Contains (conference));
            }
        }
    }
}