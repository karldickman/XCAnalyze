using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Model
{  
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner : IComparable<Runner>
    {
        /// <summary>
        /// The runner's gender.
        /// </summary>
        public Gender Gender { get; protected internal set; }

        /// <summary>
        /// The runner's given name.
        /// </summary>
        public string GivenName { get; protected internal set; }
        
        /// <summary>
        /// The runner's full name.
        /// </summary>
        public string Name
        {
            get { return GivenName + " " + Surname; }
        }
        
        /// <summary>
        /// The nicknames or alternate names of the runner.
        /// </summary>
        public IList<string> Nicknames { get; protected internal set; }

        /// <summary>
        /// The performances this runner has achieved.
        /// </summary>
        public IList<Performance> Performances { get; protected internal set; }

        /// <summary>
        /// The affiliations of the runner over their career.
        /// </summary>
        public IDictionary<int, Affiliation> Schools { get; protected internal set; }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public string Surname { get; protected internal set; }

        /// <summary>
        /// The runner's original graduation year.
        /// </summary>
        public int? Year { get; protected internal set; }
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        public Runner (string surname, string givenName, Gender gender,
            int? year)
            : this(surname, givenName, new List<string>(), gender, year) {}
            
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of a runners nicknames or
        /// alternative names.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        public Runner(string surname, string givenName, IList<string> nicknames,
            Gender gender, int? year)
        : this(surname, givenName, nicknames, gender, year,
                new Dictionary<int, Affiliation>(), new List<Performance>()) {}
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        /// <param name="schools">
        /// A <see cref="IDictionary<System.Int32, Affiliation>"/> of
        /// school affiliations.
        /// </param>
        /// <param name="performances">
        /// The <see cref="IList<Performance>"/> the runner owns.
        /// </param>
        public Runner (string surname, string givenName, Gender gender,
            int? year, IDictionary<int, Affiliation> schools,
            IList<Performance> performances)
            : this(surname, givenName, new List<string>(), gender, year,
                schools, performances) {}
              
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of a runners nicknames or
        /// alternative names.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        /// <param name="schools">
        /// A <see cref="IDictionary<System.Int32, Affiliation>"/> of
        /// school affiliations.
        /// </param>
        /// <param name="performances">
        /// The <see cref="IList<Performance>"/> the runner owns.
        /// </param>
        public Runner (string surname, string givenName,
            IList<string> nicknames, Gender gender, int? year,
            IDictionary<int, Affiliation> schools,
            IList<Performance> performances)
        {
            Surname = surname;
            GivenName = givenName;
            Nicknames = nicknames;
            Gender = gender;
            Year = year;
            Schools = schools;
            Performances = performances;
        }

        /// <summary>
        /// Register a new affiliation for this runner.
        /// </summary>
        /// <param>
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        public void AddSchool (Affiliation affiliation)
        {
            Schools.Add (affiliation.Year, affiliation);
        }
        
        /// <summary>
        /// Register a new performance ran by this runner.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to register.
        /// </param>
        public void AddPerformance (Performance performance)
        {
            Performances.Add (performance);
        }

        /// <summary>
        /// Runners are compared first by surname, then by year, then by gender.
        /// </summary>
        public int CompareTo (Runner other)
        {
            int comparison;
            if (this == other)
            {
                return 0;
            }
            comparison = Surname.CompareTo (other.Surname);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = GivenName.CompareTo (other.GivenName);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = NullableComparer.Compare (Year, other.Year, 1);
            if (comparison != 0)
            {
                return comparison;
            }
            return Gender.CompareTo (other.Gender);
        }

        override public bool Equals (object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is Runner)
            {
                return 0 == CompareTo ((Runner)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return base.GetHashCode ();
        }
        
        /// <summary>
        /// The school the runner was associated with in the given year.
        /// </summary>
        public School School (int year)
        {
            if (Schools.ContainsKey (year))
            {
                return Schools[year].School;
            }
            return null;
        }

        override public string ToString ()
        {
            return Name + " (" + Year + ")";
        }    
    }
    
    [TestFixture]
    public class TestRunner
    {
        [Test]
        public void TestEquals ()
        {
            Runner zim1 = new Runner ("Zimmerman", "Elizabeth", Gender.FEMALE, 2007);
            Runner zim2 = new Runner ("Zimmerman", "Elizabeth", Gender.FEMALE, null);
            Assert.IsFalse (zim1.Equals (zim2));
        }
    }
}