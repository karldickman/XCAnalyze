using System;
using System.Collections.Generic;
using System.IO;

namespace XCAnalyze.Data.Tests
{
    internal static class SupportFiles
    {
        #region Properties
        
        #region Files
        
        internal const string SqliteExampleFile = "xca_example.db";
        
        #endregion
        
        /// <summary>
        /// The support files needed to run the tests.
        /// </summary>
        private static readonly IDictionary<string, string> Files;
                
        /// <summary>
        /// The directory where XCAnalyze is installed.
        /// </summary>
        internal static readonly string InstallationDirectory = Directory.GetCurrentDirectory();
        
        /// <summary>
        /// The directory where support files for XCAnalyze are kept.
        /// </summary>
        internal static readonly string SupportDirectory = string.Join("" +
            Path.DirectorySeparatorChar, new string[] { InstallationDirectory, "..", "..", ".." });
        
        #endregion
        
        #region Constructors
        
        static SupportFiles()
        {
            Files = new Dictionary<string, string>();
            Files[SqliteExampleFile] = SupportDirectory;
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Get the path to a file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to search for.
        /// </param>
        /// <returns>
        /// The path to the file.
        /// </returns>
        internal static string GetPath(string fileName)
        {
            return Files[fileName] + Path.DirectorySeparatorChar + fileName;
        }
        
        #endregion
    }
}

