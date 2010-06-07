using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace XCAnalyze.Io.Sql
{    
    public class MySqlCreationScriptReader : IReader<IList<string>>
    {       
        /// <summary>
        /// The default sting used to delimit SQL commands.
        /// </summary>
        public const string DEFAULT_DELIMITER = ";";
        
        public const lineMode CREATE_NEW = lineMode.CREATE_NEW;
        public const lineMode EXTEND_PREVIOUS = lineMode.EXTEND_PREVIOUS;
        
        /// <summary>
        /// The final commands that will be returned from the read function.
        /// </summary>
        protected internal IList<string> Commands { get; set; }
        
        /// <summary>
        /// The line command currently being used.
        /// </summary>
        protected internal string Delimiter { get; set; }
        
        /// <summary>
        /// The path to the file that contains the creation script.
        /// </summary>
        protected internal string FilePath { get; set; }
        
        /// <summary>
        /// The mode in which lines are being added to the results.
        /// </summary>
        protected internal lineMode LineMode { get; set; }
        
        /// <summary>
        /// Create a new reader that reads from a particular file.
        /// </summary>
        /// <param name="path">
        /// The path to the file that contains the creation script.
        /// </param>
        protected internal MySqlCreationScriptReader(string path)
        {
            FilePath = path;
            Commands = new List<string>();
            Commands.Add("");
            LineMode = EXTEND_PREVIOUS;
            Delimiter = DEFAULT_DELIMITER;
        }
        
        /// <summary>
        /// Create a new reader that reads from a particular file.
        /// </summary>
        /// <param name="path">
        /// The path to the file that contains the creation script.
        /// </param>
        public static MySqlCreationScriptReader NewInstance (string path)
        {
            if(!File.Exists(path))
            {
                return null;
            }
            return new MySqlCreationScriptReader (path);
        }
        
        /// <summary>
        /// Cease operations of the reader.
        /// </summary>
        public void Close ()
        {
            Commands = null;
        }
        
        /// <summary>
        /// Read all commands out of a particular creation script.
        /// </summary>
        /// <returns>
        /// The <see cref="IList<System.String>"/> of commands in the file.
        /// </returns>
        public IList<string> Read ()
        {
            return Read (File.ReadAllLines (FilePath));
        }
        
        /// <summary>
        /// Read all commands out of a particular list of physical lines from a
        /// file.
        /// </summary>
        /// <param name="lines">
        /// The physical lines to be read.
        /// </param>
        /// <returns>
        /// The <see cref="IList<System.String>"/> of commands in the file.
        /// </returns>
        public IList<string> Read (string[] lines)
        {
            foreach (string line in lines)
            {
                ReadLine (line);
            }
            return Commands;
        }
        
        /// <summary>
        /// Read a line and set the delimiter and line mode appropriate.
        /// </summary>
        /// <param name="line">
        /// The line to be processed.
        /// </param>
        protected internal void ReadLine(string line)
        {
            line = line.Trim();
            bool withDelimiter = line.EndsWith(Delimiter);
            if(line.ToUpper().StartsWith("DELIMITER"))
            {
                Delimiter = line.Substring("DELIMITER".Length).Trim();
                LineMode = CREATE_NEW;
                return;
            }
            //Trim the delimiter off the command
            if(withDelimiter)
            {
                line = line.Substring(0, line.Length - Delimiter.Length).Trim();
            }
            switch(LineMode)
            {
                case CREATE_NEW:
                    Commands.Add(line);
                    if(!withDelimiter)
                    {
                        LineMode = EXTEND_PREVIOUS;
                    }
                    break;
                case EXTEND_PREVIOUS:
                    Commands[Commands.Count - 1] += " " + line;
                    if(withDelimiter)
                    {
                        LineMode = CREATE_NEW;
                    }
                    break;
            }
        }
        
        public enum lineMode { CREATE_NEW, EXTEND_PREVIOUS };
    }
    
    [TestFixture]
    public class TestMySqlCreationScriptReader
    {
        public const MySqlCreationScriptReader.lineMode CREATE_NEW = MySqlCreationScriptReader.lineMode.CREATE_NEW;
        public const MySqlCreationScriptReader.lineMode EXTEND_PREVIOUS = MySqlCreationScriptReader.lineMode.EXTEND_PREVIOUS;
        
        protected internal MySqlCreationScriptReader Reader { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Reader = MySqlCreationScriptReader.NewInstance (SupportFiles.GetPath ("xca_create.mysql"));
        }
        
        [Test]
        public void TestRead ()
        {
            Reader.Read ();
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
