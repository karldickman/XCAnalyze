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
    public class TestStateRepository : TestRepository<State>
    {
        public override IEnumerable<State> TestData
        {
            get { return SampleData.States; }
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
            State oregon = SampleData.Oregon.Clone<State>();
            Repository.Add(oregon);
            Assert.Contains(oregon, Repository);
            foreach(string newName in new List<string> { "California's Canada", "Washington's Mexico", "Idaho's Portugal", })
            {
                oregon.SetProperty("Name", newName);
                Repository.Update(oregon);
                using(ISession session = SessionFactory.OpenSession())
                {
                    State actual = session.Get<State>(oregon.Code);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
    }
}

