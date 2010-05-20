using System;
using System.Collections.Generic;
using xcanalyze.model;

namespace xcanalyze.io.sql
{

	public class SqlAffiliation : model.Affiliation
	{
		private int id;
		private int runnerId;
		private int schoolId;

		public int Id {
			get { return id; }
		}

		public new Runner Runner {
			get { return SqlRunner.Get (runnerId); }
		}

		public int RunnerId {
			get { return runnerId; }
		}

		public new model.School School {
			get { return SqlSchool.Get (schoolId); }
		}

		public int SchoolId {
			get { return schoolId; }
		}

		public SqlAffiliation (int id, int runnerId, int schoolId, int year)
		{
			this.id = id;
			this.runnerId = runnerId;
			this.schoolId = schoolId;
			this.Year = year;
		}
	}

	public class SqlConference
	{
		private static Dictionary<int, SqlConference> idMap = new Dictionary<int, SqlConference> ();

		private int id;
		private string name;
		private string abbreviation;

		public string Abbreviation {
			get { return abbreviation; }
		}

		public int Id {
			get { return id; }
		}

		public string Name {
			get { return name; }
		}

		public SqlConference (int id, string name, string abbreviation)
		{
			this.id = id;
			this.name = name;
			this.abbreviation = abbreviation;
			idMap[id] = this;
		}

		public static SqlConference Get (int id)
		{
			return idMap[id];
		}
	}

	public class SqlMeet
	{
		private static Dictionary<int, SqlMeet> idMap = new Dictionary<int, SqlMeet> ();
		private int id;
		private string name;

		public int Id {
			get { return id; }
		}

		public string Name {
			get { return name; }
		}

		public SqlMeet (int id, string name)
		{
			this.id = id;
			this.name = name;
			idMap[id] = this;
		}

		public static SqlMeet Get (int id)
		{
			return idMap[id];
		}
	}
	
	public class SqlPerformance : Performance {
		private static Dictionary<int, SqlPerformance> idMap = new Dictionary<int, SqlPerformance>();
		private int id;
		private int runnerId;
		private int raceId;
		
		public int Id {
			get {
				return id;
			}
		}
		
		public new model.Race Race {
			get { return SqlRace.Get (raceId); }
		}
		
		public int RaceId {
			get {
				return raceId;
			}
		}
		
		public new model.Runner Runner {
			get { return SqlRunner.Get (runnerId); }
		}
		
		public int RunnerId {
			get {
				return runnerId;
			}
		}
		
		public SqlPerformance (int id, int runnerId, int raceId, float time)
		{
			this.id = id;
			this.runnerId = runnerId;
			this.raceId = raceId;
			this.Time = time;
			idMap[id] = this;
		}
		
		public SqlPerformance Get (int id)
		{
			return idMap[id];
		}
	}

	public class SqlRace : Race
	{
		private static Dictionary<int, SqlRace> idMap = new Dictionary<int, SqlRace>();
		private int id;
		private int meetId;
		private int venueId;

		public new string City {
			get { return SqlVenue.Get (venueId).City; }
		}

		public int Id {
			get { return id; }
		}

		public new string Meet {
			get { return SqlMeet.Get (meetId).Name; }
		}

		public int MeetId {
			get { return meetId; }
		}

		public new string State {
			get { return SqlVenue.Get (venueId).State; }
		}

		public new string Venue {
			get { return SqlVenue.Get (venueId).Name; }
		}

		public int VenueId {
			get { return venueId; }
		}

		public SqlRace (int id, int meetId, int venueId, DateTime date, model.Gender gender, int distance)
		{
			this.id = id;
			this.meetId = meetId;
			this.venueId = venueId;
			this.Date = date;
			this.Gender = gender;
			this.Distance = distance;
			idMap[id] = this;
		}

		public static SqlRace Get (int id)
		{
			return idMap[id];
		}
		
	}

	public class SqlRunner : Runner
	{
		private static Dictionary<int, SqlRunner> idMap = new Dictionary<int, SqlRunner>();

		public static SqlRunner Get (int id)
		{
			return idMap[id];
		}
	}

	public class SqlSchool : School
	{
		private static Dictionary<int, School> idMap = new Dictionary<int, School>();
		private int id;
		private int conferenceId;
		private string[] nicknames;
		
		public new string Conference {
			get { return SqlConference.Get (ConferenceId).Name; }
		}
		
		public int ConferenceId {
			get {
				return conferenceId;
			}
		}
		
		public int Id {
			get { return id; }
		}
		
		public string[] Nicknames {
			get {
				return nicknames;
			}
		}
		
		public SqlSchool (int id, string name, string[] nicknames, string type, bool nameFirst, int conferenceId)
		{
			this.id = id;
			Name = name;
			this.nicknames = nicknames;
			Type = type;
			NameFirst = nameFirst;
			this.conferenceId = conferenceId;
			idMap[id] = this;
		}

		public static School Get (int id)
		{
			return idMap[id];
		}
	}

	public class SqlVenue
	{
		private static Dictionary<int, SqlVenue> idMap = new Dictionary<int, SqlVenue>();
		private int id;
		private string name;
		private string city;
		private string state;
		private int elevation;
		
		public string State {
			get { return state; }
		}

		public string Name {
			get { return name; }
		}

		public int Id {
			get { return id; }
		}

		public int Elevation {
			get { return elevation; }
		}

		public string City {
			get { return city; }
		}

		public SqlVenue (int id, string name, string city, string state, int elevation)
		{
			this.id = id;
			this.name = name;
			this.city = city;
			this.state = state;
			this.elevation = elevation;
			idMap[id] = this;
		}
		
		public static SqlVenue Get (int id)
		{
			return idMap[id];
		}
		
	}
}
