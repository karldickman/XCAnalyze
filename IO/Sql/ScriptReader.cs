using System;
using System.Collections.Generic;
using System.IO;

namespace XCAnalyze.IO.Sql
{    
    public partial class ScriptReader : IDisposable
    {
        #region Properties
        
        #region Constants
        
        /// <summary>
        /// The default sting used to delimit SQL commands.
        /// </summary>
        public const string DefaultDelimiter = ";";
        
        public const LineMode CreateNew = LineMode.CreateNew;
        public const LineMode ExtendPrevious = LineMode.ExtendPrevious;
        
        #endregion
        
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
        protected internal LineMode CurrentLineMode { get; set; }
        
        #endregion
        
        #region Constructors
        
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
            CurrentLineMode = ExtendPrevious;
            Delimiter = DefaultDelimiter;
        }
        
        #endregion
        
        #region IDisposable implementation
        
        void IDisposable.Dispose ()
        {
            Commands = null;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Cease operations of the reader.
        /// </summary>
        public void Close ()
        {
            ((IDisposable)this).Dispose ();
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
                CurrentLineMode = CreateNew;
                return;
            }
            //Trim the delimiter off the command
            if(withDelimiter)
            {
                line = line.Substring(0, line.Length - Delimiter.Length).Trim();
            }
            switch(CurrentLineMode)
            {
                case CreateNew:
                    Commands.Add(line);
                    if(!withDelimiter)
                    {
                        CurrentLineMode = ExtendPrevious;
                    }
                    break;
                case ExtendPrevious:
                    Commands[Commands.Count - 1] += " " + line;
                    if(withDelimiter)
                    {
                        CurrentLineMode = CreateNew;
                    }
                    break;
            }
        }
        
        #endregion
        
        public enum LineMode { CreateNew, ExtendPrevious };
    }
}
