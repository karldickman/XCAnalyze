using System;
using System.Collections.Generic;
using System.Linq;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race : ICloneable
    {
        #region Properties

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
        public int ID
        {
            get;
            set;
        }

        /*/// <summary>
        /// Has the race been scored?
        /// </summary>
        public bool IsScored
        {
            get;
            set;
        }*/

        /// <summary>
        /// The meet of which this race is a part.
        /// </summary>
        public MeetInstance MeetInstance
        {
            get;
            set;
        }

        /*/// <summary>
        /// The results of the meet.
        /// </summary>
        public IList<Performance> Results
        {
            get;
            set;
        }*/

        /*/// <summary>
        /// The team scores of the meet.
        /// </summary>
        public IEnumerable<TeamScore> Scores
        {
            get { return ScoresCollection; }
        }*/

        /*protected readonly ICollection<TeamScore> ScoresCollection
        {
            get;
            set;
        }*/

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the race.
        /// </param>
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
        /// Thrown if <paramref name="meetInstance"/> or <paramref name="gender"/>
        /// is <see langword="null" />.
        /// </exception>
        public Race(int id, MeetInstance meetInstance, Gender gender, int distance)
        {
            ID = id;
            MeetInstance = meetInstance;
            Distance = distance;
            Gender = gender;
            //IsScored = false;
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
        public void Score()
        {
            //if(IsScored)
            //{
            //    return;
            //}
            Dictionary<Team, TeamScore> scores;
            //if(Results.Count == 0)
            //{
            //    _scores = new XList<TeamScore>();
            //    return;
            //}
            scores = new Dictionary<Team, TeamScore>();
            //Add the runners to the school
            /*foreach(Performance result in Results)
            {
                if(result.School != null)
                {
                    result.Points = 0;
                    if(!scores.ContainsKey(result.School))
                    {
                        scores[result.School] = new TeamScore(this, result.School);
                    }
                    scores[result.School].AddRunner(result);
                }
            }*/
            //Tag runners on teams with fewer than five as scoreless
            //Tag runner 8 and beyond on each team as scorelss
            foreach(TeamScore score in scores.Values)
            {
                if(score.Runners.Count() < 5)
                {
                    //foreach(Performance runner in score.Runners)
                    {
                        //runner.Points = null;
                    }
                }

                else if(score.Runners.Count() > 7)
                {
                    for(int i = 7; i < score.Runners.Count(); i++)
                    {
                        //score.Runners[i].Points = null;
                    }
                }
            }
            //Find first runner with a score
            //int firstWithScore;
            //for(firstWithScore = 0; firstWithScore < Results.Count; firstWithScore++)
            {
                //if(Results[firstWithScore].Points != null)
                {
                    //Results[firstWithScore].Points = 1;
                    //break;
                }
            }
            //Tag each runner with their points
            //if(firstWithScore < Results.Count)
            {
                //Performance previous = Results[firstWithScore];
                int points = 2;
                //for(int i = firstWithScore + 1; i < Results.Count; i++)
                {
                    //if(Results[i].Points != null)
                    {
                        //if(Results[i].Time != previous.Time)
                        {
                            //Results[i].Points = points;
                        }
                        //else
                        {
                            //Results[i].Points = previous.Points;
                        }
                        points++;
                    }
                }
            }
            //Create the final list
            // ScoresCollection = scores.Values.Sort();
            //IsScored = true;
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
