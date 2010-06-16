using System;

namespace XCAnalyze.Model
{
    public class Venue : IComparable<Venue>
    {
        /// <summary>
        /// The city in which the venue is.
        /// </summary>
        public string City { get; protected internal set; }
        
        /// <summary>
        /// The name of the venue.
        /// </summary>
        public string Name { get; protected internal set; }
        
        /// <summary>
        /// The state where the venue is.
        /// </summary>
        public string State { get; protected internal set; }
        
        /// <summary>
        /// Create a new venue.
        /// </summary>
        /// <param name="name">
        /// The name of the venue.
        /// </param>
        /// <param name="city">
        /// The city in which the venue is.
        /// </param>
        /// <param name="state">
        /// The state in which the venue is.
        /// </param>
        public Venue (string name, string city, string state)
        {
            Name = name;
            City = city;
            State = state;
        }
        
        /// <summary>
        /// Venues are compared first by state, then by city, then by name.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Venue"/> with which to compare.
        /// </param>
        public int CompareTo (Venue other)
        {
            int comparison;
            if (this == other) 
            {
                return 0;
            }
            comparison = State.CompareTo (other.State);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = City.CompareTo (other.City);
            if (comparison != 0)
            {
                return comparison;
            }
            return ObjectComparer<string>.Compare (Name, other.Name, -1);
        }  
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Venue)
            {
                return 0 == CompareTo((Venue)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return ToString().GetHashCode();
        } 
        
        override public string ToString ()
        {
            return Name + ", " + City + ", " + State;
        }    
    }
}
