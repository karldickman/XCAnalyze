using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row of the meets table in the database.
    /// </summary>
    public class Meet : IComparable<Meet>
    {
        /// <summary>
        /// A registry of all the instances (i.e. rows).
        /// </summary>
        protected internal static IDictionary<int, Meet> IdMap =
            new Dictionary<int, Meet>();
        
        /// <summary>
        /// Get all instances of this class.
        /// </summary>
        public static IList<Meet> List
        {
            get { return new List<Meet> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the meet.  Example: Lewis & Clark Invitational.
        /// </summary>
        public string Name { get; protected internal set; }

        /// <summary>
        /// Create a new row description.
        /// </summary>
        /// <param name="id">
        /// The row id number.
        /// </param>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        public Meet (int id, string name)
        {
            Id = id;
            Name = name;
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
        /// Check whether an instance with a particular id number exists.
        /// </summary>
        /// <param name="id">
        /// The id to search for.
        /// </param>
        /// <returns>
        /// True if the instance exists; false otherwise.
        /// </returns>
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        /// <summary>
        /// Get the instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number.
        /// </param>
        /// <returns>
        /// The <see cref="Meet"/> with that id number.
        /// </returns>
        public static Meet Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Meets are compared by name.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Meet"/> being compared.
        /// </param>
        public int CompareTo (Meet other)
        {
            return Name.CompareTo (other.Name);
        }
        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                 return true;
            }
            if(other is Meet)
            {
                return 0 == CompareTo((Meet)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return Name.GetHashCode();
        }
        
        override public string ToString ()
        {
            return Name;
        }
    }
}
