using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row in the database's conference table.
    /// </summary>
    public class Conference : IComparable<Conference>
    {
        /// <summary>
        /// A registry of all the instances (i.e. rows) by id number.
        /// </summary>
        protected internal static IDictionary<int, Conference> IdMap =
            new Dictionary<int, Conference>();
        
        /// <summary>
        /// Get all the instances of this class.
        /// </summary>
        public static IList<Conference> List
        {
            get { return new List<Conference> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the conference.
        /// </summary>
        public string Name { get; protected internal set; }
        
        /// <summary>
        /// The standard abbreviation of the conference.
        /// </summary>
        public string Abbreviation { get; protected internal set; }

        /// <summary>
        /// Create a new conference.
        /// </summary>
        /// <param name="id">
        /// The id number of the conference.
        /// </param>
        /// <param name="name">
        /// The name of the conference.
        /// </param>
        /// <param name="abbreviation">
        /// The abbreviation of the name, if any.
        /// </param>
        public Conference (int id, string name, string abbreviation)
        {
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
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
        /// Check whether there is an instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to look for.
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>.  True if an instance was found;
        /// false if none was found.
        /// </returns>
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        /// <summary>
        /// Get the instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// The <see cref="Conference"/> with the given id number.
        /// </returns>
        public static Conference Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Get the id number of a particular conference.
        /// </summary>
        /// <param name="conferenceName">
        /// The name of the conference.
        /// </param>
        /// <returns>
        /// A <see cref="System.Nullable<System.Int32>"/>.  The id number of the
        /// conference, or null if none was found.
        /// </returns>
        public static int? GetId (string conferenceName)
        {
            if (conferenceName == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, Conference> entry in IdMap)
            {
                if (conferenceName.Equals (entry.Value.Name))
                {
                    return entry.Key;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get the id number of a particular conference.
        /// </summary>
        /// <param name="conference">
        /// The <see cref="Conference"/> to search for.
        /// </param>
        /// <returns>
        /// A <see cref="System.Nullable<System.Int32>"/>.  The id number of the
        /// conference, or null if none was found.
        /// </returns>
        public static int? GetId (Conference conference)
        {
            if (conference == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, Conference> entry in IdMap)
            {
                if (conference.Equals (entry.Value)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// Compare conferences based on their names.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Conference"/> to compare with.
        /// </param>
        public int CompareTo (Conference other)
        {
            return Name.CompareTo (other.Name);
        }
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            else if(other is Conference)
            {
                return 0 == CompareTo((Conference)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return Name.GetHashCode();
        }  
        
        override public string ToString ()
        {
            return Name;
        } 
    }
    
    [TestFixture]
    public class TestConference
    {
        protected internal string NwcName { get; set; }
        protected internal string SciacName { get; set; }
        protected internal string ScacName { get; set; }
        protected internal Conference Nwc { get; set; }
        protected internal Conference Sciac { get; set; }
        protected internal Conference Scac { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Conference.Clear();
            NwcName = "Northwest Conference";
            Nwc = new Conference (1, NwcName, "NWC");
            SciacName = "Southern California Intercollegiate Athletic Conference";
            Sciac = new Conference (2, SciacName, "SCIAC");
            ScacName = "Southern Collegiate Athletic Conference";
            Scac = new Conference (3, ScacName, "SCAC");
        }
        
        [TearDown]
        public void TearDown ()
        {
            Conference.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            Assert.IsNull (Conference.GetId ((string)null));
            Assert.IsNull(Conference.GetId((Conference)null));
            Assert.IsNull(Conference.GetId("xkcd"));
            Assert.AreEqual(1, Conference.GetId(Nwc));
            Assert.AreEqual(2, Conference.GetId(Sciac));
            Assert.AreEqual(3, Conference.GetId(Scac));
            Assert.AreEqual(1, Conference.GetId(NwcName));
            Assert.AreEqual(2, Conference.GetId(SciacName));
            Assert.AreEqual(3, Conference.GetId(ScacName));
        }
    }
}