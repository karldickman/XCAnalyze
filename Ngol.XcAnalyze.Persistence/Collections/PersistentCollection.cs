using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ngol.Utilities.Collections.Extensions;
using Ngol.XcAnalyze.Persistence.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace Ngol.XcAnalyze.Persistence.Collections
{
    /// <summary>
    /// Persistence repository for various classes.
    /// </summary>
    public class PersistentCollection<T> : ICollection<T>, IPersistentCollection<T>
    {
        #region Properties

        /// <summary>
        /// The queue of items to delete.
        /// </summary>
        protected internal readonly ICollection<T> DeleteQueue;

        /// <summary>
        /// The queue of items to insert.
        /// </summary>
        protected internal readonly ICollection<T> InsertQueue;

        /// <summary>
        /// The queue of items to update.
        /// </summary>
        protected internal readonly ICollection<T> UpdateQueue;

        /// <summary>
        /// The queryable to which to delegate all queryable calls.
        /// </summary>
        protected readonly IQueryable<T> InnerQueryable;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new repository.
        /// </summary>
        public PersistentCollection(IQueryable<T> queryable)
        {
            InnerQueryable = queryable;
            DeleteQueue = new List<T>();
            InsertQueue = new List<T>();
            UpdateQueue = new List<T>();
        }

        #endregion

        #region IRepository[T] implementation

        /// <inheritdoc />
        public int ClearDeleteQueue()
        {
            int count = DeleteQueue.Count;
            DeleteQueue.Clear();
            return count;
        }

        /// <inheritdoc />
        public int ClearInsertQueue()
        {
            int count = InsertQueue.Count;
            InsertQueue.Clear();
            return count;
        }

        /// <inheritdoc />
        public int ClearUpdateQueue()
        {
            int count = UpdateQueue.Count;
            UpdateQueue.Clear();
            return count;
        }

        /// <inheritdoc />
        public bool IsPersisted(T item)
        {
            return ((IQueryable<T>)this).Contains(item);
        }

        /// <inheritdoc />
        public void QueueDelete(T item)
        {
            DeleteQueue.Add(item);
        }

        /// <inheritdoc />
        public void QueueInsert(T item)
        {
            InsertQueue.Add(item);
        }

        /// <inheritdoc />
        public void QueueUpdate(T item)
        {
            UpdateQueue.Add(item);
        }

        /// <inheritdoc />
        public bool UnQueueDelete(T item)
        {
            return DeleteQueue.Remove(item);
        }

        /// <inheritdoc />
        public bool UnQueueInsert(T item)
        {
            return InsertQueue.Remove(item);
        }

        /// <inheritdoc />
        public bool UnQueueUpdate(T item)
        {
            return UpdateQueue.Remove(item);
        }

        #region IQueryable[T] implementation

        /// <inheritdoc />
        public Type ElementType
        {
            get { return InnerQueryable.ElementType; }
        }

        /// <inheritdoc />
        public Expression Expression
        {
            get { return InnerQueryable.Expression; }
        }

        /// <inheritdoc />
        public IQueryProvider Provider
        {
            get { return InnerQueryable.Provider; }
        }

        #endregion
        
        #endregion

        #region ICollection[T] implementation

        /// <inheritdoc />
        public int Count
        {
            get { return this.Count(); }
        }

        /// <inheritdoc />
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        void ICollection<T>.Add(T item)
        {
            QueueInsert(item);
        }

        /// <exception cref="NotSupportedException">
        /// Thrown if this method is called.
        /// </exception>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        bool ICollection<T>.Contains(T item)
        {
            return IsPersisted(item) || InsertQueue.Contains(item);
        }

        /// <inheritdoc />
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        bool ICollection<T>.Remove(T item)
        {
            QueueDelete(item);
            return true;
        }

        #region IEnumerable[T] implementation

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #endregion
        
        #endregion
    }
}

