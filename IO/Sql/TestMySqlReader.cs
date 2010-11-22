using System;
using System.Collections;
using System.Data;

using MySql.Data.MySqlClient;
using NUnit.Framework;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{
    public partial class MySqlReader
    {
        #if DEBUG
        [TestFixture]
        public class Test
        {
            public const string ExampleDatabase = "xca_example";

            Reader Reader { get; set; }

            [SetUp]
            public void SetUp()
            {
                Reader = new MySqlReader(ExampleDatabase, "xcanalyze", "xcanalyze");
            }

            [TearDown]
            public void TearDown()
            {
                Reader.Close();
            }

            [Test]
            public void TestRead()
            {
                DataContext data = Reader.Read();
                Assert.IsNotEmpty((ICollection)data.Affiliations);
                Assert.IsNotEmpty((ICollection)data.Conferences);
                Assert.IsNotEmpty((ICollection)data.MeetInstances);
                Assert.IsNotEmpty((ICollection)data.Performances);
                Assert.IsNotEmpty((ICollection)data.Races);
                Assert.IsNotEmpty((ICollection)data.Runners);
                Assert.IsNotEmpty((ICollection)data.Teams);
                Assert.IsNotEmpty((ICollection)data.Venues);
            }
        }
        #endif
    }
}
