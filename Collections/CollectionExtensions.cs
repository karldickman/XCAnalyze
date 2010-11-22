using System;
using System.Collections.Generic;

namespace XCAnalyze.Collections
{
    public static class Extensions
    {
        public static void AddRange<T> (this IList<T> self, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                self.Add (item);
            }
        }
    }
}
