using System;

using XCAnalyze.IO.Sql;

namespace XCAnalyze.IO
{
    /// <summary>
    /// The <see cref="IWriter"/> for the default file format of XCAnalyze, .xca
    /// files.
    /// </summary>
    public class XcaWriter : SqliteWriter
    {
        public XcaWriter (string fileName) : base(fileName)
        {
        }
    }
}
