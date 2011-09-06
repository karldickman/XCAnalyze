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
    public class TestCityRepository : TestRepository<City>
    {
        public override IEnumerable<City> TestData
        {
            get { return SampleData.Cities; }
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
            City portland = SampleData.Portland.Clone<City>();
            Repository.Add(portland);
            Assert.Contains(portland, Repository);
            foreach(string newName in new List<string> { "Little Beirut", "Stumptown", "Rose City", "PDX", })
            {
                portland.SetProperty("Name", newName);
                Repository.Update(portland);
                using(ISession session = SessionFactory.OpenSession())
                {
                    City actual = session.Get<City>(portland.ID);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
    }
}

