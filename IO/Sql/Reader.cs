using System;
using System.Collections.Generic;
using System.Data;

using XCAnalyze.Collections;
using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{
    /// <summary>
    /// A <see cref="IReader"/> that reads all the required data for the model out of a
    /// database.
    /// </summary>
    public class Reader : AbstractReader
    {
        #region Constructors

        /// <summary>
        /// Create a new reader.
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/> to connect to.
        /// </param>
        /// <param name="database">
        /// The name of the database from which this reader should read.
        /// </param>
        public Reader(IDbConnection connection, string database) : base(connection, database)
        {
        }

        #endregion

        #region AbstractReader implementation

        public override IDictionary<int, IDictionary<int, Affiliation>> ReadAffiliations(IDictionary<int, Runner> runners, IDictionary<int, Team> teams)
        {
            IDictionary<int, IDictionary<int, Affiliation>> affiliations = new Dictionary<int, IDictionary<int, Affiliation>>();
            int runnerID, teamID, season;
            Command.CommandText = "SELECT runner_id, team_id, season FROM affiliations";
            Reader = Command.ExecuteReader();
            while(Reader.Read()) {
                runnerID = Convert.ToInt32(Reader["runner_id"]);
                teamID = Convert.ToInt32(Reader["team_id"]);
                season = Convert.ToInt32(Reader["season"]);
                if(!affiliations.ContainsKey(runnerID)) {
                    affiliations[runnerID] = new Dictionary<int, Affiliation>();
                }
                affiliations[runnerID][season] = Affiliation.NewEntity(runners[runnerID], teams[teamID], season);
            }
            Reader.Close();
            return affiliations;
        }

        public override IDictionary<int, City> ReadCities(IDictionary<string, State> states)
        {
            IDictionary<int, City> cities = new Dictionary<int, City>();
            Command.CommandText = "SELECT city_id, name, state_code FROM cities";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int id = Convert.ToInt32(Reader["city_id"]);
                    string name = Reader["name"].ToString();
                    string stateCode = Reader["state_code"].ToString();
                    State state = states[stateCode];
                    cities.Add(id, City.NewEntity(id, name, state));
                }
            }
            return cities;
        }

        public override IDictionary<int, Conference> ReadConferences()
        {
            IDictionary<int, Conference> conferences = new Dictionary<int, Conference>();
            Command.CommandText = "SELECT conference_id, name, acronym " + "FROM conferences";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int id = Convert.ToInt32(Reader["conference_id"]);
                    Conference conference = Conference.NewEntity(id, (string)Reader["name"], (string)Reader["acronym"]);
                    conferences.Add(id, conference);
                }
            }
            return conferences;
        }

        public override IDictionary<int, IDictionary<DateTime, MeetInstance>> ReadMeetInstances(IDictionary<int, Meet> meets, IDictionary<int, Venue> venues, IDictionary<int, Team> teams)
        {
            IDictionary<int, IDictionary<DateTime, MeetInstance>> lookup = new Dictionary<int, IDictionary<DateTime, MeetInstance>>();
            Command.CommandText = "SELECT meet_id, date, venue_id FROM meet_instances";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    Meet meet = meets[Convert.ToInt32(Reader["meet_id"])];
                    DateTime date = (DateTime)Reader["date"];
                    Venue venue = venues[Convert.ToInt32(Reader["venue_id"])];
                    MeetInstance instance = MeetInstance.NewEntity(meet, date, venue);
                    if(!lookup.ContainsKey(meet.ID)) {
                        lookup[meet.ID] = new Dictionary<DateTime, MeetInstance>();
                    }
                    lookup[meet.ID][date] = instance;
                }
            }
            Command.CommandText = "SELECT meet_id, date, team_id FROM meet_instance_hosts";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int meetID = Convert.ToInt32(Reader["meet_id"]);
                    DateTime date = (DateTime)Reader["date"];
                    Team team = teams[Convert.ToInt32(Reader["team_id"])];
                    lookup[meetID][date].Host = team;
                }
            }
            return lookup;
        }

        public override IDictionary<int, Meet> ReadMeets(IDictionary<int, Team> teamIDs)
        {
            IDictionary<int, Meet> meets = new Dictionary<int, Meet>();
            Command.CommandText = "SELECT meet_id, name FROM meets";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int id = Convert.ToInt32(Reader["meet_id"]);
                    string meetName = (string)Reader["name"];
                    Meet meet = Meet.NewEntity(id, meetName);
                    meets.Add(id, meet);
                }
            }
            Command.CommandText = "SELECT meet_id, team_id FROM meet_hosts";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int id = Convert.ToInt32(Reader["meet_id"]);
                    int teamID = Convert.ToInt32(Reader["team_id"]);
                    meets[id].Host = teamIDs[teamID];
                }
            }
            return meets;
        }

        public override IDictionary<int, IDictionary<int, Performance>> ReadPerformances(IDictionary<int, Race> races, IDictionary<int, Runner> runners)
        {
            IDictionary<int, IDictionary<int, Performance>> performances = new Dictionary<int, IDictionary<int, Performance>>();
            int raceID, runnerID;
            double time;
            Command.CommandText = "SELECT race_id, runner_id, time FROM results";
            Reader = Command.ExecuteReader();
            while(Reader.Read()) {
                runnerID = Convert.ToInt32(Reader["runner_id"]);
                raceID = Convert.ToInt32(Reader["race_id"]);
                time = (double)Reader["time"];
                if(!performances.ContainsKey(runnerID)) {
                    performances.Add(runnerID, new Dictionary<int, Performance>());
                }
                performances[runnerID][raceID] = new Performance(runners[runnerID], races[raceID], time);
            }
            Reader.Close();
            return performances;
        }

        public override IDictionary<int, Race> ReadRaces(IDictionary<int, IDictionary<DateTime, MeetInstance>> meetInstances)
        {
            IDictionary<int, Race> races = new Dictionary<int, Race>();
            Command.CommandText = "SELECT race_id, distance, gender, meet_id, date FROM races";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int id = Convert.ToInt32(Reader["race_id"]);
                    int distance = Convert.ToInt32(Reader["distance"]);
                    int meetID = Convert.ToInt32(Reader["meet_id"]);
                    DateTime date = (DateTime)Reader["date"];
                    MeetInstance meetInstance = meetInstances[meetID][date];
                    Gender gender = Gender.FromString(Reader["gender"].ToString());
                    Race race = Race.NewEntity(id, meetInstance, gender, distance);
                    races.Add(id, race);
                }
            }
            return races;
        }

        public override IDictionary<int, Runner> ReadRunners()
        {
            IDictionary<int, Runner> runners = new Dictionary<int, Runner>();
            Gender gender;
            int id;
            string givenName, surname;
            Command.CommandText = "SELECT runner_id, surname, given_name, gender FROM runners";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    id = Convert.ToInt32(Reader["runner_id"]);
                    gender = Gender.FromString(Reader["gender"].ToString());
                    givenName = (string)Reader["given_name"];
                    surname = (string)Reader["surname"];
                    runners[id] = Runner.NewEntity(id, surname, givenName, gender);
                }
            }
            Command.CommandText = "SELECT runner_id, enrollment_year FROM college_enrollment_years";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    id = Convert.ToInt32(Reader["runner_id"]);
                    runners[id].EnrollmentYear = Convert.ToInt32(Reader["enrollment_year"]);
                }
            }
            return runners;
        }

        public override IDictionary<string, State> ReadStates()
        {
            IDictionary<string, State> states = new XDictionary<string, State>();
            Command.CommandText = "SELECT state_code, name FROM states";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    string code = Reader["state_code"].ToString();
                    string name = Reader["name"].ToString();
                    states.Add(code, State.NewEntity(code, name));
                }
            }
            return states;
        }

        public override IDictionary<int, Team> ReadTeams(IDictionary<int, Conference> conferences)
        {
            IDictionary<int, Team> teams = new Dictionary<int, Team>();
            Conference conference;
            int id;
            string name;
            Command.CommandText = "SELECT team_id, name FROM teams";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    id = Convert.ToInt32(Reader["team_id"]);
                    name = (string)Reader["name"];
                    teams.Add(id, Team.NewEntity(id, name));
                }
            }
            Command.CommandText = "SELECT team_id, conference_id FROM conference_affiliations";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    id = Convert.ToInt32(Reader["team_id"]);
                    conference = conferences[Convert.ToInt32(Reader["conference_id"])];
                    teams[id].Conference = conference;
                }
            }
            Command.CommandText = "SELECT team_id, nickname FROM team_nicknames";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    id = Convert.ToInt32(Reader["team_id"]);
                    name = Reader["nickname"].ToString();
                    teams[id].AddNickname(name);
                }
            }
            return teams;
        }

        public override IDictionary<int, Venue> ReadVenues(IDictionary<int, City> cities)
        {
            IDictionary<int, Venue> venues = new Dictionary<int, Venue>();
            Command.CommandText = "SELECT venue_id, name, city_id FROM venues";
            using(Reader = Command.ExecuteReader()) {
                while(Reader.Read()) {
                    int id = Convert.ToInt32(Reader["venue_id"]);
                    string name = Reader["name"].ToString();
                    int cityID = Convert.ToInt32(Reader["city_id"]);
                    venues.Add(id, Venue.NewEntity(id, name, cities[cityID]));
                }
            }
            return venues;
        }
        
        #endregion
    }
}
