using System;
using System.Collections.Generic;
using System.Linq;

namespace xcanalyze.model
{
	/// <summary>
	/// A handy enumeration for gender.
	/// </summary>
	public enum Gender
	{
		M,
		F
	}

	/// <summary>
	/// Describes in which year a runner ran for a particular school.
	/// </summary>
	public class Affiliation
	{
		private Runner runner;
		private School school;
		private int year;

		public Affiliation (Runner runner, School school, int year)
		{
			this.runner = runner;
			this.school = school;
			this.year = year;
		}

		/// <summary>
		/// The runner affiliated with a school.
		/// </summary>
		public Runner Runner {
			get { return runner; }
		}

		/// <summary>
		/// The school with which a runner is affiliated.
		/// </summary> 	
		public School School {
			get { return school; }
		}

		/// <summary>
		/// The year in which the runner was affiliated with the school.
		/// </summary>
		public int Year {
			get { return year; }
		}
	}

	/// <summary>
	/// A runners time (in seconds) at a particular race.
	/// </summary>
	public class Performance
	{
		private Runner runner;
		private Race race;
		private float time;

		public Performance (Runner runner, Race race, float time)
		{
			this.runner = runner;
			this.race = race;
			this.time = time;
		}

		/// <summary>
		/// The runner who ran the time.
		/// </summary>
		public Runner Runner {
			get { return runner; }
		}

		/// <summary>
		/// The race whereat the time was run.
		/// </summary>
		public Race Race {
			get { return race; }
		}

		/// <summary>
		/// The time that was run.
		/// </summary>
		public float Time {
			get { return time; }
		}
	}

	/// <summary>
	/// An instance of a meet.
	/// </summary>
	public class Race : IComparable<Race>
	{
		private string meet;
		private DateTime date;
		private Gender gender;
		private int distance;
		private string venue;
		private string city;
		private string state;

		/// <summary>
		/// The name of the meet.
		/// </summary>
		public string Meet {
			get { return meet; }
		}
		
		/// <summary>
		/// The date on which the race was held.
		/// </summary>
		public DateTime Date {
			get { return date; }
		}

		/// <summary>
		/// Was it a men's race or a women's race?
		/// </summary>
		public Gender Gender {
			get { return gender; }
		}

		/// <summary>
		/// The length of the race.
		/// </summary>
		public int Distance {
			get { return distance; }
		}
		
		/// <summary>
		/// The name of the venue.
		/// </summary>
		public string Venue {
			get { return venue; }
		}

		public string City {
			get { return city; }
		}

		public string State {
			get { return state; }
		}
		
		public Race (string meet, DateTime date, Gender gender, int distance, string venue, string city, string state)
		{
			this.meet = meet;
			this.date = date;
			this.gender = gender;
			this.distance = distance;
			this.venue = venue;
			this.city = city;
			this.state = state;
		}
		
		public int CompareTo (Race that)
		{
			int comparison;
			if (this == that) {
				return 0;
			}
			comparison = date.CompareTo (that.date);
			if (comparison != 0) {
				return comparison;
			}
			comparison = meet.CompareTo (that.meet);
			if (comparison != 0) {
				return comparison;
			}
			comparison = venue.CompareTo (that.venue);
			if (comparison != 0) {
				return comparison;
			}
			comparison = city.CompareTo (that.city);
			if (comparison != 0) {
				return comparison;
			}
			comparison = state.CompareTo (that.state);
			if (comparison != 0) {
				return comparison;
			}
			if (gender == that.gender) {
				return 0;
			}
			if (gender == Gender.M) {
				return -1;
			}
			return 1;
		}
		
		public override bool Equals (object other)
		{
			if (this == other) {
				return true;
			}
			if (other is Race) {
				Race that = (Race)other;
				return meet == that.meet && date.Year == that.date.Year && date.Month == that.date.Month && date.Day == that.date.Day && gender == that.gender && venue == that.venue && city == that.city;
			}
			return false;
		}
		
		public override int GetHashCode ()
		{
			return ToString ().GetHashCode ();
		}
		
		public override string ToString ()
		{
			string result;
			if (gender == Gender.M) {
				result = "Men";
			} else {
				result = "Women";
			}
			return result + "'s " + distance + " m run, " + meet + " (" + date + "), " + venue + ", " + city + ", " + state;
		}
	}

	/// <summary>
	/// All the information about a runner.
	/// </summary>
	public class Runner : IComparable<Runner>
	{
		private string surname;
		private string givenName;
		private Gender gender;
		private int year;

		/// <summary>
		/// The runner's surname.
		/// </summary>
		public string Surname {
			get { return surname; }
		}

		/// <summary>
		/// The runner's given name.
		/// </summary>
		public string GivenName {
			get { return givenName; }
		}

		/// <summary>
		/// The runner's gender.
		/// </summary>
		public Gender Gender {
			get { return gender; }
		}

		/// <summary>
		/// The runner's original graduation year.
		/// </summary>
		public int Year {
			get { return year; }
		}

		public Runner (string surname, string givenName, Gender gender, int year)
		{
			this.surname = surname;
			this.givenName = givenName;
			this.gender = Gender;
			this.year = year;
		}

		public override bool Equals (object other)
		{
			if (this == other) {
				return true;
			}
			if (other is Runner) {
				Runner that = (Runner)other;
				return surname == that.surname && givenName == that.givenName && gender == that.gender && year == that.year;
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return (surname + ", " + givenName + gender + year).GetHashCode ();
		}

		public int CompareTo (Runner that)
		{
			if (this == that) {
				return 0;
			}
			int comparison;
			comparison = surname.CompareTo (that.surname);
			if (comparison != 0) {
				return comparison;
			}
			comparison = givenName.CompareTo (that.givenName);
			if (comparison != 0) {
				return comparison;
			}
			comparison = year.CompareTo (that.year);
			if (comparison != 0) {
				return comparison;
			}
			if (gender == that.gender) {
				return 0;
			}
			if (gender == Gender.M) {
				return -1;
			}
			return 1;
		}
		
	}

	public class School : IComparable<School>
	{
		private string name;
		private string type;
		private bool nameFirst;
		private string conference;
		
		/// <summary>
		/// The name of the school (Linfield, Willamette, etc.)
		/// </summary>
		public string Name {
			get { return name; }
		}

		/// <summary>
		/// The type of the school (University, College, etc.)
		/// </summary>
		public string Type {
			get { return type; }
		}

		/// <summary>
		/// Should the name of the school go before its type (true Linfield College, false for University of Puget Sound).
		/// </summary>
		public bool NameFirst {
			get { return nameFirst; }
		}

		/// <summary>
		/// The athletic conference with which the school is affiliated.
		/// </summary>
		public string Conference {
			get { return conference; }
		}

		public School (string name, string type, bool nameFirst, string conference)
		{
			this.name = name;
			this.type = type;
			this.nameFirst = nameFirst;
			this.conference = conference;
		}
		
		public int CompareTo (School that)
		{
			return name.CompareTo (that.name);
		}
		
		public override bool Equals (object other)
		{
			if (this == other) {
				return true;
			}
			if(other is School) {
				School that = (School)other;
				return name == that.name && type == that.type && nameFirst == that.nameFirst && conference == that.conference;
			}
			return false;
		}
		
		public override int GetHashCode () {
			return (ToString() + ", " + conference).GetHashCode();
		}

		public override string ToString ()
		{
			
			if (nameFirst) {
				return name + " " + type;
			}
			return type + " of " + name;
		}
		
	}

	public class Model
	{
		private List<Affiliation> affiliations;
		private List<Performance> performances;
		private List<Race> races;
		private List<Runner> runners;
		private List<School> schools;

		public List<Affiliation> Affiliations {
			get { return affiliations; }
		}

		public List<Performance> Performances {
			get { return performances; }
		}

		public List<Race> Races {
			get { return races; }
		}

		public List<Runner> Runners {
			get { return runners; }
		}

		public List<School> Schools {
			get { return schools; }
		}

		public Model (List<Affiliation> affiliations, List<Performance> performances, List<Race> races, List<Runner> runners, List<School> schools)
		{
			this.affiliations = affiliations;
			this.performances = performances;
			this.races = races;
			this.runners = runners;
			this.schools = schools;
		}

		public List<Runner> Team (School school, int year, Gender gender)
		{
			List<Runner> found = new List<Runner> ();
			foreach (Affiliation affiliation in affiliations) {
				if (affiliation.School == school && affiliation.Year == year && affiliation.Runner.Gender == gender) {
					found.Add (affiliation.Runner);
				}
			}
			return found;
		}
	}
}
