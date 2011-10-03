using System;
using System.Linq;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.Persistence.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using NHibernate.Cfg;

namespace Ngol.XcAnalyze.Persistence.Collections
{
    /// <summary>
    /// The container that handles all communication with the database.
    /// </summary>
    public class PersistenceContainer : IDisposable
    {
        #region Properties

        #region Physical implementation

        private readonly ISession _session;

        #endregion

        /// <summary>
        /// The <see cref="City" />s.
        /// </summary>
        public IPersistentCollection<City> Cities
        {
            get { return CityCollection; }
        }

        /// <summary>
        /// The <see cref="Conference" />s.
        /// </summary>
        public IPersistentCollection<Conference> Conferences
        {
            get { return ConferenceCollection; }
        }

        /// <summary>
        /// The <see cref="Meet" />s.
        /// </summary>
        public IPersistentCollection<Meet> Meets
        {
            get { return MeetCollection; }
        }

        /// <summary>
        /// The <see cref="MeetInstance" />s.
        /// </summary>
        public IPersistentCollection<MeetInstance> MeetInstances
        {
            get { return MeetInstanceCollection; }
        }

        /// <summary>
        /// The <see cref="Performance" />s.
        /// </summary>
        public IPersistentCollection<Performance> Performances
        {
            get { return PerformanceCollection; }
        }

        /// <summary>
        /// The <see cref="Race" />s.
        /// </summary>
        public IPersistentCollection<Race> Races
        {
            get { return RaceCollection; }
        }

        /// <summary>
        /// The <see cref="Runner" />s.
        /// </summary>
        public IPersistentCollection<Runner> Runners
        {
            get { return RunnerCollection; }
        }

        /// <summary>
        /// The <see cref="State" />s.
        /// </summary>
        public IPersistentCollection<State> States
        {
            get { return StateCollection; }
        }

        /// <summary>
        /// The <see cref="Team" />s.
        /// </summary>
        public IPersistentCollection<Team> Teams
        {
            get { return TeamCollection; }
        }

        /// <summary>
        /// The <see cref="Venue" />s.
        /// </summary>
        public IPersistentCollection<Venue> Venues
        {
            get { return VenueCollection; }
        }

        /// <summary>
        /// Backing store for <see cref="Cities" />.
        /// </summary>
        protected readonly PersistentCollection<City> CityCollection;

        /// <summary>
        /// Backing store for <see cref="Conferences" />.
        /// </summary>
        protected readonly PersistentCollection<Conference> ConferenceCollection;

        /// <summary>
        /// Backing store for <see cref="Meets" />.
        /// </summary>
        protected readonly PersistentCollection<Meet> MeetCollection;

        /// <summary>
        /// Backing store for <see cref="MeetInstances" />.
        /// </summary>
        protected readonly PersistentCollection<MeetInstance> MeetInstanceCollection;

        /// <summary>
        /// Backing store for <see cref="Performances" />.
        /// </summary>
        protected readonly PersistentCollection<Performance> PerformanceCollection;

        /// <summary>
        /// Backing store for <see cref="Races" />.
        /// </summary>
        protected readonly PersistentCollection<Race> RaceCollection;

        /// <summary>
        /// Backing store for <see cref="Runners" />.
        /// </summary>
        protected readonly PersistentCollection<Runner> RunnerCollection;

        /// <summary>
        /// Backing store for <see cref="States" />.
        /// </summary>
        protected readonly PersistentCollection<State> StateCollection;

        /// <summary>
        /// Backing store for <see cref="Teams" />.
        /// </summary>
        protected readonly PersistentCollection<Team> TeamCollection;

        /// <summary>
        /// Backing store for <see cref="Venues" />.
        /// </summary>
        protected readonly PersistentCollection<Venue> VenueCollection;

        /// <summary>
        /// Has <see cref="IDisposable.Dispose" /> been called on this instance yet.
        /// </summary>
        protected bool IsDisposed
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="ISession" /> used to communicate with the database.
        /// </summary>
        protected ISession Session
        {
            get { return _session; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="PersistenceContainer" />.
        /// </summary>
        public PersistenceContainer()
        {
            Configuration configuration = new Configuration();
            configuration.Configure();
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            _session = sessionFactory.OpenSession();
            CityCollection = new PersistentCollection<City>(Session.Query<City>());
            ConferenceCollection = new PersistentCollection<Conference>(Session.Query<Conference>());
            MeetCollection = new PersistentCollection<Meet>(Session.Query<Meet>());
            MeetInstanceCollection = new PersistentCollection<MeetInstance>(Session.Query<MeetInstance>());
            PerformanceCollection = new PersistentCollection<Performance>(Session.Query<Performance>());
            RaceCollection = new PersistentCollection<Race>(Session.Query<Race>());
            RunnerCollection = new PersistentCollection<Runner>(Session.Query<Runner>());
            StateCollection = new PersistentCollection<State>(Session.Query<State>());
            TeamCollection = new PersistentCollection<Team>(Session.Query<Team>());
            VenueCollection = new PersistentCollection<Venue>(Session.Query<Venue>());
            IsDisposed = false;
        }

        /// <summary>
        /// Construct a new <see cref="PersistenceContainer" />.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file from which to read the configuration information.
        /// </param>
        public PersistenceContainer(string fileName)
        {
            Configuration configuration = new Configuration();
            configuration.Configure(fileName);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            _session = sessionFactory.OpenSession();
            CityCollection = new PersistentCollection<City>(Session.Query<City>());
            ConferenceCollection = new PersistentCollection<Conference>(Session.Query<Conference>());
            MeetCollection = new PersistentCollection<Meet>(Session.Query<Meet>());
            MeetInstanceCollection = new PersistentCollection<MeetInstance>(Session.Query<MeetInstance>());
            PerformanceCollection = new PersistentCollection<Performance>(Session.Query<Performance>());
            RaceCollection = new PersistentCollection<Race>(Session.Query<Race>());
            RunnerCollection = new PersistentCollection<Runner>(Session.Query<Runner>());
            StateCollection = new PersistentCollection<State>(Session.Query<State>());
            TeamCollection = new PersistentCollection<Team>(Session.Query<Team>());
            VenueCollection = new PersistentCollection<Venue>(Session.Query<Venue>());
            IsDisposed = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Persist all changes on the constituent containers to the database.
        /// </summary>
        public void SaveChanges()
        {
            using(ITransaction transaction = Session.BeginTransaction())
            {
                // States
                SaveUpdates(StateCollection.UpdateQueue);
                SaveInserts(StateCollection.InsertQueue);
                SaveDeletes(StateCollection.DeleteQueue);
                // Cities
                SaveUpdates(CityCollection.UpdateQueue);
                SaveInserts(CityCollection.InsertQueue);
                SaveDeletes(CityCollection.DeleteQueue);
                // Venues
                SaveUpdates(VenueCollection.UpdateQueue);
                SaveInserts(VenueCollection.InsertQueue);
                SaveDeletes(VenueCollection.DeleteQueue);
                // Conferences
                SaveUpdates(ConferenceCollection.UpdateQueue);
                SaveInserts(ConferenceCollection.InsertQueue);
                SaveDeletes(ConferenceCollection.DeleteQueue);
                // Teams
                SaveUpdates(TeamCollection.UpdateQueue);
                SaveInserts(TeamCollection.InsertQueue);
                SaveDeletes(TeamCollection.DeleteQueue);
                // Runners
                SaveUpdates(RunnerCollection.UpdateQueue);
                SaveInserts(RunnerCollection.InsertQueue);
                SaveDeletes(RunnerCollection.DeleteQueue);
                // Meets
                SaveUpdates(MeetCollection.UpdateQueue);
                SaveInserts(MeetCollection.InsertQueue);
                SaveDeletes(MeetCollection.DeleteQueue);
                // Meet instances
                SaveUpdates(MeetInstanceCollection.UpdateQueue);
                SaveInserts(MeetInstanceCollection.InsertQueue);
                SaveDeletes(MeetInstanceCollection.DeleteQueue);
                // Races
                SaveUpdates(RaceCollection.UpdateQueue);
                SaveInserts(RaceCollection.InsertQueue);
                SaveDeletes(RaceCollection.DeleteQueue);
                // Performances
                SaveUpdates(PerformanceCollection.UpdateQueue);
                SaveInserts(PerformanceCollection.InsertQueue);
                SaveDeletes(PerformanceCollection.DeleteQueue);
                // Commit transaction
                transaction.Commit();
            }
        }

        /// <summary>
        /// Save a sequence of deletes.
        /// </summary>
        /// <param name="deletes">
        /// The items to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="deletes"/> is <see langword="null" />.
        /// </exception>
        protected void SaveDeletes<T>(ICollection<T> deletes)
        {
            if(deletes == null)
                throw new ArgumentNullException("deletes");
            foreach(T item in deletes.ToList())
            {
                Session.Delete(item);
                deletes.Remove(item);
            }
        }

        /// <summary>
        /// Save a sequence of inserts.
        /// </summary>
        /// <param name="inserts">
        /// The items to insert.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="inserts"/> is <see langword="null" />.
        /// </exception>
        protected void SaveInserts<T>(ICollection<T> inserts)
        {
            if(inserts == null)
                throw new ArgumentNullException("inserts");
            foreach(T item in inserts.ToList())
            {
                Session.Save(item);
                inserts.Remove(item);
            }
        }

        /// <summary>
        /// Save a sequence of updates.
        /// </summary>
        /// <param name="updates">
        /// The items to update.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="updates"/> is <see langword="null" />.
        /// </exception>
        protected void SaveUpdates<T>(ICollection<T> updates)
        {
            if(updates == null)
                throw new ArgumentNullException("updates");
            foreach(T item in updates.ToList())
            {
                Session.Update(item);
                updates.Remove(item);
            }
        }

        #endregion

        #region IDisposable implementation

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose" /> of this instance.
        /// </summary>
        /// <param name="disposing">
        /// Was this method called from the <see cref="IDisposable.Dispose" /> method?
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!IsDisposed)
                {
                    if(Session != null)
                    {
                        Session.Dispose();
                    }
                }
            }
        }
        
        #endregion
    }
}

