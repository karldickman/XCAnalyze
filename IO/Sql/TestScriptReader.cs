using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{
    [TestFixture]
    public class TestSqlScriptReader
    {
        public const ScriptReader.lineMode CREATE_NEW = ScriptReader.lineMode.CREATE_NEW;
        public const ScriptReader.lineMode EXTEND_PREVIOUS = ScriptReader.lineMode.EXTEND_PREVIOUS;
        
        protected internal static readonly string[] systems = {"mysql", "sqlite"};
        protected internal ScriptReader Reader { get; set; }
        protected internal IDictionary<string, ScriptReader> Readers { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Readers = new Dictionary<string, ScriptReader> ();
            foreach (string system in systems)
            {
                Readers[system] = new ScriptReader (SupportFiles.GetPath ("xca_create." + system));
            }
            Reader = Readers[systems[0]];
        }
        
        [Test]
        public void TestRead ()
        {
            foreach (ScriptReader reader in Readers.Values)
            {
                reader.Read ();
            }
        }
        
        [Test]
        public void TestReadLine ()
        {
            string lineWithDelimiter = "SELECT thing FROM stuff;";
            string lineWithNewDelimiter = "SELECT thing FROM stuff //";
            string[] fracturedLine = { "SELECT", "thing", "FROM stuff;" };
            string newDelimiter = "  DELiMITER  // ";
            Assert.AreEqual (EXTEND_PREVIOUS, Reader.LineMode);
            //Test the first transition
            Reader.ReadLine (lineWithDelimiter);
            Assert.AreEqual (CREATE_NEW, Reader.LineMode);
            Assert.AreEqual (1, Reader.Commands.Count);
            Assert.AreEqual (lineWithDelimiter.Substring (0, lineWithDelimiter.Length - 1), Reader.Commands[0].Trim ());
            //Test the second transition
            Reader.ReadLine (lineWithDelimiter);
            Assert.AreEqual (CREATE_NEW, Reader.LineMode);
            Assert.AreEqual (2, Reader.Commands.Count);
            Assert.AreEqual (lineWithDelimiter.Substring (0, lineWithDelimiter.Length - 1), Reader.Commands[1]);
            //Test the third transtion
            Reader.ReadLine (fracturedLine[0]);
            Assert.AreEqual (EXTEND_PREVIOUS, Reader.LineMode);
            Assert.AreEqual (3, Reader.Commands.Count);
            Assert.AreEqual (fracturedLine[0], Reader.Commands[2]);
            //Test the fourth transition
            Reader.ReadLine (fracturedLine[1]);
            Assert.AreEqual (EXTEND_PREVIOUS, Reader.LineMode);
            Assert.AreEqual (3, Reader.Commands.Count);
            //Test the first transition again
            Reader.ReadLine (fracturedLine[2]);
            Assert.AreEqual (CREATE_NEW, Reader.LineMode);
            Assert.AreEqual (3, Reader.Commands.Count);
            Assert.AreEqual (lineWithDelimiter.Substring (0, lineWithDelimiter.Length - 1), Reader.Commands[2].Trim ());
            //Test delimiter handling
            Reader.ReadLine (newDelimiter);
            Assert.AreEqual (CREATE_NEW, Reader.LineMode);
            Assert.AreEqual ("//", Reader.Delimiter);
            Assert.AreEqual (3, Reader.Commands.Count);
            //Execute a command with the new delimiter
            Reader.ReadLine (lineWithNewDelimiter);
            Assert.AreEqual (CREATE_NEW, Reader.LineMode);
            Assert.AreEqual (4, Reader.Commands.Count);
            Assert.AreEqual (lineWithDelimiter.Substring(0, lineWithDelimiter.Length - 1), Reader.Commands[3]);
        }
    }
}
