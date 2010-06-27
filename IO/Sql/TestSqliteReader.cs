using System;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{    
    [TestFixture]
    public class TestSqliteReader : TestReader
    {        
        [SetUp]
        override public void SetUp ()
        {
            Reader = new SqliteReader(SupportFiles.GetPath (EXAMPLE_DATABASE + ".db"));
        }
    }
}
