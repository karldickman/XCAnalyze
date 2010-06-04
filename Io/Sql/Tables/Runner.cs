using NUnit.Framework;
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
        public static Model.Runner Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Get the id number of the given runner instance.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Model.Runner"/> runner to search for.
        /// </param>
        /// <returns>
        /// The id number of the runner.
        /// </returns>
        public static int? GetId (Model.Runner runner)
        {
            if (runner is Runner) 
            {
                return ((Runner)runner).Id;
            }
            foreach (KeyValuePair<int, Model.Runner> entry in IdMap)
            {
                if (entry.Value.Equals(runner)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }
    }
    
    [TestFixture]
    public class TestRunner
    {
        protected internal Runner Karl { get; set; }
        protected internal Runner Richie { get; set; }
        protected internal Runner Kirsten { get; set; }
        protected internal Runner Keith { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Runner.Clear ();
            Karl = new Runner (1, "Dickman", "Karl", null, Model.Gender.MALE, 2010);
            Richie = new Runner (2, "LeDonne", "Richie", null, Model.Gender.MALE, 2011);
            Kirsten = new Runner (3, "Fix", "Kirsten", null, Model.Gender.FEMALE, 2010);
            Keith = new Runner (4, "Woodard", "Keith", null, Model.Gender.MALE, null);
        }
        
        [TearDown]
        public void TearDown ()
        {
            Runner.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            Model.Runner[] clones = new Model.Runner[4];
            clones[0] = new Model.Runner (Karl.Surname, Karl.GivenName, Karl.Gender, Karl.Year);
            clones[1] = new Model.Runner (Richie.Surname, Richie.GivenName, Richie.Gender, Richie.Year);
            clones[2] = new Model.Runner (Kirsten.Surname, Kirsten.GivenName, Kirsten.Gender, Kirsten.Year);
            clones[3] = new Model.Runner (Keith.Surname, Keith.GivenName, Keith.Gender, 1973);
            Model.Runner nonExistent = new Model.Runner ("Steier", "Lars", Model.Gender.MALE, 2010);
            Model.Runner bad = new Model.Runner (Keith.Surname, Keith.GivenName, Model.Gender.FEMALE, Keith.Year);
            Assert.IsNull (Runner.GetId (nonExistent));
            Assert.IsNull (Runner.GetId (bad));
            for (int i = 0; i < clones.Length; i++)
            {
                Assert.AreEqual (i + 1, Runner.GetId (clones[i]));
            }
        }
    }
}
