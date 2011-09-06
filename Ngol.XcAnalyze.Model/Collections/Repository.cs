using System;
using System.Linq;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Ngol.XcAnalyze.Model.Collections
{
    /// <summary>
    /// Persistence repository for various classes.
    /// </summary>
    public class Repository<T> : IRepository<T>
    {
        #region Properties

        /// <summary>
        /// The NHibernate session to use to communicate with the database.
        /// </summary>
        protected readonly ISession Session;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new repository.
        /// </summary>
        /// <param name="sessionFactory">
        /// The <see cref="ISessionFactory"/> to use.
        /// </param>
        public Repository(ISessionFactory sessionFactory)
        {
            Session = sessionFactory.OpenSession();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get an object against which LINQ queries can be made.
        /// </summary>
        protected IQueryable<T> Query()
        {
            return Session.Query<T>();
        }

        #endregion

        #region IRepository[T] implementation

        /// <inheritdoc />
        public void Update(T item)
        {
            using(ITransaction transaction = Session.BeginTransaction())
            {
                Session.Update(item);
                transaction.Commit();
            }
        }

        #region ICollection[T] implementation

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return Query().Count();
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            using(ITransaction transaction = Session.BeginTransaction())
            {
                Session.Save(item);
                transaction.Commit();
            }
        }

        /// <exception cref="NotSupportedException">
        /// Thrown if this method is called.
        /// </exception>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return Query().Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            using(ITransaction transaction = Session.BeginTransaction())
            {
                if(!Contains(item))
                {
                    return false;
                }
                Session.Delete(item);
                transaction.Commit();
                return true;
            }
        }

        #region IEnumerable[T] implementation

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return Query().GetEnumerator();
        }

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion
        
        #endregion

        #endregion

        #region IDisposable implementation

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of this instance.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(Session != null)
                {
                    Session.Dispose();
                }
            }
        }

        #endregion

        #endregion
    }
}

