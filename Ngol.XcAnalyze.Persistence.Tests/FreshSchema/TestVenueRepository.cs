using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestVenueRepository : TestRepository<Venue>
    {
        #region Properties

        public override IEnumerable<Venue> TestData
        {
            get { return Data.Venues; }
        }

        protected override IPersistentCollection<Venue> Collection
        {
            get { return Container.Venues; }
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Container.States.QueueInserts(Data.States);
            Container.Cities.QueueInserts(Data.Cities);
            Container.SaveChanges();
        }

        #endregion

        #region Methods

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
            Venue mcIver = new Venue(Data.McIver.Name, Data.McIver.City);
            Collection.QueueInsert(mcIver);
            Container.SaveChanges();
            Assert.That(Collection.IsPersisted(mcIver));
            foreach(string newName in new List<string> { "The Vortex" })
            {
                mcIver.SetProperty("Name", newName);
                Collection.QueueUpdate(mcIver);
                Container.SaveChanges();
                Venue actual = Session.Get<Venue>(mcIver.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }
        
        #endregion
    }
}

