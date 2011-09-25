using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

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
            ConferenceRepository.AddRange(Data.Conferences);
            TeamRepository = new Repository<Team>(Session);
            TeamRepository.AddRange(Data.Teams);
            MeetRepository = new Repository<Meet>(Session);
            MeetRepository.AddRange(Data.Meets);
            StateRepository = new Repository<State>(Session);
            StateRepository.AddRange(Data.States);
            CityRepository = new Repository<City>(Session);
            CityRepository.AddRange(Data.Cities);
            VenueRepository = new Repository<Venue>(Session);
            VenueRepository.AddRange(Data.Venues);
            MeetInstanceRepository = new Repository<MeetInstance>(Session);
            MeetInstanceRepository.AddRange(Data.MeetInstances);
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
            Random random = new Random();
            foreach(Race race in TestData)
            {
                Repository.Add(race);
                Assert.Contains(race, Repository);
            }
            foreach(Race race in TestData)
            {
                int expected = random.Next();
                race.Distance = expected;
                Repository.Update(race);
                Race actual = Session.Get<Race>(race.ID);
                Assert.AreEqual(expected, actual.Distance);
            }
        }
        
        #endregion
    }
}

