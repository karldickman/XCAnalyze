using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Model.Collections;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public sealed class TestStateRepository
    {
        #region Properties

        private Configuration Configuration
        {
            get;
            set;
        }

        private IRepository<State> Repository
        {
            get;
            set;
        }

        private ISessionFactory SessionFactory
        {
            get;
            set;
        }

        #endregion

        #region Set up

        [TestFixtureSetUp]
        public void InitialSetUp()
        {
            Configuration = new Configuration();
            Configuration.Configure();
            Configuration.AddAssembly(typeof(State).Assembly);
            SessionFactory = Configuration.BuildSessionFactory();
        }

        [SetUp]
        public void SetUp()
        {
            new SchemaExport(Configuration).Execute(false, true, false);
            Repository = new StateRepository(SessionFactory);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete("xcanalyze.db");
        }

        #endregion

        #region Tests

        [Test]
        public void Add()
        {
            Assert.IsEmpty(Repository);
            Repository.AddRange(SampleData.States);
            Assert.AreEqual(SampleData.States.Count(), Repository.Count);
            foreach(State state in SampleData.States)
            {
                Assert.Contains(state, Repository);
            }
        }

        [Test]
        public void Contains()
        {
            Assert.IsEmpty(Repository);
            ICollection<State> addedStates = new List<State>();
            foreach(State state in SampleData.States)
            {
                addedStates.Add(state);
                Repository.Add(state);
                foreach(State addedState in addedStates)
                {
                    Assert.Contains(addedState, Repository);
                }
                foreach(State nonAddedState in SampleData.States.Except(addedStates))
                {
                    Assert.DoesNotContain(nonAddedState, Repository);
                }
            }
        }

        [Test]
        public void Clear()
        {
            try
            {
                Repository.Clear();
                Assert.Fail("The clear method should not be supported on repositories.");
            }
            catch(NotSupportedException)
            {
            }
        }

        [Test]
        public void Count()
        {
            Assert.IsEmpty(Repository);
            SampleData.States.ForEach(1, (state, index) =>
                {
                    Repository.Add(state);
                    Assert.AreEqual(index, Repository.Count);
                });
        }

        [Test]
        public void Remove()
        {
            Repository.AddRange(SampleData.States);
            ICollection<State> removedStates = new List<State>();
            foreach(State state in SampleData.States)
            {
                removedStates.Add(state);
                Repository.Remove(state);
                foreach(State removedState in removedStates)
                {
                    Assert.DoesNotContain(removedState, Repository);
                }
                foreach(State nonRemovedState in SampleData.States.Except(removedStates))
                {
                    Assert.Contains(nonRemovedState, Repository);
                }
            }
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
        
        #endregion
    }
}

