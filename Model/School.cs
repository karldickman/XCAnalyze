using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{    
    /// <summary>
    /// A college or university that fields a Cross-Country team.
    /// </summary>
    public class School
    {
        private IXList<string> _nicknames;
       
        private IXList<Affiliation> _runners;
        
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        public School (string name, string type)
        : this(name, type, new XList<string> ()) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nicknames">
        /// A <see cref="IList<System.String>"/> of nicknames or alternate names
        /// of the school.
        /// </param>
        public School (string name, string type, IXList<string> nicknames)
        : this(name, type, true, nicknames) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="nameFirst">
        /// Should the name go before the type (Willamette University) or not
        /// (University of Puget Sound).
        /// </param>
        public School (string name, string type, bool nameFirst)
            : this(name, type, nameFirst, new XList<string> ()) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
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
            IXList<string> nicknames)
            : this(name, type, nameFirst, nicknames, null) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="type">
        /// The type of the school (College, University, Institude of
        /// Technology, etc.).
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        public School (string name, string type, string conference)
        : this(name, type, new XList<string> (), conference) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
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
        public School (string name, string type, IXList<string> nicknames,
            string conference)
        : this(name, type, true, nicknames, conference) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
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
        : this(name, type, nameFirst, new XList<string> (), conference) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
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
        public School (string name, string type, bool nameFirst,
            IXList<string> nicknames, string conference)
        : this(name, type, nameFirst, nicknames, conference,
                new XList<Affiliation> ()) { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
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
            string conference, XList<Affiliation> runners)
        : this(name, type, nameFirst, new XList<string> (), conference, runners)
        { }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
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
            IXList<string> nicknames, string conference,
            IXList<Affiliation> runners)
        {
            if (name == null)
            {
                throw new ArgumentNullException (                    
                    "No school may have a null name.");
            }
            Name = name;
            Type = type;
            NameFirst = nameFirst;
            _nicknames = nicknames;
            Conference = conference;
            _runners = runners;
        }
        
        /// <summary>
        /// The athletic conference with which the school is affiliated.
        /// </summary>
        public string Conference { get; protected set; }

        /// <summary>
        /// The name of the school (Linfield, Willamette, etc.)
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Should the name of the school go before its type (true Linfield College, false for University of Puget Sound).
        /// </summary>
        public bool NameFirst { get; protected set; }

        /// <summary>
        /// The nicknames or alternate names of the school.
        /// </summary>
        public IList<string> Nicknames
        {
            get { return _nicknames.AsReadOnly (); }
        }
        
        /// <summary>
        /// The runners who have competed for this school.
        /// </summary>
        public IList<Affiliation> Runners
        {
            get { return _runners.AsReadOnly (); }
        }

        /// <summary>
        /// The type of the school (University, College, etc.)
        /// </summary>
        public string Type { get; protected set; }
        
        /// <summary>
        /// Register a runner as having competed for this school.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        public void Add (Affiliation runner)
        {
            _runners.Add (runner);
        }

        override public bool Equals (object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is School)
            {
                return Equals ((School)other);
            }
            return false;
        }
        
        /// <summary>
        /// Schools are ordered first by name, then by type, then by conference.
        /// </summary>
        protected bool Equals (School other)
        {
            return Name.Equals (other.Name) &&
                (Type == other.Type ||
                    Type != null && Type.Equals (other.Type)) &&
                NameFirst == other.NameFirst &&
                (Conference == other.Conference ||
                    Conference != null && Conference.Equals (other.Conference));
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