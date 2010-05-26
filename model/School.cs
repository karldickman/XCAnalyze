using System;
using System.Collections.Generic;

namespace XcAnalyze.Model {
    
    /// <summary>
    /// A college or university that fields a Cross-Country team.
    /// </summary>
    public class School : IComparable<School>
    {
        private List<Affiliation> affiliations;
        private string conference;
        private int id;
        private string name;
        private bool nameFirst;
        private string type;
        
        /// <summary>
        /// The runners who have competed for this school.
        /// </summary>
        public List<Affiliation> Affiliations
        {
            get { return affiliations; }
        }
        
        /// <summary>
        /// The athletic conference with which the school is affiliated.
        /// </summary>
        public string Conference
        {
            get { return conference; }
        }
        
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// The name of the school (Linfield, Willamette, etc.)
        /// </summary>
        public string Name
        {
            get { return name; }
            protected set { name = value; }
        }

        /// <summary>
        /// Should the name of the school go before its type (true Linfield College, false for University of Puget Sound).
        /// </summary>
        public bool NameFirst
        {
            get { return nameFirst; }
            protected set { nameFirst = value; }
        }

        /// <summary>
        /// The type of the school (University, College, etc.)
        /// </summary>
        public string Type
        {
            get { return type; }
            protected set { type = value; }
        }

        public School (int id, string name, string type, bool nameFirst, string conference) : this(id, name, type, nameFirst, conference, new List<Affiliation> ()) {}

        public School (int id, string name, string type, bool nameFirst, string conference, List<Affiliation> affiliations)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.nameFirst = nameFirst;
            this.conference = conference;
            this.affiliations = affiliations;
        }

        public void AddAffiliation (Affiliation affiliation)
        {
            affiliations.Add (affiliation);
            affiliations.Sort ();
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
            comparison = name.CompareTo (other.name);
            if (comparison != 0)
            {
                return comparison;
            }
            comparison = type.CompareTo (other.type);
            if (comparison != 0) {
                return comparison;
            }
            if (nameFirst != other.nameFirst)
            {
                comparison = FullName ().CompareTo (other.FullName ());
                if (comparison != 0)
                {
                    return comparison;
                }
            }
            return conference.CompareTo (other.conference);
        }

        public override bool Equals (object other)
        {
            if (this == other) {
                return true;
            }
            if (other is School) {
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
            if (nameFirst) {
                return name + " " + type;
            }
            return type + " of " + name;
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