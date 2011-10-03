using System;
using System.Collections.Generic;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Interfaces;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestStateRepository : TestRepository<State>
    {
        #region Properties

        public override IEnumerable<State> TestData
        {
            get { return Data.States; }
        }

        protected override IPersistentCollection<State> Collection
        {
            get { return Container.States; }
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
            State oregon = Data.Oregon.Clone<State>();
            Collection.QueueInsert(oregon);
            Container.SaveChanges();
            Assert.That(Collection.IsPersisted(oregon));
            foreach(string newName in new List<string> { "California's Canada", "Washington's Mexico", "Idaho's Portugal", })
            {
                oregon.SetProperty("Name", newName);
                Collection.QueueUpdate(oregon);
                Container.SaveChanges();
                State actual = Session.Get<State>(oregon.Code);
                Assert.AreEqual(newName, actual.Name);
            }
        }

        #endregion
    }
}

