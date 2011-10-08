using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestMeetRepository : TestRepository<Meet>
    {
        #region Properties

        public override IEnumerable<Meet> TestData
        {
            get { return Data.Meets; }
        }

        protected override IPersistentCollection<Meet> Collection
        {
            get { return Container.Meets; }
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Container.Conferences.QueueInserts(Data.Conferences);
            Container.Teams.QueueInserts(Data.Teams);
            Container.SaveChanges();
        }

        #endregion

        #region Tests

        [Test]
        public void Add()
        {
            base.TestAdd();
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
            Meet originalMeet = new Meet(Data.SciacMultiDuals.Name);
            Collection.QueueInsert(originalMeet);
            Container.SaveChanges();
            MoreAssert.Contains(originalMeet, Collection);
            foreach(string newName in Data.Meets.Select(meet => meet.Name))
            {
                originalMeet.SetProperty("Name", newName);
                Collection.QueueUpdate(originalMeet);
                Container.SaveChanges();
                Meet actual = Session.Get<Meet>(originalMeet.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }

        #endregion
    }
}

