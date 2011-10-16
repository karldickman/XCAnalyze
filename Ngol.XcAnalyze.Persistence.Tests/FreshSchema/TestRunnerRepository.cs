using System;
using System.Collections.Generic;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestRunnerRepository : TestRepository<Runner>
    {
        #region Properties

        protected override IPersistentCollection<Runner> Collection
        {
            get { return Container.Runners; }
        }

        public Runner Karl
        {
            get { return Data.Karl; }
        }

        public override IEnumerable<Runner> TestData
        {
            get { return Data.Runners; }
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
            Runner karl = new Runner(Karl.Surname, Karl.GivenName, Karl.Gender);
            Collection.QueueInsert(karl);
            Container.SaveChanges();
            Assert.That(Collection.IsPersisted(karl));
            karl.Surname = "Diechmann";
            Runner actual = Session.Get<Runner>(karl.Id);
            Assert.AreEqual(actual.Surname, karl.Surname);
        }
        
        #endregion
    }
}

