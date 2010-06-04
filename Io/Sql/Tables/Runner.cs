using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row of the runners table in the database.
    /// </summary>
    public class Runner : Model.Runner
    {
        /// <summary>
        /// A registry of all instances (i.e. rows) by id number.
        /// </summary>
        protected internal static IDictionary<int, Model.Runner> IdMap =
            new Dictionary<int, Model.Runner>();
        
        /// <summary>
        /// Get all instances.
        /// </summary>
        public static IList<Model.Runner> List
        {
            get { return new List<Model.Runner> (IdMap.Values); }
        }
        
        /// <summary>
        /// The id number.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// All the nicknames of this runner.
        /// </summary>
        public string[] Nicknames { get; protected internal set; }
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="id">
        /// The id number.
        /// </param>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="nicknames">
        /// Any nicknames the runner may have.
        /// </param>
        /// <param name="gender">
        /// A <see cref="Model.Gender"/>.  The gender of the runner.
        /// </param>
        /// <param name="year">
        /// The runner's original graduation year.  Null if not known.
        /// </param>
        public Runner (int id, string surname, string givenName,
            string[] nicknames, Model.Gender gender, int? year)
            : base(surname, givenName, gender, year)
        {
            Id = id;
            Nicknames = nicknames;
            IdMap[id] = this;
        }
        
        /// <summary>
        /// Clear all instances of this class.
        /// </summary>
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        /// <summary>
        /// Check if an instance with a particular id number exists.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// True if one exists, false otherwise.
        /// </returns>
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        /// <summary>
        /// Get the instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Runner"/> with the given id number.
        /// </returns>
        public static Model.Runner Get(int id)
        {
            return IdMap[id];
        }
    }
}
