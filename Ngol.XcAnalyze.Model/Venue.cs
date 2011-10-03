using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// Describes a venue at which races can be held.
    /// </summary>
    public class Venue : ICloneable
    {
        #region Properties

        #region Physical implementation

        private City _city;
        private string _name;

        #endregion

        /// <summary>
        /// The city in which the venue is.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual City City
        {
            get { return _city; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _city = value;
            }
        }

        /// <summary>
        /// The number that identifies this venue.
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="MeetInstance" />s that have been held at this
        /// <see cref="Venue" />.
        /// </summary>
        public virtual ISet<MeetInstance> MeetInstances
        {
            get;
            protected set;
        }

        /// <summary>
        /// The name of the venue.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new venue.
        /// </summary>
        /// <param name="name">
        /// The name of the venue.
        /// </param>
        /// <param name="city">
        /// The city in which the venue is.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is <see langword="null" />.
        /// </exception>
        public Venue(string name, City city) : this()
        {
            Name = name;
            City = city;
            MeetInstances = new HashedSet<MeetInstance>();
        }

        /// <summary>
        /// Construct a new venue.  Needed for NHibernate.
        /// </summary>
        protected Venue()
        {
        }

        #endregion

        #region Inherited Methods

        /// <summary>
        /// Overload of the == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(Venue venue1, Venue venue2)
        {
            if(ReferenceEquals(venue1, venue2))
            {
                return true;
            }
            if(ReferenceEquals(venue1, null) || ReferenceEquals(venue2, null))
            {
                return false;
            }
            return venue1.Equals(venue2);
        }


        /// <summary>
        /// Overload of the != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(Venue venue1, Venue venue2)
        {
            return !(venue1 == venue2);
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
            return Equals(other as Venue);
        }

        /// <summary>
        /// Check whether two venues are equal.
        /// </summary>
        protected bool Equals(Venue other)
        {
            if(ReferenceEquals(other, null))
            {
                return false;
            }
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return City == other.City && Name == other.Name;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, City);
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
