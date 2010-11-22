using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A team's score at a race.
    /// </summary>
    public partial class TeamScore : IComparable<TeamScore>
    {
        #region Properties
        
        #region Fields
        
        private Race _race;
        
        private IXList<Performance> _runners;
        
        private Team _team;
        
        #endregion
        
        /// <summary>
        /// The team which earned the score.
        /// </summary>
        public Team Team
        {
            get { return _team; }
            
            protected set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Team cannot be null.");
                }
                _team = value;
            }
        }

        /// <summary>
        /// The race to which this score belongs.
        /// </summary>
        public Race Race
        {
            get { return _race; }
            
            protected set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Race cannot be null.");
                }
                _race = value;
            }
        }

        /// <summary>
        /// The runners who were on the team at that particular race.
        /// </summary>
        public IList<Performance> Runners
        {
            get { return _runners.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    _runners = new XList<Performance> ();
                }
                else
                {
                    _runners = new XList<Performance> (value);
                }
            }
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
        public TeamScore (Race race, Team team)
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
        /// The <see cref="IList<Performance>"/> of runners who were on the team.
        /// </param>
        protected TeamScore (Race race, Team team, IList<Performance> runners)
        {
            Race = race;
            Team = team;
            Runners = runners;
        }
        
        #endregion
        
        #region Inherited methods
                        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is TeamScore)
            {
                return 0 == CompareTo((TeamScore)other);
            }
            return false;
        }
        
        override public int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        override public string ToString ()
        {
            return Team.Name + " " + Score ();
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Compares the breakAt runner of this team with the the breakAt runner
        /// of the other team.
        /// </summary>
        protected static int BreakTie (TeamScore item1, TeamScore item2,
            int breakAt)
        {
            if (item1.Runners.Count < breakAt && item2.Runners.Count < breakAt)
            {
                return 0;
            }
            if (item1.Runners.Count < breakAt)
            {
                return 1;
            }
            if (item2.Runners.Count < breakAt)
            {
                return -1;
            }
            return item1.Runners[breakAt - 1].Points.Value.CompareTo (
                item2.Runners[breakAt - 1].Points.Value);
        }
        
        /// <summary>
        /// Add a new runner from the race results.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Performance"/> to add.
        /// </param>
        public void AddRunner (Performance runner)
        {
            _runners.Add (runner);
        }        
        
        /// <summary>
        /// A teams score is the sum of the points earned by their first five runners.  Their score is incomplete if
        /// they failed to field five runners.
        /// </summary>
        public int? Score ()
        {
            if (Runners.Count < 5) {
                return null;
            }
            int? score = 0;
            for (int i = 0; i < 5; i++) {
                score += Runners[i].Points;
            }
            return score;
        }
        
        /// <summary>
        /// The averate time of the top 5 runners on the team.
        /// </summary>
        public Performance TopFiveAverage ()
        {
            return TopXAverage (5);
        }
        
        /// <summary>
        /// The average time of the top 7 runners on the team.
        /// </summary>
        public Performance TopSevenAverage ()
        {
            return TopXAverage (7);
        }
        
        /// <summary>
        /// The average time of the top x runners on the team.
        /// </summary>
        protected Performance TopXAverage(int x)
        {
            double sum = 0.0;
            int number;
            if (Runners.Count < x)
            {
                number = Runners.Count;
            }
            else
            {
                number = x;
            }
            for (int i = 0; i < number; i++)
            {
                if(Runners[i].Time != null)
                {
                    sum += Runners[i].Time.Value;
                }
            }
            return new Performance(null, Race, sum / number);
        }
        
        #endregion
        
        #region IComparable implementation
        
        /// <summary>
        /// Team scores are compared first by the numerical score, then by the sixth runner, then by the seventh, then
        /// by the name of the school.
        /// </summary>
        public int CompareTo (TeamScore other)
        {
            int comparison;
            int? score, otherScore;
            if (this == other)
            {
                return 0;
            }
            score = Score ();
            otherScore = other.Score ();
            if (score != otherScore)
            {
                if (score == null)
                {
                    return 1;
                }
                if (otherScore == null)
                {
                    return -1;
                }
                comparison = score.Value.CompareTo (otherScore.Value);
                if (comparison != 0) 
                {
                    return comparison;
                }
            }
            comparison = BreakTie (this, other, 6);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = BreakTie (this, other, 7);
            if (comparison != 0)
            {
                return comparison;
            }
            return TopFiveAverage ().CompareTo (other.TopFiveAverage ());
        }

        #endregion
    }
}