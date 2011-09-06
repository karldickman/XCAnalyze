using System;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestRunner
    {
        protected Runner Karl
        {
            get { return SampleData.Karl; }
        }
        
        protected Runner Florian
        {
            get { return SampleData.Florian; }
        }
        
        [Test]
        public void TestEquals()
        {
            Runner karl = Karl.Clone<Runner>();
            Runner florian = Florian.Clone<Runner>();
            Assert.AreEqual(karl, karl);
            Assert.AreEqual(florian, florian);
            Assert.AreNotEqual(karl, florian);
            Assert.AreNotEqual(florian, karl);
            Assert.AreEqual(Karl, karl);
            Runner zim1 = new Runner(2, "Zimmerman", "Elizabeth");//, Gender.Female, 2007);
            Runner zim2 = new Runner(5, "Zimmerman", "Elizabeth");//, Gender.Female, null);
            Assert.AreNotEqual(zim1, zim2);
        }
    }
}

