using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestTeamRepository : TestRepository<Team>
    {
        public override IEnumerable<Team> TestData
        {
            get { return SampleData.Teams; }
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
            Team pioneers = SampleData.LewisAndClark.Clone<Team>();
            Repository.Add(pioneers);
            Assert.Contains(pioneers, Repository);
            foreach(string newName in new List<string> { "Pioneers", "LC", })
            {
                pioneers.SetProperty("Name", newName);
                Repository.Update(pioneers);
                using(ISession session = SessionFactory.OpenSession())
                {
                    Team actual = session.Get<Team>(pioneers.ID);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
    }
}

