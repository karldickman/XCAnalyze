using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    public class City
    {
        #region Properties
        
        #region Fields
        
        private Cell<string> _name;
        
        private Cell<State> _state;
        
        private IXList<Venue> _venues;
        
        #endregion
         
        /// <summary>
        /// The number used to identify this city.
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// True if the city is stored in the database, false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        /// <summary>
        /// True if the city has been changed since being loaded from the
        /// database, false otherwise.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (IsAttached) 
                {
                    return _name.IsChanged || _state.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// The name of this city.
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
        /// The state in which this city is.
        /// </summary>
        public State State
        {
            get { return _state.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property State cannot be null.");
                }
                 _state.Value = value;
            }
        }
        
        /// <summary>
        /// The identification number of the state.
        /// </summary>
        public string StateCode
        {
            get { return State.Code; }
        }
        
        /// <summary>
        /// The venues in this city.
        /// </summary>
        public IList<Venue> Venues
        {
            get { return _venues.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<Venue> ();
                }
                _venues = new XList<Venue>(value);
            }
        }
        
        #endregion
        
        #region Constructors
        
        protected City ()
        {
            _name = new Cell<string> ();
            _state = new Cell<State> ();
        }
        
        /// <summary>
        /// Create a new city.
        /// </summary>
        /// <param name="name">
        /// The name of the city.
        /// </param>
        /// <param name="state">
        /// The state wherein the city is.
        /// </param>
        public City (string name, State state)
        : this()
        {
            Name = name;
            State = state;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new city.
        /// </summary>
        /// <param name="id">
        /// The number that identifies this city.
        /// </param>
        /// <param name="name">
        /// The name of the city.
        /// </param>
        /// <param name="state">
        /// The state wherein the city is.
        /// </param>
        protected City (int id, string name, State state)
        : this(name, state)
        {
            ID = id;
        }
        
        /// <summary>
        /// Create a new city.
        /// </summary>
        /// <param name="id">
        /// The number that identifies this city.
        /// </param>
        /// <param name="name">
        /// The name of the city.
        /// </param>
        /// <param name="state">
        /// The state wherein the city is.
        /// </param>
        public static City NewEntity (int id, string name, State state)
        {
            City newCity = new City (id, name, state);
            newCity.IsAttached = true;
            return newCity;
        }
        
        #endregion
        
        #region Inherited methods
        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is City)
            {
                return Equals((City)other);
            }
            return false;
        }
        
        public bool Equals (City that)
        {
            return State.Equals(that.State) && Name.Equals (that.Name);
        }
        
        override public int GetHashCode()
        {
            return ID;
        }
        
        override public string ToString()
        {
            return string.Format("{0}, {1}", Name, State);
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Add a venue to this city.
        /// </summary>
        /// <param name="venue">
        /// The <see cref="Venue"/> to add.
        /// </param>
        public void AddVenue (Venue venue)
        {
            _venues.Add (venue);
        }
        
        /// <summary>
        /// Add more venues to a city.
        /// </summary>
        /// <param name="venues">
        /// The <see cref="IEnumerable<Venue>"/> to add.
        /// </param>
        public void AddVenues (IEnumerable<Venue> venues)
        {
            _venues.AddRange (venues);
        }
        
        #endregion
    }
}

