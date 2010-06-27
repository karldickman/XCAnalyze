using System;

namespace XCAnalyze.IO
{
    /// <summary>
    /// An interfaces for classes that write data.
    /// </summary>
    public interface IWriter<T> : IDisposable
    {
        /// <summary>
        /// Write a value to the target.
        /// </summary>
        /// <param name="toWrite">
        /// The value to be written.
        /// </param>
        void Write (T toWrite);
    }
}
