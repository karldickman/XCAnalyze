using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ngol.Hytek.Interfaces;
using Ngol.Utilities.Collections.Extensions;
using SharpArch.Domain.DomainModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race : Entity, IRace
    {
        #region Properties

        #region Physical implementation

        private MeetInstance _meetInstance;

        #endregion

        /// <summary>
        /// The date on which this <see cref="Race" /> was held.
        /// </summary>
        public virtual DateTime Date
        {
            get { return MeetInstance.Date; }
        }

        /// <summary>
        /// The <see cref="Runner" />s who started the <see cref="Race" /> but did not finish.
        /// </summary>
        public virtual ISet<Runner> DidNotFinish { get; protected set; }

        /// <summary>
        /// The length of the race.
        /// </summary>
        public virtual int Distance { get; set; }

        /// <summary>
        /// Was it a men's race or a women's race?
        /// </summary>
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// The number used to identify this race.
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Has the race been scored?
        /// </summary>
        public virtual bool IsScored { get; set; }

        /// <summary>
        /// The meet of which this race is a part.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property to <see langword="null" />.
        /// </exception>
        public virtual MeetInstance MeetInstance
        {
            get { return _meetInstance; }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _meetInstance = value;
            }
        }

        /// <summary>
        /// The name of the <see cref="Race" />.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return string.Format("{0}'s {1} m", Gender == Gender.Male ? "Men" : "Women", Distance);
            }
        }

        /// <summary>
        /// The results of the meet.
        /// </summary>
        public virtual IDictionary<Runner, Performance> Results { get; protected set; }

        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        public virtual IEnumerable<TeamScore> Score
        {
            get { return ScoresCollection; }
        }

        /// <summary>
        /// The <see cref="Venue" /> whereat this <see cref="Race" /> was run.
        /// </summary>
        public virtual Venue Venue
        {
            get { return MeetInstance.Venue; }
        }

        /// <summary>
        /// The score of this meet.
        /// </summary>
        protected ICollection<TeamScore> ScoresCollection { get; set; }

        IMeet IRace.Meet
        {
            get { return MeetInstance; }
        }

        IEnumerable<IPerformance> IRace.Results
        {
            get { return Results.Values.Cast<IPerformance>().Sorted(); }
        }

        IEnumerable<ITeamScore> IRace.Scores
        {
            get { return Score.Cast<ITeamScore>(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meetInstance">
        /// The <see cref="MeetInstance"/> of which this race is a part.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="gender">
        /// Was this a men's or a women's race.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="meetInstance"/>
        /// is <see langword="null" />.
        /// </exception>
        public Race(MeetInstance meetInstance, Gender gender, int distance) : this()
        {
            MeetInstance = meetInstance;
            Distance = distance;
            Gender = gender;
            IsScored = false;
            DidNotFinish = new HashSet<Runner>();
            Results = new Dictionary<Runner, Performance>();
        }

        /// <summary>
        /// Construct a new <see cref="Race" />
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Race()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Score the race.
        /// </summary>
        public virtual void ComputeScore()
        {
            if(IsScored || Results.Count == 0)
            {
                return;
            }
            ScoresCollection = new List<TeamScore>();
            IDictionary<Team, TeamScore> scores = new Dictionary<Team, TeamScore>();
            IEnumerable<Team> teams = Results.Values.Select(result => result.Team).Distinct();
            foreach(Team team in teams)
            {
                scores[team] = new TeamScore(this, team);
            }
            // Add the runners to the school
            // Sort the results
            IEnumerable<Performance> results = Results.Values.Sorted();
            foreach(Performance result in results.Where(r => r.Team != null))
            {
                result.Points = 0;
                scores[result.Team].AddRunner(result);
            }
            //Tag runners on teams with fewer than five as scoreless
            //Tag runner 8 and beyond on each team as scorelss
            foreach(TeamScore score in scores.Values)
            {
                int runnerCount = score.Runners.Count();
                IEnumerable<Performance> scorelessRunners = Enumerable.Empty<Performance>();
                if(runnerCount < 5)
                {
                    scorelessRunners = score.Runners;
                }
                else if(runnerCount > 7)
                {
                    scorelessRunners = score.Runners.Skip(7);
                }
                foreach(Performance runner in scorelessRunners)
                {
                    runner.Points = null;
                }
            }
            // Tag first runner on a complete team with a score as the winner
            Performance firstRunnerOnCompleteTeam = results.FirstOrDefault(r => r.Points != null);
            if(firstRunnerOnCompleteTeam == default(Performance))
            {
                return;
            }
            firstRunnerOnCompleteTeam.Points = 1;
            // Tag each runner with their points
            results.Where(r => r.Points != null)
                   .ForEachIndexedPair(2, (previous, runner, points) =>
            {
                if(runner.Time != previous.Time)
                {
                    runner.Points = points;
                }
                else
                {
                    runner.Points = previous.Points;
                }
            });
            //Create the final list
            ScoresCollection.Clear();
            ScoresCollection.AddRange(scores.Values.Sorted());
            IsScored = true;
        }

        #endregion
    }
}
