using System;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestState
    {
        [Test]
        public void Equals()
        {
            State oregon;
            oregon = SampleData.Oregon.Clone<State>();
            Assert.AreEqual(SampleData.Oregon, oregon);
            // If you change the name, they should be different.
            oregon.SetProperty("Name", "California's Canada");
            Assert.AreNotEqual(SampleData.Oregon, oregon);
            // If you change the code, they should be different.
            oregon = SampleData.Oregon.Clone<State>();
            oregon.SetProperty("Code", "OX");
            Assert.AreNotEqual(SampleData.Oregon, oregon);
        }
    }
}

