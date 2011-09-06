using System;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestCity
    {
        [Test]
        public void Equals()
        {
            City estacada = SampleData.Estacada.Clone<City>();
            Assert.AreEqual(SampleData.Estacada, estacada);
            estacada.SetProperty("Name", "Hickville");
            Assert.AreNotEqual(SampleData.Estacada, estacada);
            estacada = SampleData.Estacada.Clone<City>();
            estacada.SetProperty("ID", 7);
            Assert.AreNotEqual(SampleData.Estacada, estacada);
            estacada = SampleData.Estacada.Clone<City>();
            estacada.SetProperty("State", SampleData.California);
            Assert.AreNotEqual(SampleData.Estacada, estacada);
        }
    }
}

