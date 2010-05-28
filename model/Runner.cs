using System;
using System.Collections.Generic;

namespace XcAnalyze.Model
{
    
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner : IComparable<Runner>
    {
        private Dictionary<int, Affiliation> affiliations;
        private Gender gender;
        private string givenName;
        private List<Performance> performances;
        private string surname;
        private int? year;
        
        /// <summary>
        /// The affiliations of the runner over their career.
        /// </summary>
        protected Dictionary<int, Affiliation> Affiliations
        {
            get { return affiliations; }
        }

        /// <summary>
        /// The runner's gender.
        /// </summary>
        public Gender Gender
        {
            get { return gender; }
        }
        
        /// <summary>
        /// The runner's given name.
        /// </summary>
        public string GivenName
        {
            get { return givenName; }
        }
            
        /// <summary>
        /// The runner's full name.
        /// </summary>
        public string Name
        {
            get { return GivenName + " " + Surname; }
        }
        
        /// <summary>
        /// The performances this runner has achieved.
        /// </summary>
        public List<Performance> Performances
        {
            get { return performances; }
        }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public string Surname
        {
            get { return surname; }
        }

        /// <summary>
        /// The runner's original graduation year.
        /// </summary>
        public int? Year
        {
            get { return year; }
        }     
        
        public Runner (string surname, string givenName, Gender gender, int? year) : this(surname, givenName, gender, year, new Dictionary<int, Affiliation> (), new List<Performance>())
        {
        }
            
        public Runner (string surname, string givenName, Gender gender, int? year, Dictionary<int, Affiliation> affiliations, List<Performance> performances)
        {
            this.surname = surname;
            this.givenName = givenName;
            this.gender = gender;
            this.year = year;
            this.affiliations = affiliations;
            this.performances = performances;
        }

        /// <summary>
        /// Register a new affiliation for this runner.
        /// </summary>
        public void AddAffiliation (Affiliation affiliation)
        {
            affiliations.Add (affiliation.Year, affiliation);
        }
        
        public void AddPerformance (Performance performance)
        {
            performances.Add (performance);
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
            comparison = surname.CompareTo (other.Surname);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = givenName.CompareTo (other.GivenName);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = Utilities.CompareNullable (Year, other.Year, 1);
            if (comparison != 0) 
            {
                return comparison;
            }
            return gender.CompareTo (other.Gender);
        }

        public override bool Equals (object other)
        {
            if (this == other) {
                return true;
            }
            if (other is Runner) {
                return 0 == CompareTo ((Runner)other);
            }
            return false;
        }
        
        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }
        
        /// <summary>
        /// The school the runner was associated with in the given year.
        /// </summary>
        public School School (int year)
        {
            if (affiliations.ContainsKey (year))
            {
                return affiliations[year].School;
            }
            return null;
        }

        public override string ToString ()
        {
            return Name + " (" + Year + ")";
        }    
    }
}