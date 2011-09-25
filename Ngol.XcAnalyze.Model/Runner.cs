using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Iesi.Collections.Generic;
using Ngol.XcAnalyze.Model.Interfaces;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner : ICloneable, INotifyPropertyChanged
    {
        #region Properties

        #region Physical implementation

        private int _id;

        #endregion

        /// TODO DELETE
        public static IEnumerable<Runner> Instances
        {
            get { return InstanceCollection; }
        }

        /// <summary>
        /// The <see cref="Team" />s this <see cref="Runner" /> has
        /// been affiliated with, indexed by year.
        /// </summary>
        public virtual IDictionary<int, int> Affiliations
        {
            get;
            protected set;
        }

        /// <summary>
        /// The year in which the <see cref="Runner" /> enrolled in college.
        /// </summary>
        public virtual int EnrollmentYear
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Runner" />'s <see cref="Gender" />.
        /// </summary>
        public virtual Gender Gender
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="Runner" />'s given or Christian name.
        /// </summary>
        public virtual string GivenName
        {
            get;
            set;
        }

        /// <summary>
        /// A number used to identify a <see cref="Runner" />.
        /// </summary>
        public virtual int ID
        {
            get { return _id; }

            set
            {
                if(ID != value)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        /// <summary>
        /// The nicknames of this <see cref="Runner" />.
        /// </summary>
        public virtual ISet<string> Nicknames
        {
            get;
            protected set;
        }

        /// <summary>
        /// If the <see cref="Race" /> where a <see cref="Runner" /> ran
        /// their season's best for a particular season is not known,
        /// it will appear in this collection keyed by season.
        /// </summary>
        public virtual IDictionary<int, double> SeasonsBestsUnknownRace
        {
            get;
            protected set;
        }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        public virtual string Surname
        {
            get;
            set;
        }

        /// <summary>
        /// TODO DELETE
        /// </summary>
        protected static readonly ICollection<Runner> InstanceCollection;

        #endregion

        #region Constructors

        static Runner()
        {
            InstanceCollection = new List<Runner>();
        }

        /// <summary>
        /// Create a new <see cref="Runner" />.
        /// </summary>
        /// <param name="surname">
        /// The <see cref="Runner" />'s surname.
        /// </param>
        /// <param name="givenName">
        /// The <see cref="Runner" />'s given name.
        /// </param>
        /// <param name="gender">
        /// The <see cref="Runner" />'s <see cref="Gender" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either argument is null.
        /// </exception>
        public Runner(string surname, string givenName, Gender gender) : this()
        {
            if(surname == null)
                throw new ArgumentNullException("surname");
            if(givenName == null)
                throw new ArgumentNullException("givenName");
            Surname = surname;
            GivenName = givenName;
            Gender = gender;
            Affiliations = new Dictionary<int, int>();
        }

        /// <summary>
        /// Construct a new <see cref="Runner" />.
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Runner()
        {
            InstanceCollection.Add(this);
        }

        #endregion

        #region Methods

        /// TODO DELETE
        public virtual Team GetTeam(int season)
        {
            return Team.Instances.Single(t => t.ID == Affiliations[season]);
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Runner)
            {
                return Equals((Runner)other);
            }
            return false;
        }

        /// <summary>
        /// Check if two runners are equal.
        /// </summary>
        /// <param name="that">
        /// The <see cref="Runner"/> to compare with this instance.
        /// </param>
        protected bool Equals(Runner that)
        {
            if(ID == 0 || that.ID == 0)
            {
                return Surname == that.Surname && GivenName == that.GivenName && EnrollmentYear == that.EnrollmentYear;
            }
            return ID == that.ID;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (ToString() + EnrollmentYear).GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} {1}", GivenName, Surname);
        }

        #endregion

        #region ICloneable implementation

        /// <inheritdoc />
        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region INotifyPropertyChanged

        /// <inheritdoc />
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event invoker for <see cref="PropertyChanged" />.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event invoker for <see cref="PropertyChanged" />.
        /// </summary>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, e);
            }
        }
        
        #endregion
    }
}

