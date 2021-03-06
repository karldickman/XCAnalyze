using System;
using System.Collections.Generic;
using Ngol.Hytek.Interfaces;
using SharpArch.Domain.DomainModel;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A college or university that fields a Cross-Country team.
    /// </summary>
    public class Team : Entity, ITeam
    {
        #region Properties

        #region Physical implementation

        private string _name;

        #endregion

        /// <summary>
        /// The athletic <see cref="Conference" /> with which the school is affiliated.
        /// </summary>
        public virtual Conference Conference { get; set; }

        /// <summary>
        /// The <see cref="MeetInstance"/>s hosted by this <see cref="Team" />.
        /// </summary>
        public virtual ISet<MeetInstance> HostedMeetInstances { get; protected set; }

        /// <summary>
        /// The <see cref="Meet"/>s hosted by this <see cref="Team" />.
        /// </summary>
        public virtual ISet<Meet> HostedMeets { get; protected set; }

        /// <summary>
        /// The number that identifies this team.
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// The name of the school (Linfield, Willamette, etc.)
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual string Name
        {
            get { return _name; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                _name = value;
            }
        }

        /// <summary>
        /// The nicknames of the school (UPS for University of Puget Sound, z.B.).
        /// </summary>
        public virtual ISet<string> Nicknames { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="name">
        /// The name of the team (Linfield, Willamette, etc.).  This value
        /// cannot be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="name"/> is <see langword="null" />.
        /// </exception>
        public Team(string name) : this()
        {
            Name = name;
            Nicknames = new HashSet<string>();
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
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
