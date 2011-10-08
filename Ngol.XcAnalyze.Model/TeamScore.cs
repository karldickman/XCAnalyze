using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Hytek.Interfaces;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A team's score at a race.
    /// </summary>
    public class TeamScore : IComparable<TeamScore>, ITeamScore
    {
        #region Properties

        #region Physical implementation

        private Race _race;
        private Team _team;

        #endregion

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

        /// <inheritdoc />
        public int? Score
        {
            get;
            set;
        }

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

        /// <inheritdoc />
        public double TopFiveAverage
        {
            get { return GetTopXAverage(5).Time.Value; }
        }


        /// <inheritdoc />
        public double TopSevenAverage
        {
            get { return GetTopXAverage(7).Time.Value; }
        }

        /// <summary>
        /// The collection of runners that can be manipulated externally
        /// to this class.
        /// </summary>
        protected readonly ICollection<Performance> RunnerCollection;

        int ITeamScore.FinisherCount
        {
            get { return Runners.Count(); }
        }

        IEnumerable<IPerformance> ITeamScore.Performances
        {
            get { return Runners.Cast<IPerformance>(); }
        }

        ITeam ITeamScore.Team
        {
            get { return Team; }
        }

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
        public TeamScore(Race race, Team team) : this(race, team, null)
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
                RunnerCollection = new List<Performance>();
            }
            else
            {
                RunnerCollection = runners.ToList();
            }
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return this == other ? true : Equals(other as TeamScore);
        }

        /// <summary>
        /// Determines whether the specified <see cref="TeamScore"/> is equal to the current <see cref="Ngol.XcAnalyze.Model.TeamScore"/>.
        /// </summary>
        /// <param name='that'>
        /// The <see cref="TeamScore"/> to compare with the current <see cref="Ngol.XcAnalyze.Model.TeamScore"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="TeamScore"/> is equal to the current
        /// <see cref="Ngol.XcAnalyze.Model.TeamScore"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TeamScore that)
        {
            if(that == null)
            {
                return false;
            }
            if(this == that)
            {
                return true;
            }
            return Race.Equals(that.Race) && Team.Equals(that.Team);
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

        #endregion

        #region Methods

        /// <summary>
        /// Compares the breakAt runner of this team with the the breakAt runner
        /// of the other team.
        /// </summary>
        protected static int BreakTie(ITeamScore item1, ITeamScore item2, int breakAt)
        {
            if(item1.FinisherCount < breakAt && item2.FinisherCount < breakAt)
            {
                return 0;
            }
            if(item1.FinisherCount < breakAt)
            {
                return 1;
            }
            if(item2.FinisherCount < breakAt)
            {
                return -1;
            }
            IPerformance xThPerformance1 = item1.Performances.ElementAt(breakAt - 1);
            IPerformance xThPerformance2 = item2.Performances.ElementAt(breakAt - 1);
            int points1 = xThPerformance1.Points.Value;
            int points2 = xThPerformance2.Points.Value;
            return points1.CompareTo(points2);
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
                score += Runners.ElementAt(i).Points;
            }
            return score;
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
        protected Performance GetTopXAverage(int x)
        {
            double sum = 0.0;
            int number = x;
            if(Runners.Count() < x)
            {
                number = Runners.Count();
            }
            sum = Runners.Take(number).Sum(r => r.Time.Value);
            return new Performance(Race, sum / number);
        }

        #endregion

        #region IComparable implementation

        /// <inheritdoc />
        public int CompareTo(TeamScore that)
        {
            return ((IComparable<ITeamScore>)this).CompareTo(that);
        }

        int IComparable<ITeamScore>.CompareTo(ITeamScore that)
        {
            if(that == null)
                throw new ArgumentNullException("that");
            if(ReferenceEquals(this, that))
                return 0;
            if(Score.HasValue && !that.Score.HasValue)
                return 1;
            if(!Score.HasValue && that.Score.HasValue)
                return -1;
            int comparison;
            if(Score.HasValue && that.Score.HasValue)
            {
                comparison = Score.Value.CompareTo(that.Score.Value);
                if(comparison != 0)
                    return comparison;
            }
            comparison = BreakTie(this, that, 6);
            if(comparison != 0)
                return comparison;
            comparison = BreakTie(this, that, 7);
            if(comparison != 0)
                return comparison;
            return TopFiveAverage.CompareTo(that.TopFiveAverage);
        }
        
        #endregion
    }
}
