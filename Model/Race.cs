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

        /// <summary>
        /// The day of the month on which the race was held.
        /// </summary>
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

        /// <summary>
        /// The location of the race.
        /// </summary>
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
        public IList<TeamScore> Scores { get; protected internal set; }

        /// <summary>
        /// The state in which the meet was held.
        /// </summary>
        virtual public string State { get; protected internal set; }

        /// <summary>
        /// The name of the venue.
        /// </summary>
        virtual public string Venue { get; protected internal set; }

        /// <summary>
        /// The year in which teh race was held.
        /// </summary>
        public int Year
        {
            get { return Date.Year; }
        }
        
        protected internal Race(Date date, Gender gender, int distance)
            : this(null, date, gender, distance, null, null, null) {}

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The meet which this race is an instance of.
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
        public Race (string meet, Date date, Gender gender, int distance,
            string venue, string city, string state)
            : this(meet, date, gender, distance, venue, city, state, false,
                new List<Performance>()) {}
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The meet which this race is an instance of.
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
        /// <param name="results">
        /// The <see cref="List<Performance>"/> of results.
        /// </param>
        public Race(string meet, Date date, Gender gender, int distance,
            string venue, string city, string state, List<Performance> results)
            : this(meet, date, gender, distance, venue, city, state, false,
                results) {}
        
        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="meet">
        /// The meet which this race is an instance of.
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
        /// Races are ordered first by date, then by name of meet, then by location, then by gender.
        /// </summary>
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
            comparison = Meet.CompareTo (other.Meet);
            if (comparison != 0)
            {
                return comparison;
            }
            return Location.CompareTo (other.Location);
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

        /// <summary>
        /// Score the race.
        /// </summary>
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