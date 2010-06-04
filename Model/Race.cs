using System;
using System.Collections.Generic;

namespace XCAnalyze.Model {
    
    /// <summary>
    /// An instance of a meet.
    /// </summary>
    public class Race : IComparable<Race>
    {
        /// <summary>
        /// The city in which the meet was held.
        /// </summary>
        virtual public string City { get; protected internal set; }

        /// <summary>
        /// The date on which the race was held.
        /// </summary>
        virtual public Date Date { get; protected internal set; }

        public int Day
        {
            get { return Date.Day; }
        }
        
        /// <summary>
        /// The length of the race.
        /// </summary>
        virtual public int Distance { get; protected internal set; }

        /// <summary>
        /// Was it a men's race or a women's race?
        /// </summary>
        virtual public Gender Gender { get; protected internal set; }

        public string Location
        {
            get { return Venue + ", " + City + ", " + State; }
        }

        /// <summary>
        /// The name of the meet.
        /// </summary>
        virtual public string Meet { get; protected internal set; }

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
        public List<Performance> Results { get; protected internal set; }

        /// <summary>
        /// The team scores of the meet.
        /// </summary>
        public List<TeamScore> Scores { get; protected internal set; }

        /// <summary>
        /// The state in which the meet was held.
        /// </summary>
        virtual public string State { get; protected internal set; }

        /// <summary>
        /// The name of the venue.
        /// </summary>
        virtual public string Venue { get; protected internal set; }

        public int Year
        {
            get { return Date.Year; }
        }
        
        protected internal Race(Date date, Gender gender, int distance)
            : this(null, date, gender, distance, null, null, null) {}

        public Race (string meet, Date date, Gender gender, int distance,
            string venue, string city, string state)
            : this(meet, date, gender, distance, venue, city, state, false,
                new List<Performance>()) {}

        public Race (string meet, Date date, Gender gender, int distance,
            string venue, string city, string state, bool scoreMeet,
            List<Performance> results)
        {
            Date = date;
            Gender = gender;
            Distance = distance;
            Meet = meet;
            Venue = venue;
            City = city;
            State = state;
            Results = results;
            results.Sort ();
            if (scoreMeet)
            {
                Score ();
            }
        }
        
        public void AddResult (Performance result)
        {
            Results.Add (result);
            Results.Sort ();
        }

        /// <summary>
        /// Races are ordered first by date, then by name of meet, then by location, then by gender.
        /// </summary>
        public int CompareTo (Race other)
        {
            int comparison;
            IComparable[] fields = { Year, Month, Day, Meet, Venue, City, State,
                (IComparable)Gender };
            IComparable[] otherFields = { other.Year, other.Month, other.Day,
                other.Meet, other.Venue, other.City, other.State,
                (IComparable)other.Gender };
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

        override public bool Equals (object other)
        {
            if (this == other) {
                return true;
            }
            if (other is Race) {
                return 0 == CompareTo ((Race)other);
            }
            return false;
        }

        override public int GetHashCode ()
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
            Scores = scoreList;
        }

        override public string ToString ()
        {
            string result;
            if (Gender.IsMale ()) {
                result = "Men";
            } else {
                result = "Women";
            }
            return result + "'s " + Distance + " m run, " + Meet + " (" + Date + "), " + Location;
        }
    }
}