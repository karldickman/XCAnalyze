using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A college or university that fields a Cross-Country team.
    /// </summary>
    public class Team : ICloneable
    {
        #region Properties

        /// <summary>
        /// The athletic conference with which the school is affiliated.
        /// </summary>
        public virtual Conference Conference
        {
            get;
            set;
        }

        /// <summary>
        /// The number that identifies this team.
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the school (Linfield, Willamette, etc.)
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        #endregion

        #region Constructors

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
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="name"/> is <see langword="null" />.
        /// </exception>
        public Team(int id, string name)
            : this(id, name, null)
        {
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
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="name"/> is <see langword="null" />.
        /// </exception>
        public Team(int id, string name, Conference conference)
        {
            if(name == null)
                throw new ArgumentNullException("name");
            ID = id;
            Name = name;
            Conference = conference;
        }

        /// <summary>
        /// Construct a new team.
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Team()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as Team);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Check whether two schools are equal.
        /// </summary>
        protected bool Equals(Team other)
        {
            if(ReferenceEquals(other, null))
            {
                return false;
            }
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Name == other.Name;// && (Conference == other.Conference);
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
