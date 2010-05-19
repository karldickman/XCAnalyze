using System;
using System.Collections.Generic;
using System.Data;
using xcanalyze.model;

namespace xcanalyze.io
{
	public abstract class TableWriter<T> : IWriter<List<T>>
	{
		private IDbCommand command;
		private IDbConnection connection;
		private string insertionQuery;

		/// <summary>
		/// The command to be executed.
		/// </summary>
		protected IDbCommand Command {
			get { return command; }
		}
		
		/// <summary>
		/// The database connection being used.
		/// </summary>
		protected IDbConnection Connection {
			get { return connection; }
		}

		/// <summary>
		/// The insertion query used to add the data.
		/// </summary>
		protected string InsertionQuery {
			get { return insertionQuery; }
			set { insertionQuery = value; }
		}

		public TableWriter (IDbConnection connection)
		{
			this.connection = connection;
			command = connection.CreateCommand ();
			command.CommandText = "";
		}

		public void Close ()
		{
			command.ExecuteNonQuery ();
			command.Dispose ();
		}
		
		public string SqlFormat(bool toFormat) {
			if(toFormat) {
				return "1";
			}
			return "0";
		}
		
		public string SqlFormat(int toFormat) {
			return toFormat.ToString();
		}
		
		public string SqlFormat (object toFormat)
		{
			return SqlFormat (toFormat.ToString ());
		}
		
		public string SqlFormat (string toFormat)
		{
			if (toFormat == null) {
				return "NULL";
			}
			return "\"" + toFormat + "\"";
		}

		public void Write (List<T> toWrite)
		{
			foreach (T item in toWrite) {
				WriteRow (item);
			}
		}

		public abstract void WriteRow (T item);
		
	}

	public class RunnersWriter : TableWriter<Runner>
	{
		public RunnersWriter (IDbConnection connection) : base(connection)
		{
			InsertionQuery = "INSERT INTO runners (surname, given_name, gender, year) VALUES";
		}

		public override void WriteRow (Runner runner)
		{
			Command.CommandText += InsertionQuery + " (";
			Command.CommandText += SqlFormat (runner.Surname) + ", ";
			Command.CommandText += SqlFormat (runner.GivenName) + ", ";
			Command.CommandText += SqlFormat (runner.Gender) + ", ";
			Command.CommandText += SqlFormat (runner.Year) + ");";
		}
		
	}

	public class SchoolsWriter : TableWriter<School>
	{
		public SchoolsWriter (IDbConnection connection) : base(connection)
		{
			InsertionQuery = "INSERT INTO schools (name, type, name_first, conference_id) VALUES";
		}
		
		public override void WriteRow (School school)
		{
			Command.CommandText += InsertionQuery + "(";
			Command.CommandText += SqlFormat (school.Name) + ", ";
			Command.CommandText += SqlFormat (school.Type) + ", ";
			Command.CommandText += SqlFormat (school.NameFirst) + ", ";
			if (school.Conference == null) {
				Command.CommandText += SqlFormat (null);
			} else {
				int conferenceId = ConferenceId (school.Conference);
				if (conferenceId < 0) {
					conferenceId = CreateConference (school.Conference);
				}
				Command.CommandText += SqlFormat (conferenceId);
			}
			Command.CommandText += ");";
		}
		
		public int ConferenceId (string name)
		{
			IDbCommand confCommand = Connection.CreateCommand ();
			confCommand.CommandText = "SELECT id FROM conferences WHERE name = \"" + name + "\"";
			try {
				return (int)(uint)confCommand.ExecuteScalar();
			} catch (NullReferenceException exception) {
				return -1;
			}
		}
		
		public int CreateConference (string name)
		{
			IDbCommand confCommand = Connection.CreateCommand ();
			confCommand.CommandText = "INSERT INTO conferences (name) VALUES (" + SqlFormat (name) + ")";
			confCommand.ExecuteNonQuery ();
			return ConferenceId (name);
		}
		
	}
	
	public class RacesWriter: TableWriter<Race> {
		public RacesWriter (IDbConnection connection) : base(connection)
		{
			InsertionQuery = "INSERT INTO races (mmet_id, date, gender, distance, venue) VALUES";
		}
		
		public override void WriteRow (Race item)
		{
			throw new System.NotImplementedException();
		}
		
	}
}
