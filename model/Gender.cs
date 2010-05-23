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
		private static Gender male = new Gender (gender.M);
		private static Gender female = new Gender (gender.F);

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
				return Male;
			}
			if (genderString == "F")
			{
				return Female;
			}
			return null;
		}
		
		/// <summary>
		/// Get the male instance.
		/// </summary>
		public static Gender Male
		{
			get { return male; }
		}

		/// <summary>
		/// Get the female instance.
		/// </summary>
		public static Gender Female
		{
			get { return female; }
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
			if (this == Male)
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
			return gender == gender.M;
		}

		/// <summary>
		/// Is this the female instance.
		/// </summary>
		public bool IsFemale ()
		{
			return gender == gender.F;
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