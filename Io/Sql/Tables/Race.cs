using NUnit.Framework;
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
        
        override public string Name
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
                return base.Name;
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
        /// Clear the registry of instances.
        /// </summary>
        public static void Clear ()
        {
            IdMap.Clear ();
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
        public static Model.Race Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Search for the id number of the instance matching a particular race.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Model.Race"/> to search for.
        /// </param>
        /// <returns>
        /// The id number of the race.  If none was found, return null.
        /// </returns>
        public static int? GetId (Model.Race race)
        {
            Race candidate;
            if (race is Race)
            {
                return ((Race)race).Id;
            }
            foreach (KeyValuePair<int, Model.Race> entry in IdMap)
            {
                candidate = (Race)entry.Value;
                if (candidate.Name.Equals (race.Name)
                    && candidate.Date.Equals (race.Date)
                    && candidate.Gender == race.Gender
                    && candidate.Venue.Equals (race.Venue)
                    && candidate.City.Equals (race.City)
                    && candidate.State.Equals(race.State)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }
    }
    
    [TestFixture]
    public class TestRace
    {
        protected internal IList<Model.Race> Races { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Race.Clear ();
            Races = new List<Model.Race> ();
            Races.Add (new Model.Race ("Lewis & Clark Invitational", new Model.Date (2009, 9, 5), Model.Gender.FEMALE, 6000, "Milo McIver State Park", "Estacada", "OR"));
            Races.Add (new Model.Race ("Sundodger Invitational", new Model.Date (2009, 9, 15), Model.Gender.MALE, 8000, "Lincoln Park", "Seattle", "WA"));
            Races.Add (new Model.Race ("Charles Bowles Invitational", new Model.Date (2009, 10, 1), Model.Gender.FEMALE, 5000, "Bush Pasture Park", "Salem", "OR"));
            Races.Add (new Model.Race ("Summer's End Invitational", new Model.Date (2008, 8, 31), Model.Gender.MALE, 8000, "Western Oregon University Campus", "Monmouth", "OR"));
            IDictionary<string, Meet> meets = new Dictionary<string, Meet> ();
            for (int i = 0; i < Races.Count; i++)
            {
                meets[Races[i].Name] = new Meet (i, Races[i].Name);
            }
            IDictionary<string, Venue> venues = new Dictionary<string, Venue> ();
            for (int i = 0; i < Races.Count; i++)
            {
                venues[Races[i].Venue] = new Venue (i, Races[i].Venue, Races[i].City, Races[i].State, null);
            }
            for (int i = 0; i < Races.Count; i++)
            {
                Race race = new Race (i, meets[Races[i].Name].Id, venues[Races[i].Venue].Id, Races[i].Date, Races[i].Gender, Races[i].Distance);
                if (i == 3)
                {
                    race.Distance = 7085;
                }
            }
        }
        
        [TearDown]
        public void TearDown ()
        {
            Race.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            for (int i = 0; i < Races.Count; i++)
            {
                Assert.AreEqual (i, Race.GetId (Races[i]));
            }
        }
    }
}
