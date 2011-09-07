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
    public class TestMeetRepository : TestRepository<Meet>
    {
        public override IEnumerable<Meet> TestData
        {
            get { return SampleData.Meets; }
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
            Meet originalMeet = SampleData.SciacMultiDuals.Clone<Meet>();
            Repository.Add(originalMeet);
            Assert.Contains(originalMeet, Repository);
            foreach(string newName in SampleData.Meets.Select(meet => meet.Name))
            {
                originalMeet.SetProperty("Name", newName);
                Repository.Update(originalMeet);
                using(ISession session = SessionFactory.OpenSession())
                {
                    Meet actual = session.Get<Meet>(originalMeet.ID);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
    }
}

