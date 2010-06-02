using System;
using System.Collections.Generic;
using System.IO;

namespace XCAnalyze
{
    public class SupportFiles
    {
        /// <summary>
        /// The directory where XCAnalyze is installed.
        /// </summary>
        public static readonly string INSTALL_DIR = Directory.GetCurrentDirectory();
        
        /// <summary>
        /// The location where support files are installed.
        /// </summary>
        public static readonly string SUPPORT_DIR = String.Join("" + Path.DirectorySeparatorChar, new string[] { INSTALL_DIR, "..", ".." });
        
        protected internal static IDictionary<string, string> Files { get; set; }
        
        static SupportFiles ()
        {
            Files = new Dictionary<string, string> ();
            Files.Add ("xca_create.sql", SUPPORT_DIR);
            Files.Add ("xca_create.sqlite", SUPPORT_DIR);
            Files.Add ("xca_create.mysql", SUPPORT_DIR);
        }
        
        public static string GetPath(string fileName)
        {
            return Files[fileName] + Path.DirectorySeparatorChar + fileName;
        }
    }
}
