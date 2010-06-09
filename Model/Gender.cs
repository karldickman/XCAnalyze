using System;

namespace XCAnalyze.Model
{
	/// <summary>
	/// A handy enumeration for gender.
	/// </summary>
	internal enum gender { M, F }

	/// <summary>
	/// This class can have only two instances, the male instance and the female
    /// instance.  This is an example of the doubleton pattern.
    /// (http://thedailywtf.com/articles/the-doubleton-patten.aspx)
	/// </summary>
	public class Gender : IComparable<Gender>
	{
        /// <summary>
        /// The gender of the instance.
        /// </summary>
		private gender gender;
        
        /// <summary>
        /// The male instance
        /// </summary>
		public static readonly Gender MALE = new Gender (gender.M);
        
        /// <summary>
        /// The female instance.
        /// </summary>
		public static readonly Gender FEMALE = new Gender (gender.F);
          
        /// <summary>
        /// Is this the male instance?
        /// </summary>>
        public bool IsMale { get { return this == MALE; } }

        /// <summary>
        /// Is this the female instance.
        /// </summary>
        public bool IsFemale { get { return this == FEMALE; } }
        
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
		/// Men always go before women.  Sorry ladies.
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

		override public int GetHashCode ()
		{
			return gender.GetHashCode ();
		}

		/// <summary>
		/// "M" if male, "F" if female.
		/// </summary>
		override public String ToString ()
		{
			if (IsMale)
			{
				return "M";
			}
			return "F";
		}
	}
}