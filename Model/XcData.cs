using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XCAnalyze.Model
{
    /// <summary>
    /// All of the information currently being modelled.
    /// </summary>
    public class XcData
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
        /// The names of all meets that have occurred.
        /// </summary>
        virtual public IList<string> MeetNames { get; protected internal set; }
        
        /// <summary>
        /// All the meets that have occurred.
        /// </summary>
        virtual public IList<Meet> Meets { get; protected internal set; }
        
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
        virtual public IList<Venue> Venues { get; protected internal set; }
                
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
        public XcData (IList<Affiliation> affiliations, IList<Meet> meets,
            IList<Performance> performances, IList<Runner> runners,
            IList<School> schools, IList<Venue> venues)
        {
            Affiliations = affiliations;
            Conferences = ConferencesList (schools);
            MeetNames = MeetNamesList (meets);
            Meets = meets;
            Performances = performances;
            Runners = runners;
            Schools = schools;
            Venues = venues;
            Races = new List<Race> ();
            foreach (Meet meet in meets)
            {
                if (meet.MensRace != null) 
                {
                    Races.Add (meet.MensRace);
                }
                if (meet.WomensRace != null) 
                {
                    Races.Add (meet.WomensRace);
                }
            }
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
        /// Get the of conferences a particular group of schools is
        /// affiliated with.
        /// </summary>
        /// <param name="schools">
        /// The <see cref="IList<School>"/> to consider.
        /// </param>
        /// <returns>
        /// The conferences that were found.
        /// </returns>
        public static IList<string> ConferencesList (IList<School> schools)
        {
            return new List<string> ((from school in schools
                where school.Conference != null
                select school.Conference).Distinct ());
        }
        
        /// <summary>
        /// Get the names of all of a particular group of meets.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IList<Meet>"/> to consider.
        /// </param>
        /// <returns>
        /// The meet names that were found.
        /// </returns>
        public static IList<string> MeetNamesList (IList<Meet> meets)
        {
            return new List<string> ((from meet in meets
                    where meet.Name != null
                    select meet.Name).Distinct());
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
    
    [TestFixture]
    public class TestXcData
    {
        [Test]
        public void TestConferencesList ()
        {
            string[] conferences = new string[] { "NWC", "SCIAC", "SCAC" };
            IList<School> schools = new List<School> ();
            schools.Add (new School ("Lewis & Clark", "College", conferences[0]));
            schools.Add (new School ("Linfield", "College", conferences[0]));
            schools.Add (new School ("Whitman", "College", conferences[0]));
            schools.Add (new School ("Willamette", "University", conferences[0]));
            schools.Add (new School ("Pomona-Pizer", "College", conferences[1]));
            schools.Add (new School ("Claremont-Mudd-Scripps", "College", conferences[2]));
            schools.Add (new School ("Colorado", "College", conferences[2]));
            schools.Add (new School ("Chapman", "University", null));
            IList<string> actual = XcData.ConferencesList (schools);
            Assert.AreEqual (conferences.Length, actual.Count);
            foreach (string conference in conferences)
            {
                Assert.That (actual.Contains (conference));
            }
        }
        
        [Test]
        public void TestMeetNamesList ()
        {
            string[] meetNames = new string[] { "LC Invite", "Chuck Bowles" };
            IList<Meet> meets = new List<Meet> ();
            meets.Add (new Meet (meetNames[0], new Date (2006, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[0], new Date (2007, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[0], new Date (2008, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[0], new Date (2009, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[0], new Date (2010, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[1], new Date (2006, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[1], new Date (2007, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[1], new Date (2008, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[1], new Date (2009, 9, 5), null, null, null));
            meets.Add (new Meet (meetNames[1], new Date (2010, 9, 5), null, null, null));
            meets.Add (new Meet (null, new Date (2012, 9, 27), null, null, null));
            IList<string> actual = XcData.MeetNamesList (meets);
            Assert.AreEqual (meetNames.Length, actual.Count);
            foreach (string meetName in meetNames)
            {
                Assert.That (actual.Contains (meetName));
            }
        }
    }
}
