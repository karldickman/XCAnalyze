using System;

using NUnit.Framework;

namespace XCAnalyze.Hytek
{
    public partial class HytekFormatter
    {
#if DEBUG
        [TestFixture]
        public class Test
        {
            [Test]
            public void TestFormatTime ()
            {
                Assert.AreEqual ("25:02.10", HytekFormatter.FormatTime (25 * 60 + 2.1));
            }
        }
#endif
    }
}