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
    public class TestVenueRepository : TestRepository<Venue>
    {
        public override IEnumerable<Venue> TestData
        {
            get { return SampleData.Venues; }
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
            Venue mcIver = SampleData.McIver.Clone<Venue>();
            Repository.Add(mcIver);
            Assert.Contains(mcIver, Repository);
            foreach(string newName in new List<string> { "The Vortex", })
            {
                mcIver.SetProperty("Name", newName);
                Repository.Update(mcIver);
                using(ISession session = SessionFactory.OpenSession())
                {
                    Venue actual = session.Get<Venue>(mcIver.ID);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
    }
}

