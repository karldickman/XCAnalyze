using System;
using System.Collections;
using System.Data;

using MySql.Data.MySqlClient;
using NUnit.Framework;

using XCAnalyze.Model;

namespace XCAnalyze.IO.Sql
{    
    [TestFixture]
    public class TestMySqlReader
    {
        public const string EXAMPLE_DATABASE = "xca_example";
        
        protected internal IDbConnection Connection { get; set; }
        protected internal Reader Reader { get; set; }

        [SetUp]
        public void SetUp ()
        {
            string connectionString = "Server=localhost; Database=" + EXAMPLE_DATABASE + "; User ID=xcanalyze; Password=xcanalyze; Pooling=false;";
            Connection = new MySqlConnection (connectionString);
            Reader = new Reader (Connection, EXAMPLE_DATABASE);
        }
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Dispose ();
        }

        [Test]
        public void TestRead ()
        {
            XcData data = Reader.Read ();
            Assert.IsNotEmpty ((ICollection)data.Affiliations);
            Assert.IsNotEmpty ((ICollection)data.Conferences);
            Assert.IsNotEmpty ((ICollection)data.Meets);
            Assert.IsNotEmpty ((ICollection)data.Performances);
            Assert.IsNotEmpty ((ICollection)data.Races);
            Assert.IsNotEmpty ((ICollection)data.Runners);
            Assert.IsNotEmpty ((ICollection)data.Schools);
            Assert.IsNotEmpty ((ICollection)data.Venues);
        }
    }
}
