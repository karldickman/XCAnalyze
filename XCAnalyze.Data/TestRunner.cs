using System;
using NUnit.Framework;

namespace XCAnalyze.Data.Tests
{
    [TestFixture]
    public class TestRunner
    {

        [Test]
        public void TestEquals()
        {
            PersistentRunner karl = new PersistentRunner(SampleData.Karl.Surname, SampleData.Karl.GivenName);
            PersistentRunner florian = new PersistentRunner(SampleData.Florian.Surname, SampleData.Florian.GivenName);
            Assert.AreEqual(karl, karl);
            Assert.AreEqual(florian, florian);
            Assert.AreNotEqual(karl, florian);
            Assert.AreNotEqual(florian, karl);
            Assert.AreEqual(SampleData.Karl, karl);
        }
    }
}

