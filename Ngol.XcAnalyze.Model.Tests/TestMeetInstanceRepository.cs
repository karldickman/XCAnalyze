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
    public class TestMeetInstanceRepository : TestRepository<MeetInstance>
    {
        public override IEnumerable<MeetInstance> TestData
        {
            get { return SampleData.MeetInstances; }
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
            MeetInstance originalMeetInstance = SampleData.LCInvite09.Clone<MeetInstance>();
            Repository.Add(originalMeetInstance);
            Assert.Contains(originalMeetInstance, Repository);
            foreach(DateTime date in SampleData.MeetInstances.Select(meet => meet.Date))
            {
                originalMeetInstance.SetProperty("Date", date);
                Repository.Update(originalMeetInstance);
                using(ISession session = SessionFactory.OpenSession())
                {
                    MeetInstance actual = session.Get<MeetInstance>(originalMeetInstance.ID);
                    Assert.AreEqual(date, actual.Date);
                }
            }
        }
    }
}

