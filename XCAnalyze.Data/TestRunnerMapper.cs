using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using NUnit.Framework;
using XCAnalyze.Data;
using XCAnalyze.Model;

namespace XCAnalyze.Data.Tests
{
    [TestFixture]
    public class TestRunnerMapper
    {
        #region Properties
        
        /// <summary>
        /// Connection to a blank database.
        /// </summary>
        protected IDbConnection BlankConnection { get; set; }
        
        protected readonly IRunner Florian = SampleData.Florian;
        
        protected readonly IRunner Karl = SampleData.Karl;
        
        /// <summary>
        /// The sample runners to use.
        /// </summary>
        protected readonly IList<IRunner> Runners = SampleData.Runners;
        
        /// <summary>
        /// Connection to a sample database.
        /// </summary>
        protected IDbConnection SampleConnection { get; set; }
        
        #endregion
        
        #region Set up and tear down
        
        [SetUp]
        public void SetUp()
        {
            BlankConnection = new SqliteConnection("Data Source=:memory:");
            BlankConnection.Open();
            SampleConnection = new SqliteConnection("Data Source=" + SupportFiles.GetPath(SupportFiles.SqliteExampleFile));
            SampleConnection.Open();
        }
        
        public void TearDown()
        {
            BlankConnection.Close();
            SampleConnection.Close();
        }
        
        #endregion
        
        #region Tests
        
        [Test]
        public void TestDelete()
        {
            RunnerMapper mapper = new RunnerMapper(BlankConnection);
            mapper.InitializeDatabase();
            int count = Runners.Count;
            IDictionary<IRunner, int> runnerIDs = new Dictionary<IRunner, int>();
            foreach(IRunner runner in Runners)
            {
                runnerIDs[runner] = mapper.Insert(runner);
            }
            IList<IRunner> actual = mapper.Select();
            Assert.AreEqual(count, actual.Count);
            IRunner karl = actual.Single(r => r.Surname.Equals(Karl.Surname));
            IRunner florian = actual.Single(r => r.Surname.Equals(Florian.Surname));
            mapper.Delete(runnerIDs[karl]);
            count--;
            actual = mapper.Select();
            Assert.AreEqual(count, actual.Count);
            Assert.IsFalse(actual.Contains(karl));
            mapper.Delete(runnerIDs[florian]);
            count--;
            actual = mapper.Select();
            Assert.AreEqual(count, actual.Count);
            Assert.IsFalse(actual.Contains(florian));
        }
        
        [Test]
        public void TestInitialize()
        {
            RunnerMapper mapper = new RunnerMapper(BlankConnection);
            Assert.IsFalse(mapper.IsDatabaseInitialized());
            mapper.InitializeDatabase();
            Assert.That(mapper.IsDatabaseInitialized());
        }
        
        [Test]
        public void TestIsInitialized()
        {
            RunnerMapper mapper = new RunnerMapper(BlankConnection);
            Assert.IsFalse(mapper.IsDatabaseInitialized());
            mapper = new RunnerMapper(SampleConnection);
            Assert.That(mapper.IsDatabaseInitialized());
        }
        
        [Test]
        public void TestInsert()
        {
            RunnerMapper mapper = new RunnerMapper(BlankConnection);
            mapper.InitializeDatabase();
            int count = 0;
            foreach(PersistentRunner runner in Runners)
            {
                Assert.AreEqual(count, mapper.Select().Count);
                mapper.Insert(runner);
                count++;
                Assert.That(mapper.Select().Contains(runner));
            }
        }
        
        [Test]
        public void TestSelect()
        {
            RunnerMapper mapper = new RunnerMapper(SampleConnection);
            Assert.Greater(mapper.Select().Count, 0);
        }
        
        [Test]
        public void TestUpdate()
        {
            RunnerMapper mapper = new RunnerMapper(BlankConnection);
            mapper.InitializeDatabase();
            IRunner karl = new PersistentRunner(Karl.Surname, Karl.GivenName);
            int karlID = mapper.Insert(karl);
            karl.Surname = Florian.Surname;
            karl.GivenName = Florian.GivenName;
            mapper.Update(karlID, karl);
            Assert.That(mapper.Select().Contains(Florian));
            Assert.AreEqual(Florian, karl);
        }
        
        #endregion
    }
}

