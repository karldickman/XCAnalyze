using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

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

        protected IRepository<Conference> ConferenceRepository
        {
            get;
            set;
        }

        protected IRepository<Team> TeamRepository
        {
            get;
            set;
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            ConferenceRepository = new Repository<Conference>(Session);
            ConferenceRepository.AddRange(Data.Conferences);
            TeamRepository = new Repository<Team>(Session);
            TeamRepository.AddRange(Data.Teams);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            ConferenceRepository.SafeDispose();
            TeamRepository.SafeDispose();
        }

        #endregion

        #region Tests

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
            Meet originalMeet = Data.SciacMultiDuals.Clone<Meet>();
            Repository.Add(originalMeet);
            Assert.Contains(originalMeet, Repository);
            foreach(string newName in Data.Meets.Select(meet => meet.Name))
            {
                originalMeet.SetProperty("Name", newName);
                Repository.Update(originalMeet);
                Meet actual = Session.Get<Meet>(originalMeet.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }

        #endregion
    }
}

