using System;
using Ngol.Utilities.Reflection.Extensions;
using Ngol.Utilities.System.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.SampleData;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Model.Tests
{
    [TestFixture]
    public class TestCity
    {
        [Test]
        public void Equals()
        {
            City estacada = Data.Estacada.Clone<City>();
            Assert.AreEqual(Data.Estacada, estacada);
            estacada.SetProperty("Name", "Hickville");
            Assert.AreNotEqual(Data.Estacada, estacada);
            estacada = Data.Estacada.Clone<City>();
            estacada.SetProperty("ID", 7);
            Assert.AreNotEqual(Data.Estacada, estacada);
            estacada = Data.Estacada.Clone<City>();
            estacada.SetProperty("State", Data.California);
            Assert.AreNotEqual(Data.Estacada, estacada);
        }
    }
}

