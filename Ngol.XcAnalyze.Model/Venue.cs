using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// Describes a venue at which races can be held.
    /// </summary>
    public class Venue
    {
        #region Properties
        
        #region Fields
        
        private Cell<City> _city;
        
        private IXList<MeetInstance> _meetInstances;
        
        private Cell<string> _name;
        
        #endregion
        
        /// <summary>
        /// The city in which the venue is.
        /// </summary>
        public City City
        {
            get { return _city.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property city cannot be null.");
                }
                _city.Value = value;
            }
        }
        
        /// <summary>
        /// The number that identifies the city.
        /// </summary>
        public int CityID
        {
            get { return City.ID; }
        }
        
        /// <summary>
        /// The number that identifies this venue.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// True if a corresponding entry for this venue exits in the database,
        /// false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        /// <summary>
        /// True if the venue has been changed since it was loaded from the
        /// database, false otherwise.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (IsAttached) 
                {
                    return _name.IsChanged || _city.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// The instances of meets that have been held at this venue.
        /// </summary>
        public IList<MeetInstance> MeetInstances
        {
            get { return _meetInstances.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<MeetInstance> ();
                }
                _meetInstances = new XList<MeetInstance> (value);
            }
        }
        
        /// <summary>
        /// The name of the venue.
        /// </summary>
        public string Name
        {
            get { return _name.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Name cannot be null.");
                }
                _name.Value = value;
            }
        }

        /// <summary>
        /// The state where the venue is.
        /// </summary>
        public State State
        {
            get { return City.State; }
        }
        
        #endregion
        
        #region Constructors
        
        protected Venue ()
        {
            _name = new Cell<string> ();
            _city = new Cell<City> ();
        }
        
        /// <summary>
        /// Create a new venue.
        /// </summary>
        /// <param name="name">
        /// The name of the venue.
        /// </param>
        /// <param name="city">
        /// The city in which the venue is.
        /// </param>
        public Venue (string name, City city)
        : this()
        {
            Name = name;
            City = city;
            IsAttached = false;
        }
        
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
        protected Venue(int id, string name, City city)
        : this(name, city)
        {
            ID = id;
        }
        
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
        public static Venue NewEntity (int id, string name, City city)
        {
            Venue newVenue = new Venue (id, name, city);
            newVenue.IsAttached = true;
            return newVenue;
        }
        
        #endregion
        
        #region Inherited Methods
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Venue)
            {
                return Equals((Venue)other);
            }
            return false;
        }
        
        /// <summary>
        /// Venues are compared first by state, then by city, then by name.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Venue"/> with which to compare.
        /// </param>
        public bool Equals (Venue other)
        {
            return City.Equals (other.City) && Name.Equals (other.Name);
        }  
        
        override public int GetHashCode ()
        {
            return ID;
        } 
        
        override public string ToString ()
        {
            return Name + ", " + City;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Add a meet instance run at this venue.
        /// </summary>
        /// <param name="meetInstance">
        /// The <see cref="MeetInstance"/> to add.
        /// </param>
        public void AddMeetInstance (MeetInstance meetInstance)
        {
            _meetInstances.Add (meetInstance);
        }
        
        /// <summary>
        /// Add more meet instances run at this venue.
        /// </summary>
        /// <param name="meetInstances">
        /// A <see cref="IEnumerable<MeetInstance>"/> of meet instances to add.
        /// </param>
        public void AddMeetInstances (IEnumerable<MeetInstance> meetInstances)
        {
            _meetInstances.AddRange (meetInstances);
        }
        
        #endregion
    }
}
