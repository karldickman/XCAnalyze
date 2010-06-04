using System;
using System.Collections.Generic;
using System.Linq;

namespace XCAnalyze.Model
{
    public class GlobalState
    {
        /// <summary>
        /// All the runner-school affiliations.
        /// </summary>
        virtual public IList<Affiliation> Affiliations { get; protected internal set; }
        
        /// <summary>
        /// All the athletic conferences.
        /// </summary>
        virtual public IList<string> Conferences { get; protected internal set; }
        
        /// <summary>
        /// All the meets that have occurred.
        /// </summary>
        virtual public IList<string> Meets { get; protected internal set; }
        
        /// <summary>
        /// All the performances that have been run.
        /// </summary>
        virtual public IList<Performance> Performances { get; protected internal set; }
        
        /// <summary>
        /// All the races that have occurred.
        /// </summary>
        virtual public IList<Race> Races { get; protected internal set; }
        
        /// <summary>
        /// All the runners.
        /// </summary>
        virtual public IList<Runner> Runners { get; protected internal set; }
        
        /// <summary>
        /// All the schools.
        /// </summary>
        virtual public IList<School> Schools { get; protected internal set; }
        
        /// <summary>
        /// All the venues at which races have been run.
        /// </summary>
        virtual public IList<string[]> Venues { get; protected internal set; }
        
        protected internal GlobalState(IList<Affiliation> affiliations,
            IList<Performance> performances, IList<Race> races,
            IList<Runner> runners, IList<School> schools)
            : this(affiliations, null, null, performances, races, runners,
            schools, null) {}

        /// <summary>
        /// Create a new description of the model.
        /// </summary>
        /// <param name="affiliations">
        /// The <see cref="IList<Affiliation>"/>.
        /// </param>
        /// <param name="conferences">
        /// The athletic conferences.
        /// </param>
        /// <param name="meets">
        /// All the meets that have occurred.
        /// </param>
        /// <param name="performances">
        /// The <see cref="IList<Performance>"/>.
        /// </param>
        /// <param name="races">
        /// All the <see cref="IList<Race>"/> that have been held.
        /// </param>
        /// <param name="runners">
        /// All the <see cref="IList<Runner>"/>.
        /// </param>
        /// <param name="schools">
        /// All the <see cref="IList<School>"/> schools.
        /// </param>
        /// <param name="venues">
        /// All he venues where meets are held.
        /// </param>
        public GlobalState (IList<Affiliation> affiliations,
            IList<string> conferences, IList<string> meets,
            IList<Performance> performances, IList<Race> races,
            IList<Runner> runners, IList<School> schools,
            IList<string[]> venues)
        {
            Affiliations = affiliations;
            Conferences = conferences;
            Meets = meets;
            Performances = performances;
            Races = races;
            Runners = runners;
            Schools = schools;
            foreach (Affiliation affiliation in affiliations)
            {
                Affiliate (affiliation);
            }
            foreach (Performance performance in performances)
            {
                RegisterPerformance (performance);
            }
        }
        
        /// <summary>
        /// Register an affiliation.
        /// </summary>
        /// <param name="affiliation">
        /// The <see cref="Affiliation"/> to register.
        /// </param>
        protected void Affiliate (Affiliation affiliation)
        {
            affiliation.Runner.AddSchool (affiliation);
            affiliation.School.AddRunner (affiliation);
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
            Affiliations.Add (affiliation);
            Affiliate (affiliation);
        }
        
        /// <summary>
        /// Register a performance.
        /// </summary>
        /// <param name="performance">
        /// The <see cref="Performance"/> to register.
        /// </param>
        public void RegisterPerformance (Performance performance)
        {
            performance.Race.AddResult (performance);
            performance.Runner.AddPerformance (performance);
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
        public void RegisterPerformance (Race race, Runner runner, Time time)
        {
            Performance performance = new Performance (runner, race, time);
            Performances.Add (performance);
            RegisterPerformance (performance);
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
