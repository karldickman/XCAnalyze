using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A runners time (in seconds) at a particular race.
    /// </summary>
    public class Performance : IComparable<Performance>
    {
        /// <summary>
        /// The number of points the runner earned in the race for this performance.
        /// </summary>
        public int? Points { get; protected internal set; }

        /// <summary>
        /// The race whereat the time was run.
        /// </summary>
        public Race Race { get; protected internal set; }

        /// <summary>
        /// The runner who ran the time.
        /// </summary>
        public Runner Runner { get; protected internal set; }

        /// <summary>
        /// The time that was run.
        /// </summary>
        public Time Time { get; protected internal set; }
        
        /// <summary>
        /// The length of the race whereat the time was run.
        /// </summary>
        public int Distance
        {
            get { return Race.Distance; }
        }
        
        /// <summary>
        /// The school with which this runner was associated when he ran this time.
        /// </summary>
        public School School
        {
            get { return Runner.School(Race.Meet.Date.Year); }
        }
        
        /// <summary>
        /// Create a new performance.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who owns the performance.
        /// </param>
        /// <param name="race">
        /// The <see cref="Race"/> at which the performance was ran.
        /// </param>
        /// <param name="time">
        /// The <see cref="Time"/> in which teh race was run.
        /// </param>
        public Performance (Runner runner, Race race, Time time)
        {
            Runner = runner;
            Race = race;
            Time = time;
        }

        /// <summary>
        /// A faster pace is a better performance, and comes before a slower
        /// pace.  If the paces are the same, the longer race is considered the
        /// better performance.
        /// </summary>
        public int CompareTo (Performance other)
        {
            int comparison;
            if (this == other)
            {
                return 0;
            }
            comparison = Pace ().CompareTo (other.Pace ());
            if (comparison != 0)
            {
                return comparison;
            }
            return Distance.CompareTo (other.Distance);
        }

        override public bool Equals (object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is Performance)
            {
                return 0 == CompareTo ((Performance)other);
            }
            return false;
        }

        override public int GetHashCode ()
        {
            return (new double[] { Pace (), Distance }).GetHashCode ();
        }

        /// <summary>
        /// The pace in minutes per mile of the performance.
        /// </summary>
        public double Pace ()
        {
            return Time.Seconds / Distance * 60;
        }
        
        override public string ToString()
        {
            return Time + " run by " + Runner.Name;
        }
    }
}