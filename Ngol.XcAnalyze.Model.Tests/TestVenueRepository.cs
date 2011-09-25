using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model.Collections;
using Ngol.XcAnalyze.Model.Interfaces;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestVenueRepository : TestRepository<Venue>
    {
        #region Properties

        public override IEnumerable<Venue> TestData
        {
            get { return SampleData.Venues; }
        }

        protected IRepository<City> CityRepository
        {
            get;
            set;
        }

        protected IRepository<State> StateRepository
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
            StateRepository = new Repository<State>(Session);
            StateRepository.AddRange(SampleData.States);
            CityRepository = new Repository<City>(Session);
            CityRepository.AddRange(SampleData.Cities);
        }

        public override void TearDown()
        {
            base.TearDown();
            CityRepository.SafeDispose();
            StateRepository.SafeDispose();
        }

        #endregion

        #region Methods

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
            Venue mcIver = SampleData.McIver.Clone<Venue>();
            Repository.Add(mcIver);
            Assert.Contains(mcIver, Repository);
            foreach(string newName in new List<string> { "The Vortex" })
            {
                mcIver.SetProperty("Name", newName);
                Repository.Update(mcIver);
                Venue actual = Session.Get<Venue>(mcIver.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }
        
        #endregion
    }
}

