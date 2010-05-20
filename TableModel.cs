using System;
using System.Collections.Generic;
using xcanalyze.model;

namespace xcanalyze.io.sql
{

	public class SqlAffiliation : Affiliation
	{
		private uint id;
		private uint runnerId;
		private uint schoolId;

		public uint Id {
			get { return id; }
		}

		public new Runner Runner {
			get { return SqlRunner.Get (runnerId); }
		}

		public uint RunnerId {
			get { return runnerId; }
		}

		public new model.School School {
			get { return SqlSchool.Get (schoolId); }
		}

		public uint SchoolId {
			get { return schoolId; }
		}

		public SqlAffiliation (uint id, uint runnerId, uint schoolId, int year) : base(year)
		{
			this.id = id;
			this.runnerId = runnerId;
			this.schoolId = schoolId;
			this.Year = year;
		}
	}

	public class SqlConference
	{
		private static Dictionary<uint?, SqlConference> idMap = new Dictionary<uint?, SqlConference> ();

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
			idMap[id] = this;
		}

		public static SqlConference Get (uint? id)
		{
			return idMap[id];
		}
	}

	public class SqlMeet
	{
		private static Dictionary<uint?, SqlMeet> idMap = new Dictionary<uint?, SqlMeet> ();
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
			idMap[id] = this;
		}

		public static SqlMeet Get (uint? id)
		{
			return idMap[id];
		}
	}
	
	public class SqlPerformance : Performance {
		private static Dictionary<uint, SqlPerformance> idMap = new Dictionary<uint, SqlPerformance>();
		private uint id;
		private uint runnerId;
		private uint raceId;
		
		public uint Id {
			get {
				return id;
			}
		}
		
		public new model.Race Race {
			get { return SqlRace.Get (raceId); }
		}
		
		public uint RaceId {
			get {
				return raceId;
			}
		}
		
		public new model.Runner Runner {
			get { return SqlRunner.Get (runnerId); }
		}
		
		public uint RunnerId {
			get {
				return runnerId;
			}
		}
		
		public SqlPerformance (uint id, uint runnerId, uint raceId, double time) : base(time)
		{
			this.id = id;
			this.runnerId = runnerId;
			this.raceId = raceId;
			this.Time = time;
			idMap[id] = this;
		}
		
		public SqlPerformance Get (uint id)
		{
			return idMap[id];
		}
	}

	public class SqlRace : Race
	{
		private static Dictionary<uint, SqlRace> idMap = new Dictionary<uint, SqlRace>();
		private uint id;
		private uint? meetId;
		private uint? venueId;

		public new string City {
			get {
				if (venueId == null) 
				{
					return null;
				}
				return SqlVenue.Get (venueId).City;
			}
		}

		public uint Id {
			get { return id; }
		}

		public new string Meet {
			get {
				if (meetId == null) 
				{
					return null;
				}
				return SqlMeet.Get (meetId).Name;
			}
		}

		public uint? MeetId {
			get { return meetId; }
		}

		public new string State {
			get {
				if (venueId == null) 
				{
					return null;
				}
				return SqlVenue.Get (venueId).State;
			}
		}

		public new string Venue {
			get {
				if (venueId == null) 
				{
					return null;
				}
				return SqlVenue.Get (venueId).Name;
			}
		}

		public uint? VenueId {
			get { return venueId; }
		}

		public SqlRace (uint id, uint? meetId, uint? venueId, DateTime date, Gender gender, int distance) : base(date, gender, distance)
		{
			this.id = id;
			this.meetId = meetId;
			this.venueId = venueId;
			idMap[id] = this;
		}

		public static SqlRace Get (uint id)
		{
			return idMap[id];
		}
		
	}

	public class SqlRunner : Runner
	{
		private static Dictionary<uint, SqlRunner> idMap = new Dictionary<uint, SqlRunner>();
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

		public static SqlRunner Get (uint id)
		{
			return idMap[id];
		}
	}

	public class SqlSchool : School
	{
		private static Dictionary<uint, School> idMap = new Dictionary<uint, School>();
		private uint id;
		private uint? conferenceId;
		private string[] nicknames;
		
		public new string Conference {
			get {
				if (ConferenceId == null) {
					return null;
				}
				return SqlConference.Get (ConferenceId).Name;
			}
		}
		
		public uint? ConferenceId {
			get { return conferenceId; }
		}
		
		public uint Id {
			get { return id; }
		}
		
		public string[] Nicknames {
			get {
				return nicknames;
			}
		}
		
		public SqlSchool (uint id, string name, string[] nicknames, string type, bool nameFirst, uint? conferenceId) : base(name, type, nameFirst)
		{
			this.id = id;
			this.nicknames = nicknames;
			this.conferenceId = conferenceId;
			idMap[id] = this;
		}

		public static School Get (uint id)
		{
			return idMap[id];
		}
	}

	public class SqlVenue
	{
		private static Dictionary<uint?, SqlVenue> idMap = new Dictionary<uint?, SqlVenue>();
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
			idMap[id] = this;
		}
		
		public static SqlVenue Get (uint? id)
		{
			return idMap[id];
		}
		
	}
}
