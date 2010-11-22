using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// A writer to write all the data in the model to a database.
    /// </summary>
    public abstract class Writer : AbstractWriter
    {
        #region Properties

        /// <summary>
        /// The creation script for the database.
        /// </summary>
        protected string CreationScriptFileName {
            get { return SupportFiles.GetPath("xca_create." + CreationScriptExtension); }
        }

        /// <summary>
        /// The file extension of the creation script.
        /// </summary>
        protected abstract string CreationScriptExtension { get; }

        /// <summary>
        /// The title of the column that has the names of all the tables.
        /// </summary>
        protected abstract string GetTablesColumn { get; }

        /// <summary>
        /// The script used to get the list of tables in the database.
        /// </summary>
        protected abstract string GetTablesCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> connection to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        public Writer(IDbConnection connection, string database) : base(connection, database)
        {
        }

        /// <summary>
        /// Create a new writer.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> connection to use.
        /// </param>
        /// <param name="database">
        /// The name of the database to use.
        /// </param>
        /// <param name="initializeDatabase">
        /// Should the database be initialized.
        /// </param>
        protected Writer(IDbConnection connection, string database, bool initializeDatabase) : base(connection, database, initializeDatabase)
        {
        }

        #endregion

        #region AbstractWriter implementation

        protected override IList<string> CreationScript()
        {
            IList<string> commands;
            using(ScriptReader reader = new ScriptReader(CreationScriptFileName)) {
                commands = reader.Read();
            }
            return commands;
        }

        protected override void InitializeDatabase()
        {
            IList<string> creationCommands = CreationScript();
            foreach(string command in creationCommands) {
                Command.CommandText = command;
                Command.ExecuteNonQuery();
            }
        }

        protected override bool IsDatabaseInitialized()
        {
            IList<string> foundTables = new List<string>();
            Command.CommandText = GetTablesCommand;
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    foundTables.Add(Reader[GetTablesColumn].ToString());
                }
            }
            if(foundTables.Count < Tables.Count) {
                return false;
            }
            foreach(string table in Tables) {
                if(!foundTables.Contains(table)) {
                    return false;
                }
            }
            return true;
        }

        public override void WriteAffiliations(IEnumerable<Affiliation> affiliations)
        {
            foreach(Affiliation affiliation in affiliations) {
                if(!affiliation.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO affiliations (runner_id, team_id, season) VALUES ({0}, {1}, {2})", affiliation.RunnerID, affiliation.TeamID, affiliation.Season);
                    Command.ExecuteNonQuery();
                    affiliation.IsAttached = true;
                }

                else {
                    Command.CommandText = String.Format("UPDATE affiliations SET team_id = {0} WHERE runner_id = {1} AND season = {2}", affiliation.TeamID, affiliation.RunnerID, affiliation.Season);
                    Command.ExecuteNonQuery();
                }
            }
        }

        public override void WriteCities(IEnumerable<City> cities)
        {
            foreach(City city in cities) {
                if(city.IsAttached) {
                    Command.CommandText = String.Format("UPDATE cities SET name = {0}, state_code = {1} WHERE city_id = {2}", Format(city.Name), Format(city.StateCode), city.ID);
                }

                else {
                    Command.CommandText = String.Format("INSERT INTO cities (name, state_code) VALUES ({0}, {1})", Format(city.Name), Format(city.StateCode));
                }
                Command.ExecuteNonQuery();
                if(!city.IsAttached) {
                    Command.CommandText = String.Format("SELECT city_id FROM cities WHERE name = {0} AND state_code = {1}", Format(city.Name), Format(city.StateCode));
                    city.ID = Convert.ToInt32(Command.ExecuteScalar());
                    city.IsAttached = true;
                }
            }
        }

        public override void WriteConferences(IEnumerable<Conference> conferences)
        {
            foreach(Conference conference in conferences) {
                if(!conference.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO conferences (name, acronym) VALUES ({0}, {1})", Format(conference.Name), Format(conference.Acronym));
                }

                else {
                    Command.CommandText = String.Format("UPDATE conferences SET name = {0}, acronym = {1} WHERE conference_id = {2}", Format(conference.Name), Format(conference.Acronym), conference.ID);
                }
                Command.ExecuteNonQuery();
                if(!conference.IsAttached) {
                    Command.CommandText = String.Format("SELECT conference_id FROM conferences WHERE name = {0} AND acronym = {1}", Format(conference.Name), Format(conference.Acronym));
                    conference.ID = Convert.ToInt32(Command.ExecuteScalar());
                    conference.IsAttached = true;
                }
            }
        }

        public override void WriteMeets(IEnumerable<Meet> meets)
        {
            foreach(Meet meet in meets) {
                if(!meet.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO meets (name) VALUES ({0})", Format(meet.Name));
                    Command.ExecuteNonQuery();
                    Command.CommandText = String.Format("SELECT meet_id FROM meets WHERE name = {0}", Format(meet.Name));
                    meet.ID = Convert.ToInt32(Command.ExecuteScalar());
                    meet.IsAttached = true;
                }

                else {
                    Command.CommandText = String.Format("UPDATE meets SET name = {0} WHERE meet_id = {1}", meet.ID, Format(meet.Name));
                    Command.ExecuteNonQuery();
                }
                Command.CommandText = String.Format("DELETE FROM meet_hosts WHERE meet_id = {0}", meet.ID);
                Command.ExecuteNonQuery();
                if(meet.Host != null) {
                    Command.CommandText = String.Format("INSERT INTO meet_hosts (meet_id, team_id) VALUES ({0}, {1})", meet.ID, meet.HostID);
                    Command.ExecuteNonQuery();
                }
            }
        }

        public override void WriteMeetInstances(IEnumerable<MeetInstance> meetInstances)
        {
            foreach(MeetInstance meetInstance in meetInstances) {
                if(!meetInstance.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO meet_instances (meet_id, date, venue_id) VALUES ({0}, {1}, {2})", meetInstance.MeetID, Format(meetInstance.Date), meetInstance.VenueID);
                    meetInstance.IsAttached = true;
                }

                else {
                    Command.CommandText = String.Format("UPDATE meet_instances SET venue_id = {0} WHERE meet_id = {1} AND date = {2}", meetInstance.VenueID, meetInstance.MeetID, Format(meetInstance.Date));
                }
                Command.ExecuteNonQuery();
                Command.CommandText = String.Format("DELETE FROM meet_instance_hosts WHERE meet_id = {0} AND date = {1}", meetInstance.MeetID, Format(meetInstance.Date));
                Command.ExecuteNonQuery();
                if(meetInstance.Host != null) {
                    Command.CommandText = String.Format("INSERT INTO meet_instance_hosts (meet_id, date, team_id) VALUES ({0}, {1}, {2})", meetInstance.MeetID, Format(meetInstance.Date), meetInstance.HostID);
                    Command.ExecuteNonQuery();
                }
            }
        }

        public override void WritePerformances(IEnumerable<Performance> performances)
        {
            foreach(Performance performance in performances) {
                Command.CommandText = String.Format("DELETE FROM results WHERE runner_id = {0} AND race_id = {1}", performance.RunnerID, performance.RaceID);
                Command.ExecuteNonQuery();
                Command.CommandText = String.Format("DELETE FROM did_not_finish WHERE runner_id = {0} AND race_id = {1}", performance.RunnerID, performance.RaceID);
                Command.ExecuteNonQuery();
                if(performance.Time != null) {
                    Command.CommandText = String.Format("INSERT INTO results (runner_id, race_id, time) VALUES ({0}, {1}, {2})", performance.RunnerID, performance.RaceID, performance.Time);
                }

                else {
                    Command.CommandText = String.Format("INSERT INTO did_not_finish (runner_id, race_id) VALUES ({0}, {1})", performance.RunnerID, performance.RaceID);
                }
                Command.ExecuteNonQuery();
                performance.IsAttached = true;
            }
        }

        public override void WriteRaces(IEnumerable<Race> races)
        {
            foreach(Race race in races) {
                if(!race.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO races (distance, gender, meet_id, date) VALUES ({0}, {1}, {2}, {3})", race.Distance, Format(race.Gender), race.MeetID, Format(race.Date));
                    Command.ExecuteNonQuery();
                    Command.CommandText = String.Format("SELECT race_id FROM races WHERE distance = {0} AND gender = {1} AND meet_id = {2} AND date = {3}", race.Distance, Format(race.Gender), race.MeetID, Format(race.Date));
                    race.ID = Convert.ToInt32(Command.ExecuteScalar());
                    race.IsAttached = true;
                }

                else {
                    Command.CommandText = String.Format("UPDATE races SET distance = {0}, gender = {1}, meet_id = {2}, date = {3} WHERE race_id = {4}", race.Distance, Format(race.Gender), race.MeetID, Format(race.Date), race.ID);
                    Command.ExecuteNonQuery();
                }
            }
        }

        public override void WriteRunners(IEnumerable<Runner> runners)
        {
            foreach(Runner runner in runners) {
                if(!runner.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO runners (surname, given_name, gender) VALUES ({0}, {1}, {2})", Format(runner.Surname), Format(runner.GivenName), Format(runner.Gender));
                    Command.ExecuteNonQuery();
                    Command.CommandText = String.Format("SELECT MAX(runner_id) FROM runners WHERE surname = {0} AND given_name = {1} AND gender = {2}", Format(runner.Surname), Format(runner.GivenName), Format(runner.Gender));
                    runner.ID = Convert.ToInt32(Command.ExecuteScalar());
                    runner.IsAttached = true;
                }
                else {
                    Command.CommandText = String.Format("UPDATE runners SET surname = {0}, given_name = {1}, gender = {2} WHERE runner_id = {3}", Format(runner.Surname), Format(runner.GivenName), Format(runner.Gender), runner.ID);
                    Command.ExecuteNonQuery();
                    Command.CommandText = String.Format("DELETE FROM college_enrollment_years WHERE runner_id = {0}", runner.ID);
                    Command.ExecuteNonQuery();
                }
                if(runner.EnrollmentYear != null) {
                    Command.CommandText = String.Format("INSERT INTO college_enrollment_years (runner_id, enrollment_year) VALUES ({0}, {1})", runner.ID, runner.EnrollmentYear);
                    Command.ExecuteNonQuery();
                }
            }
        }

        public override void WriteStates(IEnumerable<State> states)
        {
            Command.CommandText = "SELECT COUNT(state_code) FROM states";
            foreach(State state in states) {
                if(!state.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO states (state_code, name) VALUES ({0}, {1})", Format(state.Code), Format(state.Name));
                    Command.ExecuteNonQuery();
                    state.IsAttached = true;
                }
            }
            Command.CommandText = "SELECT COUNT(state_code) FROM states";
        }

        public override void WriteTeams(IEnumerable<Team> teams)
        {
            foreach(Team team in teams) {
                if(!team.IsAttached) {
                    Command.CommandText = String.Format("INSERT INTO teams (name) VALUES ({0})", Format(team.Name));
                    Command.ExecuteNonQuery();
                    Command.CommandText = String.Format("SELECT team_id FROM teams WHERE name = {0}", Format(team.Name));
                    team.ID = Convert.ToInt32(Command.ExecuteScalar());
                    team.IsAttached = true;
                }

                else {
                    Command.CommandText = String.Format("UPDATE teams SET name = {0} WHERE team_id = {1}", Format(team.Name), team.ID);
                }
                Command.CommandText = String.Format("DELETE FROM conference_affiliations WHERE team_id = {0}", team.ID);
                Command.ExecuteNonQuery();
                Command.CommandText = String.Format("DELETE FROM unaffiliated_teams WHERE team_id = {0}", team.ID);
                Command.ExecuteNonQuery();
                if(team.Conference != null) {
                    Command.CommandText = String.Format("INSERT INTO conference_affiliations (team_id, conference_id) VALUES ({0}, {1})", team.ID, team.ConferenceID);
                    Command.ExecuteNonQuery();
                }
                else {
                    Command.CommandText = String.Format("INSERT INTO unaffiliated_teams (team_id) VALUES ({0})", team.ID);
                    Command.ExecuteNonQuery();
                }
                Command.CommandText = String.Format("DELETE FROM team_nicknames WHERE team_id = {0}", team.ID);
                Command.ExecuteNonQuery();
                foreach(string nickname in team.Nicknames) {
                    Command.CommandText = String.Format("INSERT INTO team_nicknames (team_id, nickname) VALUES ({0}, {1})", team.ID, Format(nickname));
                    Command.ExecuteNonQuery();
                }
            }
        }

        public override void WriteVenues(IEnumerable<Venue> venues)
        {
            foreach(Venue venue in venues) {
                if(venue.IsAttached) {
                    Command.CommandText = String.Format("UPDATE venues SET name = {0}, city_id = {1} WHERE venue_id = {2}", Format(venue.Name), venue.CityID, venue.ID);
                }

                else {
                    Command.CommandText = String.Format("INSERT INTO venues (name, city_id) VALUES ({0}, {1})", Format(venue.Name), venue.CityID);
                }
                Command.ExecuteNonQuery();
                if(!venue.IsAttached) {
                    Command.CommandText = String.Format("SELECT venue_id FROM venues WHERE name = {0} AND city_id = {1}", Format(venue.Name), venue.CityID);
                    venue.ID = Convert.ToInt32(Command.ExecuteScalar());
                    venue.IsAttached = true;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Format a particular date for insertion in an SQL query.
        /// </summary>
        /// <param name="value_">
        /// The <see cref="Date"/> to format.
        /// </param>
        public string Format(DateTime value_)
        {
            string result = value_.Year + "-";
            if(value_.Month < 10) {
                result += "0";
            }
            result += value_.Month + "-";
            if(value_.Day < 10) {
                result += "0";
            }
            return Format(result + value_.Day);
        }

        /// <summary>
        /// Format a particular boolean value for insertion in an SQL query.
        /// </summary>
        public string Format(bool value_)
        {
            if(value_) {
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
        public string Format(string value_)
        {
            if(value_ == null) {
                return "NULL";
            }
            return "\"" + value_ + "\"";
        }
        
        #endregion
    }
}
