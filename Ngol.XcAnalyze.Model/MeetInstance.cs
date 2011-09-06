using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class MeetInstance
    {
        #region Properties

        #region Constants

        /// <summary>
        /// The date on which this meet was held.
        /// </summary>
        public readonly DateTime Date;

        /// <summary>
        /// The meet of which this is an instance.
        /// </summary>
        public readonly Meet Meet;

        #endregion

        #region Fields

        private Cell<Team> _host;
        private IXList<Race> _races;
        private Cell<Venue> _venue;

        #endregion

        /// <summary>
        /// The host of this meet instance.
        /// </summary>
        public Team Host {
            get { return _host.Value; }

            set { _host.Value = value; }
        }

        /// <summary>
        /// The number that identifies the host of this meet.
        /// </summary>
        public int? HostID {
            get {
                if (Host == null) {
                    return null;
                }
                return Host.ID;
            }
        }

        /// <summary>
        /// True if the meet instance has been stored to the database, false
        /// otherwise.
        /// </summary>
        public bool IsAttached { get; set; }

        /// <summary>
        /// True if the meet instance has been changed since it was loaded from
        /// the database, false otherwise.
        /// </summary>
        public bool IsChanged {
            get {
                if (IsAttached) {
                    return _venue.IsChanged || _host.IsChanged;
                }
                return false;
            }
        }

        /// <summary>
        /// The number that identifies the meet of which this is an instance.
        /// </summary>
        public int MeetID {
            get { return Meet.ID; }
        }

        /// <summary>
        /// The name of the meet of which this is an instance.
        /// </summary>
        public string Name {
            get { return Meet.Name; }
        }

        /// <summary>
        /// The races held at this meet instance.
        /// </summary>
        public IList<Race> Races {
            get
            {
                return _races.AsReadOnly ();
            }

            protected set {
                if (value == null) {
                    value = new List<Race> ();
                }
                _races = new XList<Race> (value);
            }
        }

        /// <summary>
        /// The venue whereat the instance of this meet was held.
        /// </summary>
        public Venue Venue {
            get { return _venue.Value; }

            protected set {
                if (value == null) {
                    throw new ArgumentNullException ("Property Venue cannot be null.");
                }
                _venue.Value = value;
            }
        }

        /// <summary>
        /// The number that identifies the venue for this race.
        /// </summary>
        public int VenueID {
            get { return Venue.ID; }
        }

        #endregion

        #region Constructors

        protected MeetInstance()
        {
            _venue = new Cell<Venue>();
            _host = new Cell<Team>();
            _races = new XList<Race>();
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
        /// The venue whereat the meet was held.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is null.
        /// </exception>
        public MeetInstance (Meet meet, DateTime date, Venue venue) : this(meet, date, venue, null)
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
        /// The venue whereat the meet was held.
        /// </param>
        /// <param name="host">
        /// The host of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is null.
        /// </exception>
        public MeetInstance (Meet meet, DateTime date, Venue venue, Team host) : this()
        {
            Meet = meet;
            Date = date;
            Venue = venue;
            Host = host;
            IsAttached = false;
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
        /// The venue whereat the meet was held.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any required argument is null.
        /// </exception>
        public static MeetInstance NewEntity (Meet meet, DateTime date, Venue venue)
        {
            return NewEntity (meet, date, venue, null);
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
        /// The venue whereat the meet was held.
        /// </param>
        /// <param name="host">
        /// The host of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any required argument is null.
        /// </exception>
        public static MeetInstance NewEntity (Meet meet, DateTime date, Venue venue, Team host)
        {
            MeetInstance newInstance = new MeetInstance (meet, date, venue, host);
            newInstance.IsAttached = true;
            return newInstance;
        }

        #endregion

        #region Inherited methods

        public override bool Equals (object other)
        {
            if (this == other) {
                return true;
            }
            if (other is MeetInstance) {
                return Equals ((MeetInstance)other);
            }
            return false;
        }

        public bool Equals (MeetInstance other)
        {
            return Date.Year == other.Date.Year && Date.Month == other.Date.Month && Date.Day == other.Date.Day && Meet.Equals (other.Meet) && Venue.Equals (other.Venue);
        }

        public override int GetHashCode ()
        {
            return string.Format ("{0} {1}", MeetID, Date).GetHashCode ();
        }

        public override string ToString ()
        {
            return string.Format ("{0} ({1})", Name, Date);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a race to this meet instance.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> to add.
        /// </param>
        public void AddRace (Race race)
        {
            _races.Add (race);
        }

        /// <summary>
        /// Add more races to this meet instance.
        /// </summary>
        /// <param name="races">
        /// A <see cref="IEnumerable<Race>"/> of races to add.
        /// </param>
        public void AddRaces (IEnumerable<Race> races)
        {
            _races.AddRange (races);
        }
        
        #endregion
    }
}
