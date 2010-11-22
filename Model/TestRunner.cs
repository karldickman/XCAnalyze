using System;

using NUnit.Framework;

namespace XCAnalyze.Model
{
    public partial class Runner
    {
#if DEBUG
        [TestFixture]
        public class Test
        {
            [Test]
            public void TestEquals ()
            {
                Runner zim1 = new Runner (2, "Zimmerman", "Elizabeth",
                    Gender.Female, 2007);
                Runner zim2 = new Runner (5, "Zimmerman", "Elizabeth",
                    Gender.Female, null);
                Assert.IsFalse (zim1.Equals (zim2));
            }
        }
#endif
    }
}
