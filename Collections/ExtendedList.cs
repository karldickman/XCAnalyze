using System;
using System.Collections.Generic;

namespace XCAnalyze.Collections
{
    public class ExtendedList<T> : List<T>, IExtendedList<T> 
    {
        public ExtendedList() : base() {}
    
        public ExtendedList(IEnumerable<T> enumerable) : base(enumerable) {}
    
        public ExtendedList(int capacity) : base(capacity) {}
    }        
}
