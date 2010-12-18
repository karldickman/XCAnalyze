using System;
using System.Collections.Generic;

namespace XCAnalyze.Collections
{
    public static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                self.Add(item);
            }
        }
        
        public static T[] ToArray<T>(this ICollection<T> self)
        {
            T[] array = new T[self.Count];
            int count = 0;
            foreach(T item in self)
            {
                array[count] = item;
                count++;
            }
            return array;
        }
    }
}
