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
    public class TestCityRepository : TestRepository<City>
    {
        #region Properties

        public IRepository<State> StateRepository
        {
            get;
            set;
        }

        public override IEnumerable<City> TestData
        {
            get { return SampleData.Cities; }
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            StateRepository = new Repository<State>(Session);
            StateRepository.AddRange(SampleData.States);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            StateRepository.SafeDispose();
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
            City portland = SampleData.Portland.Clone<City>();
            Repository.Add(portland);
            Assert.Contains(portland, Repository);
            foreach(string newName in new List<string> { "Little Beirut", "Stumptown", "Rose City", "PDX", })
            {
                portland.SetProperty("Name", newName);
                Repository.Update(portland);
                City actual = Session.Get<City>(portland.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }

        #endregion
    }
}

