using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using XCAnalyze.Model;

namespace XCAnalyze.Data
{
    /// <summary>
    /// A class that maps domain operations on runners into the appropriate
    /// SQL statements.
    /// </summary>
    internal sealed class RunnerMapper
    {
        #region Properties
        
        /// <summary>
        /// The database connection to use.
        /// </summary>
        private readonly IDbConnection Connection;
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Create a new <see cref="RunnerMapper">.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> connection to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if connection is null.
        /// </exception>
        public RunnerMapper(IDbConnection connection)
        {
            if(connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            Connection = connection;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Delete the specified runner.
        /// </summary>
        /// <param name="toDeleteID">
        /// The ID number of the runner to delete.
        /// </param>
        public void Delete(int toDeleteID)
        {
            using(IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = String.Format("DELETE FROM runners WHERE runner_id = {0}", toDeleteID);                
                command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Initialize the database so it can be used for data.
        /// </summary>
        public void InitializeDatabase()
        {
            using(IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "DROP TABLE IF EXISTS runners;" +
                        "CREATE TABLE runners" +
                        "(" +
                        "    runner_id INTEGER PRIMARY KEY," +
                        "    surname VARCHAR(64) NOT NULL," +
                        "    given_name VARCHAR(64) NOT NULL" + //," +
                        //"    gender CHAR(1) NOT NULL" +
                        ");";
                command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Insert a new runner into the database.
        /// </summary>
        /// <param name="toInsert">
        /// The <see cref="IRunner" /> to insert.
        /// </param>
        /// <returns>
        /// The ID number of the newly inserted runner.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if surname or givenName is null.
        /// </exception>
        public int Insert(IRunner toInsert)
        {
            if(toInsert == null)
            {
                throw new ArgumentNullException("toInsert");
            }
            using(IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = String.Format("INSERT INTO runners (surname, given_name) VALUES (\"{0}\", \"{1}\")", toInsert.Surname, toInsert.GivenName);
                command.ExecuteNonQuery();
                command.CommandText = String.Format("SELECT MAX(runner_id) FROM runners WHERE surname = \"{0}\" AND given_name = \"{1}\"", toInsert.Surname, toInsert.GivenName);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        
        /// <summary>
        /// Check if the database has been initialized.
        /// </summary>
        /// <returns>
        /// True if the database has been initialized, false if otherwise.
        /// </returns>
        public bool IsDatabaseInitialized()
        {
            using(IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type=\"table\" AND name = \"runners\"";
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
        
        /// <summary>
        /// Select all runners in the database.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable<Runner>"/> of runners.
        /// </returns>
        public IList<IRunner> Select()
        {
            IList<IRunner> runners = new List<IRunner>();
            using(IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT runner_id, surname, given_name FROM runners";
                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = Convert.ToInt32(reader["runner_id"]);
                        string surname = reader["surname"].ToString();
                        string givenName = reader["given_name"].ToString();
                        runners.Add(new PersistentRunner(id, surname, givenName));
                    }
                }
            }
            return runners;
        }
        
        /// <summary>
        /// Record changes made to the specified runner in the database.
        /// </summary>
        /// <param name="toUpdateID">
        /// The ID number of the runner to update.
        /// </param>
        /// <param name="toUpdate">
        /// The <see cref="IRunner" /> to update.
        /// </param>
        /// <exception cref="ArgumenNullException">
        /// Thrown if toUpdate is null.
        /// </exception>
        public void Update(int toUpdateID, IRunner toUpdate)
        {
            if(toUpdate == null)
            {
                throw new ArgumentNullException("surname");
            }
            using(IDbCommand command = Connection.CreateCommand())
            {
                command.CommandText = String.Format("UPDATE runners SET surname = \"{1}\", given_name = \"{2}\" WHERE runner_id = {0}", toUpdateID, toUpdate.Surname, toUpdate.GivenName);
                command.ExecuteNonQuery();
            }
        }
        
        #endregion
    }
}

