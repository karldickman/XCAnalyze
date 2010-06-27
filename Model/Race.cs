using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{    
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race
    {
        private IXList<Performance> _results;
        
        private IXList<TeamScore> _scores;
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The meet which this race is part of.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        public Race (Meet meet, int distance)
        : this(meet, distance, new XList<Performance> ()) { }

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> which this race is a part of.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>       
        public Race (Meet meet, int distance, XList<Performance> results)
        : this(meet, distance, results, false) { }

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> which this race is a part of.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>
        /// <param name="scoreMeet">
        /// Should this meet be scored right away or not?
        /// </param>
        public Race (Meet meet, int distance, XList<Performance> results,
            bool scoreMeet)
        {
            Distance = distance;
            Meet = meet;
            _results = results;
            _results.Sort ();
            if (scoreMeet) {
                Score ();
            }
        }
        
        /// <summary>
        /// The date on which this race was held.
        /// </summary>
        public DateTime Date
        {
            get { return Meet.Date; }
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
            get
            {
                if(this == Meet.MensRace)
                {
                    return Gender.MALE;
                }
                return Gender.FEMALE;
            }
        }

        /// <summary>
        /// The meet of which this race is a part.
        /// </summary>
        public Meet Meet { get; protected internal set; }
        
        /// <summary>
        /// The name of the meet of which this race is a part.
        /// </summary>
        public string Name
        {
            get { return Meet.Name; }
        }

        /// <summary>
        /// The results of the meet.
        /// </summary>
        public IList<Performance> Results
        {
            get { return _results.AsReadOnly (); }
        }

        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        public IList<TeamScore> Scores
        {
            get { return _scores.AsReadOnly (); }
        }
        
        /// <summary>
        /// The location of the race.
        /// </summary>
        public Venue Location { get { return Meet.Location; } }
        
        /// <summary>
        /// Add a new result to the race.
        /// </summary>
        /// <param name="result">
        /// The <see cref="Performance"/> to add.
        /// </param>
        public void Add (Performance result)
        {
            _results.Add (result);
            _results.Sort ();
        }
        
        /// <summary>
        /// Delete one of the race results.
        /// </summary>
        /// <param name="result">
        /// The <see cref="Performance"/> to delete.
        /// </param>
        public void Delete (Performance result)
        {
            _results.Remove (result);
        }
        
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
            return Meet.Equals(other.Meet) && Gender == other.Gender;
        }
        
        override public int GetHashCode ()
        {
            return Meet.GetHashCode() + Distance;
        }
        
        /// <summary>
        /// Score the race.
        /// </summary>
        public void Score ()
        {
            Dictionary<School, TeamScore> scores;
            if (Results.Count == 0)
            {
                _scores = new XList<TeamScore> ();
                return;
            }
            scores = new Dictionary<School, TeamScore> ();
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
        }
        
        override public string ToString()
        {
            return Distance + " m run.";
        }
    }
}