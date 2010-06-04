using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{
    public class SqlData : Data
    {
        private IList<string> conferences;
        private IList<string> meets;
        private IList<string[]> venues;
        
        override public IList<string> Conferences
        {
            get
            {
                if(conferences == null)
                {
                    conferences = new List<string>(from conference in SqlConferences
                        select conference.Name);
                }
                return conferences;
            }
        }
        
        override public IList<string> Meets
        {
            get
            {
                if(meets == null)
                {
                    meets = new List<string>(from meet in SqlMeets
                        select meet.Name);
                }
                return meets;
            }
        }
        
        override public IList<string[]> Venues
        {
            get
            {
                if(venues == null)
                {
                    venues = new List<string[]>(from venue in SqlVenues
                        select new string[] {venue.Name, venue.City, venue.State});
                }
                return venues;
            }
        }
        
        public IList<SqlConference> SqlConferences { get; protected internal set; }
        public IList<SqlMeet> SqlMeets { get; protected internal set; }
        public IList<SqlVenue> SqlVenues { get; protected internal set; }

        protected internal SqlData(IList<Affiliation> affiliations,
            IList<SqlConference> conferences, IList<string> conferenceNames,
            IList<SqlMeet> meets, IList<string> meetNames,
            IList<Performance> performances, IList<Race> races,
            IList<Runner> runners, IList<School> schools,
            IList<SqlVenue> venues, IList<string[]> venueNames)
            : base(affiliations, conferenceNames, meetNames, performances,
                races, runners, schools, venueNames)
        {
            SqlConferences = conferences;
            SqlMeets = meets;
            SqlVenues = venues;
        }

        public static SqlData NewInstance (IList<Affiliation> affiliations,
            IList<SqlConference> conferences, IList<SqlMeet> meets,
            IList<Performance> performances, IList<Race> races,
            IList<Runner> runners, IList<School> schools,
            IList<SqlVenue> venues)
        {
            IList<string> conferenceNames = new List<string> ();
            IList<string> meetNames = new List<string> ();
            IList<string> venueNames = new List<string> ();
            IList<string[]> venueInfo = new List<string[]> ();
            foreach (SqlConference conference in conferences)
            {
                if (!conferenceNames.Contains (conference.Name))
                {
                    conferenceNames.Add (conference.Name);
                }
            }
            foreach (SqlMeet meet in meets)
            {
                if (!meetNames.Contains (meet.Name))
                {
                    meetNames.Add (meet.Name);
                }
            }
            foreach (SqlVenue venue in venues)
            {
                if (!venueNames.Contains (venue.Name))
                {
                    venueNames.Add (venue.Name);
                    venueInfo.Add (new string[] { venue.Name, venue.City, venue.State });
                }
            }
            return new SqlData (affiliations, conferences, conferenceNames,
                meets, meetNames, performances, races, runners, schools, venues,
                venueInfo);
        }
    }

    public class SqlAffiliation : Affiliation
    {
        protected internal static IDictionary<int, Affiliation> IdMap = new Dictionary<int, Affiliation>();
        
        public static IList<Affiliation> List
        {
            get { return new List<Affiliation> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id of the affiliation.
        /// </summary>
        public int Id { get; protected internal set; }
        
        override public Runner Runner
        {
            get
            {
                if(SqlRunner.Exists(RunnerId))
                {
                    return SqlRunner.Get(RunnerId);
                }
                return base.Runner;
            }
            
            protected internal set
            {
                if(value is SqlRunner)
                {
                    RunnerId = ((SqlRunner)value).Id;
                }
                else
                {
                    base.Runner = value;
                }
            }
        }
        
        /// <summary>
        /// The row id of the runner.
        /// </summary>
        public int RunnerId { get; protected internal set; }
        
        override public School School
        {
            get { return SqlSchool.Get(SchoolId); }
        }
        
        /// <summary>
        /// The row id of the school.
        /// </summary>
        public int SchoolId { get; protected internal set; }

        public SqlAffiliation (int id, int runnerId, int schoolId, int year)
            : base(year)
        {
            Id = id;
            RunnerId = runnerId;
            SchoolId = schoolId;
        }
    }

    public class SqlConference : IComparable<SqlConference>
    {
        protected internal static IDictionary<int, SqlConference> IdMap = new Dictionary<int, SqlConference>();
        
        public static IList<SqlConference> List
        {
            get { return new List<SqlConference> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the conference.
        /// </summary>
        public string Name { get; protected internal set; }
        
        /// <summary>
        /// The standard abbreviation of the conference.
        /// </summary>
        public string Abbreviation { get; protected internal set; }

        public SqlConference (int id, string name, string abbreviation)
        {
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
            IdMap[id] = this;
        }
        
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static SqlConference Get (int id)
        {
            return IdMap[id];
        }
        
        public static int? GetId (string conferenceName)
        {
            if (conferenceName == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, SqlConference> entry in IdMap)
            {
                if (conferenceName.Equals (entry.Value.Name))
                {
                    return entry.Key;
                }
            }
            return null;
        }
        
        public static int? GetId (SqlConference conference)
        {
            if (conference == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, SqlConference> entry in IdMap)
            {
                if (conference.Equals (entry.Value)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public int CompareTo (SqlConference other)
        {
            return Name.CompareTo (other.Name);
        }
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            else if(other is SqlConference)
            {
                return 0 == CompareTo((SqlConference)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return Name.GetHashCode();
        }  
        
        override public string ToString ()
        {
            return Name;
        } 
    }
    
    [TestFixture]
    public class TestSqlConference
    {
        protected internal string NwcName { get; set; }
        protected internal string SciacName { get; set; }
        protected internal string ScacName { get; set; }
        protected internal SqlConference Nwc { get; set; }
        protected internal SqlConference Sciac { get; set; }
        protected internal SqlConference Scac { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            SqlConference.Clear();
            NwcName = "Northwest Conference";
            Nwc = new SqlConference (1, NwcName, "NWC");
            SciacName = "Southern California Intercollegiate Athletic Conference";
            Sciac = new SqlConference (2, SciacName, "SCIAC");
            ScacName = "Southern Collegiate Athletic Conference";
            Scac = new SqlConference (3, ScacName, "SCAC");
        }
        
        [TearDown]
        public void TearDown ()
        {
            SqlConference.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            Assert.IsNull (SqlConference.GetId ((string)null));
            Assert.IsNull(SqlConference.GetId((SqlConference)null));
            Assert.IsNull(SqlConference.GetId("xkcd"));
            Assert.AreEqual(1, SqlConference.GetId(Nwc));
            Assert.AreEqual(2, SqlConference.GetId(Sciac));
            Assert.AreEqual(3, SqlConference.GetId(Scac));
            Assert.AreEqual(1, SqlConference.GetId(NwcName));
            Assert.AreEqual(2, SqlConference.GetId(SciacName));
            Assert.AreEqual(3, SqlConference.GetId(ScacName));
        }
    }

    public class SqlMeet
    {
        protected internal static IDictionary<int, SqlMeet> IdMap = new Dictionary<int, SqlMeet>();
        
        public static IList<SqlMeet> List
        {
            get { return new List<SqlMeet> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the meet.  Example: Lewis & Clark Invitational.
        /// </summary>
        public string Name { get; protected internal set; }

        public SqlMeet (int id, string name)
        {
            Id = id;
            Name = name;
            IdMap[id] = this;
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static SqlMeet Get (int id)
        {
            return IdMap[id];
        }
    }
    
    public class SqlPerformance : Performance
    {
        protected internal static IDictionary<int, Performance> IdMap = new Dictionary<int, Performance>();
        
        public static IList<Performance> List
        {
            get { return new List<Performance>(IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        override public Runner Runner
        {
            get
            {
                if (SqlRunner.Exists (RunnerId))
                {
                    return SqlRunner.Get (RunnerId);
                }
                return base.Runner;
            }
            
            protected internal set
            {
                if(value is SqlRunner)
                {
                    RunnerId = ((SqlRunner)value).Id;
                }
                else
                {
                    base.Runner = value;
                }
            }
        }
        
        /// <summary>
        /// The row id of the runner.
        /// </summary>
        public int RunnerId { get; protected internal set; }
        
        override public Race Race
        {
            get
            {
                if(SqlRace.Exists(RaceId))
                {
                    return SqlRace.Get(RaceId);
                }
                return base.Race;
            }
            
            protected internal set
            {
                if(value is SqlRace)
                {
                    RaceId = ((SqlRace)value).Id;
                }
                else
                {
                    base.Race = value;
                }
            }
        }
        
        /// <summary>
        /// The row id of the race.
        /// </summary>
        public int RaceId { get; protected internal set; }
        
        public SqlPerformance (int id, int runnerId, int raceId, Time time)
            : base(time)
        {
            Id = id;
            RunnerId = runnerId;
            RaceId = raceId;
        }
    }

    public class SqlRace : Race
    {
        protected internal static IDictionary<int, Race> IdMap = new Dictionary<int, Race>();
        
        public static IList<Race> List
        {
            get { return new List<Race> (IdMap.Values); }
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

        public SqlRace (int id, int? meetId, int? venueId, Date date,
            Gender gender, int distance)
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
        
        public static Race Get(int id)
        {
            return IdMap[id];
        }
    }

    public class SqlRunner : Runner
    {
        protected internal static IDictionary<int, Runner> IdMap = new Dictionary<int, Runner>();
        
        public static IList<Runner> List
        {
            get { return new List<Runner> (IdMap.Values); }
        }
        
        public int Id { get; protected internal set; }
        public string[] Nicknames { get; protected internal set; }
        
        public SqlRunner (int id, string surname, string givenName,
            string[] nicknames, Gender gender, int? year)
            : base(surname, givenName, gender, year)
        {
            Id = id;
            Nicknames = nicknames;
            IdMap[id] = this;
        }
        
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static Runner Get(int id)
        {
            return IdMap[id];
        }
    }

    public class SqlSchool : School
    {
        protected internal static IDictionary<int, School> IdMap = new Dictionary<int, School>();
        
        public static IList<School> List
        {
            get { return new List<School> (IdMap.Values); }
        }
        
        override public string Conference
        {
            get
            {
                if (ConferenceId == null)
                {
                    return null;
                }
                if (SqlConference.Exists (ConferenceId.Value))
                {
                    return SqlConference.Get (ConferenceId.Value).Name;
                }
                return null;
            }
        }
        
        public int? ConferenceId { get; protected internal set; }
        public int Id { get; protected internal set; }
        public string[] Nicknames { get; protected internal set; }
        
        public SqlSchool (int id, string name, string[] nicknames,
            string type, bool nameFirst, int? conferenceId)
            : base(name, type, nameFirst)
        {
            Id = id;
            Nicknames = nicknames;
            ConferenceId = conferenceId;
            IdMap[id] = this;
        }
        
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static School Get(int id)
        {
            return IdMap[id];
        }
    }

    public class SqlVenue
    {
        protected internal static IDictionary<int, SqlVenue> IdMap = new Dictionary<int, SqlVenue>();
        
        public static IList<SqlVenue> List
        {
            get { return new List<SqlVenue> (IdMap.Values); }
        }
            
        public int Id { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string City { get; protected internal set; }
        public string State { get; protected internal set; }
        public int? Elevation { get; protected internal set; }

        public SqlVenue (int id, string name, string city, string state,
            int? elevation)
        {
            Id = id;
            Name = name;
            City = city;
            State = state;
            Elevation = elevation;
            IdMap[id] = this;
        }       
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static SqlVenue Get (int id)
        {
            return IdMap[id];
        }
    }
}
