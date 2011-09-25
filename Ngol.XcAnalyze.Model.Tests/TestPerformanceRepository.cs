using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model.Collections;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate.Linq;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestPerformanceRepository : TestRepository<Performance>
    {
        #region Properties

        public override IEnumerable<Performance> TestData
        {
            get { return SampleData.Performances; }
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

        protected IRepository<MeetInstance> MeetInstanceRepository
        {
            get;
            set;
        }

        protected IRepository<Race> RaceRepository
        {
            get;
            set;
        }

        protected IRepository<Runner> RunnerRepository
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
            MeetInstanceRepository = new Repository<MeetInstance>(Session);
            MeetInstanceRepository.AddRange(SampleData.MeetInstances);
            RaceRepository = new Repository<Race>(Session);
            RaceRepository.AddRange(SampleData.Races.Values);
            RunnerRepository = new Repository<Runner>(Session);
            RunnerRepository.AddRange(SampleData.Runners);
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
            MeetInstanceRepository.SafeDispose();
            RaceRepository.SafeDispose();
            RunnerRepository.SafeDispose();
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
            foreach(Performance performance in TestData)
            {
                Repository.Add(performance);
                Assert.Contains(performance, Repository);
            }
            Random random = new Random();
            foreach(Performance performance in TestData)
            {
                double expected = random.NextDouble();
                performance.Time = expected;
                Repository.Update(performance);
                Performance actual = Session.Query<Performance>()
                                            .Single(p => p.RunnerID == performance.RunnerID
                                                && p.RaceID == performance.RaceID);
                Assert.IsTrue(actual.Time.HasValue);
                Assert.AreEqual(expected, actual.Time.Value, 0.001);
            }
        }
        
        #endregion
    }
}

