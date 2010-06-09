using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row of the venue table in the database.
    /// </summary>
    public class Venue : Model.Venue
    {
        /// <summary>
        /// A registry of the instances (i.e rows) by id number.
        /// </summary>
        protected internal static IDictionary<int, Model.Venue> IdMap =
            new Dictionary<int, Model.Venue>();
        
        /// <summary>
        /// Get all instances of this class.
        /// </summary>
        public static IList<Model.Venue> List
        {
            get { return new List<Model.Venue> (IdMap.Values); }
        }
            
        /// <summary>
        /// The id number.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The elevation (in meters) of the venue.
        /// </summary>
        public int? Elevation { get; protected internal set; }

        /// <summary>
        /// Create a venue.
        /// </summary>
        /// <param name="id">
        /// The id number.
        /// </param>
        /// <param name="name">
        /// The name of the venue.
        /// </param>
        /// <param name="city">
        /// The city nearest the venue.
        /// </param>
        /// <param name="state">
        /// Which state the venue is in.
        /// </param>
        /// <param name="elevation">
        /// The elevation of the venue.  If not known, then null.
        /// </param>
        public Venue (int id, string name, string city, string state,
            int? elevation)
            : base(name, city, state)
        {
            Id = id;
            Elevation = elevation;
            IdMap[id] = this;
        }    
        
        /// <summary>
        /// Clear the registry of instances.
        /// </summary>
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        /// <summary>
        /// Check for an instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// True if such an instance exists, false otherwise.
        /// </returns>
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        /// <summary>
        /// Get an instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// The <see cref="Venue"/> with the requested id number.
        /// </returns>
        public static Model.Venue Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Get the id number of the particular venue instance.
        /// </summary>
        /// <param name="venue">
        /// The <see cref="Venue"/> for which to search.
        /// </param>
        /// <returns>
        /// The id number of the found instance.  If no instance is found,
        /// returns null.
        /// </returns>
        public static int? GetId (Model.Venue venue)
        {
            if (venue == null)
            {
                return null;
            }
            if (venue is Venue)
            {
                return ((Venue)venue).Id;
            }
            return (from Venue candidate in IdMap.Values
                where venue.Equals (candidate)
                select candidate.Id).FirstOrDefault();
        }
        
        /// <summary>
        /// Get the ids of all the venue instances with a particular name.
        /// </summary>
        /// <param name="name">
        /// The name to search for.
        /// </param>
        /// <returns>
        /// A <see cref="IList<Venue>"/> of all the id numbers that were found.
        /// </returns>
        public static IList<int> GetIds (string name)
        {
            return new List<int>(from Venue venue in IdMap.Values
                where venue.Name.Equals (name)
                select venue.Id);
        }
    }
    
    [TestFixture]
    public class TestVenue
    {
        protected internal IList<Venue> Venues { get; set; }
        protected internal Venue LincolnOr { get; set; }
        protected internal Venue LincolnWa { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Venue.Clear ();
            Venues = new List<Venue> ();
            LincolnOr = new Venue (1, "Lincoln Park", "Forest Grove", "OR", null);
            LincolnWa = new Venue (3, "Lincoln Park", "Seattle", "WA", null);
            Venues.Add (LincolnOr);
            Venues.Add (LincolnWa);
            Venues.Add (new Venue (4, "Milo McIver State Park", "Estacada", "OR", null));
            Venues.Add (new Venue (5, "Bush Pasture Park", "Salem", "OR", null));
            Venues.Add (new Venue (7, "Fort Steilacoom Park", "Tacompton", "WA", null));
        }
        
        [TearDown]
        public void TearDown ()
        {
            Venue.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            foreach (Venue venue in Venues)
            {
                Assert.AreEqual (venue.Id, Venue.GetId (venue));
            }
        }
        
        [Test]
        public void TestGetIds ()
        {
            Assert.AreEqual (0, Venue.GetIds ("Drake Park").Count);
            Assert.AreEqual (2, Venue.GetIds ("Lincoln Park").Count);
            Assert.That (Venue.GetIds ("Lincoln Park").Contains (LincolnOr.Id));
            Assert.That (Venue.GetIds ("Lincoln Park").Contains (LincolnWa.Id));
        }
    }
}
