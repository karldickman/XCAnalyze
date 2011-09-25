using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model.Collections;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Assert = Ngol.Utilities.NUnit.MoreAssert;

namespace Ngol.XcAnalyze.Model.Tests
{
    public abstract class TestRepository<T>
    {
        #region Properties

        protected Configuration Configuration
        {
            get;
            set;
        }

        protected IRepository<T> Repository
        {
            get;
            set;
        }

        protected ISessionFactory SessionFactory
        {
            get;
            set;
        }

        protected ISession Session
        {
            get;
            set;
        }

        public abstract IEnumerable<T> TestData
        {
            get;
        }

        #endregion

        #region Set up

        [TestFixtureSetUp]
        public void InitialSetUp()
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
            Configuration = new Configuration();
            Configuration.Configure();
            Configuration.AddAssembly(typeof(State).Assembly);
            SessionFactory = Configuration.BuildSessionFactory();
            // Export the schema
            new SchemaExport(Configuration).Execute(false, true, false);
            Session = SessionFactory.OpenSession();
            Repository = new Repository<T>(Session);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Repository.SafeDispose();
            File.Delete("xcanalyze.db");
        }

        #endregion

        #region Tests

        public void TestAdd()
        {
            TestAdd(TestData);
        }

        public void TestContains()
        {
            TestContains(TestData);
        }

        public void TestClear()
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

        public void TestCount()
        {
            TestCount(TestData);
        }

        public void TestRemove()
        {
            TestRemove(TestData);
        }

        protected void TestAdd(IEnumerable<T> testWith)
        {
            Assert.IsEmpty(Repository);
            Repository.AddRange(testWith);
            Assert.AreEqual(testWith.Count(), Repository.Count);
            foreach(T item in testWith)
            {
                Assert.Contains(item, Repository);
            }
        }

        protected void TestCount(IEnumerable<T> testWith)
        {
            Assert.IsEmpty(Repository);
            testWith.ForEachIndexed(1, (item, index) =>
            {
                Repository.Add(item);
                Assert.AreEqual(index, Repository.Count);
            });
        }

        protected void TestContains(IEnumerable<T> testWith)
        {
            Assert.IsEmpty(Repository);
            ICollection<T> addedItems = new List<T>();
            foreach(T item in testWith)
            {
                addedItems.Add(item);
                Repository.Add(item);
                foreach(T addedItem in addedItems)
                {
                    Assert.Contains(addedItem, Repository);
                }
                foreach(T nonAddedItem in testWith.Except(addedItems))
                {
                    Assert.DoesNotContain(nonAddedItem, Repository);
                }
            }
        }

        protected void TestRemove(IEnumerable<T> testWith)
        {
            Repository.AddRange(testWith);
            ICollection<T> removedItems = new List<T>();
            foreach(T item in testWith)
            {
                removedItems.Add(item);
                Repository.Remove(item);
                foreach(T removedItem in removedItems)
                {
                    Assert.DoesNotContain(removedItem, Repository);
                }
                foreach(T nonRemovedItem in testWith.Except(removedItems))
                {
                    Assert.Contains(nonRemovedItem, Repository);
                }
            }
        }
        
        #endregion
    }
}

