using System;
using System.Collections.Generic;

namespace XcAnalyze.Model {
    
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race : IComparable<Race>
    {
        private string city;
        private Date date;
        private int distance;
        private Gender gender;
        private string meet;
        private List<Performance> results;
        private List<TeamScore> scores;
        private string state;
        private string venue;
        
        /// <summary>
        /// The city in which the meet was held.
        /// </summary>
        public string City
        {
            get { return city; }
        }
        
        /// <summary>
        /// The date on which the race was held.
        /// </summary>
        public Date Date
        {
            get { return date; }
        }
        
        /// <summary>
        /// The day of the month on which this race occurred.
        /// </summary>
        public int Day
        {
            get { return Date.Day; }
        }

        /// <summary>
        /// The length of the race.
        /// </summary>
        public int Distance
        {
            get { return distance; }
        }

        /// <summary>
        /// Was it a men's race or a women's race?
        /// </summary>
        public Gender Gender
        {
            get { return gender; }
        }
        
        public string Location
        {
            get { return venue + ", " + city + ", " + state; }
        }
        
        /// <summary>
        /// The name of the meet.
        /// </summary>
        public string Meet
        {
            get { return meet; }
        }
        
        /// <summary>
        /// The month in which this race occurred.
        /// </summary>
        public int Month
        {
            get { return Date.Month; }
        }
        
        /// <summary>
        /// The results of the meet.
        /// </summary>
        public List<Performance> Results
        {
            get { return results; }
        }
        
        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        public List<TeamScore> Scores
        {
            get { return scores; }
        }
        
        /// <summary>
        /// The state in which the meet was held.
        /// </summary>
        public string State
        {
            get { return state; }
        }
        
        /// <summary>
        /// The year in which this race occurred.
        /// </summary>
        public int Year
        {
            get { return Date.Year; }
        }
        
        /// <summary>
        /// The name of the venue.
        /// </summary>
        public string Venue
        {
            get { return venue; }
        }    

        public Race (string meet, Date date, Gender gender, int distance, string venue, string city, string state) : this(meet, date, gender, distance, venue, city, state, false, new List<Performance>())
        {
        }

        public Race (string meet, Date date, Gender gender, int distance, string venue, string city, string state, bool scoreMeet, List<Performance> results)
        {
            this.date = date;
            this.gender = gender;
            this.distance = distance;
            this.meet = meet;
            this.venue = venue;
            this.city = city;
            this.state = state;
            this.results = results;
            results.Sort ();
            if (scoreMeet)
            {
                Score ();
            }
        }
        
        public void AddResult (Performance result)
        {
            results.Add (result);
            results.Sort ();
        }

        /// <summary>
        /// Races are ordered first by date, then by name of meet, then by location, then by gender.
        /// </summary>
        public int CompareTo (Race other)
        {
            int comparison;
            IComparable[] fields = { Year, Month, Day, meet, venue, city, state, (IComparable)gender };
            IComparable[] otherFields = { other.Year, other.Month, other.Day, other.meet,
                other.venue, other.city, other.state, (IComparable)other.gender };
            if (this == other)
            {
                return 0;
            }
            for (int i = 0; i < fields.Length; i++)
            {
                comparison = fields[i].CompareTo (otherFields[i]);
                if (comparison != 0) 
                {
                    return comparison;
                }
            }
            return 0;
        }

        public override bool Equals (object other)
        {
            if (this == other) {
                return true;
            }
            if (other is Race) {
                return 0 == CompareTo ((Race)other);
            }
            return false;
        }

        public override int GetHashCode ()
        {
            return ToString ().GetHashCode ();
        }

        public void Score ()
        {
            Dictionary<School, TeamScore> scores = new Dictionary<School, TeamScore> ();
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
            this.scores = scoreList;
        }

        public override string ToString ()
        {
            string result;
            if (gender.IsMale ()) {
                result = "Men";
            } else {
                result = "Women";
            }
            return result + "'s " + distance + " m run, " + meet + " (" + date + "), " + venue + ", " + city + ", " + state;
        }
        
    }
}