using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{
    abstract public class DatabaseWriter : IWriter<Data>
    {
        public static readonly string[] TABLES = {"affiliations", "conferences", "meets", "races", "results", "runners", "schools", "venues"};

        protected internal IDbCommand Command { get; set; }
        protected internal IDbConnection Connection { get; set; }
        protected internal IDataReader Reader { get; set; }

        public DatabaseWriter (IDbConnection connection)
        {
            Connection = connection;
            Connection.Open();
        }

        public void Close ()
        {
            if(Reader != null)
            {
                Reader.Close();
                Reader = null;
            }
            if(Command != null)
            {
                Command.Dispose();
                Command = null;
            }
            Connection.Close();
        }

        abstract public void CreateDatabase();

        abstract public bool DatabaseIsValid();

        virtual public void Write(Data data)
        {
            throw new NotImplementedException();
        }

        virtual public void WriteAffiliations(Affiliation affiliation)
        {
            if(affiliation is SqlAffiliation)
            {
                SqlAffiliation sqlAffiliation = (SqlAffiliation) affiliation;

            }
        }
    }

    public class MySqlDatabaseWriter : DatabaseWriter
    {
        public MySqlDatabaseWriter(string host, string database, string username) : this(host, database, username, "") {}

        public MySqlDatabaseWriter(string host, string database, string username, string password) : this("Server=" + host + "; Database=" + database + "; User Id=" + username + "; Password=" + password + "; Pooling=false;") {}

        public MySqlDatabaseWriter(string connectionString) : this(new MySqlConnection(connectionString)) {}

        public MySqlDatabaseWriter(IDbConnection connection) : base(connection) {}

        override public void CreateDatabase ()
        {
            throw new NotImplementedException();
        }

        override public bool DatabaseIsValid()
        {
            throw new NotImplementedException();
        }
    }

    public class SqliteDatabaseWriter : DatabaseWriter
    {
        private const string CREATION_SCRIPT = "/home/karl/Programming/monodevelop/XCAnalyze/create.sqlite";

        public SqliteDatabaseWriter() : base(new SqliteConnection("Data Source=:memory:")) {}

        public SqliteDatabaseWriter(string fileName) : base(new SqliteConnection("Data Source=" + fileName)) {}

        override public void CreateDatabase ()
        {
            Command = Connection.CreateCommand();
            Command.CommandText = File.ReadAllText(CREATION_SCRIPT);
            Command.ExecuteNonQuery();
        }

        override public bool DatabaseIsValid()
        {
            List<string> foundTables = new List<string>();
            Command = Connection.CreateCommand();
            Command.CommandText = "SELECT name FROM sqlite_master WHERE type=\"table\"";
            Reader = Command.ExecuteReader();
            while(Reader.Read())
            {
                foundTables.Add((string)Reader["name"]);
            }
            if(foundTables.Count != TABLES.Length)
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
    }

    [TestFixture]
    public class TestSqliteDatabaseWriter
    {
        private string databaseFile;
        private SqliteDatabaseWriter[] writers;

        [SetUp]
        public void SetUp()
        {
            writers = new SqliteDatabaseWriter[2];
            writers[0] = new SqliteDatabaseWriter();
            databaseFile = "nunit.db";
            writers[1] = new SqliteDatabaseWriter(databaseFile);
        }

        [TearDown]
        public void TearDown()
        {
            for(int i = 0; i < writers.Length; i++)
            {
                writers[i].Close();
            }
            File.Delete(databaseFile);
        }

        [Test]
        public void TestCreateDatabase()
        {
            for(int i = 0; i < writers.Length; i++)
            {
                writers[i].CreateDatabase();
                Assert.That(writers[i].DatabaseIsValid());
            }
        }

        [Test]
        public void TestDatabaseIsValid()
        {
            for(int i = 0; i < writers.Length; i++)
            {
                Assert.That(!writers[i].DatabaseIsValid());
                writers[i].CreateDatabase();
                Assert.That(writers[i].DatabaseIsValid());
            }
        }
    }
}
