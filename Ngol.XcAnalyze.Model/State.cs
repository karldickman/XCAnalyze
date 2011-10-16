using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
using SharpArch.Domain.DomainModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// One of the fifty states in the United States.
    /// </summary>
    public class State : EntityWithTypedId<string>
    {
        #region Properties

        #region Physical implementation

        private string _code;
        private string _name;

        #endregion

        /// <summary>
        /// The postal abbreviation for the state.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value is set to <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public override string Id
        {
            get { return _code; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _code = value;
            }
        }

        /// <summary>
        /// The name of the state.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value is set to <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            protected set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }

        /// <summary>
        /// The cities recorded as being within this state.
        /// </summary>
        public virtual ISet<City> Cities
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="State" />.
        /// </summary>
        /// <param name="id">
        /// The postal abbreviation for the state.
        /// </param>
        /// <param name="name">
        /// The name of the state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is <see langword="null" />.
        /// </exception>
        public State(string id, string name) : this()
        {
            Id = id;
            Name = name;
            Cities = new HashSet<City>();
        }

        /// <summary>
        /// Construct a new State.
        /// </summary>
        protected State()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}

