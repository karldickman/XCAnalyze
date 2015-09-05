using System;
using System.Collections.Generic;
using SharpArch.Domain.DomainModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// Represents a city or municipality within a <see cref="State" />.
    /// </summary>
    public class City : Entity
    {
        #region Properties

        #region Physical implementation

        private string _name;
        private State _state;

        #endregion

        /// <summary>
        /// The identifying number of this instance.
        /// </summary>
        public override int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// The name of the city.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            protected set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if(value != Name)
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// The state in which the city is located geographically.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set this property <see langword="null" />.
        /// </exception>
        public virtual State State
        {
            get { return _state; }

            protected set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if(value != State)
                {
                    _state = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Venue" />s in this <see cref="City" />.
        /// </summary>
        public virtual ISet<Venue> Venues
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new city.
        /// </summary>
        /// <param name="name">
        /// The name of the city.
        /// </param>
        /// <param name="state">
        /// The state in which the city is located geographically..
        /// </param>
        public City(string name, State state) : this()
        {
            Name = name;
            State = state;
            Venues = new HashSet<Venue>();
        }

        /// <summary>
        /// Construct a new city.  This constructor is required by NHibernate.
        /// </summary>
        protected City()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, State);
        }

        #endregion
    }
}