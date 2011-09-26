using System;
using System.Collections.Generic;

namespace Ngol.XcAnalyze.Persistence.Interfaces
{
    /// <summary>
    /// Useful extension methods for IPersistentCollection&gt;T&lt;.
    /// </summary>
    public static class IPersistentCollectionExtensions
    {
        /// <summary>
        /// Queue several <paramref name="items"/> to be inserted.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to insert the <paramref name="items"/>.
        /// </param>
        /// <param name="items">
        /// The items to be queued.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="collection"/>.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any argument is <see langword="null" />.
        /// </exception>
        public static void QueueInserts<T>(this IPersistentCollection<T> collection, IEnumerable<T> items)
        {
            if(collection == null)
                throw new ArgumentNullException("collection");
            if(items == null)
                throw new ArgumentNullException("items");
            foreach(T item in items)
            {
                collection.QueueInsert(item);
            }
        }
    }
}

