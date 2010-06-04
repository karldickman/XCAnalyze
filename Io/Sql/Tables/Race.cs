using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row of the races table in the database.
    /// </summary>
    public class Race : Model.Race
    {
        /// <summary>
        /// A registry of all instances (i.e. rows) by id number.
        /// </summary>
        protected internal static IDictionary<int, Model.Race> IdMap =
            new Dictionary<int, Model.Race>();
        
        /// <summary>
        /// Get all instances.
        /// </summary>
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
                if(Tables.Meet.Exists(MeetId.Value))
                {
                    return Tables.Meet.Get(MeetId.Value).Name;
                }
                return base.Meet;
            }
        }
        
        /// <summary>
        /// The row id of the meet.
        /// </summary>
        public int? MeetId { get; protected internal set; }
        
        protected internal Venue SqlVenue
        {
            get
            {
                if (VenueId == null)
                {
                    return null;
                }
                if (Tables.Venue.Exists (VenueId.Value))
                {
                    return Tables.Venue.Get (VenueId.Value);
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

        /// <summary>
        /// Create a new race.
        /// </summary>
        /// <param name="id">
        /// The id number.
        /// </param>
        /// <param name="meetId">
        /// A <see cref="System.Nullable<System.Int32>"/>.  The id number of the
        /// meet.
        /// </param>
        /// <param name="venueId">
        /// A <see cref="System.Nullable<System.Int32>"/>.  The id number of the
        /// venue where the race was held.
        /// </param>
        /// <param name="date">
        /// A <see cref="Model.Date"/>.  The date that this race was held.
        /// </param>
        /// <param name="gender">
        /// A <see cref="Model.Gender"/>.  Was it a men's race or women's race?
        /// </param>
        /// <param name="distance">
        /// The distance of the race.
        /// </param>
        public Race (int id, int? meetId, int? venueId, Model.Date date,
            Model.Gender gender, int distance)
            : base(date, gender, distance)
        {
            Id = id;
            MeetId = meetId;
            VenueId = venueId;
            IdMap[id] = this;
        }
        
        /// <summary>
        /// Check for an instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// True if one exists; false if not.
        /// </returns>
        public static bool Exists(int id)
        {
            return IdMap.ContainsKey(id);
        }
        
        /// <summary>
        /// Get the instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// The <see cref="Model.Race"/> with the given id number.
        /// </returns>
        public static Model.Race Get(int id)
        {
            return IdMap[id];
        }
    }
}
