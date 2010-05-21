using MySql.Data.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;

namespace xcanalyze.io.sql
{

	[TestFixture]
	public class TestDatabaseReader
	{
		private IDbConnection connection;
		private string connectionString;
		private DatabaseReader reader;
		
		protected IDbConnection Connection {
			get { return connection; }
			set { connection = value; }
		}

		protected string ConnectionString {
			get { return connectionString; }
			set { connectionString = value; }
		}
		
		protected DatabaseReader Reader {
			get { return reader; }
			set { reader = value; }
		}

		[SetUp]
		public void Setup ()
		{
			ConnectionString = "Server=localhost;";
			ConnectionString += "Database=xcanalyze;";
			ConnectionString += "User ID=xcanalyze;";
			ConnectionString += "Pooling=false;";
			Connection = new MySqlConnection (ConnectionString);
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
