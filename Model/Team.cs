using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A college or university that fields a Cross-Country team.
    /// </summary>
    public class Team
    {
        #region Properties

        #region Fields

        private Cell<Conference> _conference;

        private IXList<MeetInstance> _hostedMeetInstances;

        private IXList<Meet> _hostedMeets;

        private Cell<string> _name;

        private IXList<string> _nicknames;

        private IXList<Affiliation> _runners;

        #endregion

        /// <summary>
        /// The athletic conference with which the school is affiliated.
        /// </summary>
        public Conference Conference {
            get { return _conference.Value; }

            set { _conference.Value = value; }
        }

        /// <summary>
        /// The number used to identify the conference.
        /// </summary>
        public int ConferenceID {
            get { return Conference.ID; }
        }

        /// <summary>
        /// The meet instances this team has hosted.
        /// </summary>
        public IList<MeetInstance> HostedMeetInstances {
            get {
                IXList<MeetInstance> allInstances = new XList<MeetInstance>();
                foreach(Meet meet in HostedMeets) {
                    allInstances.AddRange(meet.Instances.Values);
                }
                allInstances.AddRange(_hostedMeetInstances);
                return allInstances.AsReadOnly();
            }

            protected set {
                if(value == null) {
                    value = new List<MeetInstance>();
                }
                _hostedMeetInstances = new XList<MeetInstance>(value);
            }
        }

        /// <summary>
        /// The meets that this team hosts.
        /// </summary>
        public IList<Meet> HostedMeets {
            get { return _hostedMeets.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<Meet>();
                }
                _hostedMeets = new XList<Meet>(value);
            }
        }

        /// <summary>
        /// The number that identifies this team.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// True if a corresponding record exits for this team in the database,
        /// false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }

        /// <summary>
        /// True if the team has been changed since being loaded from the
        /// database, false otherwise.
        /// </summary>
        public bool IsChanged {
            get {
                if(IsAttached) {
                    return _name.IsChanged || _conference.IsChanged;
                }
                return false;
            }
        }

        /// <summary>
        /// The name of the school (Linfield, Willamette, etc.)
        /// </summary>
        public string Name {
            get { return _name.Value; }

            protected set {
                if(value == null) {
                    throw new ArgumentNullException("Property Name cannot be null.");
                }
                _name.Value = value;
            }
        }

        /// <summary>
        /// The nicknames or alternate names of the school.
        /// </summary>
        public IList<string> Nicknames {
            get { return _nicknames.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<string>();
                }
                _nicknames = new XList<string>(value);
            }
        }

        /// <summary>
        /// The runners who have competed for this school.
        /// </summary>
        public IList<Affiliation> Runners {
            get { return _runners.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<Affiliation>();
                }
                _runners = new XList<Affiliation>(value);
            }
        }

        #endregion

        #region Constructors

        public Team()
        {
            _name = new Cell<string>();
            _conference = new Cell<Conference>();
            _runners = new XList<Affiliation>();
            _nicknames = new XList<string>();
        }

        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        public Team(string name) : this(name, null)
        {
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="name">
        /// The name of the team (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="conference">
        /// The conference with which this school is affiliated.
        /// </param>
        public Team(string name, Conference conference) : this()
        {
            Name = name;
            Conference = conference;
            IsAttached = false;
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="id">
        /// The number used to identify this team.
        /// </param>
        /// <param name="name">
        /// The name of the team (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="conference">
        /// The conference with which this team is affiliated.
        /// </param>
        protected Team(int id, string name, Conference conference) : this(name, conference)
        {
            ID = id;
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="id">
        /// The number used to identify this team.
        /// </param>
        /// <param name="name">
        /// The name of the team (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        public static Team NewEntity(int id, string name)
        {
            return NewEntity(id, name, null);
        }

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="id">
        /// The number used to identify this team.
        /// </param>
        /// <param name="name">
        /// The name of the team (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <param name="conference">
        /// The conference with which this team is affiliated.
        /// </param>
        public static Team NewEntity(int id, string name, Conference conference)
        {
            Team newTeam = new Team(id, name, conference);
            newTeam.IsAttached = true;
            return newTeam;
        }

        #endregion

        #region Inherited methods

        public override bool Equals(object other)
        {
            if(this == other) {
                return true;
            }
            if(other is Team) {
                return Equals((Team)other);
            }
            return false;
        }

        /// <summary>
        /// Schools are ordered first by name, then by type, then by conference.
        /// </summary>
        public bool Equals(Team other)
        {
            return Name.Equals(other.Name) && (Conference == other.Conference || Conference != null && Conference.Equals(other.Conference));
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add another nickname for this team.
        /// </summary>
        /// <param name="nickname">
        /// The nickname to add.
        /// </param>
        public void AddNickname(string nickname)
        {
            _nicknames.Add(nickname);
        }
        
        /// <summary>
        /// Register a runner as having competed for this school.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        public void AddRunner(Affiliation runner)
        {
            _runners.Add(runner);
        }
        
        #endregion
    }
}
