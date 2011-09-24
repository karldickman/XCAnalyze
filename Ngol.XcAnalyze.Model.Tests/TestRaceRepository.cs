using System;
using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestRaceRepository : TestRepository<Race>
    {
        public override IEnumerable<Race> TestData
        {
            get { return SampleData.Races.Values; }
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
            Random random = new Random();
            foreach(Race race in TestData)
            {
                Repository.Add(race);
                Assert.Contains(race, Repository);
            }
            foreach(Race race in TestData)
            {
                int expected = random.Next();
                race.Distance = expected;
                Repository.Update(race);
                using(ISession session = SessionFactory.OpenSession())
                {
                    Race actual = session.Get<Race>(race.ID);
                    Assert.AreEqual(expected, actual.Distance);
                }
            }
        }
    }
}

