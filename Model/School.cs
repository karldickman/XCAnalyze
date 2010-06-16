using System;
using System.Collections.Generic;

namespace XCAnalyze.Model
{    
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
        /// The nicknames or alternate names of the school.
        /// </summary>
        public IList<string> Nicknames { get; protected internal set; }
        
        /// <summary>
        /// The runners who have competed for this school.
        /// </summary>
        public IList<Affiliation> Runners { get; protected internal set; }

        /// <summary>
        /// The type of the school (University, College, etc.)
        /// </summary>
        public string Type { get; protected internal set; }
                
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        public School(string name, string type)
        : this(name, type, new List<string>()) {}
         
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of nicknames or alternate names
        /// of the school.
        /// </param>
        public School(string name, string type, IList<string> nicknames)
        : this(name, type, true, nicknames) {}
                
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        public School(string name, string type, bool nameFirst)
        : this(name, type, nameFirst, new List<string>()) {}
        
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of nicknames or alternate names
        /// of the school.
        /// </param>
        public School (string name, string type, bool nameFirst,
            IList<string> nicknames)
        : this(name, type, nameFirst, nicknames, null) {}
            
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        public School(string name, string type, string conference)
        : this(name, type, new List<string>(), conference) {}
        
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of nicknames or alternate names
        /// of the school.
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        public School(string name, string type, IList<string> nicknames,
            string conference)
        : this(name, type, true, nicknames, conference) {}
                
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        public School (string name, string type, bool nameFirst,
            string conference)
        : this(name, type, nameFirst, new List<string>(), conference) {}

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of nicknames or alternate names
        /// of the school.
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        public School(string name, string type, bool nameFirst, 
            IList<string> nicknames, string conference)
        : this(name, type, nameFirst, nicknames, conference,
            new List<Affiliation>()) {}
                
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        /// <param name="runners">
        /// A <see cref="List<Affiliation>"/> of runners who have
        /// competed for this school.
        /// </param>
        public School (string name, string type, bool nameFirst,
            string conference, List<Affiliation> runners)
        : this(name, type, nameFirst, new List<string>(), conference, runners) {}
        
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of nicknames or alternate names
        /// of the school.
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        /// <param name="runners">
        /// A <see cref="List<Affiliation>"/> of runners who have compted for
        /// this school.
        /// </param>
        public School (string name, string type, bool nameFirst,
            IList<string> nicknames, string conference,
            IList<Affiliation> runners)
        {
            Name = name;
            Type = type;
            NameFirst = nameFirst;
            Nicknames = nicknames;
            Conference = conference;
            Runners = runners;
        }

        /// <summary>
        /// Register a runner as having competed for this school.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        public void AddRunner (Affiliation runner)
        {
            Runners.Add (runner);
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

        override public bool Equals (object other)
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
        /// The full name of the school.  For example, "Linfield College",
        /// "California Institute of Technology", or "University of California,
        /// Santa Cruz".
        /// </summary>
        public string FullName ()
        {
            if (NameFirst)
            {
                return Name + " " + Type;
            }
            return Type + " of " + Name;
        }
        
        override public int GetHashCode ()
        {
            return base.GetHashCode ();
        }

        override public string ToString ()
        {
            return FullName ();
        }
        
    }
}