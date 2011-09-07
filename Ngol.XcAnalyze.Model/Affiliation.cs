using System;
using System.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// Describes in which Year a Runner ran for a particular Team.
    /// </summary>
    public class Affiliation : ICloneable
    {
        #region Properties

        /// <summary>
        /// TODO DELETE ME!
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Runner"/> affiliated with a <see cref="Team" />.
        /// </summary>
        public virtual Runner Runner
        {
            get;
            set;
        }

        /// <summary>
        /// The year in which the <see cref="Runner" /> was affiliated with the <see cref="Team" />.
        /// </summary>
        public virtual int Season
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Team"/> with which a <see cref="Runner" /> is affiliated in a particular season.
        /// </summary>
        public virtual Team Team
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new <see cref="Affiliation" />.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who is affiliated with the team.
        /// </param>
        /// <param name="team">
        /// The <see cref="Team"/> the runner ran for.
        /// </param>
        /// <param name="season">
        /// The season in which the affiliation occurred.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="runner"/> or <paramref name="team"/>
        /// is <see langword="null" />.
        /// </exception>
        public Affiliation(Runner runner, Team team, int season)
        {
            if(runner == null)
                throw new ArgumentNullException("runner");
            if(team == null)
                throw new ArgumentNullException("team");
            Runner = runner;
            Team = team;
            Season = season;
        }

        /// <summary>
        /// Construct a new <see cref="Affiliation" />.
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Affiliation()
        {
        }

        #endregion

        #region Inherited methods

        /// <summary>
        /// Overload of == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(Affiliation affiliation1, Affiliation affiliation2)
        {
            if(ReferenceEquals(affiliation1, affiliation2))
            {
                return true;
            }
            if(ReferenceEquals(affiliation1, null) || ReferenceEquals(affiliation2, null))
            {
                return false;
            }
            return affiliation1.Equals(affiliation2);
        }


        /// <summary>
        /// Overload of != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(Affiliation affiliation1, Affiliation affiliation2)
        {
            return !(affiliation1 == affiliation2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as Affiliation);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0}, {1} ({2}", Runner.FullName, Team.Name, Season);
        }

        /// <summary>
        /// Check whether to <see cref="Affiliation" />s are equal.
        /// </summary>
        protected bool Equals(Affiliation that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            if(ReferenceEquals(this, that))
            {
                return true;
            }
            return Team == that.Team && Runner == that.Runner && Season == that.Season;
        }

        #endregion

        #region ICloneable implementation

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
        
        #endregion
    }
}
