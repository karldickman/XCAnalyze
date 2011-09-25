using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Iesi.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner : ICloneable
    {
        #region Properties

        #region Physical implementation

        private string _givenName;
        private string _surname;

        #endregion

        /// <summary>
        /// The <see cref="Team" />s this <see cref="Runner" /> has
        /// been affiliated with, indexed by year.
        /// </summary>
        public virtual IDictionary<int, Team> Affiliations
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
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string GivenName
        {
            get { return _givenName; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _givenName = value;
            }
        }

        /// <summary>
        /// A number used to identify a <see cref="Runner" />.
        /// </summary>
        public virtual int ID
        {
            get;
            set;
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
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string Surname
        {
            get { return _surname; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _surname = value;
            }
        }

        /// <summary>
        /// The <see cref="Performance"/>s the <see cref="Runner" /> has under their belt,
        /// indexed by <see cref="Race.ID" />.
        /// </summary>
        public virtual IDictionary<Race, Performance> Performances
        {
            get
            {
                IDictionary<Race, Performance> performances = new Dictionary<Race, Performance>(Times.Count + UnfinishedRaces.Count);
                foreach(Race race in UnfinishedRaces)
                {
                    performances[race] = new Performance(this, race, null);
                }
                Times.ForEach((race, time) =>
                {
                    performances[race] = new Performance(this, race, time);
                });
                return performances;
            }
        }

        /// <summary>
        /// The times the <see cref="Runner" /> has under their belt,
        /// indexed by <see cref="Race.ID" />.
        /// </summary>
        public virtual IDictionary<Race, double> Times
        {
            get;
            protected set;
        }

        /// <summary>
        /// <see cref="Race"/>s that the <see cref="Runner" /> started but did not finish.
        /// </summary>
        public virtual ISet<Race> UnfinishedRaces
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

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
            Surname = surname;
            GivenName = givenName;
            Gender = gender;
            Affiliations = new Dictionary<int, Team>();
            Times = new Dictionary<Race, double>();
            UnfinishedRaces = new HashedSet<Race>();
        }

        /// <summary>
        /// Construct a new <see cref="Runner" />.
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Runner()
        {
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
    }
}

