using System;
using System.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class MeetInstance : ICloneable
    {
        #region Properties

        /// <summary>
        /// The date on which this meet was held.
        /// </summary>
        public virtual DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// The host of this meet instance.
        /// </summary>
        public virtual Team Host
        {
            get;
            set;
        }

        /// <summary>TODO DELETE</summary>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The meet of which this is an instance.
        /// </summary>
        public virtual Meet Meet
        {
            get;
            set;
        }

        /// <summary>
        /// The venue whereat the instance of this meet was held.
        /// </summary>
        public virtual Venue Venue
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new meet instance.
        /// </summary>
        /// <param name="id">
        /// Delete this stupid fucking parameter!
        /// </param>
        /// <param name="meet">
        /// The meet of which this is an instance.
        /// </param>
        /// <param name="date">
        /// The date on which the meet was held.
        /// </param>
        /// <param name="venue">
        /// The <see cref="Venue" /> whereat the meet was held.
        /// </param>
        /// <param name="host">
        /// The host of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="meet"/> or <paramref name="venue"/>
        /// is <see langword="null" />.
        /// </exception>
        public MeetInstance(int id, Meet meet, DateTime date, Venue venue, Team host)
        {
            if(meet == null)
                throw new ArgumentNullException("meet");
            if(venue == null)
                throw new ArgumentNullException("venue");
            ID = id;
            Meet = meet;
            Date = date;
            Venue = venue;
            Host = host;
        }

        /// <summary>
        /// Construct a new <see cref="MeetInstance" />
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected MeetInstance()
        {
        }

        #endregion

        #region Inherited methods

        /// <summary>
        /// Overload of == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(MeetInstance instance1, MeetInstance instance2)
        {
            if(ReferenceEquals(instance1, instance2))
            {
                return true;
            }
            if(ReferenceEquals(instance1, null) || ReferenceEquals(instance2, null))
            {
                return false;
            }
            return instance1.Equals(instance2);
        }

        /// <summary>
        /// Overload of != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(MeetInstance instance1, MeetInstance instance2)
        {
            return !(instance1 == instance2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as MeetInstance);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return string.Format("{0} {1}", Meet.ID, Date).GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ({1})", Meet.Name, Date);
        }

        /// <inheritdoc />
        protected bool Equals(MeetInstance that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            if(ReferenceEquals(this, that))
            {
                return true;
            }
            return Date.Year == that.Date.Year && Date.Month == that.Date.Month && Date.Day == that.Date.Day && Meet == that.Meet && Venue == that.Venue;
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
