using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestAffiliationRepository : TestRepository<Affiliation>
    {
        public override IEnumerable<Affiliation> TestData
        {
            get { return SampleData.Affiliations; }
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
            Affiliation karl = SampleData.AffiliationDictionary[SampleData.Karl][2009].Clone<Affiliation>();
            karl.ID = 1;
            Repository.Add(karl);
            Assert.Contains(karl, Repository);
            Assert.AreEqual(2009, karl.Season);
            karl.Season = 1975;
            Repository.Update(karl);
            using(ISession session = SessionFactory.OpenSession())
            {
                Affiliation actual = session.Get<Affiliation>(karl.ID);
                Assert.AreEqual(karl.Season, actual.Season);
            }
            /*foreach(Runner runner in SampleData.Runners)
            {
                karl.SetProperty("Runner", runner);
                Repository.Update(karl);
                using(ISession session = SessionFactory.OpenSession())
                {
                    var query = session.Query<Affiliation>();
                    Assert.HasCount(1, query);
                    Affiliation actual = query.SingleOrDefault();
                    Assert.AreEqual(runner, actual.Runner);
                }
            }
            foreach(Team team in SampleData.Teams)
            {
                karl.SetProperty("Team", team);
                Repository.Update(karl);
                using(ISession session = SessionFactory.OpenSession())
                {
                    var query = session.Query<Affiliation>();
                    Assert.HasCount(1, query);
                    Affiliation actual = query.SingleOrDefault();
                    Assert.AreEqual(team, actual.Team);
                }
            }*/
        }
    }
}