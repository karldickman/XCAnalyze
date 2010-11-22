using System;

using XCAnalyze.IO.Sql;

namespace XCAnalyze.IO
{
    /// <summary>
    /// The <see cref="IReader"/> for the default file format of XCAnalyze, .xca
    /// files.
    /// </summary>
    public partial class XcaReader : SqliteReader
    {
        public XcaReader(string fileName) : base(fileName)
        {
        }
    }
}
