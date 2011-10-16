using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NHibernate;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestRaceRepository : TestRepository<Race>
    {
        #region Properties

        public override IEnumerable<Race> TestData
        {
            get { return Data.Races.Values; }
        }

        protected override IPersistentCollection<Race> Collection
        {
            get { return Container.Races; }
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Container.Conferences.QueueInserts(Data.Conferences);
            Container.Teams.QueueInserts(Data.Teams);
            Container.Meets.QueueInserts(Data.Meets);
            Container.States.QueueInserts(Data.States);
            Container.Cities.QueueInserts(Data.Cities);
            Container.Venues.QueueInserts(Data.Venues);
            Container.MeetInstances.QueueInserts(Data.MeetInstances);
            Container.SaveChanges();
        }

        #endregion

        #region Tests

        [Test]
        public void Add()
        {
            base.TestAdd();
        }

        [Test]
        public void Contains()
        {
            base.TestContains();
        }

        [Test]
        public void Count()
        {
            base.TestCount();
        }

        [Test]
        public void Remove()
        {
            base.TestRemove();
        }

        [Test]
        public void Update()
        {
            Random random = new Random();
            foreach(Race race in TestData)
            {
                Collection.QueueInsert(race);
                Container.SaveChanges();
                Assert.That(Collection.IsPersisted(race));
            }
            foreach(Race race in TestData)
            {
                int expected = random.Next();
                race.Distance = expected;
                Collection.QueueUpdate(race);
                Container.SaveChanges();
                Race actual = Session.Get<Race>(race.Id);
                Assert.AreEqual(expected, actual.Distance);
            }
        }
        
        #endregion
    }
}

