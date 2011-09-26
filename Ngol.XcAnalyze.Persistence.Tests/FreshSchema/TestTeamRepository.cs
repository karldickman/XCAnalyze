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
using NHibernate;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestTeamRepository : TestRepository<Team>
    {
        #region Properties

        public override IEnumerable<Team> TestData
        {
            get { return Data.Teams; }
        }

        protected override IPersistentCollection<Team> Collection
        {
            get { return Container.Teams; }
        }

        #endregion

        #region Set up

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Container.Conferences.QueueInserts(Data.Conferences);
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
            Team pioneers = Data.LewisAndClark.Clone<Team>();
            Collection.QueueInsert(pioneers);
            Container.SaveChanges();
            Assert.That(Collection.IsPersisted(pioneers));
            foreach(string newName in new List<string> { "Pioneers", "LC" })
            {
                pioneers.SetProperty("Name", newName);
                Collection.QueueUpdate(pioneers);
                Container.SaveChanges();
                Team actual = Session.Get<Team>(pioneers.ID);
                Assert.AreEqual(newName, actual.Name);
            }
        }
        
        #endregion
    }
}

