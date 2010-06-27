using System;
using System.Collections.Generic;
using System.IO;

namespace XCAnalyze.IO.Sql
{    
    public class ScriptReader : IReader<IList<string>>
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
        protected internal ScriptReader(string path)
        {
            FilePath = path;
            Commands = new List<string>();
            Commands.Add("");
            LineMode = EXTEND_PREVIOUS;
            Delimiter = DEFAULT_DELIMITER;
        }
        
        /// <summary>
        /// Cease operations of the reader.
        /// </summary>
        public void Dispose ()
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
}
