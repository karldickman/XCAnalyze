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
        /// The session factory for the current application.
        /// </summary>
        protected readonly ISessionFactory SessionFactory;

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
            SessionFactory = sessionFactory;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected void UsingTransaction(Action<ISession, ITransaction> action)
        {
            if(action == null) throw new ArgumentNullException("action");
            using(ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    action(session, transaction);
                }
            }
        }

        /// <inheritdoc />
        protected TReturn UsingTransaction<TReturn>(Func<ISession, ITransaction, TReturn> function)
        {
            if(function == null)
                throw new ArgumentNullException("function");
            using(ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    return function(session, transaction);
                }
            }
        }

        #endregion

        #region IRepository[T] implementation

        /// <inheritdoc />
        public void Update(T item)
        {
            UsingTransaction((session, transaction) =>
            {
                session.Update(item);
                transaction.Commit();
            });
        }

        #region ICollection[T] implementation

        /// <inheritdoc />
        public int Count
        {
            get
            {
                using(ISession session = SessionFactory.OpenSession())
                {
                    return session.Query<T>().Count();
                }
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
            UsingTransaction((session, transaction) =>
            {
                session.Save(item);
                transaction.Commit();
            });
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
            using(ISession session = SessionFactory.OpenSession())
            {
                return session.Query<T>().Contains(item);
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            return UsingTransaction((session, transaction) =>
            {
                if(!Contains(item))
                {
                    return false;
                }
                session.Delete(item);
                transaction.Commit();
                return true;
            });
        }

        #region IEnumerable[T] implementation

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                return session.Query<T>().GetEnumerator();
            }
        }

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion
        
        #endregion

        #endregion

        #endregion
    }
}

