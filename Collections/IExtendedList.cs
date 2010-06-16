using System;

namespace XCAnalyze.Collections
{
    public interface IExtendedList<T> : System.Collections.Generic.IList<T>
    {
        void Sort();
    }
}
