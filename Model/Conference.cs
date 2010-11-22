using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// An athletic conference is the basic unit into which teams organize
    /// themselves.
    /// </summary>
    public class Conference
    {
        #region Properties
        
        #region Fields
        
        private Cell<string> _acronym;
        private Cell<string> _name;
        private IXList<Team> _teams;

        #endregion
        
        /// <summary>
        /// The acronym conventionally used for the conference.
        /// </summary>
        public string Acronym
        {
            get { return _acronym.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Acronym cannot be null.");
                }
                _acronym.Value = value;
            }
        }
        
        /// <summary>
        /// The number that identifies this conference.
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// True if the conference is stored in the databas.  False otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        public bool IsChanged
        {
            get
            {
                if (IsAttached) 
                {
                    return _name.IsChanged || _acronym.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// The name of the conference.
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
        /// The teams affiliated with the conference.
        /// </summary>
        public IList<Team> Teams
        {
            get { return _teams; }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<Team> ();
                }
                _teams = new XList<Team> (value);
            }
        }
        
        #endregion
        
        #region Constructors
        
        protected Conference ()
        {
            _name = new Cell<string> ();
            _acronym = new Cell<string> ();
        }
        
        /// <summary>
        /// Create a new conference.
        /// </summary>
        /// <param name="name">
        /// The name of the conference.
        /// </param>
        /// <param name="acronym">
        /// The acronym conventionally used for the conference.
        /// </param>
        public Conference (string name, string acronym)
        : this()
        {
            Name = name;
            Acronym = acronym;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new conference.
        /// </summary>
        /// <param name="id">
        /// The identification number for the conference.
        /// </param>
        /// <param name="name">
        /// The name of the conference.
        /// </param>
        /// <param name="acronym">
        /// The acronym conventionally used for the conference.
        /// </param>
        protected Conference (int id, string name, string acronym)
        : this(name, acronym)
        {
            ID = id;
        }

        /// <summary>
        /// Create a new conference.
        /// </summary>
        /// <param name="id">
        /// The identification number for the conference.
        /// </para>
        /// <param name="name">
        /// The name of the conference.
        /// </param>
        /// <param name="acronym">
        /// The acronym conventionally used for the conference.
        /// </param>
        /// <param name="teams">
        /// A <see cref="IList<Team>"/> of teams in the conference.
        /// </param>
        public static Conference NewEntity (int id, string name,
            string acronym)
        {
            Conference newConference = new Conference (id, name, acronym);
            newConference.IsAttached = true;
            return newConference;
        }
        
        #endregion
        
        #region Inherited methods
        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            else if(other is Conference)
            {
                return Equals((Conference)other);
            }
            return false;
        }
        
        public bool Equals (Conference that)
        {
            return Name.Equals (that.Name);
        }
        
        override public int GetHashCode ()
        {
            return ID;
        }
        
        override public string ToString()
        {
            return Name;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Add a new team to the conference.
        /// </summary>
        /// <param name="team">
        /// The <see cref="Team"/> to add.
        /// </param>
        public void AddTeam (Team team)
        {
            _teams.Add (team);
        }
        
        /// <summary>
        /// Add new teams to the conference.
        /// </summary>
        /// <param name="teams">
        /// A <see cref="IEnumerable<Team>"/> of teams to add.
        /// </param>
        public void AddTeams (IEnumerable<Team> teams)
        {
            _teams.AddRange (teams);
        }
        
        #endregion
    }
}

