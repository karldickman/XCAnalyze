using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace Ngol.XcAnalyze.Model.Collections
{
    /// <summary>
    /// Repository to store <see cref="State" />s.
    /// </summary>
    public class StateRepository : IRepository<State>
    {
        #region Properties

        /// <summary>
        /// The session factory for the current application.
        /// </summary>
        protected readonly ISessionFactory SessionFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new repository for <see cref="State" />s.
        /// </summary>
        /// <param name="sessionFactory">
        /// The session factory to use.
        /// </param>
        public StateRepository(ISessionFactory sessionFactory)
        {
            if(sessionFactory == null) throw new ArgumentNullException("sessionFactory");
            SessionFactory = sessionFactory;
        }

        #endregion

        #region IRepository[state] implementation

        /// <inheritdoc />
        public void Update(State item)
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(item);
                    transaction.Commit();
                }
            }
        }

        #region ICollection[State] implementation

        /// <inheritdoc />
        public int Count
        {
            get
            {
                using(ISession session = SessionFactory.OpenSession())
                {
                    return session.Query<State>().Count();
                }
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public void Add(State item)
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(item);
                    transaction.Commit();
                }
            }
        }

        /// <exception cref="NotSupportedException">
        /// Deleting all the members in a single operation is not supported.
        /// </exception>
        void ICollection<State>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(State item)
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                return session.Query<State>().Contains(item);
            }
        }

        /// <inheritdoc />
        void ICollection<State>.CopyTo(State[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(State item)
        {
            if(!Contains(item))
                return false;
            using(ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(item);
                    transaction.Commit();
                }
            }
            return true;
        }

        #region IEnumerable[State] implementation

        /// <inheritdoc />
        public IEnumerator<State> GetEnumerator()
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                return session.Query<State>().GetEnumerator();
            }
        }

        #region IEnumerable implementation

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #endregion

        #endregion

        #endregion
    }
}

