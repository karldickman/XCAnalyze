using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{  
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner
    {
        private IXList<string> _nicknames;
        
        private IXList<Performance> _performances;
        
        private IXDictionary<int, Affiliation> _schools;
        
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
        : this(surname, givenName, new XList<string> (), gender, year) { }

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
        public Runner (string surname, string givenName,
            IXList<string> nicknames, Gender gender, int? year)
        : this(surname, givenName, nicknames, gender, year,
                new XDictionary<int, Affiliation> (), new XList<Performance> ())
        {
        }

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
            int? year, IXDictionary<int, Affiliation> schools,
            IXList<Performance> performances)
        : this(surname, givenName, new XList<string> (), gender, year, schools,
                performances) { }

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
            IXList<string> nicknames, Gender gender, int? year,
            IXDictionary<int, Affiliation> schools,
            IXList<Performance> performances)
        {
            Surname = surname;
            GivenName = givenName;
            _nicknames = nicknames;
            Gender = gender;
            Year = year;
            _schools = schools;
            _performances = performances;
        }
        
        /// <summary>
        /// The runner's gender.
        /// </summary>
        public Gender Gender { get; protected set; }

        /// <summary>
        /// The runner's given name.
        /// </summary>
        public string GivenName { get; protected set; }
        
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
        public IList<string> Nicknames
        {
            get { return _nicknames.AsReadOnly (); }
        }

        /// <summary>
        /// The performances this runner has achieved.
        /// </summary>
        public IList<Performance> Performances
        {
            get { return _performances.AsReadOnly (); }
        }

        /// <summary>
        /// The affiliations of the runner over their career.
        /// </summary>
        public ReadOnlyDictionary<int, Affiliation> Schools
        {
            get { return _schools.AsReadOnly (); }
        }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public string Surname { get; protected set; }

        /// <summary>
        /// The runner's original graduation year.
        /// </summary>
        public int? Year { get; protected set; }
        
        /// <summary>
        /// Register a new affiliation for this runner.
        /// </summary>
        /// <param>
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        public void Add (Affiliation affiliation)
        {
            _schools.Add (affiliation.Year, affiliation);
        }
        
        /// <summary>
        /// Register a new performance ran by this runner.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to register.
        /// </param>
        public void Add (Performance performance)
        {
            _performances.Add (performance);
        }

        /// <summary>
        /// Delete a performance.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to delete.
        /// </param>
        public void Delete (Performance performance)
        {
            _performances.Remove (performance);
        }
        
        override public bool Equals (object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is Runner)
            {
                return Equals ((Runner)other);
            }
            return false;
        }
        
        protected bool Equals (Runner other)
        {
            return Surname.Equals (other.Surname) &&
                GivenName.Equals (other.GivenName) &&
                Year == other.Year &&
                Gender == other.Gender;
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
}