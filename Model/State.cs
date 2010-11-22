using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A state of the union.
    /// </summary>
    public class State
    {
        #region Properties
        
        #region Constants
        
        /// <summary>
        /// The number used to identify this state.
        /// </summary>
        public readonly string Code;
        
        /// <summary>
        /// The name of this state.
        /// </summary>
        public readonly string Name;
        
        #endregion
        
        #region Fields
        
        private XList<City> _cities;
        
        #endregion
        
        /// <summary>
        /// The cities in this state.
        /// </summary>
        public IList<City> Cities
        {
            get { return _cities.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<City> ();
                }
                _cities = new XList<City> (value);
            }
        }
        
        /// <summary>
        /// True if a corresponding entry exists in the database for this
        /// state, false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
            
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Create a new state.
        /// </summary>
        /// <param name="code">
        /// The mailing code used to identify this state.
        /// </param>
        /// <param name="name">
        /// The name of this state.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if code is not a two-character string.
        /// </exception>
        public State (string code, string name)
        {
            if (code.Length != 2)
            {
                throw new ArgumentException (
                    "Property Code must be a string of exactly two characters");
            }
            Code = code;
            Name = name;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new state.
        /// </summary>
        /// <param name="code">
        /// The mailing code used to identify this state.
        /// </param>
        /// <param name="name">
        /// The name of this state.
        /// </param>
        public static State NewEntity (string code, string name)
        {
            State newState = new State (code, name);
            newState.IsAttached = true;
            return newState;
        }
        
        #endregion
        
        #region Inherited methods
        
        override public bool Equals (object other)
        {
            if (this == other) 
            {
                return true;
            }
            else if (other is State)
            {
                return Equals ((State)other);
            }
            return false;
        }
        
        public bool Equals (State that)
        {
            return Name.Equals (that.Name);
        }
        
        override public int GetHashCode ()
        {
            return Code.GetHashCode();
        }
        
        override public string ToString()
        {
            return Name;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Add more cities to this state.
        /// </summary>
        /// <param name="cities">
        /// The <see cref="IEnumerable<City>"/> to add.
        /// </param>
        public void AddCities (IEnumerable<City> cities)
        {
            _cities.AddRange (cities);
        }
        
        /// <summary>
        /// Add a city to this state.
        /// </summary>
        /// <param name="city">
        /// The <see cref="City"/> to add.
        /// </param>
        public void AddCity (City city)
        {
            _cities.Add (city);
        }
        
        #endregion
    }
}

