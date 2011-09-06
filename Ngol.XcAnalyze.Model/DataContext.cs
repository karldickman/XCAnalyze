using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    /// <summary>
    /// All of the information currently being modelled.
    /// </summary>
    public class DataContext
    {
        #region Properties

        #region Fields

        private IXList<Affiliation> _affiliations;

        private IXList<City> _cities;

        private IXList<Conference> _conferences;

        private IXList<Meet> _meets;

        private IXList<MeetInstance> _meetInstances;

        private IXList<Performance> _performances;

        private IXList<Race> _races;

        private IXList<Runner> _runners;

        private IXList<State> _states;

        private IXList<Team> _teams;

        private IXList<Venue> _venues;

        #endregion

        public IList<Affiliation> Affiliations {
            get { return _affiliations.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<Affiliation>();
                }
                _affiliations = new XList<Affiliation>(value);
            }
        }

        /// <summary>
        /// All the athletic conferences.
        /// </summary>
        public IList<Conference> Conferences {
            get { return _conferences.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<Conference>();
                }
                _conferences = new XList<Conference>(value);
            }
        }

        public IList<City> Cities {
            get { return _cities.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<City>();
                }
                _cities = new XList<City>(value);
            }
        }

        /// <summary>
        /// All meets that have been held.
        /// </summary>
        public IList<Meet> Meets {
            get { return _meets.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<Meet>();
                }
                _meets = new XList<Meet>(value);
            }
        }

        /// <summary>
        /// All the instances of all the meets that have occurred.
        /// </summary>
        public IList<MeetInstance> MeetInstances {
            get { return _meetInstances.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new XList<MeetInstance>();
                }
                _meetInstances = new XList<MeetInstance>(value);
            }
        }

        /// <summary>
        /// All the performances that have been run.
        /// </summary>
        public IList<Performance> Performances {
            get { return _performances.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new XList<Performance>();
                }
                _performances = new XList<Performance>(value);
            }
        }

        /// <summary>
        /// All the races that have occurred.
        /// </summary>
        public IList<Race> Races {
            get { return _races.AsReadOnly(); }

            set {
                if(_races == null) {
                    value = new XList<Race>();
                }
                _races = new XList<Race>(value);
            }
        }

        /// <summary>
        /// All the runners.
        /// </summary>
        public IList<Runner> Runners {
            get { return _runners.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<Runner>();
                }
                _runners = new XList<Runner>(value);
            }
        }

        public IList<State> States {
            get { return _states.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new List<State>();
                }
                _states = new XList<State>(value);
            }
        }

        /// <summary>
        /// All the teams.
        /// </summary>
        public IList<Team> Teams {
            get { return _teams.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new XList<Team>();
                }
                _teams = new XList<Team>(value);
            }
        }

        /// <summary>
        /// All the venues at which races have been run.
        /// </summary>
        public IList<Venue> Venues {
            get { return _venues.AsReadOnly(); }

            protected set {
                if(value == null) {
                    value = new XList<Venue>();
                }
                _venues = new XList<Venue>(value);
            }
        }

        #endregion

        #region Constructors

        protected DataContext()
        {
            //Set default values for fields
            _affiliations = new XList<Affiliation>();
            _cities = new XList<City>();
            _conferences = new XList<Conference>();
            _meets = new XList<Meet>();
            _meetInstances = new XList<MeetInstance>();
            _performances = new XList<Performance>();
            _races = new XList<Race>();
            _runners = new XList<Runner>();
            _states = new XList<State>();
            _teams = new XList<Team>();
            _venues = new XList<Venue>();
        }

        /// <summary>
        /// Create a new description of the model.
        /// </summary>
        /// <param name="affiliations">
        /// A <see cref="IEnumerable<Affiliation>"/> of all runner-team
        /// affiliations
        /// to record.
        /// </param>
        /// <param name="conferences">
        /// A <see cref="IEnumerable<Conference>"/> of all conferences.
        /// </param>
        /// <param name="meets">
        /// A <see cref="IEnumerable<Meet>"/> of all the meets that have
        /// occurred.
        /// </param>
        /// <param name="meetInstances>
        /// A <see cref="IEnumerable<MeetInstance>"/> of all the mee intsances
        /// that have occurred.
        /// </param>
        /// <param name="performances">
        /// A <see cref="IEnumerable<Performance>"/> of all performances.
        /// </param>
        /// <param name="races">
        /// A <see cref="IEnumerable<Race>"/> of all races that have been run.
        /// </param>
        /// <param name="runners">
        /// A <see cref="IEnumerable<Runner>"/> of all runners.
        /// </param>
        /// <param name="schools">
        /// A <see cref="IEnumerable<Team>"/> of all teams.
        /// </param>
        /// <param name="venues">
        /// A <see cref="IEnumerable<Venue>"/> of all venues where meets have
        /// been held.
        /// </param>
        public DataContext(IEnumerable<Affiliation> affiliations, IEnumerable<City> cities, IEnumerable<Conference> conferences, IEnumerable<MeetInstance> meetInstances, IEnumerable<Meet> meets, IEnumerable<Performance> performances, IEnumerable<Race> races, IEnumerable<Runner> runners, IEnumerable<State> states, IEnumerable<Team> teams,
        IEnumerable<Venue> venues) : this()
        {
            AddCities(cities);
            AddConferences(conferences);
            AddVenues(venues);
            AddMeets(meets);
            AddMeetInstances(meetInstances);
            AddRaces(races);
            AddTeams(teams);
            AddRunners(runners);
            AddStates(states);
            AddPerformances(performances);
            AddAffiliations(affiliations);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Affiliate a runner with a school.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who ran for the school.
        /// </param>
        /// <param name="school">
        /// The <see cref="School"/> the runner ran for.
        /// </param>
        /// <param name="year">
        /// The season in which the runner ran for thes chool.
        /// </param>
        protected void AddAffiliation(Affiliation affiliation)
        {
            _affiliations.Add(affiliation);
            affiliation.Runner.AddAffiliation(affiliation.Season, affiliation.Team);
            affiliation.Team.AddRunner(affiliation);
        }

        /// <summary>
        /// Add new affiliations.
        /// </summary>
        /// <param name="affiliations">
        /// The <see cref="IEnumerable<Affiliation>"/> to add.
        /// </param>
        protected void AddAffiliations(IEnumerable<Affiliation> affiliations)
        {
            foreach(Affiliation affiliation in affiliations) {
                AddAffiliation(affiliation);
            }
        }

        /// <summary>
        /// Add more cities.
        /// </summary>
        /// <param name="cities">
        /// A <see cref="IEnumerable<City>"/> of cities to add.
        /// </param>
        protected void AddCities(IEnumerable<City> cities)
        {
            _cities.AddRange(cities);
        }

        /// <summary>
        /// Add another city.
        /// </summary>
        /// <param name="city">
        /// The <see cref="City"/> to add.
        /// </param>
        protected void AddCity(City city)
        {
            _cities.Add(city);
        }

        /// <summary>
        /// Add a new conference.
        /// </summary>
        /// <param name="conference">
        /// The new conference to add.
        /// </param>
        protected void AddConference(Conference conference)
        {
            _conferences.Add(conference);
        }

        /// <summary>
        /// Add new conferences.
        /// </summary>
        /// <param name="conferences">
        /// A <see cref="IEnumerable<System.String>"/> of conferences to add.
        /// </param>
        protected void AddConferences(IEnumerable<Conference> conferences)
        {
            _conferences.AddRange(conferences);
        }

        /// <summary>
        /// Add a new meet instance.
        /// </summary>
        /// <param name="meetInstance">
        /// The <see cref="MeetInstance"/> to add.
        /// </param>
        protected void AddMeetInstance(MeetInstance meetInstance)
        {
            _meetInstances.Add(meetInstance);
        }

        /// <summary>
        /// Add new meet instances.
        /// </summary>
        /// <param name="meetInstances">
        /// A <see cref="IEnumerable<MeetInstance>"/> to add.
        /// </param>
        protected void AddMeetInstances(IEnumerable<MeetInstance> meetInstances)
        {
            _meetInstances.AddRange(meetInstances);
        }

        /// <summary>
        /// Add a new meet.
        /// </summary>
        /// <param name="meet">
        /// The new <see cref="Meet"/> to add.
        /// </param>
        protected void AddMeet(Meet meet)
        {
            _meets.Add(meet);
        }

        /// <summary>
        /// Add new meets.
        /// </summary>
        /// <param name="meets">
        /// A <see cref="IEnumerable<Meet>"/> of meets to add.
        /// </param>
        protected void AddMeets(IEnumerable<Meet> meets)
        {
            _meets.AddRange(meets);
        }

        /// <summary>
        /// Add a new performance.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to add.
        /// </param>
        protected void AddPerformance(Performance performance)
        {
            _performances.Add(performance);
            performance.Runner.AddPerformance(performance);
            performance.Race.AddResult(performance);
        }

        /// <summary>
        /// Add new performances.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="IEnumerable<Performance>"/> to add.
        /// </param>
        protected void AddPerformances(IEnumerable<Performance> performances)
        {
            foreach(Performance performance in performances) {
                AddPerformance(performance);
            }
        }

        /// <summary>
        /// Add new races.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> to add.
        /// </param>
        protected void AddRace(Race race)
        {
            _races.Add(race);
        }

        /// <summary>
        /// The races to add.
        /// </summary>
        /// <param name="races">
        /// A <see cref="IEnumerable<Race>"/> to add.
        /// </param>
        protected void AddRaces(IEnumerable<Race> races)
        {
            _races.AddRange(races);
        }

        /// <summary>
        /// Add a new runner.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> to add.
        /// </param>
        protected void AddRunner(Runner runner)
        {
            _runners.Add(runner);
        }

        /// <summary>
        /// Add new runners.
        /// </summary>
        /// <param name="runners">
        /// The <see cref="IEnumerable<Runner>"/> to add.
        /// </param>
        protected void AddRunners(IEnumerable<Runner> runners)
        {
            _runners.AddRange(runners);
        }

        /// <summary>
        /// Add another state.
        /// </summary>
        /// <param name="state">
        /// The <see cref="State"/> to add.
        /// </param>
        protected void AddState(State state)
        {
            _states.Add(state);
        }

        /// <summary>
        /// Add more states.
        /// </summary>
        /// <param name="states">
        /// A <see cref="IEnumerable<State>"/> of states to add.
        /// </param>
        protected void AddStates(IEnumerable<State> states)
        {
            _states.AddRange(states);
        }

        /// <summary>
        /// Add a new team.
        /// </summary>
        /// <param name="team">
        /// The <see cref="Team"/> to add.
        /// </param>
        protected void AddTeam(Team team)
        {
            _teams.Add(team);
        }

        /// <summary>
        /// Add new teams.
        /// </summary>
        /// <param name="team">
        /// The <see cref="IEnumerable<Team>"/> to add.
        /// </param>
        protected void AddTeams(IEnumerable<Team> team)
        {
            _teams.AddRange(team);
        }

        /// <summary>
        /// Add a new venue.
        /// </summary>
        /// <param name="venue">
        /// The new <see cref="Venue"/> to add.
        /// </param>
        protected void AddVenue(Venue venue)
        {
            _venues.Add(venue);
        }

        /// <summary>
        /// Add new venues.
        /// </summary>
        /// <param name="venue">
        /// A <see cref="IEnumerable<Venue>"/> of venues to add.
        /// </param>
        protected void AddVenues(IEnumerable<Venue> venue)
        {
            _venues.AddRange(venue);
        }

        public void DetachAll()
        {
            foreach(Affiliation item in Affiliations) {
                item.IsAttached = false;
            }
            foreach(City item in Cities) {
                item.IsAttached = false;
            }
            foreach(Conference item in Conferences) {
                item.IsAttached = false;
            }
            foreach(Meet item in Meets) {
                item.IsAttached = false;
            }
            foreach(MeetInstance item in MeetInstances) {
                item.IsAttached = false;
            }
            foreach(Performance item in Performances) {
                item.IsAttached = false;
            }
            foreach(Race item in Races) {
                item.IsAttached = false;
            }
            foreach(Runner item in Runners) {
                item.IsAttached = false;
            }
            foreach(State item in States) {
                item.IsAttached = false;
            }
            foreach(Team item in Teams) {
                item.IsAttached = false;
            }
            foreach(Venue item in Venues) {
                item.IsAttached = false;
            }
        }
    }
    
    #endregion
}
