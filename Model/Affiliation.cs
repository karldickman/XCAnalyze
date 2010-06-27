using System;

namespace XCAnalyze.Model
{ 
    /// <summary>
    /// Describes in which Year a Runner ran for a particular School.
    /// </summary>
    public class Affiliation
    {        
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
        /// The <see cref="Runner"/> affiliated with a School.
        /// </summary>
        public Runner Runner { get; protected set; }

        /// <summary>
        /// The <see cref="School"/> with which a Runner is affiliated.
        /// </summary>
        public School School { get; protected set; }

        /// <summary>
        /// The Year in which the Runner was affiliated with the School.
        /// </summary>
        public int Year { get; protected set; }
        
        override public bool Equals (object other)
        {
            if (this == other) 
            {
                return true;
            }
            if (other is Affiliation) 
            {
                return Equals((Affiliation)other);
            }
            return false;
        }
        
        protected bool Equals (Affiliation other)
        {
            return School.Equals(other.School) && Runner.Equals(other.Runner) &&
                    Year == other.Year;
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