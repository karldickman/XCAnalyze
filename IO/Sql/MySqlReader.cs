using System;
using System.Collections;
using System.Data;

using MySql.Data.MySqlClient;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// A <see cref="IReader"/> to write all data in the model to a MySQL database.
    /// </summary>
    public partial class MySqlReader : Reader
    {
        /// <summary>
        /// Create an open <see cref="MySqlConnection" /> with the specified parameters.
        /// </summary>
        /// <param name="host">
        /// The name of the server where the database is hosted.
        /// </param>
        /// <param name="user">
        /// The user account to be used.
        /// </param>
        /// <param name="database">
        /// The name of the database on the server.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="pooling">
        /// Should connection pooling be used?
        /// </param>
        /// <param name="port">
        /// The TCP port to use when communicating with the database.
        /// </param>
        protected static IDbConnection CreateConnection(string host, string database, string user, string password, int port, bool pooling)
        {
            IDbConnection connection = new MySqlConnection(String.Format("Server={0}; User ID={1}; Database={2}; Password={3}; Pooling={4}; Port={5}", host, user, database, password, pooling, port));
            connection.Open();
            return connection;
        }
        
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public MySqlReader(IDbConnection connection, string database) : base(connection, database)
        {
        }

        /// <summary>
        /// Create a new reader connection to the local server.
        /// </summary>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The users password.
        /// </param>
        public MySqlReader(string database, string user, string password) : this("localhost", database, user, password)
        {
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
        public MySqlReader(string host, string database, string user, string password) : this(host, database, user, password, 3306)
        {
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
        public MySqlReader(string host, string database, string user, string password, int port) : this(host, database, user, password, port, false)
        {
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
        public MySqlReader(string host, string database, string user, string password, int port, bool pooling) : this(CreateConnection(host, database, user, password, port, pooling), database)
        {
        }
    }
}
