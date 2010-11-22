using System;
using System.Collections.Generic;

namespace XCAnalyze.Hytek
{
    /// <summary>
    /// The interface to which all formatters must adhere.
    /// </summary>
    public interface IFormatter<T>
    {
        /// <summary>
        /// Format a value into a list of lines.
        /// </summary>
        /// <param name="thing">
        /// The value to format.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines.
        /// </returns>
        IList<string> Format (T thing);
    }
}

