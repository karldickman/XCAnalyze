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
            Connection.Open ();
            Command = Connection.CreateCommand();
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
            Console.WriteLine (foundTables.Count + ", " + TABLES.Length);
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
        public void Write (Data data)
        {
            throw new NotImplementedException();
        }
    }

    public class SqliteDatabaseWriter : DatabaseWriter
    {
        override public string CREATION_SCRIPT_EXTENSION
        {
            get {
                return "sqlite";
            }
        }

        override public string GET_TABLES_COMMAND
        {
            get { return "SELECT name FROM sqlite_master WHERE type=\"table\""; }
        }

        protected internal SqliteDatabaseWriter(IDbConnection connection)
            : base(connection) {}

        public static SqliteDatabaseWriter NewInstance ()
        {
            return NewInstance (":memory:");
        }

        public static SqliteDatabaseWriter NewInstance (string fileName)
        {
            return NewInstance (
                new SqliteConnection ("Data Source=" + fileName));
        }

        public static SqliteDatabaseWriter NewInstance (IDbConnection connection)
        {
            return new SqliteDatabaseWriter (connection);
        }
    }

    [TestFixture]
    public class TestSqliteDatabaseWriter
    {
        /// <summary>
        /// The database writer that will be tested.
        /// </summary>
        protected internal SqliteDatabaseWriter Writer { get; set; }

        [SetUp]
        public void SetUp ()
        {
            Writer = SqliteDatabaseWriter.NewInstance ();
        }

        [TearDown]
        public void TearDown ()
        {
            Writer.Close ();
        }

        [Test]
        public void TestCreateDatabase ()
        {
            Writer.InitializeDatabase ();
        }

        [Test]
        public void TestIsDatabaseInitialized ()
        {
            Assert.That (!Writer.IsDatabaseInitialized ());
            Writer.InitializeDatabase ();
            Assert.That (Writer.IsDatabaseInitialized ());
        }

        [Test]
        public void TestWrite ()
        {
            Assert.Fail ("Not yet implemented.");
        }
    }
}