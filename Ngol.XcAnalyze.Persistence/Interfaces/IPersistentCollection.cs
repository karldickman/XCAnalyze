using System;
using System.Collections.Generic;
using System.Linq;

namespace Ngol.XcAnalyze.Persistence.Interfaces
{
    /// <summary>
    /// Generic interface to be implemented by all repository classes.
    /// </summary>
    public interface IPersistentCollection<T> : IQueryable<T>
    {
        /// <summary>
        /// Clear the deletion queue.
        /// </summary>
        /// <returns>
        /// The number of items that were in the queue when it was cleared.
        /// </returns>
        int ClearDeleteQueue();

        /// <summary>
        /// Clear the insertion queue.
        /// </summary>
        /// <returns>
        /// The number of items that were in the queue when it was cleared.
        /// </returns>
        int ClearInsertQueue();

        /// <summary>
        /// Clear the queue of items to be updated.
        /// </summary>
        /// <returns>
        /// The number of items that were in the queue when it was cleared.
        /// </returns>
        int ClearUpdateQueue();

        /// <summary>
        /// Determine whether an item is persisted.
        /// </summary>
        bool IsPersisted(T item);

        /// <summary>
        /// Queue an item for deletion.
        /// </summary>
        /// <param name="item">
        /// The item to queue.
        /// </param>
        void QueueDelete(T item);

        /// <summary>
        /// Queue an item for insertion.
        /// </summary>
        /// <param name="item">
        /// The item to queue.
        /// </param>
        void QueueInsert(T item);

        /// <summary>
        /// Queue an item to be updated.
        /// </summary>
        /// <param name="item">
        /// The item to queue.
        /// </param>
        void QueueUpdate(T item);

        /// <summary>
        /// Unqueue an item that was queued to be deleted.
        /// </summary>
        /// <param name="item">
        /// The item to unqueue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the item was in the queue,
        /// <see langword="false" /> if the item was not in the queue.
        /// </returns>
        bool UnQueueDelete(T item);

        /// <summary>
        /// Unqueue an item that was queued to be inserted.
        /// </summary>
        /// <param name="item">
        /// The item to unqueue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the item was in the queue,
        /// <see langword="false" /> if the item was not in the queue.
        /// </returns>
        bool UnQueueInsert(T item);

        /// <summary>
        /// Unqueue an item that was queued to be updated.
        /// </summary>
        /// <param name="item">
        /// The item to unqueue.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the item was in the queue,
        /// <see langword="false" /> if the item was not in the queue.
        /// </returns>
        bool UnQueueUpdate(T item);
    }
}

