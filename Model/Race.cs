using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{    
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race
    {
        #region Properties
        
        #region Fields
        
        private Cell<int> _distance;
        
        private Cell<Gender> _gender;
        
        private Cell<MeetInstance> _meetInstance;
        
        private IXList<Performance> _results;
        
        private IXList<TeamScore> _scores;
        
        #endregion
        
        /// <summary>
        /// The date on which this race was held.
        /// </summary>
        public DateTime Date
        {
            get { return MeetInstance.Date; }
        }
        
        /// <summary>
        /// The length of the race.
        /// </summary>
        public int Distance { get; protected set; }

        /// <summary>
        /// Was it a men's race or a women's race?
        /// </summary>
        public Gender Gender
        {
            get { return _gender.Value; }
            
            protected set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Gender cannot be null.");
                }
                _gender.Value = value;
            }
        }
        
        /// <summary>
        /// The number used to identify this race.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// True if the race has a corresponding entry in the database, 
        /// false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        /// <summary>
        /// True if the race has been changed since it was loaded from the
        /// database, false otherwise.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (IsAttached) 
                {
                    return _distance.IsChanged || _gender.IsChanged ||
                        _meetInstance.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// Has the race been scored?
        /// </summary>
        public bool IsScored { get; protected set; }
        
        /// <summary>
        /// The number that identifies the meet at which this race was held.
        /// </summary>
        public int MeetID
        {
            get { return MeetInstance.MeetID; }
        }
        
        /// <summary>
        /// The meet of which this race is a part.
        /// </summary>
        public MeetInstance MeetInstance
        {
            get { return _meetInstance.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property MeetInstance cannot be null.");
                }
                _meetInstance.Value = value;
            }
        }

        /// <summary>
        /// The results of the meet.
        /// </summary>
        public IList<Performance> Results
        {
            get { return _results.AsReadOnly(); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<Performance> ();
                }
                _results = new XList<Performance>(value);
            }
        }

        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        public IList<TeamScore> Scores
        {
            get { return _scores.AsReadOnly (); }
        }
        
        #endregion
        
        #region Constructors

        protected Race ()
        {
            _meetInstance = new Cell<MeetInstance> ();
            _distance = new Cell<int> ();
            _gender = new Cell<Gender> ();
            _results = new XList<Performance> ();
        }

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> which this race is a part of.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="gender">
        /// Was this a men's or a women's race.
        /// </param>
        public Race (MeetInstance meetInstance, Gender gender, int distance)
        : this()
        {
            MeetInstance = meetInstance;
            Distance = distance;
            Gender = gender;
            IsScored = false;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the race.
        /// </param>
        /// <param name="meet">
        /// The <see cref="Meet"/> which this race is a part of.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="gender">
        /// Was this a men's or a women's race.
        /// </param>
        /// <param name="scoreMeet">
        /// Should this meet be scored right away or not?
        /// </param>
        protected Race (int id, MeetInstance meetInstance, Gender gender,
            int distance)
        : this(meetInstance, gender, distance)
        {
            ID = id;
        }
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the race.
        /// </param>
        /// <param name="meet">
        /// The <see cref="Meet"/> which this race is a part of.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="gender">
        /// Was this a men's or a women's race.
        /// </param>
        /// <param name="scoreMeet">
        /// Should this meet be scored right away or not?
        /// </param>
        public static Race NewEntity (int id, MeetInstance meetInstance,
            Gender gender, int distance)
        {
            Race newRace = new Race (id, meetInstance, gender, distance);
            newRace.IsAttached = true;
            return newRace;
        }
        
        #endregion
        
        #region Inherited methods
                
        override public bool Equals(object other)
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
        
        protected bool Equals (Race other)
        {
            return MeetInstance.Equals(other.MeetInstance) && Gender == other.Gender;
        }
        
        override public int GetHashCode ()
        {
            return ID;
        }
        
                
        override public string ToString()
        {
            return Distance + " m run.";
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Add a new result to the race.
        /// </summary>
        /// <param name="result">
        /// The <see cref="Performance"/> to add.
        /// </param>
        public void AddResult (Performance result)
        {
            _results.Add (result);
            _results.Sort ();
            IsScored = false;
        }
        
        /// <summary>
        /// Add more results to the race.
        /// </summary>
        /// <param name="results">
        /// A <see cref="IEnumerable<Performance>"/> of results to add.
        /// </param>
        public void AddResults (IEnumerable<Performance> results)
        
        {
            _results.AddRange (results);
            _results.Sort ();
            IsScored = false;
        }
        
        /// <summary>
        /// Score the race.
        /// </summary>
        public void Score ()
        {
            if (IsScored) 
            {
                return;
            }
            Dictionary<Team, TeamScore> scores;
            if (Results.Count == 0)
            {
                _scores = new XList<TeamScore> ();
                return;
            }
            scores = new Dictionary<Team, TeamScore> ();
            //Add the runners to the school
            foreach (Performance result in Results) 
            {
                if (result.School != null) 
                {
                    result.Points = 0;
                    if (!scores.ContainsKey (result.School))
                    {
                        scores[result.School] = new TeamScore (this, result.School);
                    }
                    scores[result.School].AddRunner (result);
                }
            }
            //Tag runners on teams with fewer than five as scoreless
            //Tag runner 8 and beyond on each team as scorelss
            foreach (TeamScore score in scores.Values) 
            {
                if (score.Runners.Count < 5)
                {
                    foreach (Performance runner in score.Runners)
                    {
                        runner.Points = null;
                    }
                }
                else if (score.Runners.Count > 7) 
                {
                    for (int i = 7; i < score.Runners.Count; i++) 
                    {
                        score.Runners[i].Points = null;
                    }
                }
            }
            //Find first runner with a score
            int firstWithScore;
            for (firstWithScore = 0; firstWithScore < Results.Count; firstWithScore++) 
            {
                if (Results[firstWithScore].Points != null) 
                {
                    Results[firstWithScore].Points = 1;
                    break;
                }
            }
            //Tag each runner with their points
            if (firstWithScore < Results.Count)
            {
                Performance previous = Results[firstWithScore];
                int points = 2;
                for (int i = firstWithScore + 1; i < Results.Count; i++)
                {
                    if (Results[i].Points != null) 
                    {
                        if (Results[i].Time != previous.Time) 
                        {
                            Results[i].Points = points;
                        }
                        else 
                        {
                            Results[i].Points = previous.Points;
                        }
                        points++;
                    }
                }
            }
            //Create the final list
            IXList<TeamScore> scoreList = new XList<TeamScore> ();
            foreach (TeamScore score in scores.Values) 
            {
                scoreList.Add (score);
            }
            scoreList.Sort ();
            _scores = scoreList;
            IsScored = true;
        }
        
        #endregion
    }
}