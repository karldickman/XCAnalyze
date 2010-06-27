using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XCAnalyze.Collections
{
    public class XDictionary<K, T> : Dictionary<K, T>, IXDictionary<K, T>
    {
        public XDictionary () : base() {}
        
        public XDictionary(IDictionary<K, T> dictionary) : base(dictionary) {}

        public XDictionary(IEqualityComparer<K> comparer) : base(comparer) {}
    
        public XDictionary(int capacity) : base(capacity) {}
    
        public XDictionary(IDictionary<K, T> dictionary,
            IEqualityComparer<K> comparer)
        : base(dictionary, comparer) {}
    
        public XDictionary(int capacity, IEqualityComparer<K> comparer)
        : base(capacity, comparer) {}
    
        protected XDictionary(SerializationInfo info, StreamingContext context)
        : base(info, context) {}
    
        public ReadOnlyDictionary<K, T> AsReadOnly ()
        {
            return new ReadOnlyDictionary<K, T> (this);
        }
    }
}
