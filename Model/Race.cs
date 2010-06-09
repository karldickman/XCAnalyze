using System;
using System.Collections.Generic;

namespace XCAnalyze.Model {
    
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race : IComparable<Race>
    {        
        virtual public Date Date
        {
            get
            {
                return Meet.Date;
            }
            
            protected internal set
            {
                Meet.Date = value;
            }
        }
        
        /// <summary>
        /// The length of the race.
        /// </summary>
        virtual public int Distance { get; protected internal set; }

        /// <summary>
        /// Was it a men's race or a women's race?
        /// </summary>
        virtual public Gender Gender { get; protected internal set; }

        /// <summary>
        /// The meet of which this race is a part.
        /// </summary>
        virtual public Meet Meet { get; protected internal set; }
        
        virtual public string Name {
            get
            {
                return Meet.Name;
            }
            
            protected internal set
            {
                Meet.Name = value;
            }
        }

        /// <summary>
        /// The results of the meet.
        /// </summary>
        virtual public List<Performance> Results { get; protected internal set; }

        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        virtual public IList<TeamScore> Scores { get; protected internal set; }
        
        virtual public Venue Venue { get { return Meet.Venue; } }
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="date">
        /// The <see cref="Date"/> on which this races was held.
        /// </param>
        /// <param name="gender">
        /// A <see cref="Gender"/>.  Was this a men's or a women's race.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>
        public Race (Gender gender, int distance)
            : this(gender, distance, new List<Performance>()) {}
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="date">
        /// The <see cref="Date"/> on which this races was held.
        /// </param>
        /// <param name="gender">
        /// A <see cref="Gender"/>.  Was this a men's or a women's race.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>
        public Race(Gender gender, int distance, List<Performance> results)
            : this(null, gender, distance, results) {}
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The meet which this race is a part of.
        /// </param>
        /// <param name="date">
        /// The <see cref="Date"/> on which this races was held.
        /// </param>
        /// <param name="gender">
        /// A <see cref="Gender"/>.  Was this a men's or a women's race.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="venue">
        /// The venue where this race was held.
        /// </param>
        /// <param name="city">
        /// The city in which this race was held.
        /// </param>
        /// <param name="state">
        /// The state in which this race was held.
        /// </param>
        /// <param name="scoreMeet">
        /// Should this meet be scored right away or not?
        /// </param>
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>       
        public Race(Meet meet, Gender gender, int distance,
            List<Performance> results)
            : this(meet, gender, distance, results, false) {}
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The meet which this race is a part of.
        /// </param>
        /// <param name="date">
        /// The <see cref="Date"/> on which this races was held.
        /// </param>
        /// <param name="gender">
        /// A <see cref="Gender"/>.  Was this a men's or a women's race.
        /// </param>
        /// <param name="distance">
        /// The length of the race.
        /// </param>
        /// <param name="venue">
        /// The venue where this race was held.
        /// </param>
        /// <param name="city">
        /// The city in which this race was held.
        /// </param>
        /// <param name="state">
        /// The state in which this race was held.
        /// </param>
        /// <param name="scoreMeet">
        /// Should this meet be scored right away or not?
        /// </param>
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>
        public Race (Meet meet, Gender gender, int distance, 
            List<Performance> results, bool scoreMeet)
        {
            Gender = gender;
            Distance = distance;
            Meet = meet;
            Results = results;
            Results.Sort ();
            if (scoreMeet)
            {
                Score ();
            }
        }
        
        /// <summary>
        /// Add a new result to the race.
        /// </summary>
        /// <param name="result">
        /// The <see cref="Performance"/> to add.
        /// </param>
        public void AddResult (Performance result)
        {
            Results.Add (result);
            Results.Sort ();
        }
        
        /// <summary>
        /// Races are compared first by date, then by name, then by gender.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Race"/> to compare with.
        /// </param>
        public int CompareTo (Race other)
        {
            int comparison;
            if (this == other) 
            {
                return 0;
            }
            comparison = Date.CompareTo (other.Date);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = ObjectComparer<string>.Compare(Name, other.Name, -1);
            if (comparison != 0)
            {
                return comparison;
            }
            return Gender.CompareTo(other.Gender);
        }
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Race)
            {
                return 0 == CompareTo((Race)other);
            }
            return false;
        }
        
        override public int GetHashCode()
        {
            return ("" + Date + Name + Gender).GetHashCode();
        }
        
        /// <summary>
        /// Score the race.
        /// </summary>
        public void Score ()
        {
            Dictionary<School, TeamScore> scores;
            if (Results.Count == 0)
            {
                Scores = new List<TeamScore> ();
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
                    scores[result.School].Runners.Add (result);
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
            //Create the final list
            List<TeamScore> scoreList = new List<TeamScore> ();
            foreach (TeamScore score in scores.Values) 
            {
                scoreList.Add (score);
            }
            scoreList.Sort ();
            Scores = scoreList;
        }
        
        override public string ToString()
        {
            string gender = Gender.IsMale ? "Men's" : "Women's";
            return Name + " (" + Date + ") " + gender + " " + Distance + " m run.";
        }
    }
}