using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class MeetInstance : ICloneable
    {
        #region Properties

        #region Physical implementation

        private int _meetID;

        #endregion

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

        /// <summary>
        /// The meet of which this is an instance.
        /// </summary>
        public virtual Meet Meet
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Meet.ID" /> of the <see cref="MeetInstance.Meet" />.
        /// </summary>
        public virtual int MeetID
        {
            get { return _meetID; }
            set
            {
                if(MeetID != value)
                {
                    _meetID = value;
                    Meet = Meet.Instances.Single(meet => meet.ID == MeetID);
                }
            }
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
        /// <param name="meet">
        /// The meet of which this is an instance.
        /// </param>
        /// <param name="date">
        /// The date on which the meet was held.
        /// </param>
        /// <param name="venue">
        /// The <see cref="Venue" /> whereat the meet was held.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="meet"/> or <paramref name="venue"/>
        /// is <see langword="null" />.
        /// </exception>
        public MeetInstance(Meet meet, DateTime date, Venue venue) : this(meet, date, venue, null)
        {
        }

        /// <summary>
        /// Create a new meet instance.
        /// </summary>
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
        public MeetInstance(Meet meet, DateTime date, Venue venue, Team host) : this()
        {
            if(meet == null)
                throw new ArgumentNullException("meet");
            if(venue == null)
                throw new ArgumentNullException("venue");
            Meet = meet;
            MeetID = Meet.ID;
            Meet.PropertyChanged += HandleMeetPropertyChanged;
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
            return string.Format("{0} {1}", MeetID, Date).GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ({1:yyyy-MM-dd})", Meet.Name, Date);
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

        #region Event handlers

        /// <summary>
        /// Handler for when a property on <see cref="Meet" /> changes.
        /// </summary>
        private void HandleMeetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Sender should always be a meet; see the subscription above
            Meet meet = (Meet)sender;
            if(e.PropertyName == "ID")
            {
                // Bypass the logic that appears in MeetID
                _meetID = meet.ID;
            }
        }

        #endregion
    }
}
