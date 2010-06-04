using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;

namespace XCAnalyze.Io.Sql
{    
    public class MySqlDatabaseReader : DatabaseReader
    { 
        protected internal MySqlDatabaseReader(IDbConnection connection)
            : base(connection) {}
        
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user)
        {
            return NewInstance (host, database, user, user);
        }
        
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user, string password)
        {
            return NewInstance (host, database, user, password, 3306);
        }
        
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user, string password, int port)
        {
            return NewInstance (host, database, user, password, port, false);
        }
        
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user, string password, int port,
            bool pooling)
        {
            string connectionString = "Server=" + host + "; Database=" + database + "; User ID=" + user + "; Password=" + password + "; Pooling=" + pooling + ";";
            return NewInstance (new MySqlConnection (connectionString));
        }
        
        new public static MySqlDatabaseReader NewInstance (IDbConnection connection)
        {
            MySqlDatabaseReader reader = new MySqlDatabaseReader (connection);
            reader.Connection.Open ();
            reader.Command = reader.Connection.CreateCommand ();
            return reader;
        }
    }
    
    [TestFixture]
    public class TestMySqlDatabaseReader
    {
        protected internal IDbConnection Connection { get; set; }
        protected internal DatabaseReader Reader { get; set; }

        [SetUp]
        public void SetUp ()
        {
            string connectionString = "Server=localhost; Database=xcanalyze; User ID=xcanalyze; Password=xcanalyze; Pooling=false;";
            Connection = new MySqlConnection (connectionString);
            Reader = DatabaseReader.NewInstance (Connection);
        }
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Close ();
        }

        [Test]
        public void TestRead ()
        {
            Reader.Read ();
        }
    }
}
