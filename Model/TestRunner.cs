using System;

using NUnit.Framework;

namespace XCAnalyze.Model
{
    [TestFixture]
    public class TestRunner
    {
        [Test]
        public void TestEquals ()
        {
            Runner zim1 = new Runner ("Zimmerman", "Elizabeth", Gender.FEMALE, 2007);
            Runner zim2 = new Runner ("Zimmerman", "Elizabeth", Gender.FEMALE, null);
            Assert.IsFalse (zim1.Equals (zim2));
        }
    }
}
