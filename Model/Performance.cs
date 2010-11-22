using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A runners time (in seconds) at a particular race.
    /// </summary>
    public class Performance : IComparable<Performance>
    {
        #region Properties
        
        #region Fields
        
        private Cell<double?> _time;
        
        #endregion
         
        /// <summary>
        /// The length of the race whereat the time was run.
        /// </summary>
        public int Distance
        {
            get { return Race.Distance; }
        }
        
        /// <summary>
        /// True if a corresponding entry exists in the database, false
        /// otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        /// <summary>
        /// True if changes have been made since the performance was loaded
        /// from the database, false otherwise.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (IsAttached) 
                {
                    return _time.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// The number of points the runner earned in the race for this
        /// performance.
        /// </summary>
        public int? Points { get; set; }

        /// <summary>
        /// The race whereat the time was run.
        /// </summary>
        public readonly Race Race;
        
        /// <summary>
        /// The number that identifies the race where this performance
        /// was made.
        /// </summary>
        public int RaceID
        {
            get { return Race.ID; }
        }

        /// <summary>
        /// The runner who ran the time.
        /// </summary>
        public readonly Runner Runner;
        
        /// <summary>
        /// The number that identifies the runner that ran this performance.
        /// </summary>
        public int RunnerID
        {
            get { return Runner.ID; }
        }

        /// <summary>
        /// The time that was run.
        /// </summary>
        public double? Time
        {
            get { return _time.Value; }
            
            set { _time.Value = value; }
        }
     
        /// <summary>
        /// The school with which this runner was associated when he ran this
        /// time.
        /// </summary>
        public Team School
        {
            get { return Runner.Teams[Race.MeetInstance.Date.Year]; }
        }     
        
        #endregion
        
        #region Constructors
        
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
        /// <exception cref="ArgumentNullException">
        /// Thrown when runner or race is null.
        /// </exception>
        public Performance (Runner runner, Race race, double? time)
        {
            Runner = runner;
            Race = race;
            _time = new Cell<double?> ();
            Time = time;
            IsAttached = false;
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
        /// <exception cref="ArgumentNullException">
        /// Thrown when runner or race is null.
        /// </exception>
        public static Performance NewEntity (Runner runner, Race race,
            double? time)
        {
            Performance newPerformance = new Performance (runner, race, time);
            newPerformance.IsAttached = true;
            return newPerformance;
        }
        
        #endregion  

        #region Inherited methods
        
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
            return string.Format("{0} {1}", RunnerID, RaceID).GetHashCode();
        }
                        
        override public string ToString()
        {
            return Time + " run by " + Runner.Name;
        }
        
        #endregion
        
        #region IComparable implementation
                        
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
            double? milePace = MilePace ();
            double? otherMilePace = other.MilePace ();
            if (milePace == null)
            {
                if (otherMilePace != null) 
                {
                    return 1;
                }
                comparison = 0;
            }
            else if (otherMilePace == null)
            {
                return -1;
            }
            else
            {
                comparison = milePace.Value.CompareTo (otherMilePace.Value);
            }            
            if (comparison != 0)
            {
                return comparison;
            }
            return Distance.CompareTo (other.Distance);
        }
        
        #endregion

        #region Methods
        
        /// <summary>
        /// The pace in minutes per mile of the performance.
        /// </summary>
        public double? MilePace ()
        {
            if(Time == null)
            {
                return null;
            }
            return Time.Value / Distance * 60 * 1609.344;
        }
        
        #endregion
    }
}