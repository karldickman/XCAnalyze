using System;
using System.Collections.Generic;

namespace XCAnalyze.Model {
    
    /// <summary>
    /// A college or university that fields a Cross-Country team.
    /// </summary>
    public class School : IComparable<School>
    {
        /// <summary>
        /// The athletic conference with which the school is affiliated.
        /// </summary>
        public string Conference { get; protected internal set; }

        /// <summary>
        /// The name of the school (Linfield, Willamette, etc.)
        /// </summary>
        public string Name { get; protected internal set; }

        /// <summary>
        /// Should the name of the school go before its type (true Linfield College, false for University of Puget Sound).
        /// </summary>
        public bool NameFirst { get; protected internal set; }

        /// <summary>
        /// The runners who have competed for this school.
        /// </summary>
        public List<Affiliation> Runners { get; protected internal set; }

        /// <summary>
        /// The type of the school (University, College, etc.)
        /// </summary>
        public string Type { get; protected internal set; }
        
        public School(string name, string type) : this(name, type, true) {}

        public School(string name, string type, bool nameFirst)
            : this(name, type, nameFirst, null) {}
        
        public School(string name, string type, string conference)
        : this(name, type, true, conference) {}
        
        public School (string name, string type, bool nameFirst,
                string conference)
                : this(name, type, nameFirst, conference, new List<Affiliation> ()) {}

        public School (string name, string type, bool nameFirst,
            string conference, List<Affiliation> runners)
        {
            Name = name;
            Type = type;
            NameFirst = nameFirst;
            Conference = conference;
            Runners = runners;
        }

        public void AddRunner (Affiliation runner)
        {
            Runners.Add (runner);
            Runners.Sort ();
        }

        /// <summary>
        /// Schools are ordered first by name, then by type, then by conference.
        /// </summary>
        public int CompareTo (School other)
        {
            int comparison;
            if (this == other)
            {
                return 0;
            }
            comparison = Name.CompareTo (other.Name);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = ObjectComparer<string>.Compare (Type, other.Type, 1);
            if (comparison != 0)
            {
                return comparison;
            }
            if (NameFirst != other.NameFirst)
            {
                comparison = FullName ().CompareTo (other.FullName ());
                if (comparison != 0)
                {
                    return comparison;
                }
            }
            return ObjectComparer<string>.Compare (Conference, other.Conference, 1);
        }

        public override bool Equals (object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is School)
            {
                return 0 == CompareTo ((School)other);
            }
            return false;
        }

        /// <summary>
        /// The full name of the school.  For example, "Linfield College", "California Institute of Technology", or
        /// "University of California, Santa Cruz".
        /// </summary>
        public string FullName ()
        {
            if (NameFirst)
            {
                return Name + " " + Type;
            }
            return Type + " of " + Name;
        }
        
        public override int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        public override string ToString ()
        {
            return FullName ();
        }
        
    }
}