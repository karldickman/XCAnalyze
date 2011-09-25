using System;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.SampleData;
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
            oregon = Data.Oregon.Clone<State>();
            Assert.AreEqual(Data.Oregon, oregon);
            // If you change the name, they should be different.
            oregon.SetProperty("Name", "California's Canada");
            Assert.AreNotEqual(Data.Oregon, oregon);
            // If you change the code, they should be different.
            oregon = Data.Oregon.Clone<State>();
            oregon.SetProperty("Code", "OX");
            Assert.AreNotEqual(Data.Oregon, oregon);
        }
    }
}

