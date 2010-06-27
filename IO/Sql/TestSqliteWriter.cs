using System;
using System.Data;
using System.IO;

using Mono.Data.Sqlite;
using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{    
    [TestFixture]
    public class TestSqliteWriter : TestWriter
    {
        [SetUp]
        override public void SetUp ()
        {
            base.SetUp();
            Writer = CreateWriter();
            Reader = new Reader (new SqliteConnection (
                    Writer.Connection.ConnectionString), TEST_DATABASE);
        }
        
        override protected internal AbstractReader CreateExampleReader()
        {
            return new SqliteReader(SupportFiles.GetPath(EXAMPLE_DATABASE + ".db"));
        }
        
        override protected internal AbstractWriter CreateWriter()
        {
            return new SqliteWriter(TEST_DATABASE);
        }
        
        override protected internal void SetUpPartial ()
        {
            File.Delete(TEST_DATABASE);
            IDbConnection connection = new SqliteConnection(
                "Data Source=" + TEST_DATABASE);
            connection.Open();
            IDbCommand command = connection.CreateCommand();
            Writer = new SqliteWriter(connection, TEST_DATABASE, command);
        }

        [TearDown]
        override public void TearDown ()
        {
            base.TearDown();
            File.Delete (TEST_DATABASE);
        }
    }
}
