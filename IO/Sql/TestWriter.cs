using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using XCAnalyze.Model;
using System.Data;

namespace XCAnalyze.IO.Sql
{
    public abstract partial class AbstractWriter
    {
        #if DEBUG

        public abstract class Test
        {
            #region Delegates

            /// <summary>
            /// A delegate for test methods.
            /// </summary>
            delegate T TestMethod<T>(T value_);

            #endregion

            #region Properties

            #region Constants

            public const string ExampleDatabase = "xca_example";

            /// <summary>
            /// The name of the test database.
            /// </summary>
            public const string TestDatabase = "xca_test";

            /// <summary>
            /// The username used to connect to the test database.
            /// </summary>
            public const string TestAccount = "xcanalyze";

            #endregion

            #region Sample data

            /// <summary>
            /// A sample list of affiliations.
            /// </summary>
            IList<Affiliation> Affiliations {
                get { return SampleData.Affiliations; }
            }

            /// <summary>
            /// A sample list of cities.
            /// </summary>
            IList<City> Cities {
                get { return SampleData.Cities; }
            }

            /// <summary>
            /// A sample list of conferences.
            /// </summary>
            IList<Conference> Conferences {
                get { return SampleData.Conferences; }
            }

            /// <summary>
            /// A sample list of meet instances.
            /// </summary>
            IList<MeetInstance> MeetInstances {
                get { return SampleData.MeetInstances; }
            }

            /// <summary>
            /// A sample list of meets;
            /// </summary>
            IList<Meet> Meets {
                get { return SampleData.Meets; }
            }

            /// <summary>
            /// A sample list of performances.
            /// </summary>
            IList<Performance> Performances {
                get { return SampleData.Performances; }
            }

            /// <summary>
            /// A sample list of races.
            /// </summary>
            IList<Race> Races {
                get { return SampleData.Races; }
            }

            /// <summary>
            /// A sample list of runners.
            /// </summary>
            IList<Runner> Runners {
                get { return SampleData.Runners; }
            }

            /// <summary>
            /// A sample list of teams.
            /// </summary>
            IList<Team> Teams {
                get { return SampleData.Teams; }
            }

            /// <summary>
            /// A sample list of states.
            /// </summary>
            IList<State> States {
                get { return SampleData.States; }
            }

            /// <summary>
            /// A sample list of venues.
            /// </summary>
            IList<Venue> Venues {
                get { return SampleData.Venues; }
            }

            #endregion

            /// <summary>
            /// The reader for the database.
            /// </summary>
            protected AbstractReader Reader { get; set; }

            /// <summary>
            /// The writer for the database.
            /// </summary>
            protected AbstractWriter Writer { get; set; }

            /// <summary>
            /// The command the writer users.
            /// </summary>
            protected IDbCommand WriterCommand {
                get { return Writer.Command; }
            }

            /// <summary>
            /// The writer's database.
            /// </summary>
            protected string WriterDatabase {
                get { return Writer.Database; }
            }

            /// <summary>
            /// The writer's connection to the database.
            /// </summary>
            protected IDbConnection WriterConnection {
                get { return Writer.Connection; }
            }

            #endregion

            #region Setup

            [SetUp]
            public virtual void SetUp()
            {
                Writer = CreateWriter();
                Reader = CreateReader();
            }

            protected abstract void SetUpPartial();

            [TearDown]
            public virtual void TearDown()
            {
                Writer.Close();
                Reader.Close();
                foreach(Affiliation affiliation in Affiliations) {
                    affiliation.IsAttached = false;
                }
                foreach(City city in Cities) {
                    city.IsAttached = false;
                }
                foreach(Conference conference in Conferences) {
                    conference.IsAttached = false;
                }
                foreach(Meet meet in Meets) {
                    meet.IsAttached = false;
                }
                foreach(MeetInstance meetInstance in MeetInstances) {
                    meetInstance.IsAttached = false;
                }
                foreach(Performance performance in Performances) {
                    performance.IsAttached = false;
                }
                foreach(Race race in Races) {
                    race.IsAttached = false;
                }
                foreach(Runner runner in Runners) {
                    runner.IsAttached = false;
                }
                foreach(State state in States) {
                    state.IsAttached = false;
                }
                foreach(Team team in Teams) {
                    team.IsAttached = false;
                }
                foreach(Venue venue in Venues) {
                    venue.IsAttached = false;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Create a reader for an example file to be used by this test.
            /// </summary>
            protected abstract AbstractReader CreateExampleReader();

            /// <summary>
            /// Create a reader to be used by this test.
            /// </summary>
            protected abstract AbstractReader CreateReader();

            /// <summary>
            /// Create the writer to be used by this test.
            /// </summary>
            protected abstract AbstractWriter CreateWriter();

            /// <summary>
            /// Convert a dictionary of values to a list.
            /// </summary>
            /// <param name="dictionary">
            /// A <see cref="IDictionary<K, T>"/> of values.
            /// </param>
            /// <returns>
            /// The values of the dictionary converted to a
            /// <see cref="IList<T>"/>.
            /// </returns>
            protected static IList<T> DictToList<K, T>(IDictionary<K, T> dictionary)
            {
                return new List<T>(dictionary.Values);
            }

            /// <summary>
            /// Convert a dictionary of values to a list.
            /// </summary>
            /// <param name="dictionary">
            /// A <see cref="IDictionary<K1, IDictionary<K2, T>>"/> of values.
            /// </param>
            /// <returns>
            /// The values of the dictionary converted to a <see cref="IList<T>"/>.
            /// </returns>
            protected static IList<T> DictToList<K1, K2, T>(IDictionary<K1, IDictionary<K2, T>> dictionary)
            {
                List<T> result = new List<T>();
                foreach(IDictionary<K2, T> entry in dictionary.Values) {
                    result.AddRange(entry.Values);
                }
                return result;
            }

            /// <summary>
            /// Carry out a series of three write-read pairs.
            /// </summary>
            /// <param name="Test">
            /// The <see cref="TestMethod<T>"/> used to execute a single test.
            /// </param>
            /// <param name="original">
            /// A <see cref="T"/>: the initial set of values.
            /// </param>
            void RepeatTest<T>(TestMethod<T> Test, T original)
            {
                RepeatTest(3, Test, original);
            }

            /// <summary>
            /// Carry out a series of several write-read pairs.
            /// </summary>
            /// <param name="number">
            /// The number of times to repeat the test.
            /// </param>
            /// <param name="Test">
            /// The <see cref="TestMethod<T>"/> used to execute a single test.
            /// </param>
            /// <param name="original">
            /// A <see cref="T"/>: the initial set of values.
            /// </param>
            void RepeatTest<T>(int number, TestMethod<T> Test, T original)
            {
                for(int i = 0; i < number; i++) {
                    original = Test<T>(original);
                    Writer.Close();
                    Writer = CreateWriter();
                }
            }

            #endregion

            #region Tests

            [Test]
            public virtual void TestInitializeDatabase()
            {
                SetUpPartial();
                Writer.InitializeDatabase();
            }

            [Test]
            public virtual void TestIsDatabaseInitialized()
            {
                SetUpPartial();
                Assert.IsFalse(Writer.IsDatabaseInitialized());
                Writer.InitializeDatabase();
                Assert.That(Writer.IsDatabaseInitialized());
            }

            [Test]
            public void TestWrite()
            {
                RepeatTest(Write, SampleData.Data);
            }

            [Test]
            public void TestWriteExample()
            {
                DataContext expected;
                using(AbstractReader exampleReader = CreateExampleReader()) {
                    expected = exampleReader.Read();
                }
                expected.DetachAll();
                Writer.Write(expected);
                DataContext actual = Reader.Read();
                Assert.That(XcaWriter.Test.AreDataEqual(expected, actual));
            }

            protected virtual DataContext Write(DataContext expected)
            {
                Writer.Write(expected);
                DataContext actual = Reader.Read();
                Assert.IsNotEmpty((ICollection)actual.Affiliations);
                Assert.IsNotEmpty((ICollection)actual.Conferences);
                Assert.IsNotEmpty((ICollection)actual.Performances);
                Assert.IsNotEmpty((ICollection)actual.MeetInstances);
                Assert.IsNotEmpty((ICollection)actual.Races);
                Assert.IsNotEmpty((ICollection)actual.Runners);
                Assert.IsNotEmpty((ICollection)actual.Teams);
                Assert.IsNotEmpty((ICollection)actual.Venues);
                Assert.That(XcaWriter.Test.AreDataEqual(expected, actual));
                return actual;
            }

            #region Affiliations

            [Test]
            public virtual void TestWriteAffiliations()
            {
                RepeatTest(WriteAffiliations, Affiliations);
            }

            IList<Affiliation> WriteAffiliations(IList<Affiliation> expected)
            {
                IList<Affiliation> actual = new List<Affiliation>();
                foreach(IDictionary<int, Affiliation> runnerEntry in PrepareAffiliations(expected).Values) {
                    foreach(Affiliation affiliation in runnerEntry.Values) {
                        actual.Add(affiliation);
                    }
                }
                Assert.AreEqual(expected.Count, actual.Count);
                foreach(Affiliation affiliation in expected) {
                    Assert.That(actual.Contains(affiliation));
                }
                return actual;
            }

            IDictionary<int, IDictionary<int, Affiliation>> PrepareAffiliations(IEnumerable<Affiliation> affiliations)
            {
                IDictionary<int, Runner> runners = PrepareRunners(Runners);
                IDictionary<int, Team> teams = PrepareTeams(Teams);
                Writer.WriteAffiliations(affiliations);
                return Reader.ReadAffiliations(runners, teams);
            }

            #endregion

            #region Cities

            [Test]
            public virtual void TestWriteCities()
            {
                RepeatTest(WriteCities, Cities);
            }

            IDictionary<int, City> PrepareCities(IEnumerable<City> cities)
            {
                IDictionary<string, State> states = PrepareStates(States);
                Writer.WriteCities(cities);
                return Reader.ReadCities(states);
            }

            IList<City> WriteCities(IEnumerable<City> expected)
            {
                IList<City> actual = DictToList(PrepareCities(expected));
                Assert.AreEqual(Cities.Count, actual.Count);
                foreach(City city in Cities) {
                    Assert.That(actual.Contains(city));
                }
                return actual;
            }

            #endregion

            #region Conferences

            [Test]
            public virtual void TestWriteConferences()
            {
                RepeatTest(WriteConferences, Conferences);
            }

            IList<Conference> WriteConferences(IList<Conference> conferences)
            {
                IList<Conference> actual = DictToList(PrepareConferences(conferences));
                Assert.AreEqual(conferences.Count, actual.Count);
                foreach(Conference conference in Conferences) {
                    Assert.That(actual.Contains(conference));
                }
                return actual;
            }

            IDictionary<int, Conference> PrepareConferences(IEnumerable<Conference> conferences)
            {
                Writer.WriteConferences(conferences);
                return Reader.ReadConferences();
            }

            #endregion

            #region Performances

            [Test]
            public virtual void TestWritePerformances()
            {
                RepeatTest(WritePerformances, Performances);
            }

            IList<Performance> WritePerformances(IList<Performance> expected)
            {
                IList<Performance> actual = DictToList(PreparePerformances(expected));
                Assert.AreEqual(expected.Count, actual.Count);
                foreach(Performance performance in expected) {
                    Assert.That(actual.Contains(performance));
                }
                return actual;
            }

            IDictionary<int, IDictionary<int, Performance>> PreparePerformances(IEnumerable<Performance> performances)
            {
                IDictionary<int, Runner> runners = PrepareRunners(Runners);
                IDictionary<int, Race> races = PrepareRaces(Races);
                Writer.WritePerformances(performances);
                return Reader.ReadPerformances(races, runners);
            }

            #endregion

            #region Meet instances

            [Test]
            public virtual void TestWriteMeetInstances()
            {
                RepeatTest(WriteMeetInstances, MeetInstances);
            }

            IList<MeetInstance> WriteMeetInstances(IList<MeetInstance> meetInstances)
            {
                IList<MeetInstance> actual = new List<MeetInstance>();
                foreach(IDictionary<DateTime, MeetInstance> meet in PrepareMeetInstances(meetInstances).Values) {
                    foreach(MeetInstance meetInstance in meet.Values) {
                        actual.Add(meetInstance);
                    }
                }
                Assert.AreEqual(meetInstances.Count, actual.Count);
                foreach(MeetInstance meetInstance in meetInstances) {
                    Assert.That(actual.Contains(meetInstance));
                }
                return actual;
            }

            IDictionary<int, IDictionary<DateTime, MeetInstance>> PrepareMeetInstances(IEnumerable<MeetInstance> meetInstances)
            {
                IDictionary<int, Team> teams;
                IDictionary<int, Meet> meets = PrepareMeets(Meets, out teams);
                IDictionary<int, Venue> venues = PrepareVenues(Venues);
                Writer.WriteMeetInstances(meetInstances);
                return Reader.ReadMeetInstances(meets, venues, teams);
            }

            #endregion

            #region Meets

            [Test]
            public virtual void TestWriteMeets()
            {
                RepeatTest(WriteMeets, Meets);
            }

            IList<Meet> WriteMeets(IList<Meet> meets)
            {
                IList<Meet> actual = DictToList(PrepareMeets(meets));
                Assert.AreEqual(meets.Count, actual.Count);
                foreach(Meet meet in meets) {
                    Assert.That(actual.Contains(meet));
                }
                return actual;
            }

            IDictionary<int, Meet> PrepareMeets(IEnumerable<Meet> meets)
            {
                IDictionary<int, Team> teams;
                return PrepareMeets(meets, out teams);
            }

            IDictionary<int, Meet> PrepareMeets(IEnumerable<Meet> meets, out IDictionary<int, Team> teams)
            {
                teams = PrepareTeams(Teams);
                Writer.WriteMeets(meets);
                return Reader.ReadMeets(teams);
            }

            #endregion

            #region Races

            [Test]
            public virtual void TestWriteRaces()
            {
                RepeatTest(WriteRaces, Races);
            }

            IList<Race> WriteRaces(IList<Race> expected)
            {
                IList<Race> actual = DictToList(PrepareRaces(expected));
                Assert.AreEqual(Races.Count, actual.Count);
                foreach(Race race in Races) {
                    bool found = false;
                    foreach(Race candidate in actual) {
                        if(race.Distance == candidate.Distance) {
                            found = true;
                            break;
                        }
                    }
                    if(!found) {
                        Assert.Fail("Could not find {0} in the written results.", race);
                    }
                }
                return actual;
            }

            IDictionary<int, Race> PrepareRaces(IEnumerable<Race> races)
            {
                IDictionary<int, IDictionary<DateTime, MeetInstance>> meetInstances = PrepareMeetInstances(MeetInstances);
                Writer.WriteRaces(races);
                return Reader.ReadRaces(meetInstances);
            }

            #endregion

            #region Runners

            [Test]
            public virtual void TestWriteRunners()
            {
                RepeatTest(WriteRunners, Runners);
            }

            IList<Runner> WriteRunners(IList<Runner> runners)
            {
                IList<Runner> actual = DictToList(PrepareRunners(runners));
                Assert.AreEqual(runners.Count, actual.Count);
                foreach(Runner runner in runners) {
                    Assert.That(actual.Contains(runner));
                }
                return actual;
            }

            IDictionary<int, Runner> PrepareRunners(IEnumerable<Runner> runners)
            {
                Writer.WriteRunners(runners);
                return Reader.ReadRunners();
            }

            #endregion

            #region States

            [Test]
            public virtual void TestWriteStates()
            {
                RepeatTest(WriteStates, States);
            }

            IDictionary<string, State> PrepareStates(IEnumerable<State> states)
            {
                Writer.WriteStates(states);
                return Reader.ReadStates();
            }

            IList<State> WriteStates(IEnumerable<State> expected)
            {
                IList<State> actual = DictToList(PrepareStates(expected));
                Assert.AreEqual(States.Count, actual.Count);
                foreach(State state in States) {
                    Assert.That(actual.Contains(state));
                }
                return actual;
            }

            #endregion

            #region Teams

            [Test]
            public virtual void TestWriteTeams()
            {
                RepeatTest(WriteTeams, Teams);
            }

            IList<Team> WriteTeams(IList<Team> teams)
            {
                IList<Team> actual = DictToList(PrepareTeams(teams));
                Assert.AreEqual(Teams.Count, actual.Count);
                foreach(Team team in Teams) {
                    Assert.That(actual.Contains(team));
                    foreach(Conference conference in Conferences) {
                        if(team.Conference != null && team.Conference.Equals(conference)) {
                            Assert.AreEqual(conference, ((Team)actual[actual.IndexOf(team)]).Conference);
                            break;
                        }
                    }
                }
                return actual;
            }

            IDictionary<int, Team> PrepareTeams(IEnumerable<Team> teams)
            {
                IDictionary<int, Conference> conferences = PrepareConferences(Conferences);
                Writer.WriteTeams(teams);
                return Reader.ReadTeams(conferences);
            }

            #endregion

            #region Venues

            [Test]
            public virtual void TestWriteVenues()
            {
                RepeatTest(WriteVenues, Venues);
            }

            IList<Venue> WriteVenues(IList<Venue> venues)
            {
                IList<Venue> actual = DictToList(PrepareVenues(venues));
                Assert.AreEqual(venues.Count, actual.Count);
                foreach(Venue venue in venues) {
                    Assert.That(actual.Contains(venue));
                }
                return actual;
            }

            IDictionary<int, Venue> PrepareVenues(IEnumerable<Venue> venues)
            {
                IDictionary<int, City> cities = PrepareCities(Cities);
                Writer.WriteVenues(venues);
                return Reader.ReadVenues(cities);
            }
            
            #endregion
            
            #endregion
        }
        
        #endif
    }
}
