using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A recurring Cross-Country competition.
    /// </summary>
    public class Meet : ICloneable, INotifyPropertyChanged
    {
        #region Properties

        #region Physical implementation

        private int _id;

        #endregion

        /// <summary>
        /// DELETE ME
        /// </summary>
        public static IEnumerable<Meet> Instances
        {
            get { return InstancesCollection; }
        }

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
        /// The name of the meet.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// DELETE ME
        /// </summary>
        protected static readonly ICollection<Meet> InstancesCollection;

        #endregion

        #region Constructors

        static Meet()
        {
            InstancesCollection = new List<Meet>();
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
            if(name == null)
                throw new ArgumentNullException("name");
            Name = name;
            Host = host;
        }

        /// <summary>
        /// Construct a new <see cref="Meet" />
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected Meet()
        {
            InstancesCollection.Add(this);
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

        #region INotifyPropertyChanged

        /// <inheritdoc />
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event invoker for <see cref="PropertyChanged" />.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event invoker for <see cref="PropertyChanged" />.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
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
