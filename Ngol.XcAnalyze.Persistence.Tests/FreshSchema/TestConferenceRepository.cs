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

        public override IEnumerable<Conference> TestData
        {
            get { return Data.Conferences; }
        }

        protected override IPersistentCollection<Conference> Collection
        {
            get { return Container.Conferences; }
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
            Conference nwc = Data.Nwc.Clone<Conference>();
            Collection.QueueInsert(nwc);
            Container.SaveChanges();
            MoreAssert.Contains(nwc, Collection);
            foreach(string newName in new List<string> { "NCIC", "WCIC" })
            {
                nwc.SetProperty("Name", newName);
                Collection.QueueUpdate(nwc);
                Container.SaveChanges();
                Conference actual = Session.Get<Conference>(nwc.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }

        #endregion
    }
}

