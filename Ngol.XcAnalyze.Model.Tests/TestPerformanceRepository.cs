using System;
using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestPerformanceRepository : TestRepository<Performance>
    {
        public override IEnumerable<Performance> TestData
        {
            get { return SampleData.Performances; }
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
            foreach(Performance performance in TestData)
            {
                Repository.Add(performance);
                Assert.Contains(performance, Repository);
            }
            Random random = new Random();
            foreach(Performance performance in TestData)
            {
                double expected = random.NextDouble();
                performance.Time = expected;
                Repository.Update(performance);
                using(ISession session = SessionFactory.OpenSession())
                {
                    Performance actual = session.Get<Performance>(performance.ID);
                    Assert.IsTrue(actual.Time.HasValue);
                    Assert.AreEqual(actual.Time.Value, expected, 0.001);
                }
            }
        }
    }
}

