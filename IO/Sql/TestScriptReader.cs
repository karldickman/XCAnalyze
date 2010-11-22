using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{
    public partial class ScriptReader
    {
#if DEBUG
        [TestFixture]
        public class Test
        {
            #region Properties
            
            #region Constants
            
            public const ScriptReader.LineMode CreateNew =
                ScriptReader.CreateNew;
            public const ScriptReader.LineMode ExtendPrevious =
                ScriptReader.ExtendPrevious;
            
            protected internal static readonly string[] Systems =
            {"mysql", "sqlite"};
            
            #endregion
            
            protected internal ScriptReader Reader { get; set; }
            protected internal IDictionary<string, ScriptReader> Readers { get; set; }
            
            #endregion
            
            #region Set up
            
            [SetUp]
            public void SetUp ()
            {
                Readers = new Dictionary<string, ScriptReader> ();
                foreach (string system in Systems)
                {
                    Readers[system] =
                        new ScriptReader (
                            SupportFiles.GetPath ("xca_create." + system));
                }
                Reader = Readers[Systems[0]];
            }
            
            #endregion
            
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
                Assert.AreEqual (ExtendPrevious, Reader.CurrentLineMode);
                //Test the first transition
                Reader.ReadLine (lineWithDelimiter);
                Assert.AreEqual (CreateNew, Reader.CurrentLineMode);
                Assert.AreEqual (1, Reader.Commands.Count);
                Assert.AreEqual (lineWithDelimiter.Substring (0, lineWithDelimiter.Length - 1), Reader.Commands[0].Trim ());
                //Test the second transition
                Reader.ReadLine (lineWithDelimiter);
                Assert.AreEqual (CreateNew, Reader.CurrentLineMode);
                Assert.AreEqual (2, Reader.Commands.Count);
                Assert.AreEqual (lineWithDelimiter.Substring (0, lineWithDelimiter.Length - 1), Reader.Commands[1]);
                //Test the third transtion
                Reader.ReadLine (fracturedLine[0]);
                Assert.AreEqual (ExtendPrevious, Reader.CurrentLineMode);
                Assert.AreEqual (3, Reader.Commands.Count);
                Assert.AreEqual (fracturedLine[0], Reader.Commands[2]);
                //Test the fourth transition
                Reader.ReadLine (fracturedLine[1]);
                Assert.AreEqual (ExtendPrevious, Reader.CurrentLineMode);
                Assert.AreEqual (3, Reader.Commands.Count);
                //Test the first transition again
                Reader.ReadLine (fracturedLine[2]);
                Assert.AreEqual (CreateNew, Reader.CurrentLineMode);
                Assert.AreEqual (3, Reader.Commands.Count);
                Assert.AreEqual (lineWithDelimiter.Substring (0, lineWithDelimiter.Length - 1), Reader.Commands[2].Trim ());
                //Test delimiter handling
                Reader.ReadLine (newDelimiter);
                Assert.AreEqual (CreateNew, Reader.CurrentLineMode);
                Assert.AreEqual ("//", Reader.Delimiter);
                Assert.AreEqual (3, Reader.Commands.Count);
                //Execute a command with the new delimiter
                Reader.ReadLine (lineWithNewDelimiter);
                Assert.AreEqual (CreateNew, Reader.CurrentLineMode);
                Assert.AreEqual (4, Reader.Commands.Count);
                Assert.AreEqual (lineWithDelimiter.Substring(0, lineWithDelimiter.Length - 1), Reader.Commands[3]);
            }
        }
#endif
    }
}
