using System;
using System.ComponentModel;
using System.Linq;
using Ngol.Hytek.Interfaces;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A runners time (in seconds) at a particular race.
    /// </summary>
    public class Performance : IComparable<Performance>, IPerformance
    {
        #region Properties

        #region Physical implementation

        private Race _race;
        private Runner _runner;

        #endregion

        /// <summary>
        /// The number of points the runner earned in the race for this
        /// performance.
        /// </summary>
        public virtual int? Points
        {
            get;
            set;
        }

        /// <summary>
        /// The race whereat the time was run.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual Race Race
        {
            get { return _race; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(Race != value)
                {
                    _race = value;
                }
            }
        }

        /// <summary>
        /// The runner who ran the time.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual Runner Runner
        {
            get { return _runner; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(Runner != value)
                {
                    _runner = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Team" /> with which the <see cref="Performance.Runner" />
        /// was associated when they ran this <see cref="Time" />.
        /// </summary>
        /// <value>
        /// <see langword="null" /> if the <see cref="Performance.Runner" />
        /// was not affiliated with a team.
        /// </value>
        public virtual Team Team
        {
            get
            {
                Team team;
                if(Runner.Affiliations.TryGetValue(Race.Date.Year, out team))
                {
                    return team;
                }
                return null;
            }
        }

        /// <summary>
        /// The time that was run.
        /// </summary>
        public virtual double? Time
        {
            get;
            set;
        }

        int IPerformance.RaceDistance
        {
            get { return Race.Distance; }
        }

        IRunner IPerformance.Runner
        {
            get { return Runner; }
        }

        ITeam IPerformance.Team
        {
            get { return Team; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new <see cref="Performance" />.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who owns the performance.
        /// </param>
        /// <param name="race">
        /// The <see cref="Race"/> at which the performance was run.
        /// </param>
        /// <param name="time">
        /// The <see cref="Time"/> in which the race was run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="runner" /> or <paramref name="race" /> is <see langword="null" />.
        /// </exception>
        public Performance(Runner runner, Race race, double? time) : this()
        {
            Runner = runner;
            Race = race;
            Time = time;
        }

        /// <summary>
        /// Construct a new <see cref="Performance" /> without a <see cref="Runner" />.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> at which the performance was run.
        /// </param>
        /// <param name="time">
        /// The <see cref="Time"/> in which the race was run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="race" /> is <see langword="null" />.
        /// </exception>
        internal Performance(Race race, double time) : this()
        {
            Race = race;
            Time = time;
        }

        /// <summary>
        /// Construct a new <see cref="Performance" />.
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Performance()
        {
        }

        #endregion

        #region Inherited methods

        /// <summary>
        /// Overload of == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(Performance performance1, Performance performance2)
        {
            if(ReferenceEquals(performance1, performance2))
            {
                return true;
            }
            if(ReferenceEquals(performance1, null) || ReferenceEquals(performance2, null))
            {
                return false;
            }
            return performance1.Equals(performance2);
        }

        /// <summary>
        /// Overload of != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(Performance performance1, Performance performance2)
        {
            return !(performance1 == performance2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as Performance);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int runnerHashCode = Runner == null ? 0 : Runner.GetHashCode();
            int raceHashCode = Race == null ? 0 : Race.GetHashCode();
            return runnerHashCode + raceHashCode;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            int minutes = Convert.ToInt32(Math.Floor(Time.Value / 60.0));
            double seconds = Time.Value - 60 * minutes;
            return string.Format("{0}:{1:00.00} run by {2}", minutes, seconds, Runner.FullName);
        }

        /// <summary>
        /// Check if two <see cref="Performance" />s are equal.
        /// </summary>
        protected bool Equals(Performance that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            if(ReferenceEquals(this, that))
            {
                return true;
            }
            return 0 == CompareTo(that);
        }

        #endregion

        #region IComparable implementation

        /// <inheritdoc />
        /// <remarks>
        /// A faster pace is a better performance, and comes before a slower
        /// pace.  If the paces are the same, the longer race is considered the
        /// better performance.
        /// </remarks>
        public virtual int CompareTo(Performance that)
        {
            return ((IComparable<IPerformance>)this).CompareTo(that);
        }

        int IComparable<IPerformance>.CompareTo(IPerformance that)
        {
            if(that == null)
                throw new ArgumentNullException("that");
            if(ReferenceEquals(this, that))
            {
                return 0;
            }
            double? milePace = this.GetMilePace();
            double? otherMilePace = that.GetMilePace();
            int comparison;
            if(milePace == null)
            {
                if(otherMilePace != null)
                {
                    return 1;
                }
                comparison = 0;
            }
            else if(otherMilePace == null)
            {
                return -1;
            }
            else
            {
                comparison = milePace.Value.CompareTo(otherMilePace.Value);
            }
            if(comparison != 0)
            {
                return comparison;
            }
            return ((IPerformance)this).RaceDistance.CompareTo(that.RaceDistance);
        }

        #endregion
    }
}
