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
    public class TestConferenceRepository : TestRepository<Conference>
    {
        #region Properties

        protected override IPersistentCollection<Conference> Collection
        {
            get { return Container.Conferences; }
        }

        public Conference Nwc
        {
            get { return Data.Nwc; }
        }

        public override IEnumerable<Conference> TestData
        {
            get { return Data.Conferences; }
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
            Conference nwc = new Conference(Nwc.Name, Nwc.Acronym);
            Collection.QueueInsert(nwc);
            Container.SaveChanges();
            MoreAssert.Contains(nwc, Collection);
            foreach(string newName in new List<string> { "NCIC", "WCIC" })
            {
                nwc.SetProperty("Name", newName);
                Collection.QueueUpdate(nwc);
                Container.SaveChanges();
                Conference actual = Session.Get<Conference>(nwc.Id);
                Assert.AreEqual(newName, actual.Name);
            }
        }

        #endregion
    }
}

