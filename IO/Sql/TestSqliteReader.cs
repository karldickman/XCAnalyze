using System;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{    
    public partial class SqliteReader
    {
#if DEBUG
        [TestFixture]
        public class Test : Reader.TestReader
        {        
            [SetUp]
            override public void SetUp ()
            {
                Reader = new SqliteReader(SupportFiles.GetPath (ExampleDatabase + ".db"));
            }
        }
#endif
    }
}
