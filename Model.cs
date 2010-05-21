using System;
using System.Collections.Generic;
using System.Linq;

namespace xcanalyze.model
{
	public enum _gender
	{
		M,
		F
	}

	/// <summary>
	/// A handy enumeration for gender.
	/// </summary>

	public class Gender
	{
		private _gender gender;
		private static Gender male = new Gender (_gender.M);
		private static Gender female = new Gender (_gender.F);

		protected Gender (_gender gender)
		{
			this.gender = gender;
		}

		public static Gender FromString (string genderString)
		{
			if (genderString == "M") {
				return Male;
			}
			if (genderString == "F") {
				return Female;
			}
			return null;
		}

		public static Gender Male {
			get { return male; }
		}

		public static Gender Female {
			get { return female; }
		}

		public bool isMale ()
		{
			return gender == _gender.M;
		}

		public bool isFemale ()
		{
			return gender == _gender.F;
		}

		public override String ToString ()
		{
			if (isMale ()) {
				return "M";
			}
			return "F";
		}
	}

	/// <summary>
	/// Describes in which year a runner ran for a particular school.
	/// </summary>
	public class Affiliation : IComparable<Affiliation>
	{
		private Runner runner;
		private School school;
		private int year;

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
			protected set { year = value; }
		}

		public Affiliation (Runner runner, School school, int year)
		{
			this.runner = runner;
			this.school = school;
			this.year = year;
		}

		public int CompareTo (Affiliation other)
		{
			int comparison;
			if (this == other) {
				return 0;
			}
			comparison = Year.CompareTo (other.Year);
			if (comparison != 0) {
				return comparison;
			}
			comparison = School.CompareTo (other.School);
			if (comparison != 0) {
				return comparison;
			}
			return Runner.CompareTo (other.Runner);
		}

		public override bool Equals (object obj)
		{
			if (this == obj) {
				return true;
			}
			if (obj is Affiliation) {
				return 0 == CompareTo ((Affiliation)obj);
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return Runner.GetHashCode () + School.GetHashCode () + Year.GetHashCode ();
		}
		
	}

	/// <summary>
	/// A runners time (in seconds) at a particular race.
	/// </summary>
	public class Performance : IComparable<Performance>
	{
		private Runner runner;
		private Race race;
		private double time;

		/// <summary>
		/// The length of the race whereat the time was run.
		/// </summary>
		public int Distance {
			get { return Race.Distance; }
		}

		/// <summary>
		/// The race whereat the time was run.
		/// </summary>
		public Race Race {
			get { return race; }
		}

		/// <summary>
		/// The runner who ran the time.
		/// </summary>
		public Runner Runner {
			get { return runner; }
		}

		/// <summary>
		/// The time that was run.
		/// </summary>
		public double Time {
			get { return time; }
			protected set { time = value; }
		}
		
		public Performance (Runner runner, Race race, double time)
		{
			this.runner = runner;
			this.race = race;
			this.time = time;
		}

		public int CompareTo (Performance that)
		{
			int comparison;
			if (this == that) {
				return 0;
			}
			comparison = Pace ().CompareTo (that.Pace ());
			if (comparison != 0) {
				return comparison;
			}
			return Distance.CompareTo (that.Distance);
		}

		public override bool Equals (object other)
		{
			if (this == other) {
				return true;
			}
			if (other is Performance) {
				Performance that = (Performance)other;
				return 0 == CompareTo (that);
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return (new double[] { Pace (), Distance }).GetHashCode ();
		}

		/// <summary>
		/// The pace in minutes per mile of the performance.
		/// </summary>
		public double Pace ()
		{
			return Time / Distance * 60;
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
			protected set { date = value; }
		}

		/// <summary>
		/// Was it a men's race or a women's race?
		/// </summary>
		public Gender Gender {
			get { return gender; }
			protected set { gender = value; }
		}

		/// <summary>
		/// The length of the race.
		/// </summary>
		public int Distance {
			get { return distance; }
			protected set { distance = value; }
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
			this.date = date;
			this.gender = gender;
			this.distance = distance;
			this.meet = meet;
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
			comparison = Date.Year.CompareTo (that.Date.Year);
			if (comparison != 0) {
				return comparison;
			}
			comparison = Date.Month.CompareTo (that.Date.Month);
			if (comparison != 0) {
				return comparison;
			}
			comparison = Date.Day.CompareTo (that.Date.Day);
			if (comparison != 0) {
				return comparison;
			}
			comparison = Meet.CompareTo (that.Meet);
			if (comparison != 0) {
				return comparison;
			}
			comparison = Venue.CompareTo (that.Venue);
			if (comparison != 0) {
				return comparison;
			}
			comparison = City.CompareTo (that.City);
			if (comparison != 0) {
				return comparison;
			}
			comparison = State.CompareTo (that.State);
			if (comparison != 0) {
				return comparison;
			}
			if (Gender == that.Gender) {
				return 0;
			}
			if (Gender.isMale ()) {
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
				return 0 == CompareTo ((Race)other);
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
			if (gender.isMale ()) {
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
		private int? year;
		private List<Affiliation> affiliations;

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
		public int? Year {
			get { return year; }
		}

		public List<Affiliation> Affiliations {
			get { return affiliations; }
		}

		public Runner (string surname, string givenName, Gender gender, int? year) : this(surname, givenName, gender, year, new List<Affiliation>())
		{
		}

		public Runner (string surname, string givenName, Gender gender, int? year, List<Affiliation> affiliations)
		{
			this.surname = surname;
			this.givenName = givenName;
			this.gender = Gender;
			this.year = year;
			this.affiliations = affiliations;
		}

		public void AddAffiliation (Affiliation affiliation)
		{
			Affiliations.Add (affiliation);
			Affiliations.Sort ();
		}

		public int CompareTo (Runner that)
		{
			if (this == that) {
				return 0;
			}
			int comparison;
			comparison = Surname.CompareTo (that.Surname);
			if (comparison != 0) {
				return comparison;
			}
			comparison = GivenName.CompareTo (that.GivenName);
			if (comparison != 0) {
				return comparison;
			}
			if (Year != null && that.Year != null) {
				if (Year != null) {
					return -1;
				}
				if (that.Year != null) {
					return 1;
				}
				comparison = Year.Value.CompareTo (that.Year.Value);
				if (comparison != 0) {
					return comparison;
				}
			}
			if (Gender == that.Gender) {
				return 0;
			}
			if (Gender.isMale ()) {
				return -1;
			}
			return 1;
		}

		public override bool Equals (object other)
		{
			if (this == other) {
				return true;
			}
			if (other is Runner) {
				return 0 == CompareTo ((Runner)other);
			}
			return false;
		}

		public override int GetHashCode ()
		{
			return (surname + ", " + givenName + gender + year).GetHashCode ();
		}

		/// <summary>
		/// The most recent school with which the runner was affiliated.
		/// </summary>
		/// <returns>
		/// A <see cref="School"/>
		/// </returns>
		public School LastSchool ()
		{
			return Affiliations[Affiliations.Count - 1].School;
		}

		public override string ToString ()
		{
			return givenName + " " + surname + " (" + LastSchool () + " " + year + ")";
		}
		
	}

	public class School : IComparable<School>
	{
		private string name;
		private string type;
		private bool nameFirst;
		private string conference;
		private List<Affiliation> affiliations;

		/// <summary>
		/// The name of the school (Linfield, Willamette, etc.)
		/// </summary>
		public string Name {
			get { return name; }
			protected set { name = value; }
		}

		/// <summary>
		/// The type of the school (University, College, etc.)
		/// </summary>
		public string Type {
			get { return type; }
			protected set { type = value; }
		}

		/// <summary>
		/// Should the name of the school go before its type (true Linfield College, false for University of Puget Sound).
		/// </summary>
		public bool NameFirst {
			get { return nameFirst; }
			protected set { nameFirst = value; }
		}

		/// <summary>
		/// The athletic conference with which the school is affiliated.
		/// </summary>
		public string Conference {
			get { return conference; }
		}

		/// <summary>
		/// The runners who have competed for this school.
		/// </summary>
		public List<Affiliation> Affiliations {
			get { return affiliations; }
		}


		public School (string name, string type, bool nameFirst, string conference) : this(name, type, nameFirst, conference, new List<Affiliation>())
		{
		}

		public School (string name, string type, bool nameFirst, string conference, List<Affiliation> affiliations)
		{
			this.name = name;
			this.type = type;
			this.nameFirst = nameFirst;
			this.conference = conference;
			this.affiliations = affiliations;
		}

		public void AddAffiliation (Affiliation affiliation)
		{
			Affiliations.Add (affiliation);
			Affiliations.Sort ();
		}

		public int CompareTo (School that)
		{
			int comparison;
			if (this == that) {
				return 0;
			}
			comparison = Name.CompareTo (that.Name);
			if (comparison != 0) {
				return comparison;
			}
			comparison = Type.CompareTo (that.Type);
			if (comparison != 0) {
				return comparison;
			}
			if (NameFirst != that.NameFirst) {
				comparison = FullName ().CompareTo (that.FullName ());
				if (comparison != 0) {
					return comparison;
				}
			}
			return Conference.CompareTo (that.Conference);
		}

		public override bool Equals (object other)
		{
			if (this == other) {
				return true;
			}
			if (other is School) {
				return 0 == CompareTo ((School)other);
			}
			return false;
		}

		public string FullName ()
		{
			if (nameFirst) {
				return name + " " + type;
			}
			return type + " of " + name;
		}

		public override int GetHashCode ()
		{
			return (ToString () + ", " + conference).GetHashCode ();
		}

		public override string ToString ()
		{
			return FullName ();
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
			foreach (Affiliation affiliation in affiliations) {
				Affiliate (affiliation);
			}
		}
		
		protected void Affiliate (Affiliation affiliation)
		{
			affiliation.Runner.AddAffiliation (affiliation);
			affiliation.School.AddAffiliation (affiliation);
		}
		
		public void Affiliate (Runner runner, School school, int year)
		{
			Affiliation affiliation = new Affiliation (runner, school, year);
			Affiliations.Add (affiliation);
			Affiliate (affiliation);
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
