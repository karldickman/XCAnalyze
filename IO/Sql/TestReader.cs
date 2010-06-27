using System;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{    
    abstract public class TestReader
    {
        public const string EXAMPLE_DATABASE = "xca_example";
        
        protected internal AbstractReader Reader { get; set; }
        
        abstract public void SetUp();
        
        [TearDown]
        public void TearDown ()
        {
            Reader.Dispose ();
        }
        
        [Test]
        virtual public void TestRead ()
        {
            Reader.Read ();
        }
    }
}
