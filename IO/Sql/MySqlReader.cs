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
    public class MySqlReader : Reader
    { 
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        protected internal MySqlReader(IDbConnection connection,
            string database) : base(connection, database) {}
        
        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to use.
        /// </param>
        /// <param name="command">
        /// The <see cref="IDbCommand"/> to use.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public MySqlReader(IDbConnection connection,
            IDbCommand command, string database)
        : base(connection, command, database) {}
        
        /// <summary>
        /// Create a new reader connection to the local server.
        /// </summary>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="user">
        /// The name of the user.
        /// </param>
        public MySqlReader (string database, string user)
            : this("localhost", database, user) {}
        
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
        public MySqlReader (string host, string database, string user)
            : this(host, database, user, user) {}
        
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
        public MySqlReader (string host, string database, string user,
            string password) : this (host, database, user, password, 3306) {}
        
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
        public MySqlReader (string host, string database, string user,
            string password, int port)
            : this(host, database, user, password, port, false) {}

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
        public MySqlReader (string host, string database, string user,
            string password, int port, bool pooling)
            : this(new MySqlConnection(
                    "Server=" + host + "; Database=" + database +
                    "; User ID=" + user + "; Password=" + password +
                    "; Pooling=" + pooling + ";"), database) {}
    }
}
