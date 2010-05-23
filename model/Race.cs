using System;
using System.Collections.Generic;

namespace XcAnalyze.Model {
	
	/// <summary>
	/// An instance of a meet.
	/// </summary>
	public class Race : IComparable<Race>
	{
		private string city;
		private DateTime date;
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
		public DateTime Date
		{
			get { return date; }
			protected set { date = value; }
		}

		/// <summary>
		/// The length of the race.
		/// </summary>
		public int Distance
		{
			get { return distance; }
			protected set { distance = value; }
		}

		/// <summary>
		/// Was it a men's race or a women's race?
		/// </summary>
		public Gender Gender
		{
			get { return gender; }
			protected set { gender = value; }
		}
		
		/// <summary>
		/// The name of the meet.
		/// </summary>
		public string Meet
		{
			get { return meet; }
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
		/// The name of the venue.
		/// </summary>
		public string Venue
		{
			get { return venue; }
		}

		public Race (string meet, DateTime date, Gender gender, int distance, string venue, string city, string state) : this(meet, date, gender, distance, venue, city, state, false, new List<Performance>())
		{
		}

		public Race (string meet, DateTime date, Gender gender, int distance, string venue, string city, string state, bool scoreMeet, List<Performance> results)
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

		/// <summary>
		/// Races are ordered first by date, then by name of meet, then by location, then by gender.
		/// </summary>
		public int CompareTo (Race other)
		{
			int comparison;
			IComparable[] fields = { Date.Year, Date.Month, Date.Day, Meet, Venue, City, State, (IComparable)Gender };
			IComparable[] otherFields = { other.Date.Year, other.Date.Month, other.Date.Day, other.Meet,
				other.Venue, other.City, other.State, (IComparable)other.Gender };
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