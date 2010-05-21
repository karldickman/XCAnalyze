using System;
using System.Collections.Generic;
using xcanalyze.model;

namespace xcanalyze.io.sql
{

	public class SqlAffiliation : Affiliation
	{
		private uint id;
		
		public uint Id {
			get { return id; }
		}

		public SqlAffiliation (uint id, Runner runner, School school, int year) : base(runner, school, year)
		{
			this.id = id;
		}
	}

	public class SqlConference
	{
		private uint id;
		private string name;
		private string abbreviation;

		public string Abbreviation {
			get { return abbreviation; }
		}

		public uint Id {
			get { return id; }
		}

		public string Name {
			get { return name; }
		}

		public SqlConference (uint id, string name, string abbreviation)
		{
			this.id = id;
			this.name = name;
			this.abbreviation = abbreviation;
		}
		
		public override string ToString ()
		{
			return name;
		}

	}

	public class SqlMeet
	{
		private uint id;
		private string name;

		public uint Id {
			get { return id; }
		}

		public string Name {
			get { return name; }
		}

		public SqlMeet (uint id, string name)
		{
			this.id = id;
			this.name = name;
		}
	}
	
	public class SqlPerformance : Performance {
		private uint id;
		
		public uint Id {
			get { return id; }
		}

		public SqlPerformance (uint id, Runner runner, Race race, double time) : base(runner, race, time)
		{
			this.id = id;
		}
	}

	public class SqlRace : Race
	{
		private uint id;
		private SqlMeet meet;
		private SqlVenue venue;

		public uint Id {
			get { return id; }
		}
		
		public new SqlMeet Meet {
			get { return meet; }
		}
		
		public new SqlVenue Venue {
			get { return venue; }
		}

		protected SqlRace (uint id, SqlMeet meet, string meetName, SqlVenue venue, string venueName, string city, string state, DateTime date, Gender gender, int distance) : base(meetName, date, gender, distance, venueName, city, state)
		{
			this.id = id;
			this.meet = meet;
			this.venue = venue;
		}
		
		public static SqlRace NewInstance (uint id, SqlMeet meet, SqlVenue venue, DateTime date, Gender gender, int distance)
		{
			string meetName, venueName, city, state;
			if (meet == null) 
			{
				meetName = null;
			} else {
				meetName = meet.Name;
			}
			if (venue == null) 
			{
				venueName = null;
				city = null;
				state = null;
			} else {
				venueName = venue.Name;
				city = venue.City;
				state = venue.State;
			}
			return new SqlRace (id, meet, meetName, venue, venueName, city, state, date, gender, distance);
		}
		
	}

	public class SqlRunner : Runner
	{
		private uint id;
		private string[] nicknames;
		
		public uint Id {
			get { return id; }
		}
		
		public string[] Nicknames {
			get { return nicknames; }
		}
		
		public SqlRunner (uint id, string surname, string givenName, string[] nicknames, Gender gender, int? year) : base(surname, givenName, gender, year)
		{
			this.id = id;
			this.nicknames = nicknames;
		}
	}

	public class SqlSchool : School
	{
		private uint id;
		private SqlConference conference;
		private string[] nicknames;
		
		public new SqlConference Conference {
			get { return conference; }
		}
		
		public uint Id {
			get { return id; }
		}
		
		public string[] Nicknames {
			get {
				return nicknames;
			}
		}
		
		protected SqlSchool (uint id, string name, string[] nicknames, string type, bool nameFirst, SqlConference conference, string conferenceName) : base(name, type, nameFirst, conferenceName)
		{
			this.id = id;
			this.nicknames = nicknames;
			this.conference = conference;
		}
		
		public static SqlSchool NewInstance(uint id, string name, string[] nicknames, string type, bool nameFirst, SqlConference conference) {
			string conferenceName;
			if(conference == null) {
				conferenceName = null;
			} else {
				conferenceName = conference.Name;
			}
			return new SqlSchool(id, name, nicknames, type, nameFirst, conference, conferenceName);
		}
	}

	public class SqlVenue
	{
		private uint id;
		private string name;
		private string city;
		private string state;
		private int? elevation;
		
		public string State {
			get { return state; }
		}

		public string Name {
			get { return name; }
		}

		public uint Id {
			get { return id; }
		}

		public int? Elevation {
			get { return elevation; }
		}

		public string City {
			get { return city; }
		}

		public SqlVenue (uint id, string name, string city, string state, int? elevation)
		{
			this.id = id;
			this.name = name;
			this.city = city;
			this.state = state;
			this.elevation = elevation;
		}
		
	}
}
