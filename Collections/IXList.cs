using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XCAnalyze.Collections
{
    public interface IXList<T> : IList<T>
    {      
        /// <summary>
        /// Returns a read-only wrapper to the current List.
        /// </summary>
        ReadOnlyCollection<T> AsReadOnly();
        
        /// <summary>
        /// Removes the all the elements that match the conditions defined by
        /// the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// The <see cref="Predicate<T>"/> against which to test each element of
        /// the list.
        /// </param>
        /// <returns>
        /// The number of items removed.
        /// </returns>
        int RemoveAll(Predicate<T> predicate);
        
        /// <summary>
        /// Sorts the elements in the list using the default comparer.
        /// </summary>
        void Sort();
        
        /// <summary>
        /// Sorts the elements in the list using the specified comparer.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer<T>"/> with which to make comparisons.
        /// </param>
        void Sort(IComparer<T> comparer);
        
        /// <summary>
        /// Sorts the elements in the list using the specified comparer.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="Comparison<T>"/> with which to make comparisons.
        /// </param>
        void Sort(Comparison<T> comparer);
        
        /// <summary>
        /// Sorts the elements in the list using the specified comparer.
        /// </summary>
        /// <param name="index">
        /// The index at which to start sorting.
        /// </param>
        /// <param name="count">
        /// The number of items to sort.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer<T>"/> with which to make comparisons.
        /// </param>
        void Sort(int index, int count, IComparer<T> comparer);
    }
}
