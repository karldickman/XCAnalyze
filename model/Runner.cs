using System;
using System.Collections.Generic;

namespace XcAnalyze.Model
{
	
	/// <summary>
	/// All the information about a runner.
	/// </summary>
	public class Runner : IComparable<Runner>
	{
		private Dictionary<int, Affiliation> affiliations;
		private Gender gender;
		private string givenName;
		private string surname;
		private int? year;
		
		/// <summary>
		/// The affiliations of the runner over their career.
		/// </summary>
		protected Dictionary<int, Affiliation> Affiliations {
			get { return affiliations; }
		}

		/// <summary>
		/// The runner's gender.
		/// </summary>
		public Gender Gender {
			get { return gender; }
		}
		
		/// <summary>
		/// The runner's given name.
		/// </summary>
		public string GivenName {
			get { return givenName; }
		}

		/// <summary>
		/// The runner's surname.
		/// </summary>
		public string Surname
		{
			get { return surname; }
		}

		/// <summary>
		/// The runner's original graduation year.
		/// </summary>
		public int? Year
		{
			get { return year; }
		}
		
		public Runner (string surname, string givenName, Gender gender, int? year) : this(surname, givenName, gender, year, new Dictionary<int, Affiliation> ())
		{
		}

		public Runner (string surname, string givenName, Gender gender, int? year, Dictionary<int, Affiliation> affiliations)
		{
			this.surname = surname;
			this.givenName = givenName;
			this.gender = Gender;
			this.year = year;
			this.affiliations = affiliations;
		}

		/// <summary>
		/// Register a new affiliation for this runner.
		/// </summary>
		public void AddAffiliation (Affiliation affiliation)
		{
			Affiliations.Add (affiliation.Year, affiliation);
		}

		/// <summary>
		/// Runners are compared first by surname, then by year, then by gender.
		/// </summary>
		public int CompareTo (Runner other)
		{
			int comparison;
			if (this == other)
			{
				return 0;
			}
			comparison = Surname.CompareTo (other.Surname);
			if (comparison != 0)
			{
				return comparison;
			}
			comparison = GivenName.CompareTo (other.GivenName);
			if (comparison != 0)
			{
				return comparison;
			}
			comparison = Utilities.CompareNullable (Year, other.Year, 1);
			if (comparison != 0) 
			{
				return comparison;
			}
			return Gender.CompareTo (other.Gender);
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
			return base.GetHashCode ();
		}


		/// <summary>
		/// The most recent school with which the runner was affiliated.
		/// </summary>
		public School LastSchool ()
		{
			return Affiliations[Affiliations.Count - 1].School;
		}
		
		/// <summary>
		/// The school the runner was associated with in the given year.
		/// </summary>
		public School School (int year)
		{
			if (Affiliations.ContainsKey (year))
			{
				return Affiliations[year].School;
			}
			return null;
		}

		public override string ToString ()
		{
			return givenName + " " + surname + " (" + LastSchool () + " " + year + ")";
		}	
	}
}