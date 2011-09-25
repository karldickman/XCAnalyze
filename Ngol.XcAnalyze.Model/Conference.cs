using System;
using System.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// An athletic conference is the basic unit into which teams organize
    /// themselves.
    /// </summary>
    public class Conference : ICloneable
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

        /// <summary>
        /// Overload of operator == that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(Conference conference1, Conference conference2)
        {
            if(ReferenceEquals(conference1, conference2))
            {
                return true;
            }
            if(ReferenceEquals(conference1, null) || ReferenceEquals(conference2, null))
            {
                return false;
            }
            return conference1.Equals(conference2);
        }

        /// <summary>
        /// Overload of operator != that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(Conference conference1, Conference conference2)
        {
            return !(conference1 == conference2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(other, null))
            {
                return false;
            }
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other as Conference);
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
        /// Determine if two conferences are equal.
        /// </summary>
        protected bool Equals(Conference that)
        {
            if(ReferenceEquals(that, null))
            {
                return false;
            }
            if(ReferenceEquals(this, that))
            {
                return true;
            }
            return Name == that.Name;
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

