using System;
using System.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// Describes a venue at which races can be held.
    /// </summary>
    public class Venue : ICloneable
    {
        #region Properties

        /// <summary>
        /// The city in which the venue is.
        /// </summary>
        public virtual City City
        {
            get;
            set;
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
        /// The name of the venue.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The state where the venue is.
        /// </summary>
        /*public State State
        {
            get { return City.State; }
        }*/

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new venue.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the venue;
        /// </param>
        /// <param name="name">
        /// The name of the venue.
        /// </param>
        /// <param name="city">
        /// The city in which the venue is.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is <see langword="null" />.
        /// </exception>
        public Venue(int id, string name, City city)
        {
            if(name == null)
                throw new ArgumentNullException("name");
            if(city == null)
                throw new ArgumentNullException("city");
            ID = id;
            Name = name;
            City = city;
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
            return Name + ", " + City;
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
