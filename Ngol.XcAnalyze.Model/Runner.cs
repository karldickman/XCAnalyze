using System;
using Ngol.XcAnalyze.Model.Interfaces;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner : ICloneable
    {
        #region Properties

        /// <summary>
        /// The full name of the runner.
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0} {1}", GivenName, Surname); }
        }

        /// <summary>
        /// The runner's given or Christian name.
        /// </summary>
        public string GivenName
        {
            get;
            set;
        }

        /// <summary>
        /// A number used to identify a runner.
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public string Surname
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either argument is null.
        /// </exception>
        public Runner(string surname, string givenName)
        {
            if(surname == null)
            {
                throw new ArgumentNullException("surname");
            }
            if(givenName == null)
            {
                throw new ArgumentNullException("givenName");
            }
            Surname = surname;
            GivenName = givenName;
        }

        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="id">
        /// A number used to identify the runner.
        /// </param>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either argument is null.
        /// </exception>
        public Runner(int id, string surname, string givenName) : this(surname, givenName)
        {
            ID = id;
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Runner)
            {
                return Equals((Runner)other);
            }
            return false;
        }

        /// <summary>
        /// Check if two runners are equal.
        /// </summary>
        /// <param name="that">
        /// The <see cref="Runner"/> to compare with this instance.
        /// </param>
        public bool Equals(Runner that)
        {
            return ID == that.ID;
            //return Surname.Equals(that.Surname) && GivenName.Equals(that.GivenName);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ID;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FullName;
        }

        #endregion

        #region ICloneable implementation

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}

