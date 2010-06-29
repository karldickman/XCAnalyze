using System;

using NUnit.Framework;

namespace XCAnalyze.Hytek
{
    [TestFixture]
    public class TestHytekFormatter
    {
        [Test]
        public void TestFormatTime ()
        {
            Assert.AreEqual ("25:02.10", HytekFormatter.FormatTime (25 * 60 + 2.1));
        }
    }
}
