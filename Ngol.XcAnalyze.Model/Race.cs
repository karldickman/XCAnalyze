using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Iesi.Collections.Generic;
using Ngol.Hytek.Interfaces;
using Ngol.Utilities.Collections.Extensions;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race : ICloneable, IRace
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
        public virtual ISet<Runner> DidNotFinish
        {
            get;
            protected set;
        }

        /// <summary>
        /// The length of the race.
        /// </summary>
        public virtual int Distance
        {
            get;
            set;
        }

        /// <summary>
        /// Was it a men's race or a women's race?
        /// </summary>
        public virtual Gender Gender
        {
            get;
            set;
        }

        /// <summary>
        /// The number used to identify this race.
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// Has the race been scored?
        /// </summary>
        public virtual bool IsScored
        {
            get;
            set;
        }

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
                    throw new ArgumentNullException("value");
                _meetInstance = value;
            }
        }

        /// <summary>
        /// The results of the meet.
        /// </summary>
        public virtual IDictionary<Runner, Performance> Results
        {
            get;
            protected set;
        }

        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        public virtual IEnumerable<TeamScore> Score
        {
            get { return ScoresCollection; }
        }

        /// <summary>
        /// The score of this meet.
        /// </summary>
        protected readonly ICollection<TeamScore> ScoresCollection;

        IMeet IRace.Meet
        {
            get { return MeetInstance; }
        }

        IEnumerable<IPerformance> IRace.Results
        {
            get { return Results.Values.Sorted().Cast<IPerformance>(); }
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
            DidNotFinish = new HashedSet<Runner>();
            Results = new Dictionary<Runner, Performance>();
            ScoresCollection = new List<TeamScore>();
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

        /// <summary>
        /// Overload of == operator that delecates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(Race race1, Race race2)
        {
            if(ReferenceEquals(race1, race2))
            {
                return true;
            }
            if(ReferenceEquals(race1, null) || ReferenceEquals(race2, null))
            {
                return false;
            }
            return race1.Equals(race2);
        }

        /// <summary>
        /// Overload of != operator that delecates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(Race race1, Race race2)
        {
            return !(race1 == race2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Race)
            {
                return Equals((Race)other);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (MeetInstance.Meet.Name + MeetInstance.Date + Gender + Distance).ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} m run.", Distance);
        }

        /// <summary>
        /// Check whether two <see cref="Race" />s are equal.
        /// </summary>
        protected bool Equals(Race that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            if(ReferenceEquals(this, that))
            {
                return true;
            }
            return MeetInstance == that.MeetInstance && Gender == that.Gender;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Score the race.
        /// </summary>
        public virtual void ComputeScore()
        {
            if(IsScored)
            {
                return;
            }
            Dictionary<Team, TeamScore> scores;
            if(Results.Count == 0)
                return;
            scores = new Dictionary<Team, TeamScore>();
            // Add the runners to the school
            // Sort the results
            IEnumerable<Performance> results = Results.Values.Sorted();
            foreach(Performance result in results)
            {
                if(result.Team != null)
                {
                    result.Points = 0;
                    if(!scores.ContainsKey(result.Team))
                    {
                        scores[result.Team] = new TeamScore(this, result.Team);
                    }
                    scores[result.Team].AddRunner(result);
                }
            }
            //Tag runners on teams with fewer than five as scoreless
            //Tag runner 8 and beyond on each team as scorelss
            foreach(TeamScore score in scores.Values)
            {
                if(score.Runners.Count() < 5)
                {
                    foreach(Performance runner in score.Runners)
                    {
                        runner.Points = null;
                    }
                }

                else if(score.Runners.Count() > 7)
                {
                    foreach(Performance runner in score.Runners.Skip(7))
                    {
                        runner.Points = null;
                    }
                }
            }
            // Tag first runner on a complete team with a score as the winner
            Performance firstRunnerOnCompleteTeam = results.FirstOrDefault(r => r.Points != null);
            if(firstRunnerOnCompleteTeam == default(Performance))
                return;
            firstRunnerOnCompleteTeam.Points = 1;
            // Tag each runner with their points
            results.Where(r => r.Points != null).ForEachIndexedPair(1, (runner, previous, points) =>
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

        #region ICloneable implementation

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
