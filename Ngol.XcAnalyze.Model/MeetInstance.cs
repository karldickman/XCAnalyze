using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ngol.Hytek.Interfaces;
using SharpArch.Domain.DomainModel;
using System.Reflection;

namespace Ngol.XcAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class MeetInstance : BaseObject, IMeet
    {
        #region Properties

        #region Physical implementation

        private Meet _meet;
        private Venue _venue;

        #endregion

        /// <summary>
        /// The date on which this meet was held.
        /// </summary>
        [DomainSignature]
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// The host of this meet instance.
        /// </summary>
        public virtual Team Host { get; set; }

        /// <summary>
        /// The meet of which this is an instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        [DomainSignature]
        public virtual Meet Meet
        {
            get { return _meet; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(value != Meet)
                {
                    _meet = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="Meet.Name" /> of this <see cref="MeetInstance" />.
        /// </summary>
        public virtual string Name
        {
            get { return Meet.Name; }
        }

        /// <summary>
        /// The <see cref="Race" />s that were run as part of this <see cref="MeetInstance" />.
        /// </summary>
        public virtual ISet<Race> Races { get; protected set; }

        /// <summary>
        /// The venue whereat the instance of this meet was held.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if an attempt is made to set the property to <see langword="null" />.
        /// </exception>
        public virtual Venue Venue
        {
            get { return _venue; }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                if(value != Venue)
                {
                    _venue = value;
                }
            }
        }

        string IMeet.Venue
        {
            get { return Venue.Name; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new meet instance.
        /// </summary>
        /// <param name="meet">
        /// The meet of which this is an instance.
        /// </param>
        /// <param name="date">
        /// The date on which the meet was held.
        /// </param>
        /// <param name="venue">
        /// The <see cref="Venue" /> whereat the meet was held.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="meet"/> or <paramref name="venue"/>
        /// is <see langword="null" />.
        /// </exception>
        public MeetInstance(Meet meet, DateTime date, Venue venue) : this()
        {
            Meet = meet;
            Date = date;
            Venue = venue;
            Races = new HashSet<Race>();
        }

        /// <summary>
        /// Construct a new <see cref="MeetInstance" />
        /// </summary>
        /// <remarks>
        /// Required for NHibernate.
        /// </remarks>
        protected MeetInstance()
        {
        }

        #endregion

        #region Inherited methods

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ({1:yyyy-MM-dd})", Meet.Name, Date);
        }

        #endregion

        #region BaseObject implementation

        /// <inheritdoc />
        protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
        {
            Type type = GetTypeUnproxied();
            yield return type.GetProperty("Meet");
            yield return type.GetProperty("Date");
        }

        #endregion
    }
}
