using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A recurring Cross-Country competition.
    /// </summary>
    public class Meet
    {
        #region Properties

        #region Physical implementation
        
        private string _name;

        #endregion

        /// <summary>
        /// The team that hosts the meet.
        /// </summary>
        public virtual Team Host
        {
            get;
            set;
        }

        /// <summary>
        /// The number used to identify this meet.
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }

        /// <summary>
        /// The instances of this <see cref="Meet" />.
        /// </summary>
        public virtual ISet<MeetInstance> MeetInstances
        {
            get;
            protected set;
        }

        /// <summary>
        /// The name of the meet.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property to <see langword="null" />.
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
        /// Create a new meet.
        /// </summary>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if name is null.
        /// </exception>
        public Meet(string name) : this(name, null)
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
        protected Meet(string name, Team host) : this()
        {
            Name = name;
            Host = host;
            MeetInstances = new HashedSet<MeetInstance>();
        }

        /// <summary>
        /// Construct a new <see cref="Meet" />
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Meet()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return this == other ? true : Equals(other as Meet);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Meet"/> is equal to the current <see cref="Ngol.XcAnalyze.Model.Meet"/>.
        /// </summary>
        /// <param name='that'>
        /// The <see cref="Meet"/> to compare with the current <see cref="Ngol.XcAnalyze.Model.Meet"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Meet"/> is equal to the current
        /// <see cref="Ngol.XcAnalyze.Model.Meet"/>; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Equals(Meet that)
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
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
