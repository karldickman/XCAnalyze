using System;
using System.Collections.Generic;
using System.Data;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{   
    /// <summary>
    /// A writer to write all the data in the model to a database.
    /// </summary>
    abstract public class Writer : AbstractWriter
    {
        /// <summary>
        /// The creation script for the database.
        /// </summary>
        public string CREATION_SCRIPT
        {
            get
            {
                return SupportFiles.GetPath ("xca_create." +
                    CREATION_SCRIPT_EXTENSION);
            }
        }
        
        /// <summary>
        /// The file extension of the creation script.
        /// </summary>
        abstract public string CREATION_SCRIPT_EXTENSION { get; }
        
        /// <summary>
        /// The title of the column that has the names of all the tables.
        /// </summary>
        abstract public string GET_TABLES_COLUMN { get; }

        /// <summary>
        /// The script used to get the list of tables in the database.
        /// </summary>
        abstract public string GET_TABLES_COMMAND { get; }
        
        public Writer(IDbConnection connection, string database)
        : base(connection, database) {}
        
        public Writer(IDbConnection connection, string database,
            IDbCommand command) : base(connection, database, command) {}
        
        override public IList<string> CreationScript()
        {
            IList<string> commands;
            ScriptReader reader;
            reader = new ScriptReader(CREATION_SCRIPT);
            commands = reader.Read();
            reader.Dispose();
            return commands;
        }
        
        /// <summary>
        /// Format a particular date for insertion in an SQL query.
        /// </summary>
        /// <param name="value_">
        /// The <see cref="Date"/> to format.
        /// </param>
        public string Format(DateTime value_)
        {
            string result = value_.Year + "-";
            if(value_.Month < 10)
            {
                result += "0";
            }
            result += value_.Month + "-";
            if(value_.Day < 10)
            {
                result += "0";
            }
            return Format(result + value_.Day);
        }
        
        /// <summary>
        /// Format a particular boolean value for insertion in an SQL query.
        /// </summary>
        public string Format (bool value_)
        {
            if (value_)
            {
                return "1";
            }
            return "0";
        }
        
        /// <summary>
        /// Format the given gender for insertion in an SQL query.
        /// </summary>
        public string Format(Gender value_)
        {
            return Format(value_.ToString());
        }
        
        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        public string Format(int? value_)
        {
            if(value_ == null)
            {
                return "NULL";
            }
            return value_.ToString();
        }
        
        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        public string Format (string value_)
        {
            if (value_ == null)
            {
                return "NULL";
            }
            return "\"" + value_ + "\"";
        }

        /// <summary>
        /// Format the given value for insertion in an SQL query.
        /// </summary>
        public string Format(string[] value_)
        {
            if(value_ == null)
            {
                return "NULL";
            }
            return Format(String.Join(", ", value_));
        }
        
        override protected internal void InitializeDatabase ()
        {
            IList<string> creationCommands = CreationScript();
            foreach(string command in creationCommands)
            {
                Command.CommandText = command;
                Command.ExecuteNonQuery();
            }
        }
        
        override protected internal bool IsDatabaseInitialized ()
        {
            IList<string> foundTables = new List<string> ();
            Command.CommandText = GET_TABLES_COMMAND;
            ResultsReader = Command.ExecuteReader ();
            while (ResultsReader.Read ())
            {
                foundTables.Add ((string)ResultsReader[GET_TABLES_COLUMN]);
            }
            ResultsReader.Dispose ();
            if (foundTables.Count < TABLES.Length)
            {
                return false;
            }
            foreach (string table in TABLES)
            {
                if (!foundTables.Contains (table))
                {
                    return false;
                }
            }
            if (foundTables.Count == TABLES.Length)
            {
                return true;
            }
            if (foundTables.Count != TABLES.Length + VIEWS.Length)
            {
                return false;
            }
            foreach (string table in VIEWS)
            {
                if (!foundTables.Contains (table))
                {
                    return false;
                }
            }
            return true;
        }
        
        override public void WriteAffiliations(IList<Affiliation> affiliations,
            IList<Runner> runners, IList<School> schools)
        {
            for(int i = 0; i < affiliations.Count; i++)
            {
                Command.CommandText = "INSERT INTO affiliations (id, runner_id, school_id, year) VALUES (" + (i + 1) + ", " + (runners.IndexOf(affiliations[i].Runner) + 1) + ", " + (schools.IndexOf(affiliations[i].School) + 1) + ", " + affiliations[i].Year + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteConferences(IList<string> conferenceSet)
        {
            IList<string> conferences = new List<string>(conferenceSet);
            for(int i = 0; i < conferences.Count; i++)
            {
                Command.CommandText = "INSERT INTO conferences (id, name) VALUES (" + (i + 1) + ", " + Format(conferences[i]) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteMeetNames(IList<string> meetNameSet)
        {
            IList<string> meetNames = new List<string>(meetNameSet);
            for(int i = 0; i < meetNames.Count; i++)
            {
                Command.CommandText = "INSERT INTO meet_names (id, name) VALUES (" + (i + 1) + ", " + Format(meetNames[i]) + ")";
                Command.ExecuteNonQuery();
            }
        }
       
        override public void WriteMeets(IList<Meet> meets,
            IList<string> meetNameSet, IList<Race> races, IList<Venue> venues)
        {
            int? meetNameId, venueId, mensRaceId, womensRaceId;
            IList<string> meetNames = new List<string>(meetNameSet);
            for(int i = 0; i < meets.Count; i++)
            {
                meetNameId = venueId = mensRaceId = womensRaceId = null;
                if(meets[i].Name != null)
                {
                    meetNameId = meetNames.IndexOf(meets[i].Name) + 1;
                }
                if(meets[i].Location != null)
                {
                    venueId = venues.IndexOf(meets[i].Location) + 1;
                }
                if(meets[i].MensRace != null)
                {
                    mensRaceId = races.IndexOf(meets[i].MensRace) + 1;
                }
                if(meets[i].WomensRace != null)
                {
                    womensRaceId = races.IndexOf(meets[i].WomensRace) + 1;
                }
                Command.CommandText = "SELECT id FROM races";
                ResultsReader = Command.ExecuteReader();
                ResultsReader.Close();
                Command.CommandText = "INSERT INTO meets (id, meet_name_id, date, venue_id, mens_race_id, womens_race_id) VALUES (" + (i + 1) + ", " + Format(meetNameId) + ", " + Format(meets[i].Date) + ", " + Format(venueId) + ", " + Format(mensRaceId) + ", " + Format(womensRaceId) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WritePerformances(IList<Performance> performances,
            IList<Race> races, IList<Runner> runners)
        {
            for(int i = 0; i < performances.Count; i++)
            {                
                Command.CommandText = "INSERT INTO results (id, runner_id, race_id, time) VALUES (" + (i + 1) + ", " + Format(runners.IndexOf(performances[i].Runner) + 1) + ", " + Format(races.IndexOf(performances[i].Race) + 1) + ", " + performances[i].Time + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteRaces(IList<Race> races)
        {
            for(int i = 0; i < races.Count; i++)
            {
                Command.CommandText = "INSERT INTO races (id, distance) VALUES (" + (i + 1) + ", " + races[i].Distance + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteRunners(IList<Runner> runners)
        {
            for(int i = 0; i < runners.Count; i++)
            {
                Command.CommandText = "INSERT INTO runners (id, surname, given_name, gender, year) VALUES (" + (i + 1) + ", " + Format(runners[i].Surname) + ", " + Format(runners[i].GivenName) + ", " + Format(runners[i].Gender) + ", " + Format(runners[i].Year) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteSchools(IList<School> schools,
            IList<string> conferenceSet)
        {
            IList<string> conferences = new List<string>(conferenceSet);
            int? conferenceId;
            foreach(School school in schools)
            {
                conferenceId = null;
                if(school.Conference != null)
                {
                    conferenceId = conferences.IndexOf(school.Conference) + 1;
                }
                Command.CommandText = "INSERT INTO schools (name, type, name_first, conference_id) VALUES (" + Format(school.Name) + ", " + Format(school.Type) + ", " + Format(school.NameFirst) + ", " + Format(conferenceId) + ")";
                Command.ExecuteNonQuery();
            }
        }
        
        override public void WriteVenues(IList<Venue> venues)
        {
            foreach(Venue venue in venues)
            {
                Command.CommandText = "INSERT INTO venues (name, city, state) VALUES (" + Format(venue.Name) + ", " + Format(venue.City) + ", " + Format(venue.State) + ")";                
                Command.ExecuteNonQuery();
            }
        }
    }
}