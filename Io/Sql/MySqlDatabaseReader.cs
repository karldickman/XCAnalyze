using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;

namespace XCAnalyze.Io.Sql
{    
    /// <summary>
    /// A <see cref="IReader"/> to write all data in the model to a MySQL database.
    /// </summary>
    public class MySqlDatabaseReader : DatabaseReader
    { 
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        protected internal MySqlDatabaseReader(IDbConnection connection)
            : base(connection) {}
        
        /// <summary>
        /// Create a new reader connection to the local server.
        /// </summary>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        public static MySqlDatabaseReader NewInstance (string database,
            string user)
        {
            return NewInstance ("localhost", database, user);
        }
        
        /// <summary>
        /// Create a new reader using no password.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user)
        {
            return NewInstance (host, database, user, user);
        }
        
        /// <summary>
        /// Connect to a database using a password.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user, string password)
        {
            return NewInstance (host, database, user, password, 3306);
        }
        
        /// <summary>
        /// Connect to a database server on a particular port using a password.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="port">
        /// The port number the server is listening on.
        /// </param>
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user, string password, int port)
        {
            return NewInstance (host, database, user, password, port, false);
        }

        /// <summary>
        /// Connect to a database server on a particular port using a password.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="port">
        /// The port number the server is listening on.
        /// </param>
        /// <param name="pooling">
        /// Should pooling be turned on or off.
        /// </param>
        public static MySqlDatabaseReader NewInstance (string host,
            string database, string user, string password, int port,
            bool pooling)
        {
            string connectionString = "Server=" + host + "; Database=" + database + "; User ID=" + user + "; Password=" + password + "; Pooling=" + pooling + ";";
            return NewInstance (new MySqlConnection (connectionString));
        }
        
        /// <summary>
        /// Create a new reader using a particular connection.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
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
        public const string EXAMPLE_DATABASE = "xca_example";
        
        protected internal IDbConnection Connection { get; set; }
        protected internal DatabaseReader Reader { get; set; }

        [SetUp]
        public void SetUp ()
        {
            string connectionString = "Server=localhost; Database=" + EXAMPLE_DATABASE + "; User ID=xcanalyze; Password=xcanalyze; Pooling=false;";
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
