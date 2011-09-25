using System;
using System.Collections.Generic;
using System.ComponentModel;
using Iesi.Collections.Generic;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A recurring Cross-Country competition.
    /// </summary>
    public class Meet : ICloneable
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

        /// <summary>
        /// Overload of == operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator ==(Meet meet1, Meet meet2)
        {
            if(ReferenceEquals(meet1, meet2))
            {
                return true;
            }
            if(ReferenceEquals(meet1, null) || ReferenceEquals(meet2, null))
            {
                return false;
            }
            return meet1.Equals(meet2);
        }

        /// <summary>
        /// Overload of != operator that delegates to <see cref="Equals(object)" />.
        /// </summary>
        public static bool operator !=(Meet meet1, Meet meet2)
        {
            return !(meet1 == meet2);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            if(other is Meet)
            {
                return Equals((Meet)other);
            }
            return Equals(other as Meet);
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

        /// <summary>
        /// Determine if two <see cref="Meet" />s are equal.
        /// </summary>
        protected bool Equals(Meet that)
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

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
        
        #endregion
    }
}
