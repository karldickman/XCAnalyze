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
    public class XcData
    {
        private IXList<Affiliation> _affiliations;
        
        private ISet<string> _conferences;
        
        private ISet<string> _meetNames;
        
        private IXList<Meet> _meets;
        
        private IXList<Performance> _performances;
        
        private IXList<Race> _races;
        
        private IXList<Runner> _runners;
        
        private IXList<School> _schools;
        
        private ISet<Venue> _venues;
        
        /// <summary>
        /// Create a new description of the model.
        /// </summary>
        /// <param name="affiliations">
        /// The <see cref="IList<Affiliation>"/>.
        /// </param>
        /// <param name="meets">
        /// A <see cref="IList<Meet>"/> of all the meets that have occurred.
        /// </param>
        /// <param name="performances">
        /// A <see cref="IList<Performance>"/> of all performances.
        /// </param>
        /// <param name="runners">
        /// A <see cref="IList<Runner>"/> of all runners.
        /// </param>
        /// <param name="schools">
        /// A <see cref="IList<School>"/> of all schools.
        /// </param>
        /// <param name="venues">
        /// A <see cref="IList<Venue>"/> of all venues where meets have been
        /// held.
        /// </param>
        public XcData (IXList<Affiliation> affiliations, IXList<Meet> meets,
            IXList<Performance> performances, IXList<Runner> runners,
            IXList<School> schools)
        {
            //Add the affiliations
            _affiliations = new XList<Affiliation> ();
            Add (affiliations);
            //Add the meets            
            _venues = new XHashSet<Venue> ();
            _meetNames = new XHashSet<string> ();
            _races = new XList<Race> ();
            _meets = new XList<Meet> ();
            Add (meets);
            //Add the performances
            _performances = new XList<Performance> ();
            Add (performances);
            _runners = runners;
            //Add the schools
            _conferences = new XHashSet<string> ();
            _schools = new XList<School>();
            Add (schools);
        }
        
        /// <summary>
        /// All the runner-school affiliations.
        /// </summary>
        public IList<Affiliation> Affiliations
        {
            get { return _affiliations.AsReadOnly(); }
        }
        
        /// <summary>
        /// All the athletic conferences.
        /// </summary>
        public IList<string> Conferences
        {
            get { return _conferences.AsReadOnly (); }
        }
        
        /// <summary>
        /// The names of all meets that have occurred.
        /// </summary>
        public IList<string> MeetNames
        {
            get { return _meetNames.AsReadOnly (); }
        }
        
        /// <summary>
        /// All the meets that have occurred.
        /// </summary>
        public IList<Meet> Meets
        {
            get { return _meets.AsReadOnly (); }
        }
        
        /// <summary>
        /// All the performances that have been run.
        /// </summary>
        public IList<Performance> Performances
        {
            get { return _performances.AsReadOnly (); }
        }
        
        /// <summary>
        /// All the races that have occurred.
        /// </summary>
        public IList<Race> Races
        {
            get { return _races.AsReadOnly (); }
        }
        
        /// <summary>
        /// All the runners.
        /// </summary>
        public IList<Runner> Runners
        {
            get { return _runners.AsReadOnly (); }
        }
        
        /// <summary>
        /// All the schools.
        /// </summary>
        public IList<School> Schools
        {
            get { return _schools.AsReadOnly (); }
        }
        
        /// <summary>
        /// All the venues at which races have been run.
        /// </summary>
        public IList<Venue> Venues
        {
            get { return _venues.AsReadOnly (); }
        }
        
        /// <summary>
        /// Add an affiliation.
        /// </summary>
        /// <param name="affiliation">
        /// The <see cref="Affiliation"/> to add.
        /// </param>
        public void Add (Affiliation affiliation)
        {
            _affiliations.Add (affiliation);
            affiliation.Runner.Add (affiliation);
            affiliation.School.Add (affiliation);
        }
        
        /// <summary>
        /// Add a bunch of affiliations.
        /// </summary>
        /// <param name="affiliations">
        /// The <see cref="IEnumerable<Affiliation>"/> to add.
        /// </param>
        public void Add (IEnumerable<Affiliation> affiliations)
        {
            foreach (Affiliation affiliation in affiliations)
            {
                Add (affiliation);
            }
        }
        
        /// <summary>
        /// Add a meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to add.
        /// </param>
        public void Add (Meet meet)
        {
            _meets.Add (meet);
            if (meet.Name != null)
            {
                _meetNames.Add (meet.Name);
            }
            if (meet.Location != null)
            {
                _venues.Add (meet.Location);                
            }
            if (meet.MensRace != null)
            {
                _races.Add (meet.MensRace);
            }
            if (meet.WomensRace != null)
            {
                _races.Add (meet.WomensRace);
            }
        }
        
        /// <summary>
        /// Add a bunch of meets.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IEnumerable<Meet>"/> to add.
        /// </param>
        public void Add (IEnumerable<Meet> meets)
        {
            foreach (Meet meet in meets)
            {
                Add (meet);
            }
        }
        
        /// <summary>
        /// Add a performance.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to add.
        /// </param>
        public void Add (Performance performance)
        {
            _performances.Add (performance);
            performance.Race.Add (performance);
            performance.Runner.Add (performance);
        }
        
        /// <summary>
        /// Add a bunch of performances.
        /// </summary>
        /// <param name="performances">
        /// The <see cref="IEnumerable<Performance>"/> to add.
        /// </param>
        public void Add (IEnumerable<Performance> performances)
        {
            foreach (Performance performance in performances)
            {
                Add (performance);
            }
        }
        
        /// <summary>
        /// Add a runner.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> to add.
        /// </param>
        public void Add (Runner runner)
        {
            _runners.Add (runner);
        }
        
        /// <summary>
        /// Add a bunch of runners.
        /// </summary>
        /// <param name="runners">
        /// The <see cref="IEnumerable<Runner>"/> to add.
        /// </param>
        public void Add (IEnumerable<Runner> runners)
        {
            _runners.AddRange (runners);
        }
        
        /// <summary>
        /// Add a school.
        /// </summary>
        /// <param name="school">
        /// The <see cref="School"/> to add.
        /// </param>
        public void Add (School school)
        {
            _schools.Add (school);
            if (school.Conference != null)
            {
                _conferences.Add (school.Conference);
            }
        }
        
        /// <summary>
        /// Add a bunch of schools.
        /// </summary>
        /// <param name="schools">
        /// The <see cref="IEnumerable<School>"/> to add.
        /// </param>
        public void Add(IEnumerable<School> schools)
        {
            foreach(School school in schools)
            {
                Add(school);
            }
        }
        
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
        public void Affiliate (Runner runner, School school, int year)
        {
            Affiliation affiliation = new Affiliation (runner, school, year);
            Add (affiliation);
        }
        
        /// <summary>
        /// Register a performance.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> at which the performance occurred.
        /// </param>
        /// <param name="runner">
        /// The <see cref="Runner"/> who owns the performance.
        /// </param>
        /// <param name="time">
        /// The <see cref="Time"/> it took to run the race.
        /// </param>
        public void RegisterPerformance (Race race, Runner runner, double time)
        {
            Performance performance = new Performance (runner, race, time);
            Add (performance);
        }
        
        /// <summary>
        /// Remove a meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to remove.
        /// </param>
        public void Remove (Meet meet)
        {
            Remove (meet.MensRace);
            Remove (meet.WomensRace);
            _meets.Remove (meet);
        }
        
        /// <summary>
        /// Remove a meet.
        /// </summary>
        /// <param name="meet">
        /// A <see cref="IEnumerable<Meet>"/>
        /// </param>
        public void Remove (IEnumerable<Meet> meets)
        {
            foreach (Meet meet in meets)
            {
                Remove (meet);
            }
        }
        
        /// <summary>
        /// Remove a race.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> to remove.
        /// </param>
        public void Remove (Race race)
        {
            foreach (Performance performance in Performances)
            {
                if (performance.Race == race) 
                {
                    performance.Runner.Delete (performance);
                }
            }
            _races.Remove (race);
        }

        /// <summary>
        /// Get the team that ran for a particular school in a particular
        /// season.
        /// </summary>
        /// <param name="school">
        /// A <see cref="School"/>
        /// </param>
        /// <param name="year">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <param name="gender">
        /// A <see cref="Gender"/>
        /// </param>
        /// <returns>
        /// A <see cref="IList<Runner>"/> of the teams.
        /// </returns>
        public IList<Runner> Team (School school, int year, Gender gender)
        {
            return new List<Runner> (from affiliation in Affiliations
                where (affiliation.School == school
                    && affiliation.Year == year
                    && affiliation.Runner.Gender == gender)
                select affiliation.Runner);
        }       
    }
}
