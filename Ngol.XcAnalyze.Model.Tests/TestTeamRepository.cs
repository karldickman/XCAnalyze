using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Model.Collections;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestTeamRepository : TestRepository<Team>
    {
        #region Properties

        public override IEnumerable<Team> TestData
        {
            get { return SampleData.Teams; }
        }

        protected IRepository<Conference> ConferenceRepository
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
            ConferenceRepository.AddRange(SampleData.Conferences);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            ConferenceRepository.SafeDispose();
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
            Team pioneers = SampleData.LewisAndClark.Clone<Team>();
            Repository.Add(pioneers);
            Assert.Contains(pioneers, Repository);
            foreach(string newName in new List<string> { "Pioneers", "LC" })
            {
                pioneers.SetProperty("Name", newName);
                Repository.Update(pioneers);
                Team actual = Session.Get<Team>(pioneers.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }
        
        #endregion
    }
}

