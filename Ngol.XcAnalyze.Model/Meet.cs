using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A recurring Cross-Country competition.
    /// </summary>
    public class Meet
    {
        #region Properties
        
        #region Fields
        
        private Cell<Team> _host;
        
        private IXDictionary<int, MeetInstance> _instances;
        
        private Cell<string> _name;
        
        #endregion

        /// <summary>
        /// The team that hosts the meet.
        /// </summary>
        public Team Host
        {
            get
            {
                return _host.Value;
            }
            
            set
            {
                _host.Value = value;
            }
        }
        
        /// <summary>
        /// The number used to identify the team that hosts the meet.
        /// </summary>
        public int? HostID
        {
            get
            {
                if (Host == null) 
                {
                    return null;
                }
                return Host.ID;
            }
        }
         
        /// <summary>
        /// The number used to identify this meet.
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// True if the meet has been stored to the database, false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        /// <summary>
        /// True if the meet has been changed since being loaded from the
        /// database, false otherwise.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (IsAttached) 
                {
                    return _host.IsChanged || _name.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// The instances of this meet.
        /// </summary>
        public IDictionary<int, MeetInstance> Instances
        {
            get
            {
                return _instances.AsReadOnly();
            }
            
            protected set
            {
                if (value == null) 
                {
                    value = new Dictionary<int, MeetInstance> ();
                }
                _instances = new XDictionary<int, MeetInstance>(value);
            }
        }
        
        /// <summary>
        /// The name of the meet.
        /// </summary>
        public string Name
        {
            get
            {
                return _name.Value;
            }
        
            protected set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Name cannot be null.");
                }
                _name.Value = value;
            }            
        }
        
        #endregion
        
        #region Constructors
        
        protected Meet ()
        {
            _name = new Cell<string> ();
            _host = new Cell<Team> ();
        }
        
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if name is null.
        /// </exception>
        public Meet (string name)
        : this(name, null)
        {
        }
        
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <param name="host">
        /// The <see cref="Team"/> that hosts the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if name is null.
        /// </exception>
        public Meet (string name, Team host)
        : this()
        {
            Name = name;
            Host = host;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the meet.
        /// </param>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <param name="host">
        /// The <see cref="Team"/> that hosts the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if name is null.
        /// </exception>
        protected Meet (int id, string name, Team host)
        : this(name, host)
        {
            ID = id;
        }
         
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the meet.
        /// </param>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if name is null.
        /// </exception>
        public static Meet NewEntity (int id, string name)
        {
            return NewEntity (id, name, null);
        }
        
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the meet.
        /// </param>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if name is null.
        /// </exception>
        public static Meet NewEntity (int id, string name, Team host)
        {
            Meet newMeet = new Meet (id, name, host);
            newMeet.IsAttached = true;
            return newMeet;
        }
        
        #endregion
        
        #region Inherited methods
        
        override public bool Equals (object other)
        {
            if (this == other) 
            {
                return true;
            }
            if (other is Meet)
            {
                return Equals ((Meet)other);
            }
            return false;
        }
        
        public bool Equals (Meet that)
        {
            return Name.Equals (that.Name);
        }
        
        override public int GetHashCode()
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
        /// Add an instance to this meet.
        /// </summary>
        /// <param name="instance">
        /// The <see cref="MeetInstance"/> to add.
        /// </param>
        public void AddInstance (MeetInstance instance)
        {
            _instances[instance.Date.Year] = instance;
        }
        
        /// <summary>
        /// Add more instances of this meet.
        /// </summary>
        /// <param name="instances">
        /// A <see cref="IEnumerable<MeetINstance>"/> of instances to add.
        /// </param>
        public void AddInstances (IEnumerable<MeetInstance> instances)
        {
            foreach (MeetInstance instance in instances)
            {
                AddInstance (instance);
            }
        }
        
        #endregion
    }
}
