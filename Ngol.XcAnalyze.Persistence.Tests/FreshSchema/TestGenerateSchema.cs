using System;
using Ngol.XcAnalyze.Model;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Ngol.XcAnalyze.Persistence.Tests.FreshSchema
{
    [TestFixture]
    public class TestGenerateSchema
    {
        [Test]
        public void TestGenerationOfSchema()
        {
            Configuration configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(State).Assembly);
            SchemaExport schemaExport = new SchemaExport(configuration);
            schemaExport.SetOutputFile("schema.sql");
            schemaExport.Execute(false, true, false);
        }
    }
}

