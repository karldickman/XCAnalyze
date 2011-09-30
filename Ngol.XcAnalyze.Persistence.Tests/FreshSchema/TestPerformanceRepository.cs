using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NHibernate.Linq;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestPerformanceRepository : TestRepository<Performance>
    {
        #region Properties

        protected override IPersistentCollection<Performance> Collection
        {
            get { return Container.Performances; }
        }

        public override IEnumerable<Performance> TestData
        {
            get { return Data.Performances; }
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
            Container.Races.QueueInserts(Data.Races.Values);
            Container.Runners.QueueInserts(Data.Runners);
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
            foreach(Performance performance in TestData)
            {
                Collection.QueueInsert(performance);
                Container.SaveChanges();
                Assert.That(Collection.IsPersisted(performance));
            }
            Random random = new Random();
            foreach(Performance performance in TestData)
            {
                double expected = random.NextDouble();
                performance.Time = expected;
                Collection.QueueUpdate(performance);
                Performance actual = Session.Query<Performance>()
                                            .Single(p => p.Runner == performance.Runner
                                                && p.Race == performance.Race);
                Assert.That(actual.Time.HasValue);
                Assert.AreEqual(expected, actual.Time.Value, 0.001);
            }
        }
        
        #endregion
    }
}