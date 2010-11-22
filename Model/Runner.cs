using System;
using System.Collections.Generic;
using XCAnalyze.Collections;

namespace XCAnalyze.Model
{  
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public partial class Runner
    {
        #region Properties
        
        #region Fields
        
        private Cell<int?> _enrollmentYear;
        
        private Cell<Gender> _gender;
        
        private Cell<string> _givenName;
        
        private IXList<string> _nicknames;
        
        private IXList<Performance> _performances;
        
        private IXDictionary<int, Team> _teams;
        
        private Cell<string> _surname;
        
        #endregion
        
        /// <summary>
        /// The year the runner enrolled in college.
        /// </summary>
        public int? EnrollmentYear { get; set; }
        
        /// <summary>
        /// The runner's gender.
        /// </summary>
        public Gender Gender
        {
            get { return _gender.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Gender cannot be null.");
                }
                _gender.Value = value;
            }
        }

        /// <summary>
        /// The runner's given name.
        /// </summary>
        public string GivenName
        {
            get { return _givenName.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property GivenName cannot be null.");
                }
                _givenName.Value = value;
            }
        }
        
        /// <summary>
        /// The number that identifies this runner.
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// True if a corresponding row exists in the database, false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }

        /// <summary>
        /// True if changes have been made since the runner was loaded from
        /// the database, false otherwise.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (!IsAttached) 
                {
                    return _surname.IsChanged || _givenName.IsChanged ||
                        _gender.IsChanged || _enrollmentYear.IsChanged;
                }
                return false;
            }
        }
        
        /// <summary>
        /// The runner's full name.
        /// </summary>
        public string Name
        {
            get { return GivenName + " " + Surname; }
        }

        /// <summary>
        /// The nicknames or alternate names of the runner.
        /// </summary>
        public IList<string> Nicknames
        {
            get { return _nicknames.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<string> ();
                }
                _nicknames = new XList<string> (value);
            }
        }

        /// <summary>
        /// The performances this runner has achieved.
        /// </summary>
        public IList<Performance> Performances
        {
            get { return _performances.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new List<Performance> ();
                }
                _performances = new XList<Performance> (value);
            }
        }

        /// <summary>
        /// The affiliations of the runner over their career.
        /// </summary>
        public IDictionary<int, Team> Teams
        {
            get { return _teams.AsReadOnly (); }
            
            protected set
            {
                if (value == null) 
                {
                    value = new Dictionary<int, Team> ();
                }
                _teams = new XDictionary<int, Team> (value);
            }
        }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public string Surname
        {
            get { return _surname.Value; }
            
            set
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (
                        "Property Surname cannot be null.");
                }
                _surname.Value = value;
            }
        }
        
        #endregion

        #region Constructors
        
        protected Runner ()
        {
            _surname = new Cell<string> ();
            _givenName = new Cell<string> ();
            _gender = new Cell<Gender> ();
            _enrollmentYear = new Cell<int?> ();
            _nicknames = new XList<string> ();
            _performances = new XList<Performance> ();
            _teams = new XDictionary<int, Team> ();
        }
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        public Runner (string surname, string givenName, Gender gender)
        : this(surname, givenName, gender, null)
        {
        }

        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        public Runner (string surname, string givenName, Gender gender,
            int? year)
        : this()
        {
            Surname = surname;
            GivenName = givenName;
            Gender = gender;
            EnrollmentYear = year;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the runner.
        /// </param>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        protected Runner (int id, string surname, string givenName,
            Gender gender, int? year)
        : this(surname, givenName, gender, year)
        {
            ID = id;
        }
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the runner.
        /// </param>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        public static Runner NewEntity (int id, string surname, string givenName, Gender gender)
        {
            return NewEntity (id, surname, givenName, gender, null);
        }
        
        /// <summary>
        /// Create a new runner.
        /// </summary>
        /// <param name="id">
        /// The number used to identify the runner.
        /// </param>
        /// <param name="surname">
        /// The runner's surname.
        /// </param>
        /// <param name="givenName">
        /// The runner's given name.
        /// </param>
        /// <param name="gender">
        /// The runner's gender.
        /// </param>
        /// <param name="year">
        /// The year in which the runner was initially scheduled to graduate.
        /// </param>
        public static Runner NewEntity (int id, string surname,
            string givenName, Gender gender, int? year)
        {
            Runner newRunner = new Runner (id, surname, givenName, gender,
                year);
            newRunner.IsAttached = true;
            return newRunner;
        }
        
        #endregion
        
        #region Inherited methods
                
        override public bool Equals (object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is Runner)
            {
                return Equals ((Runner)other);
            }
            return false;
        }
        
        public bool Equals (Runner other)
        {
            return Surname.Equals (other.Surname) &&
                GivenName.Equals (other.GivenName) &&
                EnrollmentYear == other.EnrollmentYear &&
                Gender == other.Gender;
        }
        
        override public int GetHashCode ()
        {
            return ID;
        }
        
        override public string ToString ()
        {
            return Name + " (" + EnrollmentYear + ")";
        }  
        
        #endregion
        
        #region Methods
                
        /// <summary>
        /// Register a new affiliation for this runner.
        /// </summary>
        /// <param>
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        public void AddAffiliation (Affiliation affiliation)
        {
            AddAffiliation (affiliation.Season, affiliation.Team);
        }
        
        /// <summary>
        /// Register a new affiliation for this runner.
        /// </summary>
        /// <param name="season">
        /// The season in which the runner ran for the team.
        /// </param>
        /// <param name="team">
        /// The <see cref="Team"/> for which the runner ran.
        /// </param>
        public void AddAffiliation (int season, Team team)
        {
            _teams[season] = team;
        }
        
        /// <summary>
        /// Add another nickname for this user.
        /// </summary>
        /// <param name="nickname">
        /// The nickname to add.
        /// </param>
        public void AddNickname (string nickname)
        {
            _nicknames.Add (nickname);
        }
        
        /// <summary>
        /// Add more nicknames for this user.
        /// </summary>
        /// <param name="nicknames">
        /// A <see cref="IEnumerable<System.String>"/> of nicknames to add.
        /// </param>
        public void AddNicknames (IEnumerable<string> nicknames)
        {
            _nicknames.AddRange (nicknames);
        }
        
        /// <summary>
        /// Register a new performance ran by this runner.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to register.
        /// </param>
        public void AddPerformance (Performance performance)
        {
            _performances.Add (performance);
        }
        
        /// <summary>
        /// Register more performances by this user.
        /// </summary>
        /// <param name="performances">
        /// A <see cref="IEnumerable<Performance>"/> of performances to add.
        /// </param>
        public void AddPerformances (IEnumerable<Performance> performances)
        {
            _performances.AddRange (performances);
        }
        
        #endregion  
    }
}