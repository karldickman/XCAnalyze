using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XCAnalyze.Collections
{
    public class XHashSet<T> : HashSet<T>, ISet<T>
    {
        #region XCAnalyze.Collections.ISet<T> implementation
        public XHashSet() : base() {}

        public XHashSet(IEnumerable<T> items) : base(items) {}
        
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
