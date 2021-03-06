using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ngol.Hytek.Interfaces;
using Ngol.Utilities.Collections.Extensions;
using SharpArch.Domain.DomainModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public class Runner : Entity, IRunner
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
        public virtual IDictionary<int, Team> Affiliations { get; protected set; }

        /// <summary>
        /// The year in which the <see cref="Runner" /> enrolled in college.
        /// </summary>
        public virtual int? EnrollmentYear { get; set; }

        /// <summary>
        /// The full name of the <see cref="Runner" />.
        /// </summary>
        public virtual string FullName
        {
            get { return string.Format("{0} {1}", GivenName, Surname); }
        }

        /// <summary>
        /// The <see cref="Runner" />'s <see cref="Gender" />.
        /// </summary>
        public virtual Gender Gender { get; set; }

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
        public override int Id { get; protected set; }

        /// <summary>
        /// The nicknames of this <see cref="Runner" />.
        /// </summary>
        public virtual ISet<string> Nicknames { get; protected set; }

        /// <summary>
        /// The year the <see cref="Runner" /> was originally scheduled to graduate.
        /// </summary>
        public virtual int? OriginalGraduationYear
        {
            get { return EnrollmentYear + 4; }
        }

        /// <summary>
        /// If the <see cref="Race" /> where a <see cref="Runner" /> ran
        /// their season's best for a particular season is not known,
        /// it will appear in this collection keyed by season.
        /// </summary>
        public virtual IDictionary<int, double> SeasonsBestsUnknownRace { get; protected set; }

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
        /// indexed by <see cref="Race.Id" />.
        /// </summary>
        public virtual IDictionary<Race, Performance> Performances { get; protected set; }

        /// <summary>
        /// <see cref="Race"/>s that the <see cref="Runner" /> started but did not finish.
        /// </summary>
        public virtual ISet<Race> UnfinishedRaces { get; protected set; }

        int? IRunner.GraduationYear
        {
            get { return OriginalGraduationYear; }
        }

        string IRunner.Name
        {
            get { return FullName; }
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
            Performances = new Dictionary<Race, Performance>();
            UnfinishedRaces = new HashSet<Race>();
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
        public override string ToString()
        {
            return FullName;
        }

        #endregion
    }
}

