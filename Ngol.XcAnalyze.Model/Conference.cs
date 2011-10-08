using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// An athletic conference is the basic unit into which teams organize
    /// themselves.
    /// </summary>
    public class Conference
    {
        #region Properties

        #region Physical implementation

        private string _acronym;
        private string _name;

        #endregion

        /// <summary>
        /// The acronym conventionally used for the conference.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual string Acronym
        {
            get { return _acronym; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(Acronym != value)
                {
                    _acronym = value;
                }
            }
        }

        /// <summary>
        /// The number that identifies this conference.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the conference.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(Name != value)
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Team" />s affiliated with this <see cref="Conference" />.
        /// </summary>
        public virtual ISet<Team> Teams
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new conference.
        /// </summary>
        /// <param name="name">
        /// The name of the conference.
        /// </param>
        /// <param name="acronym">
        /// The acronym conventionally used for the conference.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="name"/> or <paramref name="acronym"/>
        /// is <see langword="null" />.
        /// </exception>
        public Conference(string name, string acronym)
        {
            Name = name;
            Acronym = acronym;
            Teams = new HashedSet<Team>();
        }

        /// <summary>
        /// Construct a new conference.
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Conference()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return this == other ? true : Equals(other as Conference);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Conference"/> is equal to the current <see cref="Ngol.XcAnalyze.Model.Conference"/>.
        /// </summary>
        /// <param name='that'>
        /// The <see cref="Conference"/> to compare with the current <see cref="Ngol.XcAnalyze.Model.Conference"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Conference"/> is equal to the current
        /// <see cref="Ngol.XcAnalyze.Model.Conference"/>; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Equals(Conference that)
        {
            if(that == null)
            {
                return false;
            }
            if(this == that)
            {
                return true;
            }
            return Name == that.Name;
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

        #endregion
    }
}

