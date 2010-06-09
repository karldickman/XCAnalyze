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
        
        override public IList<string> MeetNames
        {
            get
            {
                return new List<string>(from meetName in SqlMeetNames
                    select meetName.Name);
            }
        }
        
        /// <summary>
        /// The conferences stored in the database.
        /// </summary>
        public IList<Conference> SqlConferences { get; protected internal set; }
        
        /// <summary>
        /// The meets stored in the database.
        /// </summary>
        public IList<MeetName> SqlMeetNames { get; protected internal set; }

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
            IList<Conference> conferences, IList<MeetName> meetNames, 
            IList<Model.Performance> performances, IList<Model.Race> races,
            IList<Model.Runner> runners, IList<Model.School> schools,
            IList<Model.Venue> venues)
            : base(affiliations, Meet.MeetsList(races), performances, runners,
                schools, venues)
        {
            SqlConferences = conferences;
            SqlMeetNames = meetNames;
        }
    }
}
