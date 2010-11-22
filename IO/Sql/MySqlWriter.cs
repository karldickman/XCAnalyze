using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using MySql.Data.MySqlClient;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// A <see cref="IWriter"/> to write the model to a MySQL database.
    /// </summary>
    public partial class MySqlWriter : Writer
    {
        #region Constructors

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
        protected static IDbConnection CreateConnection(string host, string user, string database, string password, int port, bool pooling)
        {
            IDbConnection connection = new MySqlConnection(String.Format("Server={0}; User ID={1}; Database={2}; Password={3}; Pooling={4}; Port={5}", host, user, database, password, pooling, port));
            connection.Open();
            return connection;
        }
        
        /// <summary>
        /// Create a new database writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database.
        /// </param>
        public MySqlWriter(IDbConnection connection, string database) : base(connection, database)
        {
        }

        /// <summary>
        /// Create a new writer connected to a local database.
        /// </summary>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="pasword">
        /// The user's password.
        /// </param>
        public MySqlWriter(string database, string user, string password) : this("localhost", database, user, password)
        {
        }

        /// <summary>
        /// Create a new writer connected to a password-protected database.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        public MySqlWriter(string host, string database, string user, string password) : this(host, database, user, password, 3306)
        {
        }

        /// <summary>
        /// Create a new writer connected to a password-protected database on a
        /// server listening at a particular port.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="port">
        /// The port number on which the server is listening.
        /// </param>
        public MySqlWriter(string host, string database, string user, string password, int port) : this(host, database, user, password, port, false)
        {
        }

        /// <summary>
        /// Create a new writer connected to a password-protected database on a
        /// server listening at a particular port.
        /// </summary>
        /// <param name="host">
        /// The host of the database.
        /// </param>
        /// <param name="database">
        /// The name of the database to connect to.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        /// <param name="password">
        /// The user's password.
        /// </param>
        /// <param name="port">
        /// The port number on which the server is listening.
        /// </param>
        /// <param name="pooling">
        /// Should pooling be turned on or off.
        /// </param>
        public MySqlWriter(string host, string database, string user, string password, int port, bool pooling) : this(CreateConnection(host, user, database, password, port, pooling), database)
        {
        }

        #endregion

        #region Writer implementation

        protected override string CreationScriptExtension {
            get { return "mysql"; }
        }

        protected override string GetTablesColumn {
            get { return "Tables_in_" + Database; }
        }

        protected override string GetTablesCommand {
            get { return "SHOW TABLES"; }
        }
        
        #endregion
    }
}
