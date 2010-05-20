using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using xcanalyze.model;

namespace xcanalyze.io
{

	public class TestIoBase<T>
	{

		private IDbConnection connection;
		private IReader<List<T>> reader;
		private IWriter<List<T>> writer;
		private List<T> items;
		protected readonly static string[] cities = {"Estacada", "Forest Grove", "Seattle"};
		protected readonly static string[] conferences = {"Northwest Conference", "Southern California Intercollegiate Athletic Conference"};
		protected readonly static string[] meets = {"Lewis & Clark Invitational", "Northwest Conference Championships"};
		protected readonly static string[] states = {"OR", "OR", "WA"};
		protected readonly static string[] tables = {"results", "races", "meets", "venues", "schools", "conferences", "affiliations", "runners"};
		protected readonly static string[] venues = {"Milo McIver State Park", "Lincoln Park", "Lincoln Park"};

		/// <summary>
		/// The database connection to be used for the tests.
		/// </summary>
		public IDbConnection Connection {
			get { return connection; }
		}

		/// <summary>
		/// The items to be written.
		/// </summary>
		public List<T> Items {
			get { return items; }
			set { items = value; }
		}

		/// <summary>
		/// The reader with which the items will be read.
		/// </summary>
		public IReader<List<T>> Reader {
			get { return reader; }
			set { reader = value; }
		}

		/// <summary>
		/// The writer with which the items will be written.
		/// </summary>
		public IWriter<List<T>> Writer {
			get { return writer; }
			set { writer = value; }
		}

		public void InitDatabase ()
		{
			connection = new MySqlConnection ("Server=localhost; Database=xca_test; User ID=xcanalyze; Pooling = false");
			connection.Open ();
			IDbCommand command = Connection.CreateCommand ();
			foreach (String table in tables) {
				command.CommandText = "DELETE FROM " + table;
				command.ExecuteNonQuery ();
			}
			command.CommandText = "INSERT INTO conferences (name) VALUES (";
			command.CommandText += TableWriter<T>.SqlFormat (conferences[0]) + "), (";
			command.CommandText += TableWriter<T>.SqlFormat (conferences[1]) + ");";
			command.CommandText += "INSERT INTO meets (name) VALUES (";
			command.CommandText += TableWriter<T>.SqlFormat (meets[0]) + "), (";
			command.CommandText += TableWriter<T>.SqlFormat (meets[1]) + ");";
			command.CommandText += "INSERT INTO venues (name, city, state) VALUES (";
			command.CommandText += TableWriter<T>.SqlFormat (venues[0]) + ", ";
			command.CommandText += TableWriter<T>.SqlFormat (cities[0]) + ", ";
			command.CommandText += TableWriter<T>.SqlFormat (states[0]) + "), (";
			command.CommandText += TableWriter<T>.SqlFormat (venues[1]) + ", ";
			command.CommandText += TableWriter<T>.SqlFormat (cities[1]) + ", ";
			command.CommandText += TableWriter<T>.SqlFormat (states[1]) + "), (";
			command.CommandText += TableWriter<T>.SqlFormat (venues[2]) + ", ";
			command.CommandText += TableWriter<T>.SqlFormat (cities[2]) + ", ";
			command.CommandText += TableWriter<T>.SqlFormat (states[2]) + ");";
			command.ExecuteNonQuery ();
		}

		[TearDown]
		public void TearDown ()
		{
			connection.Close ();
		}
		
		public void BaseTestIo ()
		{
			writer.Write (items);
			writer.Close ();
			List<T> actual = reader.Read ();
			Assert.AreEqual (items.Count, actual.Count);
			for (int i = 0; i < Items.Count; i++) 
			{
				Assert.AreEqual (Items[0], actual[0]);
			}
		}
	}

	[TestFixture]
	public class TestRunnersIo : TestIoBase<Runner>
	{

		[SetUp]
		public void Setup ()
		{
			base.InitDatabase();
			Reader = new RunnersReader (Connection);
			Writer = new RunnersWriter (Connection);
			Items = new List<Runner> ();
			Items.Add (new Runner ("Dickman", "Karl", Gender.M, 2010));
			Items.Add (new Runner ("Roberts", "John", Gender.M, 2012));
			Items.Add (new Runner ("Carman", "Jeff", Gender.M, 2003));
			Items.Add (new Runner ("Fix", "Kirsten", Gender.F, 2010));
		}
		
		[Test]
		public void TestIo ()
		{
			base.BaseTestIo ();
		}
	}

	[TestFixture]
	public class TestSchoolsIo : TestIoBase<School>
	{
		private SchoolsWriter sWriter;
		[SetUp]
		public void Setup ()
		{
			base.InitDatabase();
			Reader = new SchoolsReader (Connection);
			Writer = sWriter = new SchoolsWriter (Connection);
			Items = new List<School> ();
			Items.Add (new School ("Lewis & Clark", "College", true, conferences[0]));
			Items.Add (new School ("Puget Sound", "University", false, conferences[0]));
			Items.Add (new School ("Willamette", "University", true, conferences[0]));
			Items.Add (new School ("Claremont-Mudd-Scripps", null, true, conferences[1]));
			Items.Add (new School ("Chapman", "University", true, null));
			Items.Add (new School ("California Santa Cruz", "University", false, null));
		}
		
		[Test]
		public void TestIo ()
		{
			base.BaseTestIo ();
		}
		
		[Test]
		public void TestCreateConference ()
		{
			uint id1 = sWriter.ConferenceId (conferences[conferences.Length - 1]);
			sWriter.CreateConference ("Southern Collegiate Athletic Conference");
			sWriter.CreateConference ("Inter Mountain Conference");
			uint id2 = sWriter.ConferenceId ("Southern Collegiate Athletic Conference");
			uint id3 = sWriter.ConferenceId ("Inter Mountain Conference");
			Assert.AreEqual (id2 + 1, id3);
			Assert.Less (id1, id2);
		}
		
		[Test]
		public void TestConferenceId ()
		{
			//Check that it does not find invalid conferences.
			try {
				sWriter.ConferenceId ("xkcd");
				Assert.Fail ("Searching for xkcd should throw a TooFewResultsException.");
			} catch (TooFewResultsException exception) {
			}
			uint id0 = sWriter.ConferenceId (conferences[0]);
			uint id1 = sWriter.ConferenceId (conferences[1]);
			Assert.AreEqual (id0 + 1, id1);
		}
	}
	
	[TestFixture]
	public class TestRacesIo : TestIoBase<Race>
	{
		private RacesWriter rWriter;
		
		[SetUp]
		public void Setup ()
		{
			base.InitDatabase();
			Reader = new RacesReader (Connection);
			Writer = rWriter = new RacesWriter (Connection);
			Items = new List<Race> ();
			Items.Add (new Race (meets[0], new DateTime (2009, 9, 23), Gender.M, 8000, "Milo McIver State Park", "Estacada", "OR"));
			Items.Add (new Race (meets[1], new DateTime (2008, 11, 1), Gender.F, 6000, null, "Walla Walla", "WA"));
			Items.Add (new Race (null, new DateTime (2005, 8, 1), Gender.M, 8000, null, null, null));
		}
		
		[Test]
		public void TestIo ()
		{
			base.BaseTestIo ();
		}
				
		[Test]
		public void TestCreateMeet ()
		{
			uint id1 = rWriter.MeetId (meets[meets.Length - 1]);
			rWriter.CreateMeet ("Charles Bowles Invitational");
			rWriter.CreateMeet ("Sundodger Invitational");
			uint id2 = rWriter.MeetId("Charles Bowles Invitational");
			uint id3 = rWriter.MeetId("Sundodger Invitational");
			Assert.AreEqual (id2 + 1, id3);
			Assert.Less (id1, id2);
		}
		
		[Test]
		public void TestCreateVenue ()
		{
			int numVenues = venues.Length;
			uint id1 = rWriter.VenueId (venues[numVenues - 1], cities[numVenues - 1], states[numVenues - 1]);
			rWriter.CreateVenue ("PLU Golf Course", "Spanaway", "WA");
			rWriter.CreateVenue ("Willamette Mission State Park", "Brooks", "OR");
			uint id2 = rWriter.VenueId ("PLU Golf Course");
			uint id3 = rWriter.VenueId ("Willamette Mission State Park");
			Assert.AreEqual (id2 + 1, id3);
			Assert.Less (id1, id2);
		}
		
		[Test]
		public void testMeetId ()
		{
			try {
				rWriter.MeetId ("xkcd");
				Assert.Fail ("Searching for xkcd should raise a TooFewResultsException.");
			} catch (TooFewResultsException exception) {
			}
			uint id0 = rWriter.MeetId (meets[0]);
			uint id1 = rWriter.MeetId (meets[1]);
			Assert.AreEqual (id0 + 1, id1);
		}
		
		[Test]
		public void testVenueId ()
		{
			try {
				rWriter.VenueId ("xkcd");
				Assert.Fail ("Searching for xkcd should raise a TooFewResultsException.");
			} catch (TooFewResultsException exception) {
			}
			try {
				rWriter.VenueId ("Lincoln Park");
				Assert.Fail ("Searching for Lincoln Park should raise a TooManyResultsException.");
			} catch (TooManyResultsException exception) {
			}
			uint id0 = rWriter.VenueId (venues[0]);
			uint id1 = rWriter.VenueId (venues[1], cities[1]);
			uint id2 = rWriter.VenueId (venues[2], cities[2]);
			Assert.AreEqual (id0 + 1, id1);
			Assert.AreEqual (id1 + 1, id2);
		}
	}
}
