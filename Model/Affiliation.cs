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
        virtual public Runner Runner { get; protected internal set; }

        /// <summary>
        /// The <see cref="School"/> with which a Runner is affiliated.
        /// </summary>
        virtual public School School { get; protected internal set; }

        /// <summary>
        /// The Year in which the Runner was affiliated with the School.
        /// </summary>
        virtual public int Year { get; protected internal set; }
        
        protected internal Affiliation(int year) : this(null, null, year) {}
        
        public Affiliation (Runner runner, School school, int year)
        {
            Runner = runner;
            School = school;
            Year = year;
        }

        /// <summary>
        /// Affiliations are compared first by School, then by Runner, then by Year.
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
        
        public override bool Equals (object other)
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
        
        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }
        
        public override string ToString ()
        {
            return Runner.Name + ", " + School.Name + " " + Year;
        }
    }
}