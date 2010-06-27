using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// Describes a venue at which races can be held.
    /// </summary>
    public class Venue
    {
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
            if (city == null)
            {
                throw new ArgumentNullException ("No venue has a null city.");
            }
            if (state == null)
            {
                throw new ArgumentNullException ("No venue has a null sate.");
            }
            Name = name;
            City = city;
            State = state;
        }
        
        /// <summary>
        /// The city in which the venue is.
        /// </summary>
        public string City { get; protected set; }
        
        /// <summary>
        /// The name of the venue.
        /// </summary>
        public string Name { get; protected set; }
        
        /// <summary>
        /// The state where the venue is.
        /// </summary>
        public string State { get; protected set; }
        
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
        protected bool Equals (Venue other)
        {
            return State.Equals (other.State) && City.Equals (other.City) &&
                (Name == other.Name || Name != null && Name.Equals (other.Name));
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
