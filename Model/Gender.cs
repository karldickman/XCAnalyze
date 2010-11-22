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
        #region Constants
                
        /// <summary>
        /// The male instance
        /// </summary>
        public static readonly Gender Male = new Gender ();
        
        /// <summary>
        /// The female instance.
        /// </summary>
        public static readonly Gender Female = new Gender ();
        
        #endregion
        
        #region Fields
                  
        /// <summary>
        /// Is this the male instance?
        /// </summary>>
        public bool IsMale { get { return this == Male; } }

        /// <summary>
        /// Is this the female instance.
        /// </summary>
        public bool IsFemale { get { return this == Female; } }

        #endregion
        
        #region Inherited methods
                
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
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Get a gender instance from a string.  "M" returns the male instance,
        /// "F" the female instance.
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
            throw new ArgumentException(genderString + " is not a valid gender string.");
        }
        
        #endregion
	}
}