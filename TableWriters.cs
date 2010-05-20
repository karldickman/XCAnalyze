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
		
		public uint CreateForeignKey(string table, string name)
		{
			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = "INSERT INTO " + table + " (name) VALUES (" + SqlFormat (name) + ")";
			command.ExecuteNonQuery ();
			return NameToForeignKey(table, name);
		}
		
		public uint NameToForeignKey (string table, string name)
		{
			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = "SELECT id FROM " + table + " WHERE name = " + SqlFormat (name);
			try {
				return (uint)command.ExecuteScalar ();
			} catch (NullReferenceException exception) {
				throw(new TooFewResultsException());
			}
		}
		
		public static string SqlFormat(bool toFormat) {
			if(toFormat) {
				return "1";
			}
			return "0";
		}
		
		public static string SqlFormat (DateTime toFormat)
		{
			return SqlFormat (toFormat.Year + "-" + toFormat.Month + "-" + toFormat.Day);
		}
		
		public static string SqlFormat(int toFormat) {
			return toFormat.ToString();
		}
		
		public static string SqlFormat (object toFormat)
		{
			return SqlFormat (toFormat.ToString ());
		}
		
		public static string SqlFormat (string toFormat)
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
				uint conferenceId = NameToForeignKey ("conferences", school.Conference);
				if (conferenceId < 0) {
					conferenceId = CreateForeignKey ("conferences", school.Conference);
				}
				Command.CommandText += SqlFormat (conferenceId);
			}
			Command.CommandText += ");";
		}
	}
	
	public class RacesWriter: TableWriter<Race> {
		public RacesWriter (IDbConnection connection) : base(connection)
		{
			InsertionQuery = "INSERT INTO races (meet_id, date, gender, distance, venue_id) VALUES";
		}
		
		public override void WriteRow (Race race)
		{
			uint meetId, venueId;
			Command.CommandText += InsertionQuery + "(";
			if (race.Meet == null) {
				Command.CommandText += SqlFormat (null);
			} else {
				try {
					meetId = NameToForeignKey ("meets", race.Meet);
				} catch (TooFewResultsException exception) {
					meetId = CreateForeignKey ("meets", race.Meet);
				}
				Command.CommandText += SqlFormat (meetId);
			}
			Command.CommandText += ", " + SqlFormat (race.Date) + ", ";
			Command.CommandText += SqlFormat (race.Gender) + ", ";
			Command.CommandText += SqlFormat (race.Distance) + ", ";
			if (race.Venue == null) {
				Command.CommandText += SqlFormat (null);
			} else {
				try {
					venueId = VenueId (race.Venue, race.City, race.State);
				} catch (TooFewResultsException exception) {
					venueId = CreateVenue (race.Venue, race.City, race.State);
				}
				Command.CommandText += SqlFormat (venueId);
			}
			Command.CommandText += ");";
		}
		
		public uint CreateVenue (string name, string city, string state)
		{
			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = "INSERT INTO venues (name, city, state) VALUES (" + SqlFormat (name);
			command.CommandText += ", " + SqlFormat (city) + ", " + SqlFormat (state) + ")";
			command.ExecuteNonQuery ();
			return VenueId (name, city, state);
		}
		
		protected uint VenueId (IDbCommand command) {
			IDataReader reader = command.ExecuteReader ();
			if (!reader.Read ()) {
				reader.Close ();
				throw (new TooFewResultsException ());
			}
			uint foundId = (uint)reader["id"];
			if (reader.Read ()) {
				reader.Close ();
				throw (new TooManyResultsException ());
			}
			reader.Close ();
			return foundId;
		}
		
		public uint VenueId (string name)
		{
			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = "SELECT id FROM venues WHERE name = " + SqlFormat (name);
			return VenueId (command);
		}
		
		public uint VenueId (string name, string city)
		{
			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = "SELECT id FROM venues WHERE name = " + SqlFormat (name);
			command.CommandText += " AND city = " + SqlFormat (city);
			return VenueId (command);
		}
		
		public uint VenueId (string name, string city, string state)
		{
			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = "SELECT id FROM venues WHERE name = " + SqlFormat (name);
			command.CommandText += " AND city = " + SqlFormat (city);
			command.CommandText += " AND state = " + SqlFormat (state);
			return VenueId (command);
		}
		
	}
	
	public class TooFewResultsException : Exception {}
	public class TooManyResultsException : Exception {}
}
