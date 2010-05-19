using System;
using System.Collections.Generic;
using System.Data;
using xcanalyze.model;

namespace xcanalyze.io {

	public abstract class TableReader<T> : IReader<List<T>> {
		private IDataReader reader;
		private IDbCommand command;
		
		/// <summary>
		/// The reader for the command.
		/// </summary>
		protected IDataReader Reader {
			get {
				return reader;
			}
			set {
				reader = value;
			}
		}
		
		/// <summary>
		/// The command to be executed.
		/// </summary>
		protected IDbCommand Command {
			get {
				return command;
			}
			set {
				command = value;
			}
		}
		
		public TableReader (IDbConnection connection)
		{
			command = connection.CreateCommand ();
		}
		
		public void Close ()
		{
			Reader.Close ();
			command.Dispose ();
		}
		
		public List<T> Read ()
		{
			List<T> found = new List<T> ();
			reader = command.ExecuteReader ();
			while (reader.Read ()) {
				found.Add (ReadRow ());
			}
			return found;
		}
		
		/// <summary>
		/// Read a single row of the database into an instance of T.
		/// </summary>
		/// <returns>
		/// A <see cref="T"/>
		/// </returns>
		public abstract T ReadRow();
	}
	
	public class RunnersReader: TableReader<Runner> {
		public RunnersReader (IDbConnection connection) : base(connection)
		{
			Command.CommandText = "SELECT surname, given_name, gender, year FROM runners";
		}
		
		/// <summary>
		/// Read a single row of the database into a runner instance.
		/// </summary>
		/// <returns>
		/// A <see cref="Runner"/>
		/// </returns>
		public override Runner ReadRow ()
		{
			string surname = (string)Reader["surname"];
			string givenName = (string)Reader["given_name"];
			Gender gender;
			if ((string)Reader["gender"] == "M") {
				gender = Gender.M;
			} else {
				gender = Gender.F;
			}
			int year = (int)Reader["year"];
			return new Runner (surname, givenName, gender, year);
		}
	}
	
	public class SchoolsReader : TableReader<School> {
		public SchoolsReader (IDbConnection connection) : base(connection)
		{
			Command.CommandText = "SELECT schools.name, type, name_first, conferences.name AS conference FROM schools LEFT OUTER JOIN conferences ON conferences.id = schools.conference_id";
		}
		
		public override School ReadRow ()
		{
			bool nameFirst = (bool)Reader["name_first"];
			string name, type, conference;
			if (Reader["name"] is DBNull) {
				name = null;
			} else {
				name = (string)Reader["name"];
			}
			if (Reader["type"] is DBNull) {
				type = null;
			} else {
				type = (string)Reader["type"];
			}
			if (Reader["conference"] is DBNull) {
				conference = null;
			} else {
				conference = (string)Reader["conference"];
			}
			return new School (name, type, nameFirst, conference);
		}
	}
	
	public class RacesReader : TableReader<Race> {
	
		public RacesReader(IDbConnection connection) : base(connection) {
			Command.CommandText = "SELECT meet.name, date, gender, distance, venue.name AS venue, venue.city, venue.state FROM races";
		}
		
		public override Race ReadRow ()
		{
			throw new System.NotImplementedException();
		}
		
	}
}
