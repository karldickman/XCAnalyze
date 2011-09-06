using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ngol.XcAnalyze.Model.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace Ngol.XcAnalyze.Model.Collections
{
    /// <summary>
    /// Repository for storing <see cref="City" />s.
    /// </summary>
    public class CityRepository : IRepository<City>
    {
        #region Properties

        /// <summary>
        /// The NHibernate session factory for the current application.
        /// </summary>
        protected readonly ISessionFactory SessionFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new city repository.
        /// </summary>
        /// <param name="sessionFactory">
        /// The NHibernate <see cref="ISessionFactory"/> to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="sessionFactory"/> is <see langword="null" />.
        /// </exception>
        public CityRepository(ISessionFactory sessionFactory)
        {
            if(sessionFactory == null)
                throw new ArgumentNullException("sessionFactory");
            SessionFactory = sessionFactory;
        }

        #endregion

        #region IRepository[City] implementation

        /// <inheritdoc />
        public void Update(City item)
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

        #region ICollection[City] implementation

        /// <inheritdoc />
        public int Count
        {
            get
            {
                using(ISession session = SessionFactory.OpenSession())
                {
                    return session.Query<City>().Count();
                }
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public void Add(City item)
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

        /// <inheritdoc />
        void ICollection<City>.Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public bool Contains(City item)
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                return session.Query<City>().Contains(item);
            }
        }

        /// <inheritdoc />
        public void CopyTo(City[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(City item)
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    if(!Contains(item))
                    {
                        return false;
                    }
                    session.Delete(item);
                    transaction.Commit();
                    return true;
                }
            }
        }

        #region IEnumerable[City] implementation

        /// <inheritdoc />
        public IEnumerator<City> GetEnumerator()
        {
            using(ISession session = SessionFactory.OpenSession())
            {
                return session.Query<City>().GetEnumerator();
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

