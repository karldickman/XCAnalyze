using System;

namespace XcAnalyze.Model
{
	
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
		public Runner Runner
		{
			get { return runner; }
		}

		/// <summary>
		/// The school with which a runner is affiliated.
		/// </summary> 	
		public School School
		{
			get { return school; }
		}

		/// <summary>
		/// The year in which the runner was affiliated with the school.
		/// </summary>
		public int Year
		{
			get { return year; }
			protected set { year = value; }
		}
		
		public Affiliation (Runner runner, School school, int year)
		{
			this.runner = runner;
			this.school = school;
			this.year = year;
		}

		/// <summary>
		/// Affiliations are compared first by school, then by runner, then by year.
		/// </summary>
		public int CompareTo (Affiliation other)
		{
			int comparison;
			IComparable[] fields = { (IComparable)School, (IComparable)Runner, Year };
			IComparable[] otherFields = { (IComparable)other.School, (IComparable)other.Runner, other.Year };
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
			if (this == other) 
			{
				return true;
			}
			if (other is Affiliation) 
			{
				return 0 == CompareTo ((Affiliation)other);
			}
			return false;
		}
		
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}