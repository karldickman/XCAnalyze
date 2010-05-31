using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using XCAnalyze.Model;

namespace XCAnalyze.Io.Sql
{

	public class DatabaseReader : IReader<Data>
	{
		protected internal IDataReader Reader { get; set; }
		protected internal IDbCommand Command { get; set; }
		protected internal IDbConnection Connection { get; set; }

		public DatabaseReader (IDbConnection connection)
		{
			Connection = connection;
			Command = Connection.CreateCommand ();
		}

		public void Close ()
		{
			Reader.Close ();
			Command.Dispose ();
            Connection.Close();
		}

		public Data Read ()
		{
			Dictionary<int, SqlConference> conferences = ReadConferences ();
			Dictionary<int, Runner> runners = ReadRunners ();
			Dictionary<int, School> schools = ReadSchools (conferences);
			Dictionary<int, Affiliation> affiliations = ReadAffiliations (runners, schools);
			Dictionary<int, SqlMeet> meets = ReadMeets ();
			Dictionary<int, SqlVenue> venues = ReadVenues ();
			Dictionary<int, Race> races = ReadRaces (meets, venues);
			Dictionary<int, Performance> performances = ReadPerformances (runners, races);
			return new Data (new List<Affiliation>(affiliations.Values), new List<Performance>(performances.Values), new List<Race>(races.Values), new List<Runner>(runners.Values), new List<School>(schools.Values));
		}
		
		Dictionary<int, Affiliation> ReadAffiliations (Dictionary<int, Runner> runners, Dictionary<int, School> schools)
		{
			Dictionary<int, Affiliation> affiliations = new Dictionary<int, Affiliation> ();
			int id, runnerId, schoolId;
			Command.CommandText = "SELECT * FROM affiliations";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				runnerId = (int)(uint)Reader["runner_id"];
				schoolId = (int)(uint)Reader["school_id"];
				affiliations.Add (id, new SqlAffiliation ((int)id, runners[runnerId], schools[schoolId], (int)Reader["year"]));
			}
			Reader.Close ();
			return affiliations;
		}
		
		Dictionary<int, SqlConference> ReadConferences ()
		{
			Dictionary<int, SqlConference> conferences = new Dictionary<int, SqlConference> ();
			int id;
			Command.CommandText = "SELECT * FROM conferences";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				conferences.Add (id, new SqlConference (id, (string)Reader["name"], (string)Reader["abbreviation"]));
			}
			Reader.Close ();
			return conferences;
		}
		
		Dictionary<int, SqlMeet> ReadMeets ()
		{
			Dictionary<int, SqlMeet> meets = new Dictionary<int, SqlMeet> ();
			int id;
			Command.CommandText = "SELECT * FROM meets";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				meets.Add (id, new SqlMeet (id, (string)Reader["name"]));
			}
			Reader.Close ();
			return meets;
		}
		
		Dictionary<int, Performance> ReadPerformances (Dictionary<int, Runner> runners, Dictionary<int, Race> races)
		{
			Dictionary<int, Performance> performances = new Dictionary<int, Performance> ();
			int id, runnerId, raceId;
			Command.CommandText = "SELECT * FROM results";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				runnerId = (int)(uint)Reader["runner_id"];
				raceId = (int)(uint)Reader["race_id"];
				performances.Add (id, new SqlPerformance ((int)id, runners[runnerId], races[raceId], new Time((double)Reader["time"])));
			}
			Reader.Close ();
			return performances;
		}
		
		Dictionary<int, Race> ReadRaces (Dictionary<int, SqlMeet> meets, Dictionary<int, SqlVenue> venues)
		{
			Dictionary<int, Race> races = new Dictionary<int, Race> ();
			int id;
			SqlMeet meet;
			SqlVenue venue;
			Command.CommandText = "SELECT * FROM races";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				if (Reader["meet_id"] is DBNull) {
					meet = null;
				} else {
					meet = meets[(int)(uint)Reader["meet_id"]];
				}
				if (Reader["venue_id"] is DBNull) {
					venue = null;
				} else {
					venue = venues[(int)(uint)Reader["venue_id"]];
				}
				races.Add (id, SqlRace.NewInstance (id, meet, venue, new Date((DateTime)Reader["date"]), Gender.FromString ((string)Reader["gender"]), (int)Reader["distance"]));
			}
			Reader.Close ();
			return races;
		}
		
		Dictionary<int, Runner> ReadRunners ()
		{
			Dictionary<int, Runner> runners = new Dictionary<int, Runner> ();
			int id;
			string[] nicknames;
			int? year;
			Command.CommandText = "SELECT * FROM runners";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				if (Reader["nicknames"] is DBNull) {
					nicknames = null;
				} else {
					nicknames = ((string)Reader["nicknames"]).Split (',');
					foreach (string nickname in nicknames) {
						nickname.Trim ();
					}
				}
				if (Reader["year"] is DBNull) {
					year = null;
				} else {
					year = (int?)Reader["year"];
				}
				runners.Add (id, new SqlRunner ((int)id, (string)Reader["surname"], (string)Reader["given_name"], nicknames, Gender.FromString ((string)Reader["gender"]), year));
			}
			Reader.Close ();
			return runners;
		}

		Dictionary<int, School> ReadSchools (Dictionary<int, SqlConference> conferences)
		{
			Dictionary<int, School> schools = new Dictionary<int, School> ();
			int id;
			SqlConference conference;
			string[] nicknames;
			string type;
			Command.CommandText = "SELECT * FROM schools";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (int)(uint)Reader["id"];
				if (Reader["nicknames"] is DBNull) {
					nicknames = null;
				} else {
					nicknames = ((string)Reader["nicknames"]).Split (',');
					foreach (string nickname in nicknames) {
						nickname.Trim ();
					}
				}
				if (Reader["type"] is DBNull) {
					type = null;
				} else {
					type = (string)Reader["type"];
				}
				if (Reader["conference_id"] is DBNull) {
					conference = null;
				} else {
					conference = conferences[(int)(uint)Reader["conference_id"]];
				}
				schools.Add (id, SqlSchool.NewInstance (id, (string)Reader["name"], nicknames, type, (bool)Reader["name_first"], conference));
			}
			Reader.Close ();
			return schools;
		}
		
		Dictionary<int, SqlVenue> ReadVenues ()
		{
			Dictionary<int, SqlVenue> venues = new Dictionary<int, SqlVenue> ();
			int id;
			int? elevation;
			string name;
			Command.CommandText = "SELECT * FROM venues";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ())
			{
				id = (int)(uint)Reader["id"];
				if (Reader["name"] is DBNull) 
				{
					name = null;
				} else {
					name = (string)Reader["name"];
				}
				if (Reader["elevation"] is DBNull) 
				{
					elevation = null;
				} else {
					elevation = (int)Reader["elevation"];
				}
				venues.Add (id, new SqlVenue (id, name, (string)Reader["city"], (string)Reader["state"], elevation));
			}
			Reader.Close ();
			return venues;
		}
	}
	
	[TestFixture]
	public class TestDatabaseReader
	{
		protected internal IDbConnection Connection;
		protected internal DatabaseReader Reader;

		[SetUp]
		public void Setup ()
		{
			string connectionString = "Server=localhost;";
			connectionString += "Database=xcanalyze;";
			connectionString += "User ID=xcanalyze;";
			connectionString += "Pooling=false;";
			Connection = new MySqlConnection (connectionString);
			Connection.Open ();
			Reader = new DatabaseReader (Connection);
		}

		[Test]
		public void TestRead ()
		{
			Reader.Read ();
			Reader.Close ();
		}
	}
}
