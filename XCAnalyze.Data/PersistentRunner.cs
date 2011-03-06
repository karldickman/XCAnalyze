using System;
using XCAnalyze.Model;

namespace XCAnalyze.Data
{
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    internal sealed class PersistentRunner : IRunner
    {
        #region Properties

        /// <summary>
        /// The number that identifies this runner.
        /// </summary>
        public int ID { get; set; }

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
        public PersistentRunner(string surname, string givenName)
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
        /// The number that identifies this runner.
        /// </param>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if surname or givenName is null.
        /// </exception>
        public PersistentRunner(int id, string surname, string givenName)
            : this(surname, givenName)
        {
            ID = id;
        }

        #endregion

        #region Methods

        public override bool Equals(object other)
        {
            if(this == other) {
                return true;
            }
            if(other is PersistentRunner) {
                return Equals((PersistentRunner)other);
            }
            return false;
        }

        public bool Equals(PersistentRunner that)
        {
            return Surname.Equals(that.Surname) && GivenName.Equals(that.GivenName);
        }

        public override int GetHashCode()
        {
            return this.FullName().GetHashCode();
        }

        #endregion
        
        #region IRunner implementation
         
        /// <summary>
        /// The runner's given or Christian name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public string Surname { get; set; }
        
        #endregion
    }
}

