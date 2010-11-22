using System;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{
    public abstract partial class AbstractReader
    {
        #if DEBUG
        public abstract class TestReader
        {
            public const string ExampleDatabase = "xca_example";

            protected AbstractReader Reader { get; set; }

            public abstract void SetUp ();

            [TearDown]
            public void TearDown ()
            {
                Reader.Close ();
            }

            [Test]
            public virtual void TestRead ()
            {
                Reader.Read ();
            }
        }
        #endif
    }
}
