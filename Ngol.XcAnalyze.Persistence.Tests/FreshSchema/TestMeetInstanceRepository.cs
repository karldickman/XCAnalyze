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
    public class TestMeetInstanceRepository : TestRepository<MeetInstance>
    {
        #region Properties

        public override IEnumerable<MeetInstance> TestData
        {
            get { return Data.MeetInstances; }
        }

        protected override IPersistentCollection<MeetInstance> Collection
        {
            get { return Container.MeetInstances; }
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
            MeetInstance originalMeetInstance = Data.LCInvite09.Clone<MeetInstance>();
            Collection.QueueInsert(originalMeetInstance);
            Container.SaveChanges();
            Assert.That(Collection.IsPersisted(originalMeetInstance));
            foreach(Venue venue in Data.MeetInstances.Select(meet => meet.Venue))
            {
                originalMeetInstance.Venue = venue;
                Collection.QueueInsert(originalMeetInstance);
                Container.SaveChanges();
                IEnumerable<MeetInstance> matches = Session.Query<MeetInstance>()
                                                           .Where(m => m.Meet == originalMeetInstance.Meet
                                                                && m.Date == originalMeetInstance.Date);
                Assert.HasCount(1, matches);
                MeetInstance actual = matches.First();
                Assert.AreEqual(venue, actual.Venue);
            }
        }

        #endregion
    }
}

