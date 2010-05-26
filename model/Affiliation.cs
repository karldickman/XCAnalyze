using System;

namespace XcAnalyze.Model
{
    
    /// <summary>
    /// Describes in which year a runner ran for a particular school.
    /// </summary>
    public class Affiliation : IComparable<Affiliation>
    {
        private int id;
        private Runner runner;
        private School school;
        private int year;
        
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// The runner affiliated with a school.
        /// </summary>
        public Runner Runner
        {
            get { return runner; }
        }

        /// <summary>
        /// The school with which a runner is affiliated.
        /// </summary>     
        public School School
        {
            get { return school; }
        }

        /// <summary>
        /// The year in which the runner was affiliated with the school.
        /// </summary>
        public int Year
        {
            get { return year; }
        }
        
        public Affiliation (int id, Runner runner, School school, int year)
        {
            this.id = id;
            this.runner = runner;
            this.school = school;
            this.year = year;
        }

        /// <summary>
        /// Affiliations are compared first by school, then by runner, then by year.
        /// </summary>
        public int CompareTo (Affiliation other)
        {
            int comparison;
            if (this == other) 
            {
                return 0;
            }
            comparison = school.CompareTo (other.school);
            if (comparison != 0) 
            {
                return comparison;
            }
            comparison = runner.CompareTo (other.runner);
            if (comparison != 0) 
            {
                return comparison;
            }
            return year.CompareTo(other.year);
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
            return runner.Name + ", " + school.Name + " " + year;
        }
    }
}