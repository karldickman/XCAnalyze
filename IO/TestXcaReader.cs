using System;

using NUnit.Framework;

namespace XCAnalyze.IO
{    
    [TestFixture]
    public class TestXcaReader
    {
        protected internal XcaReader Reader { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Reader = new XcaReader(SupportFiles.GetPath ("example.xca"));
        }
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Dispose ();
        }
        
        [Test]
        public void TestRead ()
        {
            Reader.Read ();
        }
    }
}
