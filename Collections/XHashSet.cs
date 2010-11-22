using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XCAnalyze.Collections
{
    public class XHashSet<T> : HashSet<T>, ISet<T>
    {
        public XHashSet() : base() {}
        
        public XHashSet(IEnumerable<T> items) : base(items) {}
        
        public XHashSet(IEqualityComparer<T> comparer) : base(comparer) {}
        
        public XHashSet(IEnumerable<T> items, IEqualityComparer<T> comparer)
        : base(items, comparer) {}
        
        #region XCAnalyze.Collections.ISet[T] implementation       
        
        public void AddRange (IEnumerable<T> range)
        {
            foreach (T item in range) 
            {
                Add (item);
            }
        }
        
        public ReadOnlyCollection<T> AsReadOnly ()
        {
            return new ReadOnlyCollection<T> (new List<T>(this));
        }
        
        new public void UnionWith (IEnumerable<T> other)
        {
            base.UnionWith(other);
        }
        
        #endregion
    }
}
