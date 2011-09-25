using System;
using Iesi.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// Represents a city or municipality within a <see cref="State" />.
    /// </summary>
    public class City : ICloneable
    {
        #region Properties

        #region Physical implementation

        private string _name;
        private State _state;

        #endregion

        /// <summary>
        /// The identifying number of this instance.
        /// </summary>
        public virtual int ID
        {
            get;
            protected set;
        }

        /// <summary>
        /// The name of the city.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(value != Name)
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// The state in which the city is located geographically.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual State State
        {
            get { return _state; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(value != State)
                {
                    _state = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Venue" />s in this <see cref="City" />.
        /// </summary>
        public virtual ISet<Venue> Venues
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new city.
        /// </summary>
        /// <param name="name">
        /// The name of the city.
        /// </param>
        /// <param name="state">
        /// The state in which the city is located geographically..
        /// </param>
        public City(string name, State state) : this()
        {
            Name = name;
            State = state;
            Venues = new HashedSet<Venue>();
        }

        /// <summary>
        /// Construct a new city.  This constructor is required by NHibernate.
        /// </summary>
        protected City()
        {
        }

        #endregion

        #region Inherited methods

        /// <summary>
        /// Overloading of the == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(City city1, City city2)
        {
            if(ReferenceEquals(city1, city2))
            {
                return true;
            }
            if(ReferenceEquals(city1, null) || ReferenceEquals(city2, null))
            {
                return false;
            }
            return city1.Equals(city2);
        }

        /// <summary>
        /// Overloading of the != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(City city1, City city2)
        {
            return !(city1 == city2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
            {
                return false;
            }
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as City);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, State);
        }

        private bool Equals(City that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            if(ReferenceEquals(this, that))
            {
                return true;
            }
            return ID == that.ID && Name == that.Name && State == that.State;
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

