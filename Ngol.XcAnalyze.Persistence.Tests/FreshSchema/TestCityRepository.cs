using System;
using System.Collections.Generic;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestCityRepository : TestRepository<City>
    {
        #region Properties

        protected override IPersistentCollection<City> Collection
        {
            get { return Container.Cities; }
        }

        public City Portland
        {
            get { return Data.Portland; }
        }

        public override IEnumerable<City> TestData
        {
            get { return Data.Cities; }
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Container.States.QueueInserts(Data.States);
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
            City portland = new City(Portland.Name, Portland.State);
            Collection.QueueInsert(portland);
            Container.SaveChanges();
            MoreAssert.Contains(portland, Collection);
            foreach(string newName in new List<string> { "Little Beirut", "Stumptown", "Rose City", "PDX" })
            {
                portland.SetProperty("Name", newName);
                Collection.QueueUpdate(portland);
                Container.SaveChanges();
                City actual = Session.Get<City>(portland.Id);
                Assert.AreEqual(newName, actual.Name);
            }
        }
        
        #endregion
    }
}

