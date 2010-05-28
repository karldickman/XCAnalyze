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
				id = (int)(uint)reader["id"];
				runnerId = (int)(uint)reader["runner_id"];
				schoolId = (int)(uint)reader["school_id"];
				affiliations.Add (id, new SqlAffiliation ((int)id, runners[runnerId], schools[schoolId], (int)reader["year"]));
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
				id = (int)(uint)reader["id"];
				conferences.Add (id, new SqlConference (id, (string)reader["name"], (string)reader["abbreviation"]));
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
				id = (int)(uint)reader["id"];
				meets.Add (id, new SqlMeet (id, (string)reader["name"]));
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
				id = (int)(uint)reader["id"];
				runnerId = (int)(uint)reader["runner_id"];
				raceId = (int)(uint)reader["race_id"];
				performances.Add (id, new SqlPerformance ((int)id, runners[runnerId], races[raceId], new Time((double)reader["time"])));
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
				id = (int)(uint)reader["id"];
				if (reader["meet_id"] is DBNull) {
					meet = null;
				} else {
					meet = meets[(int)(uint)reader["meet_id"]];
				}
				if (reader["venue_id"] is DBNull) {
					venue = null;
				} else {
					venue = venues[(int)(uint)reader["venue_id"]];
				}
				races.Add (id, SqlRace.NewInstance (id, meet, venue, new Date((DateTime)reader["date"]), Gender.FromString ((string)reader["gender"]), (int)reader["distance"]));
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
				id = (int)(uint)reader["id"];
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
				id = (int)(uint)reader["id"];
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
					conference = conferences[(int)(uint)reader["conference_id"]];
				}
				schools.Add (id, SqlSchool.NewInstance (id, (string)reader["name"], nicknames, type, (bool)reader["name_first"], conference));
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
				id = (int)(uint)reader["id"];
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
