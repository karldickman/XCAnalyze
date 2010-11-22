using System;

using NUnit.Framework;

namespace XCAnalyze.IO
{
    public partial class XcaReader
    {
        #if DEBUG
        [TestFixture]
        public new class Test
        {
            XcaReader Reader { get; set; }

            [SetUp]
            public void SetUp()
            {
                Reader = new XcaReader(SupportFiles.GetPath("example.xca"));
            }

            [TearDown]
            public void TearDown()
            {
                Reader.Close();
            }

            [Test]
            public void TestRead()
            {
                Reader.Read();
            }
        }
        #endif
    }
}
