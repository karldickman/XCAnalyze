using System;
using System.Collections.Generic;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{
    public class SqlData : Data
    {
        new public IList<Conference> Conferences { get; protected internal set; }
        new public IList<SqlMeet> Meets { get; protected internal set; }
        new public IList<SqlVenue> Venues { get; protected internal set; }

        protected internal SqlData(IList<Affiliation> affiliations, IList<Conference> conferences, IList<string> conferenceNames, IList<SqlMeet> meets, IList<string> meetNames, IList<Performance> performances, IList<Race> races, IList<Runner> runners, IList<School> schools, IList<SqlVenue> venues, IList<string> venueNames) : base(affiliations, conferenceNames, meetNames, performances, races, runners, schools, venueNames)
        {
            Conferences = conferences;
            Meets = meets;
            Venues = venues;
        }

        public static SqlData NewInstance(IList<Affiliation> affiliations, IList<Conference> conferences, IList<SqlMeet> meets, IList<Performance> performances, IList<Race> races, IList<Runner> runners, IList<School> schools, IList<SqlVenue> venues)
        {
            IList<string> conferenceNames = new List<string>();
            IList<string> meetNames = new List<string>();
            IList<string> venueNames = new List<string>();
            foreach(SqlConference conference in conferences)
            {
                if(!conferenceNames.Contains(conference.Name))
                {
                    conferenceNames.Add(conference.Name);
                }
            }
            foreach(SqlMeet meet in meets)
            {
                if(!meetNames.Contains(meet.Name))
                {
                    meetNames.Add(meet.Name);
                }
            }
            foreach(SqlVenue venue in venues)
            {
                if(!venueNames.Contains(venue.Name))
                {
                    venueNames.Add(venue.Name);
                }
            }
            return new SqlData(affiliations, conferences, conferenceNames, meets, meetNames, performances, races, runners, schools, venues, venueNames);
        }
    }

    public class SqlAffiliation : Affiliation
    {
        public int Id { get; protected internal set; }

        public SqlAffiliation (int id, Runner runner, School school, int year)
            : base(runner, school, year)
        {
            Id = id;
        }
    }

    public class Conference : IComparable<Conference>
    {
        public string Name { get; protected internal set; }
        public string Abbreviation { get; protected internal set; }

        public Conference (string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public int CompareTo (Conference other)
        {
            return Name.CompareTo (other.Name);
        }
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            else if(other is Conference)
            {
                return 0 == CompareTo((Conference)other);
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
    
    public class SqlConference : Conference
    {
        public int Id { get; protected internal set; }
        
        public SqlConference (int id, string name, string abbreviation)
            : base(name, abbreviation)
        {
            Id = id;
        }
    }

    public class SqlMeet
    {
        public int Id { get; protected internal set; }
        public string Name { get; protected internal set; }

        public SqlMeet (int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    
    public class SqlPerformance : Performance
    {
        public int Id { get; protected internal set; }
        
        public SqlPerformance (int id, Runner runner, Race race, Time time) : base(runner, race, time)
        {
            Id = id;
        }
    }

    public class SqlRace : Race
    {
        public int Id { get; protected internal set; }
        new public SqlMeet Meet { get; protected internal set; }
        new public SqlVenue Venue { get; protected internal set; }

        protected internal SqlRace (int id, SqlMeet meet, string meetName, SqlVenue venue, string venueName, string city, string state, Date date, Gender gender, int distance) : base(meetName, date, gender, distance, venueName, city, state)
        {
            Id = id;
            Meet = meet;
            Venue = venue;
        }
        
        public static SqlRace NewInstance (int id, SqlMeet meet, SqlVenue venue, Date date, Gender gender, int distance)
        {
            string meetName, venueName, city, state;
            if (meet == null) 
            {
                meetName = null;
            }
            else
            {
                meetName = meet.Name;
            }
            if (venue == null) 
            {
                venueName = null;
                city = null;
                state = null;
            }
            else
            {
                venueName = venue.Name;
                city = venue.City;
                state = venue.State;
            }
            return new SqlRace ((int)id, meet, meetName, venue, venueName, city, state, date, gender, distance);
        }
    }

    public class SqlRunner : Runner
    {
        public int Id { get; protected internal set; }
        public string[] Nicknames { get; protected internal set; }
        
        public SqlRunner (int id, string surname, string givenName, string[] nicknames, Gender gender, int? year) : base(surname, givenName, gender, year)
        {
            Id = id;
            Nicknames = nicknames;
        }
    }

    public class SqlSchool : School
    {
        new public Conference Conference { get; protected internal set; }
        public int Id { get; protected internal set; }
        public string[] Nicknames { get; protected internal set; }
        
        protected SqlSchool (int id, string name, string[] nicknames, string type, bool nameFirst, Conference conference, string conferenceName) : base(name, type, nameFirst, conferenceName)
        {
            Id = id;
            Nicknames = nicknames;
            Conference = conference;
        }
        
        public static SqlSchool NewInstance (int id, string name, string[] nicknames, string type, bool nameFirst, Conference conference)
        {
            string conferenceName;
            if (conference == null)
            {
                conferenceName = null;
            }
            else
            {
                conferenceName = conference.Name;
            }
            return new SqlSchool((int)id, name, nicknames, type, nameFirst, conference, conferenceName);
        }
    }

    public class SqlVenue
    {
        public int Id { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string City { get; protected internal set; }
        public string State { get; protected internal set; }
        public int? Elevation { get; protected internal set; }

        public SqlVenue (int id, string name, string city, string state, int? elevation)
        {
            Id = id;
            Name = name;
            City = city;
            State = state;
            Elevation = elevation;
        }       
    }
}