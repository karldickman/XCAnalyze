using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.Reflection.Extensions;
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
    [TestFixture]
    public class TestCityRepository
    {
        #region Properties

        private Configuration Configuration
        {
            get;
            set;
        }

        private IRepository<City> Repository
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
            Configuration.AddAssembly(typeof(City).Assembly);
            SessionFactory = Configuration.BuildSessionFactory();
        }

        [SetUp]
        public void SetUp()
        {
            new SchemaExport(Configuration).Execute(false, true, false);
            Repository = new CityRepository(SessionFactory);
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
            Repository.AddRange(SampleData.Cities);
            Assert.AreEqual(SampleData.Cities.Count(), Repository.Count);
            foreach(City state in SampleData.Cities)
            {
                Assert.Contains(state, Repository);
            }
        }

        [Test]
        public void Contains()
        {
            Assert.IsEmpty(Repository);
            ICollection<City> addedCities = new List<City>();
            foreach(City state in SampleData.Cities)
            {
                addedCities.Add(state);
                Repository.Add(state);
                foreach(City addedCity in addedCities)
                {
                    Assert.Contains(addedCity, Repository);
                }
                foreach(City nonAddedCity in SampleData.Cities.Except(addedCities))
                {
                    Assert.DoesNotContain(nonAddedCity, Repository);
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
            SampleData.Cities.ForEach(1, (state, index) =>
                {
                    Repository.Add(state);
                    Assert.AreEqual(index, Repository.Count);
                });
        }

        [Test]
        public void Remove()
        {
            Repository.AddRange(SampleData.Cities);
            ICollection<City> removedCities = new List<City>();
            foreach(City state in SampleData.Cities)
            {
                removedCities.Add(state);
                Repository.Remove(state);
                foreach(City removedCity in removedCities)
                {
                    Assert.DoesNotContain(removedCity, Repository);
                }
                foreach(City nonRemovedCity in SampleData.Cities.Except(removedCities))
                {
                    Assert.Contains(nonRemovedCity, Repository);
                }
            }
        }

        [Test]
        public void Update()
        {
            City portland = SampleData.Portland.Clone<City>();
            Repository.Add(portland);
            Assert.Contains(portland, Repository);
            foreach(string newName in new List<string> { "Little Beirut", "Stumptown", "Rose City", "PDX", })
            {
                portland.SetProperty("Name", newName);
                Repository.Update(portland);
                using(ISession session = SessionFactory.OpenSession())
                {
                    City actual = session.Get<City>(portland.ID);
                    Assert.AreEqual(newName, actual.Name);
                }
            }
        }
        
        #endregion
    }
}

