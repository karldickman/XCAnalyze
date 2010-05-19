using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using xcanalyze.io;
using xcanalyze.model;

namespace xcanalyze.io.test
{

	public class IoTest<T>
	{

		private IDbConnection connection;
		private IReader<List<T>> reader;
		private IWriter<List<T>> writer;
		private List<T> items;
		protected readonly static string[] tables = {"venues", "meets", "races", "schools", "conferences", "runners", "affiliations", "results"};
		protected readonly static string[] conferences = {"Northwest Conference", "Southern California Intercollegiate Athletic Conference"};

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

		public void Setup ()
		{
			connection = new MySqlConnection ("Server=localhost; Database=xca_test; User ID=xcanalyze; Pooling = false");
			connection.Open ();
			IDbCommand command = Connection.CreateCommand ();
			foreach (String table in tables) {
				command.CommandText = "DELETE FROM " + table;
				command.ExecuteNonQuery ();
			}
			command.CommandText = "INSERT INTO conferences (name) VALUES (\"" + conferences[0] + "\"), (\"" + conferences[1] + "\")";
			command.ExecuteNonQuery ();
		}

		[TearDown]
		public void TearDown ()
		{
			connection.Close ();
		}
		
		public void TestIo ()
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
	public class RunnersIo : IoTest<Runner>
	{

		[SetUp]
		public void Setup ()
		{
			base.Setup ();
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
			base.TestIo ();
		}
	}

	[TestFixture]
	public class SchoolsIo : IoTest<School>
	{
		[SetUp]
		public void Setup ()
		{
			base.Setup ();
			Reader = new SchoolsReader (Connection);
			Writer = new SchoolsWriter (Connection);
			Items = new List<School> ();
			Items.Add (new School ("Lewis & Clark", "College", true, conferences[0]));
			Items.Add (new School ("Puget Sound", "University", false, conferences[0]));
			Items.Add (new School ("Willamette", "University", true, conferences[0]));
			Items.Add (new School ("Claremont-Mudd-Scripps", null, true, conferences[1]));
			Items.Add (new School ("Chapman", "University", true, null));
			Items.Add (new School ("California Santa Cruz", "University", false, null));
		}
		
		[Test]
		public void TestIo() {
			base.TestIo();
		}
		
		[Test]
		public void TestCreateConference ()
		{
			SchoolsWriter sWriter = (SchoolsWriter)Writer;
			int id1 = sWriter.ConferenceId (conferences[conferences.Length - 1]);
			sWriter.CreateConference ("Southern Collegiate Athletic Conference");
			sWriter.CreateConference ("Inter Mountain Conference");
			int id2 = sWriter.ConferenceId ("Southern Collegiate Athletic Conference");
			int id3 = sWriter.ConferenceId ("Inter Mountain Conference");
			Assert.AreEqual (id2 + 1, id3);
			Assert.Less (id1, id2);
		}
		
		[Test]
		public void TestConferenceId ()
		{
			SchoolsWriter sWriter = (SchoolsWriter)Writer;
			Assert.AreEqual (-1, sWriter.ConferenceId ("xkcd"));
			int id0 = sWriter.ConferenceId (conferences[0]);
			int id1 = sWriter.ConferenceId (conferences[1]);
			Assert.AreNotEqual (-1, id0);
			Assert.AreNotEqual (-1, id1);
			Assert.AreEqual (id0 + 1, id1);
		}
	}
	
	[TestFixture]
	public class RacesIo : IoTest<Race>
	{
		[SetUp]
		public void Setup ()
		{
			base.Setup ();
			Reader = new RacesReader (Connection);
			Writer = new RacesWriter (Connection);
			Items = new List<Race> ();
			Items.Add (new Race ("Lewis & Clark Invitational", new DateTime (2009, 9, 23), Gender.M, 8000, "Milo McIver State Park", "Estacada", "OR"));
			Items.Add (new Race ("Northwest Conference Championship", new DateTime (2008, 11, 1), Gender.F, 6000, null, "Walla Walla", "WA"));
			Items.Add (new Race (null, new DateTime (2005, 8, 1), Gender.M, 8000, null, null, null));
		}
		
		[Test]
		public void testIo ()
		{
			base.TestIo ();
		}
	}
}
