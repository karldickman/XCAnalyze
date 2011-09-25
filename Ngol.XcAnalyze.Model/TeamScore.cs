using System;
using System.Collections.Generic;
using System.Linq;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A team's score at a race.
    /// </summary>
    public class TeamScore : IComparable<TeamScore>
    {
        #region Properties

        #region Physical implementation

        private Race _race;
        private Team _team;

        #endregion

        /// <summary>
        /// The team which earned the score.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public Team Team
        {
            get { return _team; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _team = value;
            }
        }

        /// <summary>
        /// The race to which this score belongs.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public Race Race
        {
            get { return _race; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _race = value;
            }
        }

        /// <summary>
        /// The runners who were on the team at that particular race.
        /// </summary>
        public IEnumerable<Performance> Runners
        {
            get { return RunnerCollection; }
        }

        /// <summary>
        /// The collection of runners that can be manipulated externally
        /// to this class.
        /// </summary>
        protected readonly ICollection<Performance> RunnerCollection;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new team score.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> from which the score comes.
        /// </param>
        /// <param name="team">
        /// The <see cref="Team"/> to whome the score belongs.
        /// </param>
        public TeamScore(Race race, Team team)
            : this(race, team, null)
        {
        }

        /// <summary>
        /// Create a new team score.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> from which the score comes.
        /// </param>
        /// <param name="team">
        /// The <see cref="Team"/> to whome the score belongs.
        /// </param>
        /// <param name="runners">
        /// A collection of runners who were on the team.
        /// </param>
        protected TeamScore(Race race, Team team, IEnumerable<Performance> runners)
        {
            Race = race;
            Team = team;
            if(runners == null)
            {
                runners = new List<Performance>();
            }
            else
            {
                RunnerCollection = runners.ToList();
            }
        }

        #endregion

        #region Inherited methods

        /// <summary>
        /// Overload of == operator that delegtes to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(TeamScore score1, TeamScore score2)
        {
            if(ReferenceEquals(score1, score2))
            {
                return true;
            }
            if(ReferenceEquals(score1, null) || ReferenceEquals(score2, null))
            {
                return false;
            }
            return score1.Equals(score2);
        }

        /// <summary>
        /// Overload of != operator that delegtes to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(TeamScore score1, TeamScore score2)
        {
            return !(score1 == score2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as TeamScore);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} {1}", Team.Name, CalculateScore());
        }

        /// <summary>
        /// Check if two <see cref="TeamScore" />s are equal.
        /// </summary>
        protected bool Equals(TeamScore that)
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

        #region Methods

        /// <summary>
        /// Compares the breakAt runner of this team with the the breakAt runner
        /// of the other team.
        /// </summary>
        protected static int BreakTie(TeamScore item1, TeamScore item2, int breakAt)
        {
            if(item1.Runners.Count() < breakAt && item2.Runners.Count() < breakAt)
            {
                return 0;
            }
            if(item1.Runners.Count() < breakAt)
            {
                return 1;
            }
            if(item2.Runners.Count() < breakAt)
            {
                return -1;
            }
            return int.MaxValue;//item1.Runners[breakAt - 1].Points.Value.CompareTo(item2.Runners[breakAt - 1].Points.Value);
        }

        /// <summary>
        /// A teams score is the sum of the points earned by their first five runners.  Their score is incomplete if
        /// they failed to field five runners.
        /// </summary>
        public int? CalculateScore()
        {
            if(Runners.Count() < 5)
            {
                return null;
            }
            int? score = 0;
            for(int i = 0; i < 5; i++)
            {
                //score += Runners[i].Points;
            }
            return score;
        }

        /// <summary>
        /// The averate time of the top 5 runners on the team.
        /// </summary>
        public Performance CalculateTopFiveAverage()
        {
            return CalculateTopXAverage(5);
        }

        /// <summary>
        /// The average time of the top 7 runners on the team.
        /// </summary>
        public Performance CalculateTopSevenAverage()
        {
            return CalculateTopXAverage(7);
        }

        /// <summary>
        /// Add a new runner from the race results.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Performance"/> to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="runner"/> is <see langword="null" />.
        /// </exception>
        public void AddRunner(Performance runner)
        {
            if(runner == null)
                throw new ArgumentNullException("runner");
            RunnerCollection.Add(runner);
        }

        /// <summary>
        /// The average time of the top x runners on the team.
        /// </summary>
        protected Performance CalculateTopXAverage(int x)
        {
            double sum = 0.0;
            int number;
            if(Runners.Count() < x)
            {
                number = Runners.Count();
            }
            else
            {
                number = x;
            }
            for(int i = 0; i < number; i++)
            {
                //if(Runners[i].Time != null)
                {
                    //sum += Runners[i].Time.Value;
                }
            }
            return new Performance(null, Race, sum / number);
        }

        #endregion

        #region IComparable implementation

        /// <inheritdoc />
        public int CompareTo(TeamScore that)
        {
            if(ReferenceEquals(that, null))
            {
                throw new ArgumentNullException("that");
            }
            int comparison;
            int? score, otherScore;
            if(ReferenceEquals(this, that))
            {
                return 0;
            }
            score = CalculateScore();
            otherScore = that.CalculateScore();
            if(score != otherScore)
            {
                if(score == null)
                {
                    return 1;
                }
                if(otherScore == null)
                {
                    return -1;
                }
                comparison = score.Value.CompareTo(otherScore.Value);
                if(comparison != 0)
                {
                    return comparison;
                }
            }
            comparison = BreakTie(this, that, 6);
            if(comparison != 0)
            {
                return comparison;
            }
            comparison = BreakTie(this, that, 7);
            if(comparison != 0)
            {
                return comparison;
            }
            return int.MaxValue;//TopFiveAverage().CompareTo(that.CalculateTopFiveAverage());
        }
        
        #endregion
    }
}
