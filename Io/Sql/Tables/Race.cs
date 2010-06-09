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
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        override public Model.Date Date { get; protected internal set; }
        
        /// <summary>
        /// The meet name of this race.
        /// </summary>
        public MeetName MeetName
        {
            get
            {
                if (MeetNameId == null) 
                {
                    return null;
                }
                if (MeetName.Exists (MeetNameId.Value)) 
                {
                    return MeetName.Get (MeetNameId.Value);
                }
                return null;
            }
        }
        
        /// <summary>
        /// The row id of the meet.
        /// </summary>
        public int? MeetNameId { get; protected internal set; }
        
        override public string Name
        {
            get
            {
                if(MeetName == null)
                {
                    return null;
                }
                return MeetName.Name;
            }
            
            protected internal set
            {
                MeetName.Name = value;
            }
        }
        
        override public Model.Venue Venue
        {
            get
            {
                if (VenueId == null)
                {
                    return null;
                }
                if (Tables.Venue.Exists (VenueId.Value))
                {
                    return (Venue)Tables.Venue.Get (VenueId.Value);
                }
                return null;
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
        public Race (int id, int? meetId, Model.Date date, int? venueId,
            Model.Gender gender, int distance)
            : base(gender, distance)
        {
            Id = id;
            MeetNameId = meetId;
            Date = date;
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
            if (race is Race)
            {
                return ((Race)race).Id;
            }
            foreach (KeyValuePair<int, Model.Race> entry in IdMap)
            {
                Race candidate = (Race)entry.Value;
                if (race.Name.Equals (candidate.Name)
                    && race.Date.Equals (candidate.Date)
                    && race.Gender == candidate.Gender
                    && race.Venue.Equals (candidate.Venue))
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
        protected internal IList<Model.Meet> Meets { get; set; }
        protected internal IList<Model.Venue> Venues { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Race.Clear ();
            Venues = new List<Model.Venue> ();
            Venues.Add (new Model.Venue ("Milo McIver State Park", "Estacada", "OR"));
            Venues.Add (new Model.Venue ("Lincoln Park", "Seattle", "WA"));
            Venues.Add (new Model.Venue ("Bush Pasture Park", "Salem", "OR"));
            Venues.Add (new Model.Venue ("Western Oregon University Campus", "Monmouth", "OR"));
            Meets = new List<Model.Meet> ();
            Meets.Add (new Model.Meet ("Lewis & Clark Invitational", new Model.Date (2009, 9, 5), Venues[0],
                    new Model.Race (Model.Gender.MALE, 8000), new Model.Race (Model.Gender.FEMALE, 6000)));
            Meets.Add (new Model.Meet ("Sundodger Invitational", new Model.Date (2009, 9, 15), Venues[1],
                    new Model.Race (Model.Gender.MALE, 8000), new Model.Race (Model.Gender.FEMALE, 6000)));
            Meets.Add (new Model.Meet ("Charles Bowles Invitational", new Model.Date (2009, 10, 1), Venues[2],
                    new Model.Race (Model.Gender.MALE, 8000), new Model.Race (Model.Gender.FEMALE, 5000)));
            Meets.Add (new Model.Meet ("Summer's End Invitational", new Model.Date (2008, 8, 31), Venues[3],
                    new Model.Race (Model.Gender.MALE, 7085), new Model.Race (Model.Gender.FEMALE, 4840)));
            IDictionary<string, MeetName> meetNames = new Dictionary<string, MeetName> ();
            for (int i = 0; i < Meets.Count; i++)
            {
                meetNames[Meets[i].Name] = new MeetName (i, Meets[i].Name);
            }
            IDictionary<string, Venue> venues = new Dictionary<string, Venue> ();
            for (int i = 0; i < Meets.Count; i++)
            {
                venues[Meets[i].Venue.Name] = new Venue (i, Meets[i].Venue.Name, Meets[i].City, Meets[i].State, null);
            }
            for (int i = 0; i < Meets.Count; i++)
            {
                Race race = new Race (2 * i, meetNames[Meets[i].Name].Id, Meets[i].Date, venues[Meets[i].Venue.Name].Id, Model.Gender.MALE, Meets[i].MensDistance);
                race = new Race (2 * i + 1, meetNames[Meets[i].Name].Id, Meets[i].Date, venues[Meets[i].Venue.Name].Id, Model.Gender.FEMALE, Meets[i].WomensDistance);
                Console.WriteLine ("\0" + race);
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
            for (int i = 0; i < Meets.Count; i++)
            {
                Assert.AreEqual (2 * i, Race.GetId (Meets[i].MensRace));
                Assert.AreEqual (2 * i + 1, Race.GetId (Meets[i].WomensRace));
            }
        }
    }
}
