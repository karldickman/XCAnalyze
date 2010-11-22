using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XCAnalyze.Collections
{
    public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Add several values to the set.
        /// </summary>
        /// <param name="range">
        /// The <see cref="IEnumerable<T>"/> of values to add.
        /// </param>
        void AddRange(IEnumerable<T> range);
        
        /// <summary>
        /// A read only wrapper around this collection.
        /// </summary>
        ReadOnlyCollection<T> AsReadOnly();
        
        /// <summary>
        /// Removes all elements in the specified collection from the current
        /// set.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> to remove.
        /// </param>
        void ExceptWith(IEnumerable<T> other);
        
        /// <summary>
        /// Modifies the current set so that it contains only elements that are
        /// also in a specified collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> in which to check for
        /// intersections.
        /// </param>
        void IntersectWith(IEnumerable<T> other);
        
        /// <summary>
        /// Determines whether the current set is a property (strict) subset of
        /// a specified collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        bool IsProperSubsetOf(IEnumerable<T> other);
        
        /// <summary>
        /// Determines whether the current set is a correct superset of a
        /// specified collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        bool IsProperSupersetOf(IEnumerable<T> other);
        
        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        bool IsSubsetOf(IEnumerable<T> other);
        
        /// <summary>
        /// Determines whether the current set is a superset of a specified
        /// collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        bool IsSupersetOf(IEnumerable<T> other);
        
        /// <summary>
        /// Determines whether the current set overlaps with the specified
        /// collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        bool Overlaps(IEnumerable<T> other);
     
        /// <summary>
        /// Modifies the current set so that it contains only elements that are
        /// present either in the current set or in the specified collection,
        /// but not both. 
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        void SymmetricExceptWith(IEnumerable<T> other);
        
        /// <summary>
        /// Modifies the current set so that it contains all elements that are
        /// present in both the current set and in the specified collection.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IEnumerable<T>"/> with which to compare.
        /// </param>
        void UnionWith(IEnumerable<T> other);
    }
}
