using System;

namespace XCAnalyze.IO
{
    /// <summary>
    /// An interface for classes that read data.
    /// </summary>
    public interface IReader<T> : IDisposable
    {
        /// <summary>
        /// Read the source.
        /// </summary>
        T Read();
    }
}
