using System;

namespace XCAnalyze.Model
{ 
    /// <summary>
    /// Describes in which Year a Runner ran for a particular School.
    /// </summary>
    public class Affiliation : IComparable<Affiliation>
    {
        /// <summary>
        /// The <see cref="Runner"/> affiliated with a School.
        /// </summary>
        public Runner Runner { get; protected internal set; }

        /// <summary>
        /// The <see cref="School"/> with which a Runner is affiliated.
        /// </summary>
        public School School { get; protected internal set; }

        /// <summary>
        /// The Year in which the Runner was affiliated with the School.
        /// </summary>
        public int Year { get; protected internal set; }
        
        protected internal Affiliation(int year) : this(null, null, year) {}
        
        /// <summary>
        /// Create a new affiliation.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who is affiliated with the school.
        /// </param>
        /// <param name="school">
        /// The <see cref="School"/> the runner ran for.
        /// </param>
        /// <param name="year">
        /// The season in which the affiliation occurred.
        /// </param>
        public Affiliation (Runner runner, School school, int year)
        {
            Runner = runner;
            School = school;
            Year = year;
        }

        /// <summary>
        /// Affiliations are compared first by School, then by Runner, then by
        /// Year.
        /// </summary>
        public int CompareTo (Affiliation other)
        {
            int comparison;
            if (this == other) 
            {
                return 0;
            }
            comparison = School.CompareTo (other.School);
            if (comparison != 0) 
            {
                return comparison;
            }
            comparison = Runner.CompareTo (other.Runner);
            if (comparison != 0) 
            {
                return comparison;
            }
            return Year.CompareTo(other.Year);
        }
        
        override public bool Equals (object other)
        {
            if (this == other) 
            {
                return true;
            }
            if (other is Affiliation) 
            {
                return 0 == CompareTo ((Affiliation)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return base.GetHashCode ();
        }
        
        override public string ToString ()
        {
            return Runner.Name + ", " + School.Name + " " + Year;
        }
    }
}