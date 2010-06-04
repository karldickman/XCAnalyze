using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class SqlRace : Model.Race
    {
        protected internal static IDictionary<int, Model.Race> IdMap = new Dictionary<int, Model.Race>();
        
        public static IList<Model.Race> List
        {
            get { return new List<Model.Race> (IdMap.Values); }
        }
        
        override public string City
        {
            get
            {
                if(SqlVenue == null)
                {
                    return null;
                }
                return SqlVenue.City;
            }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        override public string Meet
        {
            get
            {
                if(MeetId == null)
                {
                    return null;
                }
                if(SqlMeet.Exists(MeetId.Value))
                {
                    return SqlMeet.Get(MeetId.Value).Name;
                }
                return base.Meet;
            }
        }
        
        /// <summary>
        /// The row id of the meet.
        /// </summary>
        public int? MeetId { get; protected internal set; }
        
        protected internal SqlVenue SqlVenue
        {
            get
            {
                if (VenueId == null)
                {
                    return null;
                }
                if (SqlVenue.Exists (VenueId.Value))
                {
                    return SqlVenue.Get (VenueId.Value);
                }
                return null;
            }
        }
        
        override public string State
        {
            get
            {
                if(SqlVenue == null)
                {
                    return null;
                }
                return SqlVenue.State;
            }
        }
        
        override public string Venue
        {
            get
            {
                if(SqlVenue == null)
                {
                    return null;
                }
                return SqlVenue.Name;
            }
        }
        
        /// <summary>
        /// The row id of the venue.
        /// </summary>
        public int? VenueId { get; protected internal set; }

        public SqlRace (int id, int? meetId, int? venueId, Model.Date date,
            Model.Gender gender, int distance)
            : base(date, gender, distance)
        {
            Id = id;
            MeetId = meetId;
            VenueId = venueId;
            IdMap[id] = this;
        }
        
        public static bool Exists(int id)
        {
            return IdMap.ContainsKey(id);
        }
        
        public static Model.Race Get(int id)
        {
            return IdMap[id];
        }
    }
}
