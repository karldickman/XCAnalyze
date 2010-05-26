using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using XcAnalyze.Model;

namespace XcAnalyze.Io.Sql
{

	public class DatabaseReader : IReader<Data>
	{
		private IDataReader reader;
		private IDbCommand command;
		private IDbConnection connection;
		
		protected IDataReader Reader {
			get { return reader; }
			set { reader = value; }
		}
		
		protected IDbCommand Command {
			get { return command; }
		}
		
		protected IDbConnection Connection {
			get { return connection; }
		}

		public DatabaseReader (IDbConnection connection)
		{
			this.connection = connection;
			command = connection.CreateCommand ();
		}

		public void Close ()
		{
			Reader.Close ();
			Command.Dispose ();
		}

		public Data Read ()
		{
			Dictionary<uint, SqlConference> conferences = ReadConferences ();
			Dictionary<uint, Runner> runners = ReadRunners ();
			Dictionary<uint, School> schools = ReadSchools (conferences);
			Dictionary<uint, Affiliation> affiliations = ReadAffiliations (runners, schools);
			Dictionary<uint, SqlMeet> meets = ReadMeets ();
			Dictionary<uint, SqlVenue> venues = ReadVenues ();
			Dictionary<uint, Race> races = ReadRaces (meets, venues);
			Dictionary<uint, Performance> performances = ReadPerformances (runners, races);
			return new Data (new List<Affiliation>(affiliations.Values), new List<Performance>(performances.Values), new List<Race>(races.Values), new List<Runner>(runners.Values), new List<School>(schools.Values));
		}
		
		Dictionary<uint, Affiliation> ReadAffiliations (Dictionary<uint, Runner> runners, Dictionary<uint, School> schools)
		{
			Dictionary<uint, Affiliation> affiliations = new Dictionary<uint, Affiliation> ();
			uint id, runnerId, schoolId;
			Command.CommandText = "SELECT * FROM affiliations";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				runnerId = (uint)reader["runner_id"];
				schoolId = (uint)reader["school_id"];
				affiliations.Add (id, new SqlAffiliation ((int)id, runners[runnerId], schools[schoolId], (int)reader["year"]));
			}
			Reader.Close ();
			return affiliations;
		}
		
		Dictionary<uint, SqlConference> ReadConferences ()
		{
			Dictionary<uint, SqlConference> conferences = new Dictionary<uint, SqlConference> ();
			uint id;
			Command.CommandText = "SELECT * FROM conferences";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				conferences.Add (id, new SqlConference (id, (string)reader["name"], (string)reader["abbreviation"]));
			}
			Reader.Close ();
			return conferences;
		}
		
		Dictionary<uint, SqlMeet> ReadMeets ()
		{
			Dictionary<uint, SqlMeet> meets = new Dictionary<uint, SqlMeet> ();
			uint id;
			Command.CommandText = "SELECT * FROM meets";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				meets.Add (id, new SqlMeet (id, (string)reader["name"]));
			}
			Reader.Close ();
			return meets;
		}
		
		Dictionary<uint, Performance> ReadPerformances (Dictionary<uint, Runner> runners, Dictionary<uint, Race> races)
		{
			Dictionary<uint, Performance> performances = new Dictionary<uint, Performance> ();
			uint id, runnerId, raceId;
			Command.CommandText = "SELECT * FROM results";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				runnerId = (uint)reader["runner_id"];
				raceId = (uint)reader["race_id"];
				performances.Add (id, new SqlPerformance ((int)id, runners[runnerId], races[raceId], new Time((double)reader["time"])));
			}
			Reader.Close ();
			return performances;
		}
		
		Dictionary<uint, Race> ReadRaces (Dictionary<uint, SqlMeet> meets, Dictionary<uint, SqlVenue> venues)
		{
			Dictionary<uint, Race> races = new Dictionary<uint, Race> ();
			uint id;
			SqlMeet meet;
			SqlVenue venue;
			Command.CommandText = "SELECT * FROM races";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				if (reader["meet_id"] is DBNull) {
					meet = null;
				} else {
					meet = meets[(uint)reader["meet_id"]];
				}
				if (reader["venue_id"] is DBNull) {
					venue = null;
				} else {
					venue = venues[(uint)reader["venue_id"]];
				}
				races.Add (id, SqlRace.NewInstance (id, meet, venue, new Date((DateTime)reader["date"]), Gender.FromString ((string)reader["gender"]), (int)reader["distance"]));
			}
			Reader.Close ();
			return races;
		}
		
		Dictionary<uint, Runner> ReadRunners ()
		{
			Dictionary<uint, Runner> runners = new Dictionary<uint, Runner> ();
			uint id;
			string[] nicknames;
			int? year;
			Command.CommandText = "SELECT * FROM runners";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				if (reader["nicknames"] is DBNull) {
					nicknames = null;
				} else {
					nicknames = ((string)reader["nicknames"]).Split (',');
					foreach (string nickname in nicknames) {
						nickname.Trim ();
					}
				}
				if (reader["year"] is DBNull) {
					year = null;
				} else {
					year = (int?)reader["year"];
				}
				runners.Add (id, new SqlRunner ((int)id, (string)reader["surname"], (string)reader["given_name"], nicknames, Gender.FromString ((string)reader["gender"]), year));
			}
			Reader.Close ();
			return runners;
		}

		Dictionary<uint, School> ReadSchools (Dictionary<uint, SqlConference> conferences)
		{
			Dictionary<uint, School> schools = new Dictionary<uint, School> ();
			uint id;
			SqlConference conference;
			string[] nicknames;
			string type;
			Command.CommandText = "SELECT * FROM schools";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ()) {
				id = (uint)reader["id"];
				if (reader["nicknames"] is DBNull) {
					nicknames = null;
				} else {
					nicknames = ((string)reader["nicknames"]).Split (',');
					foreach (string nickname in nicknames) {
						nickname.Trim ();
					}
				}
				if (reader["type"] is DBNull) {
					type = null;
				} else {
					type = (string)reader["type"];
				}
				if (reader["conference_id"] is DBNull) {
					conference = null;
				} else {
					conference = conferences[(uint)reader["conference_id"]];
				}
				schools.Add (id, SqlSchool.NewInstance (id, (string)reader["name"], nicknames, type, (bool)reader["name_first"], conference));
			}
			Reader.Close ();
			return schools;
		}
		
		Dictionary<uint, SqlVenue> ReadVenues ()
		{
			Dictionary<uint, SqlVenue> venues = new Dictionary<uint, SqlVenue> ();
			uint id;
			int? elevation;
			string name;
			Command.CommandText = "SELECT * FROM venues";
			Reader = Command.ExecuteReader ();
			while (Reader.Read ())
			{
				id = (uint)reader["id"];
				if (reader["name"] is DBNull) 
				{
					name = null;
				} else {
					name = (string)reader["name"];
				}
				if (reader["elevation"] is DBNull) 
				{
					elevation = null;
				} else {
					elevation = (int)reader["elevation"];
				}
				venues.Add (id, new SqlVenue (id, name, (string)reader["city"], (string)reader["state"], elevation));
			}
			Reader.Close ();
			return venues;
		}
	}
	
	[TestFixture]
	public class TestDatabaseReader
	{
		private IDbConnection connection;
		private string connectionString;
		private DatabaseReader reader;

		[SetUp]
		public void Setup ()
		{
			connectionString = "Server=localhost;";
			connectionString += "Database=xcanalyze;";
			connectionString += "User ID=xcanalyze;";
			connectionString += "Pooling=false;";
			connection = new MySqlConnection (connectionString);
			connection.Open ();
			reader = new DatabaseReader (connection);
		}

		[Test]
		public void TestRead ()
		{
			reader.Read ();
			reader.Close ();
		}
	}
}
