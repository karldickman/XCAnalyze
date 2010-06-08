using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// All the information in the database.
    /// </summary>
    public class XcData : Model.XcData
    {
        override public IList<string> Conferences
        {
            get
            {
                return new List<string>(from conference in SqlConferences
                    select conference.Name);
            }
        }
        
        override public IList<string> Meets
        {
            get
            {
                return new List<string>(from meet in SqlMeets
                    select meet.Name);
            }
        }
        
        override public IList<string[]> Venues
        {
            get
            {
                return new List<string[]>(from venue in SqlVenues
                    select new string[] {venue.Name, venue.City, venue.State});
            }
        }
        
        /// <summary>
        /// The conferences stored in the database.
        /// </summary>
        public IList<Conference> SqlConferences { get; protected internal set; }
        
        /// <summary>
        /// The meets stored in the database.
        /// </summary>
        public IList<Meet> SqlMeets { get; protected internal set; }
        
        /// <summary>
        /// The venues stored in the database.
        /// </summary>
        public IList<Venue> SqlVenues { get; protected internal set; }

        /// <summary>
        /// Create a new description of the database.
        /// </summary>
        /// <param name="affiliations">
        /// The contents of the affiliations table.
        /// </param>
        /// <param name="conferences">
        /// The contents of the conferences table.
        /// </param>
        /// <param name="meets">
        /// The contents of the meets table.
        /// </param>
        /// <param name="performances">
        /// The contents of the performances table.
        /// </param>
        /// <param name="races">
        /// The contents of the races table.
        /// </param>
        /// <param name="runners">
        /// The contents of the runenrs table.
        /// </param>
        /// <param name="schools">
        /// The contents of the schools table.
        /// </param>
        /// <param name="venues">
        /// The contents of the venues table.
        /// </param>
        public XcData(IList<Model.Affiliation> affiliations,
            IList<Conference> conferences, IList<Meet> meets, 
            IList<Model.Performance> performances, IList<Model.Race> races,
            IList<Model.Runner> runners, IList<Model.School> schools,
            IList<Venue> venues)
            : base(affiliations, performances, races, runners, schools)
        {
            SqlConferences = conferences;
            SqlMeets = meets;
            SqlVenues = venues;
        }
    }
}
