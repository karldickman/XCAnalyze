using System;

namespace XCAnalyze.Model
{
	/// <summary>
	/// This class can have only two instances, the male instance and the female
    /// instance.  This is an example of the doubleton pattern.
    /// (http://thedailywtf.com/articles/the-doubleton-patten.aspx)
	/// </summary>
	sealed public class Gender
	{   
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
        /// The male instance
        /// </summary>
		public static readonly Gender MALE = new Gender ();
        
        /// <summary>
        /// The female instance.
        /// </summary>
		public static readonly Gender FEMALE = new Gender ();
          
        /// <summary>
        /// Is this the male instance?
        /// </summary>>
        public bool IsMale { get { return this == MALE; } }

        /// <summary>
        /// Is this the female instance.
        /// </summary>
        public bool IsFemale { get { return this == FEMALE; } }

        public static implicit operator string (Gender gender)
        {
            return gender.ToString ();
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