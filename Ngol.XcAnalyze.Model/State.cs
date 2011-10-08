using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// One of the fifty states in the United States.
    /// </summary>
    public class State
    {
        #region Properties

        #region Physical implementation

        private string _code;
        private string _name;

        #endregion

        /// <summary>
        /// The postal abbreviation for the state.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value is set to <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string Code
        {
            get { return _code; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _code = value;
            }
        }

        /// <summary>
        /// The name of the state.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value is set to <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }

        /// <summary>
        /// The cities recorded as being within this state.
        /// </summary>
        public virtual ISet<City> Cities
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="State" />.
        /// </summary>
        /// <param name="code">
        /// The postal abbreviation for the state.
        /// </param>
        /// <param name="name">
        /// The name of the state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is <see langword="null" />.
        /// </exception>
        public State(string code, string name) : this()
        {
            Code = code;
            Name = name;
            Cities = new HashedSet<City>();
        }

        /// <summary>
        /// Construct a new <see cref="State" />.
        /// </summary>
        /// <param name="code">
        /// The postal abbreviation for the <see cref="State" />.
        /// </param>
        /// <param name="name">
        /// The name of the <see cref="State" />.
        /// </param>
        /// <param name="cities">
        /// The <see cref="City"/>s that are in this <see cref="State" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is <see langword="null" />.
        /// </exception>
        public State(string code, string name, IEnumerable<City> cities) : this(code, name)
        {
            if(cities == null)
                throw new ArgumentNullException("cities");
            Cities.AddRange(cities);
        }

        /// <summary>
        /// Construct a new State.
        /// </summary>
        protected State()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return this == other ? true : Equals(other as State);
        }

        /// <summary>
        /// Determines whether the specified <see cref="State"/> is equal to the current <see cref="Ngol.XcAnalyze.Model.State"/>.
        /// </summary>
        /// <param name='that'>
        /// The <see cref="State"/> to compare with the current <see cref="Ngol.XcAnalyze.Model.State"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="State"/> is equal to the current
        /// <see cref="Ngol.XcAnalyze.Model.State"/>; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Equals(State that)
        {
            if(that == null)
            {
                return false;
            }
            if(this == that)
            {
                return true;
            }
            return Code == that.Code && Name == that.Name;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}

