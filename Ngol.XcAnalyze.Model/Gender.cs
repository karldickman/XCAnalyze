using System;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// This class can have only two instances, the male instance and the female
    /// instance.  This is an example of the doubleton pattern.
    /// (http://thedailywtf.com/articles/the-doubleton-patten.aspx)
    /// </summary>
    public sealed class Gender
    {
        #region Properties

        /// <summary>
        /// Is this the male instance?
        /// </summary>>
        public bool IsMale
        {
            get { return this == Male; }
        }

        /// <summary>
        /// Is this the female instance.
        /// </summary>
        public bool IsFemale
        {
            get { return this == Female; }
        }

        /// <summary>
        /// The male instance
        /// </summary>
        public static readonly Gender Male;

        /// <summary>
        /// The female instance.
        /// </summary>
        public static readonly Gender Female;

        #endregion

        #region Constructors

        static Gender()
        {
            Male = new Gender();
            Female = new Gender();
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        /// <returns>
        /// "M" if male, "F" if female.
        /// </returns>
        public override String ToString()
        {
            return IsMale ? "M" : "F";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a gender instance from a string.  "M" returns the male instance,
        /// "F" the female instance.
        /// </summary>
        public static Gender FromString(string genderString)
        {
            if(genderString == "M")
            {
                return Male;
            }
            if(genderString == "F")
            {
                return Female;
            }
            throw new ArgumentException(string.Format("\"{0}\" is not a valid gender string.", genderString));
        }
        
        #endregion
    }
}
