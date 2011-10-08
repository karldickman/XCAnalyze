using System;
using Ngol.Hytek.Interfaces;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestRunner
    {
        protected Runner Karl
        {
            get { return Data.Karl; }
        }
        
        protected Runner Florian
        {
            get { return Data.Florian; }
        }
        
        [Test]
        public void TestEquals()
        {
            Runner karl = new Runner(Karl.Surname, Karl.GivenName, Karl.Gender);
            Runner florian = new Runner(Florian.Surname, Florian.GivenName, Florian.Gender);
            Assert.AreEqual(karl, karl);
            Assert.AreEqual(florian, florian);
            Assert.AreNotEqual(karl, florian);
            Assert.AreNotEqual(florian, karl);
            Assert.AreEqual(Karl, karl);
            Runner zim1 = new Runner("Zimmerman", "Elizabeth", Gender.Female) { EnrollmentYear = 2007 };
            Runner zim2 = new Runner("Zimmerman", "Elizabeth", Gender.Female);
            Assert.AreNotEqual(zim1, zim2);
        }
    }
}

