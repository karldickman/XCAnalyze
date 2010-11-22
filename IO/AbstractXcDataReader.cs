using System;
using System.Collections.Generic;
using XCAnalyze.Model;

namespace XCAnalyze.IO
{
    abstract public class AbstractXcDataReader
    {   
        #region Methods
        
        /// <summary>
        /// Read the data out of the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Model.DataContext"/> containing all the data in the
        /// database.
        /// </returns>
        public DataContext Read ()
        {
            IDictionary<string, State> stateIDs = ReadStates ();
            IDictionary<int, City> cityIDs = ReadCities (stateIDs);
            IDictionary<int, Conference> conferenceIDs = ReadConferences ();
            IDictionary<int, Runner> runnerIDs = ReadRunners ();
            IDictionary<int, Team> teamIDs = ReadTeams (conferenceIDs);
            IDictionary<int, IDictionary<int, Affiliation>> affiliationIDs =
                ReadAffiliations (runnerIDs, teamIDs);
            IDictionary<int, Meet> meetIDs = ReadMeets (teamIDs);
            IDictionary<int, Venue> venueIDs = ReadVenues (cityIDs);
            IDictionary<int, IDictionary<DateTime, MeetInstance>> meetInstanceLookup =
                ReadMeetInstances (meetIDs, venueIDs, teamIDs);
            IDictionary<int, Race> raceIDs = ReadRaces (meetInstanceLookup);
            List<Affiliation> affiliations = new List<Affiliation> ();
            foreach (IDictionary<int, Affiliation> runnerEntry in affiliationIDs.Values)
            {
                affiliations.AddRange (runnerEntry.Values);
            }
            IList<City> cities = new List<City> (cityIDs.Values);
            IList<Conference> conferences =
                new List<Conference> (conferenceIDs.Values);
            List<MeetInstance> meetInstances = new List<MeetInstance> ();
            foreach (IDictionary<DateTime, MeetInstance> dateLookup in meetInstanceLookup.Values) 
            {
                meetInstances.AddRange (dateLookup.Values);
            }
            IList<Meet> meets = new List<Meet> (meetIDs.Values);
            List<Performance> performances = new List<Performance> ();
            foreach (IDictionary<int, Performance> runnerEntry in ReadPerformances (raceIDs, runnerIDs).Values) 
            {
                performances.AddRange (runnerEntry.Values);
            }
            IList<Race> races = new List<Race> (raceIDs.Values);
            IList<Runner> runners = new List<Runner> (runnerIDs.Values);
            IList<State> states = new List<State> (stateIDs.Values);
            IList<Team> teams = new List<Team> (teamIDs.Values);
            IList<Venue> venues = new List<Venue> (venueIDs.Values);
            return new DataContext (affiliations, cities, conferences, meetInstances, meets, performances, races, runners, states, teams, venues);
        }
                
        /// <summary>
        /// Read the affiliations.
        /// </summary>
        /// <param name="runners">
        /// A <see cref="IDictionary<System.Int32, Runner>"/> mapping id numbers
        /// to runners.
        /// </param>
        /// <param name="schools">
        /// A <see cref="IDictionary<System.Int32, School>"/> mapping id numbers
        /// to schools.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Affiliation>"/> mapping id
        /// numbers to affiliations.
        /// </returns>
        abstract public IDictionary<int, IDictionary<int, Affiliation>> ReadAffiliations(
            IDictionary<int, Runner> runners, IDictionary<int, Team> schools);
        
        /// <summary>
        /// Read the cities.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, City>"/> mapping id numbers
        /// to cities.
        /// </returns>
        abstract public IDictionary<int, City> ReadCities(
            IDictionary<string, State> states);
        
        /// <summary>
        /// Read the conferences.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping
        /// id numbers to conferences.
        /// </returns>
        abstract public IDictionary<int, Conference> ReadConferences();        
        
        /// <summary>
        /// Read the meets.
        /// </summary>
        /// <param name="meetNames">
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping id
        /// numbers to meet names.
        /// </param>
        /// <param name="races">
        /// A <see cref="IDictionary<System.Int32, Race>"/> mapping id numbers
        /// to races.
        /// </param>
        /// <param name="venues">
        /// A <see cref="IDictionary<System.Int32, Venue>"/> mapping id numbers
        /// to venues.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Meet>"/> mapping id numbers
        /// to meets.
        /// </returns>
        abstract public IDictionary<int, IDictionary<DateTime, MeetInstance>> ReadMeetInstances(
            IDictionary<int, Meet> meets, IDictionary<int, Venue> venues,
            IDictionary<int, Team> teams);
        
        /// <summary>
        /// Read the meets.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping
        /// id numbers to meet names.
        /// </returns>
        abstract public IDictionary<int, Meet> ReadMeets(
            IDictionary<int, Team> teams);
        
        /// <summary>
        /// Read the performances.
        /// </summary>
        /// <param name="races">
        /// A <see cref="IDictionary<System.Int32, Race>"/> mapping id numbers
        /// to races.
        /// </param>
        /// <param name="runners">
        /// A <see cref="IDictionary<System.Int32, Runner>"/> mapping id numbers
        /// to runners.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Performance>"/> mapping id
        /// numbers to performances.
        /// </returns>
        abstract public IDictionary<int, IDictionary<int, Performance>> ReadPerformances(
            IDictionary<int, Race> races, IDictionary<int, Runner> runners);
        
        /// <summary>
        /// Read the races.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Race>"/> mapping id nubmers
        /// to races.
        /// </returns>
        abstract public IDictionary<int, Race> ReadRaces(
            IDictionary<int, IDictionary<DateTime, MeetInstance>> meetInstances);
                
        /// <summary>
        /// Read the runners.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Runner>"/> mapping id numbers
        /// to runners.
        /// </returns>
        abstract public IDictionary<int, Runner> ReadRunners();
        
        /// <summary>
        /// Read the states.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, State>"/> mapping id numbers
        /// to states.
        /// </returns>
        abstract public IDictionary<string, State> ReadStates();
        
        /// <summary>
        /// Read the schools.
        /// </summary>
        /// <param name="conferences">
        /// A <see cref="IDictionary<System.Int32, System.String>"/> mapping id
        /// numbers to conferences.
        /// </param>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, School>"/> mapping id numbers
        /// to schools.
        /// </returns>
        abstract public IDictionary<int, Team> ReadTeams(
            IDictionary<int, Conference> conferences);
                
        /// <summary>
        /// Read the venues.
        /// </summary>
        /// <returns>
        /// A <see cref="IDictionary<System.Int32, Venue>"/> mapping id numbers
        /// to venues.
        /// </returns>
        abstract public IDictionary<int, Venue> ReadVenues(
            IDictionary<int, City> cities);
        
        #endregion
    }
}
