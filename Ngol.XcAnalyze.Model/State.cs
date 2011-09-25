using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// One of the fifty states in the United States.
    /// </summary>
    public class State : ICloneable
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

        /// <summary>
        /// Overloading of the == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(State state1, State state2)
        {
            if(ReferenceEquals(state1, state2))
            {
                return true;
            }
            if(ReferenceEquals(state1, null) || ReferenceEquals(state2, null))
            {
                return false;
            }
            return state1.Equals(state2);
        }

        /// <summary>
        /// Overloading of the != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(State state1, State state2)
        {
            return !(state1 == state2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(other == null)
            {
                return false;
            }
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as State);
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

        private bool Equals(State that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            return Code == that.Code && Name == that.Name;
        }

        #endregion

        #region ICloneable implementation

        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
        
        #endregion
    }
}

