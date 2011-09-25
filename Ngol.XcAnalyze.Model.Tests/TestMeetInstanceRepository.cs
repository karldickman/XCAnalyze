using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Model.Collections;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate.Linq;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestMeetInstanceRepository : TestRepository<MeetInstance>
    {
        #region Properties

        public override IEnumerable<MeetInstance> TestData
        {
            get { return SampleData.MeetInstances; }
        }

        protected IRepository<City> CityRepository
        {
            get;
            set;
        }

        protected IRepository<Conference> ConferenceRepository
        {
            get;
            set;
        }

        protected IRepository<Meet> MeetRepository
        {
            get;
            set;
        }

        protected IRepository<State> StateRepository
        {
            get;
            set;
        }

        protected IRepository<Team> TeamRepository
        {
            get;
            set;
        }

        protected IRepository<Venue> VenueRepository
        {
            get;
            set;
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            ConferenceRepository = new Repository<Conference>(Session);
            ConferenceRepository.AddRange(SampleData.Conferences);
            TeamRepository = new Repository<Team>(Session);
            TeamRepository.AddRange(SampleData.Teams);
            MeetRepository = new Repository<Meet>(Session);
            MeetRepository.AddRange(SampleData.Meets);
            StateRepository = new Repository<State>(Session);
            StateRepository.AddRange(SampleData.States);
            CityRepository = new Repository<City>(Session);
            CityRepository.AddRange(SampleData.Cities);
            VenueRepository = new Repository<Venue>(Session);
            VenueRepository.AddRange(SampleData.Venues);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            ConferenceRepository.SafeDispose();
            TeamRepository.SafeDispose();
            MeetRepository.SafeDispose();
            StateRepository.SafeDispose();
            CityRepository.SafeDispose();
            VenueRepository.SafeDispose();
        }

        #endregion

        #region Tests

        [Test]
        public void Add()
        {
            base.TestAdd();
        }

        [Test]
        public void Clear()
        {
            base.TestClear();
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
            MeetInstance originalMeetInstance = SampleData.LCInvite09.Clone<MeetInstance>();
            Repository.Add(originalMeetInstance);
            Assert.Contains(originalMeetInstance, Repository);
            foreach(Venue venue in SampleData.MeetInstances.Select(meet => meet.Venue))
            {
                originalMeetInstance.Venue = venue;
                Repository.Update(originalMeetInstance);
                IEnumerable<MeetInstance> matches = Session.Query<MeetInstance>()
                                                           .Where(m => m.MeetID == originalMeetInstance.MeetID
                                                                && m.Date == originalMeetInstance.Date);
                Assert.HasCount(1, matches);
                MeetInstance actual = matches.First();
                Assert.AreEqual(venue, actual.Venue);
            }
        }

        #endregion
    }
}

