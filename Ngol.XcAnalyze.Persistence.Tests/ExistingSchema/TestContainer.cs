using System;
using System.Linq;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.ExistingSchema
{
    [TestFixture]
    public class TestContainer
    {
        #region Properties

        protected PersistenceContainer Container
        {
            get;
            set;
        }

        #endregion

        #region Set up

        [SetUp]
        public void SetUp()
        {
            Container = new PersistenceContainer("mysql-hibernate.cfg.xml");
        }

        [TearDown]
        public void TearDown()
        {
            Container.SafeDispose();
        }

        #endregion

        #region Tests

        [Test]
        public void Cities()
        {
            MoreAssert.IsNotEmpty(Container.Cities);
            foreach(City city in Container.Cities)
            {
                Console.WriteLine(city);
            }
        }

        [Test]
        public void Conferences()
        {
            MoreAssert.IsNotEmpty(Container.Conferences);
            foreach(Conference conference in Container.Conferences)
            {
                Console.WriteLine(conference);
            }
        }

        [Test]
        public void MeetInstances()
        {
            MoreAssert.IsNotEmpty(Container.MeetInstances);
            foreach(MeetInstance meetInstance in Container.MeetInstances)
            {
                Assert.IsNotNull(meetInstance.Venue);
                Console.WriteLine(meetInstance);
            }
        }

        [Test]
        public void Meets()
        {
            MoreAssert.IsNotEmpty(Container.Meets);
            foreach(Meet meet in Container.Meets)
            {
                Console.WriteLine(meet);
            }
        }

        [Test]
        public void Performances()
        {
            MoreAssert.IsNotEmpty(Container.Performances);
            foreach(Performance performance in Container.Performances)
            {
                Console.WriteLine(performance);
            }
        }

        [Test]
        public void Races()
        {
            MoreAssert.IsNotEmpty(Container.Races);
            foreach(Race race in Container.Races)
            {
                Console.WriteLine(race);
            }
        }

        [Test]
        public void Runners()
        {
            MoreAssert.IsNotEmpty(Container.Runners);
            foreach(Runner runner in Container.Runners)
            {
                Console.WriteLine(runner);
            }
            Runner karl = Container.Runners.Single(r => r.Surname == "Dickman");
            MoreAssert.HasCount(4, karl.Affiliations.Values);
            MoreAssert.IsNotEmpty(karl.Performances.Values);
        }

        [Test]
        public void States()
        {
            MoreAssert.IsNotEmpty(Container.States);
            foreach(State state in Container.States)
            {
                Console.WriteLine(state);
            }
        }

        [Test]
        public void Teams()
        {
            MoreAssert.IsNotEmpty(Container.Teams);
            foreach(Team team in Container.Teams)
            {
                Console.WriteLine(team);
            }
        }

        [Test]
        public void Venues()
        {
            MoreAssert.IsNotEmpty(Container.Venues);
            foreach(Venue venue in Container.Venues)
            {
                Console.WriteLine(venue);
            }
        }
        
        #endregion
    }
}

