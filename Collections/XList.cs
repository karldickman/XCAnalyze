using System;
using System.Collections.Generic;

namespace XCAnalyze.Collections
{
    public class XList<T> : List<T>, IXList<T> 
    {
        public XList() : base() {}
    
        public XList(IEnumerable<T> enumerable) : base(enumerable) {}
    
        public XList(int capacity) : base(capacity) {}
    }        
}
