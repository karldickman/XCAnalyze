using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestConferenceRepository : TestRepository<Conference>
    {
        public override IEnumerable<Conference> TestData
        {
            get { return SampleData.Conferences; }
        }

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
            Conference nwc = SampleData.Nwc.Clone<Conference>();
            Repository.Add(nwc);
            Assert.Contains(nwc, Repository);
            foreach(string newName in new List<string> { "NCIC", "WCIC" })
            {
                nwc.SetProperty("Name", newName);
                Repository.Update(nwc);
                using(ISession session = SessionFactory.OpenSession())
                {
                    Conference actual = session.Get<Conference>(nwc.ID);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
    }
}

