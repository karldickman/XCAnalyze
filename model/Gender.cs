using System;

namespace XcAnalyze.Model
{
	
	/// <summary>
	/// A handy enumeration for gender.
	/// </summary>
	internal enum gender { M, F }

	/// <summary>
	/// This class can have only two instances, the male instance and the female instance.
	/// </summary>
	public class Gender : IComparable<Gender>
	{
		private gender gender;
		private static readonly Gender MALE = new Gender (gender.M);
		private static readonly Gender FEMALE = new Gender (gender.F);

		internal Gender (gender gender)
		{
			this.gender = gender;
		}

		/// <summary>
		/// Get a gender instance from a string.  "M" returns the male instance, "F" the female instance.
		/// </summary>
		public static Gender FromString (string genderString)
		{
			if (genderString == "M")
			{
				return MALE;
			}
			if (genderString == "F")
			{
				return FEMALE;
			}
			return null;
		}

		/// <summary>
		/// Men always go before women.
		/// </summary>
		public int CompareTo (Gender other)
		{
			if (this == other)
			{
				return 0;
			}
			if (this == MALE)
			{
				return -1;
			}
			return 1;
		}

		public override int GetHashCode ()
		{
			return gender.GetHashCode ();
		}

		/// <summary>
		/// Is this the male instance?
		/// </summary>>
		public bool IsMale ()
		{
			return this == MALE;
		}

		/// <summary>
		/// Is this the female instance.
		/// </summary>
		public bool IsFemale ()
		{
			return this == FEMALE;
		}

		/// <summary>
		/// "M" if male, "F" if female.
		/// </summary>
		public override String ToString ()
		{
			if (IsMale ())
			{
				return "M";
			}
			return "F";
		}
	}
}