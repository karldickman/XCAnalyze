using System;
using System.Collections.Generic;

namespace XCAnalyze.Collections
{
    public interface IXDictionary<K, T> : IDictionary<K, T>
    {
        ReadOnlyDictionary<K, T> AsReadOnly();
    }
}
