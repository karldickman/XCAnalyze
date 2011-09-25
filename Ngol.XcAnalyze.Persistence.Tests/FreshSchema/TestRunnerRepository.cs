using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
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
    public class TestRunnerRepository : TestRepository<Runner>
    {
        #region Properties

        public override IEnumerable<Runner> TestData
        {
            get { return Data.Runners; }
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
            base.SetUp();
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
            Runner karl = Data.Karl.Clone<Runner>();
            Repository.Add(karl);
            Assert.Contains(karl, Repository);
            karl.Surname = "Diechmann";
            Runner actual = Session.Get<Runner>(karl.ID);
            Assert.AreEqual(actual.Surname, karl.Surname);
        }
        
        #endregion
    }
}

