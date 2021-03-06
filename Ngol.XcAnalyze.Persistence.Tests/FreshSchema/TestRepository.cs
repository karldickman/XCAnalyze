using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ngol.Utilities.NUnit;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.Persistence.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    public abstract class TestRepository<T>
    {
        #region Properties

        protected Configuration Configuration
        {
            get;
            set;
        }

        protected abstract IPersistentCollection<T> Collection
        {
            get;
        }

        protected PersistenceContainer Container
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
            Configuration.Configure("sqlite-hibernate.cfg.xml");
            // Export the schema
            new SchemaExport(Configuration).Execute(false, true, false);
            Container = new PersistenceContainer("sqlite-hibernate.cfg.xml");
            Session = (ISession)Container.GetProperty("Session");
        }

        [TearDown]
        public void TearDown()
        {
            Container.SafeDispose();
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
            MoreAssert.IsEmpty(Collection);
            Collection.QueueInserts(testWith);
            Container.SaveChanges();
            MoreAssert.HasCount(testWith.Count(), Collection);
            foreach(T item in testWith)
            {
                Assert.That(Collection.IsPersisted(item));
            }
        }

        protected void TestCount(IEnumerable<T> testWith)
        {
            MoreAssert.IsEmpty(Collection);
            testWith.ForEachIndexed(1, (item, index) =>
            {
                Collection.QueueInsert(item);
                Container.SaveChanges();
                MoreAssert.HasCount(index, Collection);
            });
        }

        protected void TestContains(IEnumerable<T> testWith)
        {
            MoreAssert.IsEmpty(Collection);
            ICollection<T> addedItems = new List<T>();
            foreach(T item in testWith)
            {
                addedItems.Add(item);
                Collection.QueueInsert(item);
                Container.SaveChanges();
                foreach(T addedItem in addedItems)
                {
                    Assert.That(Collection.IsPersisted(addedItem));
                }
                foreach(T nonAddedItem in testWith.Except(addedItems))
                {
                    Assert.IsFalse(Collection.IsPersisted(nonAddedItem));
                }
            }
        }

        protected void TestRemove(IEnumerable<T> testWith)
        {
            Collection.QueueInserts(testWith);
            Container.SaveChanges();
            ICollection<T> removedItems = new List<T>();
            foreach(T item in testWith)
            {
                removedItems.Add(item);
                Collection.QueueDelete(item);
                Container.SaveChanges();
                foreach(T removedItem in removedItems)
                {
                    Assert.IsFalse(Collection.IsPersisted(removedItem));
                }
                foreach(T nonRemovedItem in testWith.Except(removedItems))
                {
                    Assert.That(Collection.IsPersisted(nonRemovedItem));
                }
            }
        }
        
        #endregion
    }
}

