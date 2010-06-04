using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row of the venue table in the database.
    /// </summary>
    public class Venue : IComparable<Venue>
    {
        /// <summary>
        /// A registry of the instances (i.e rows) by id number.
        /// </summary>
        protected internal static IDictionary<int, Venue> IdMap =
            new Dictionary<int, Venue>();
        
        /// <summary>
        /// Get all instances of this class.
        /// </summary>
        public static IList<Venue> List
        {
            get { return new List<Venue> (IdMap.Values); }
        }
            
        /// <summary>
        /// The id number.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the venue.
        /// </summary>
        public string Name { get; protected internal set; }
        
        /// <summary>
        /// The city nearest to the venue.
        /// </summary>
        public string City { get; protected internal set; }
        
        /// <summary>
        /// Which state the venue is in.
        /// </summary>
        public string State { get; protected internal set; }
        
        /// <summary>
        /// The elevation (in meters) of the venue.
        /// </summary>
        public int? Elevation { get; protected internal set; }

        /// <summary>
        /// Create a venue.
        /// </summary>
        /// <param name="id">
        /// The id number.
        /// </param>
        /// <param name="name">
        /// The name of the venue.
        /// </param>
        /// <param name="city">
        /// The city nearest the venue.
        /// </param>
        /// <param name="state">
        /// Which state the venue is in.
        /// </param>
        /// <param name="elevation">
        /// The elevation of the venue.  If not known, then null.
        /// </param>
        public Venue (int id, string name, string city, string state,
            int? elevation)
        {
            Id = id;
            Name = name;
            City = city;
            State = state;
            Elevation = elevation;
            IdMap[id] = this;
        }    
        
        /// <summary>
        /// Clear the registry of instances.
        /// </summary>
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        /// <summary>
        /// Check for an instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// True if such an instance exists, false otherwise.
        /// </returns>
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        /// <summary>
        /// Get an instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// The <see cref="Venue"/> with the requested id number.
        /// </returns>
        public static Venue Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Venues are compared first by state, then by city, then by name.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Venue"/> to compare with.
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
            return Name.CompareTo (other.Name);
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
            return base.GetHashCode();
        }    
    }
}
