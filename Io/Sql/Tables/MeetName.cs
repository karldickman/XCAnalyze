using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of a row of the meets table in the database.
    /// </summary>
    public class MeetName : IComparable<MeetName>
    {
        /// <summary>
        /// A registry of all the instances (i.e. rows).
        /// </summary>
        protected internal static IDictionary<int, MeetName> IdMap =
            new Dictionary<int, MeetName>();
        
        /// <summary>
        /// Get all instances of this class.
        /// </summary>
        public static IList<MeetName> List
        {
            get { return new List<MeetName> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the meet.  Example: Lewis & Clark Invitational.
        /// </summary>
        public string Name { get; protected internal set; }

        /// <summary>
        /// Create a new row description.
        /// </summary>
        /// <param name="id">
        /// The row id number.
        /// </param>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        public MeetName (int id, string name)
        {
            Id = id;
            Name = name;
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
        /// Check whether an instance with a particular id number exists.
        /// </summary>
        /// <param name="id">
        /// The id to search for.
        /// </param>
        /// <returns>
        /// True if the instance exists; false otherwise.
        /// </returns>
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        /// <summary>
        /// Get the instance with a particular id number.
        /// </summary>
        /// <param name="id">
        /// The id number.
        /// </param>
        /// <returns>
        /// The <see cref="Meet"/> with that id number.
        /// </returns>
        public static MeetName Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Search for the id number of the instance with a particular name.
        /// </summary>
        /// <param name="name">
        /// The name of the meet to search for.
        /// </param>
        /// <returns>
        /// The id of the instance.  If none is found, returns null.
        /// </returns>
        public static int? GetId (string name)
        {
            if (name == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, MeetName> entry in IdMap)
            {
                if (entry.Value.Name.Equals (name)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Meets are compared by name.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Meet"/> being compared.
        /// </param>
        public int CompareTo (MeetName other)
        {
            return Name.CompareTo (other.Name);
        }
        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                 return true;
            }
            if(other is MeetName)
            {
                return 0 == CompareTo((MeetName)other);
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
    public class TestMeetName
    {
        protected internal IList<MeetName> Meets { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            MeetName.Clear ();
            Meets = new List<MeetName> ();
            Meets.Add(new MeetName (1, "Lewis & Clark Inviational"));
            Meets.Add(new MeetName (2, "Northwest Conference Championship"));
            Meets.Add(new MeetName (5, "NCAA National Championship"));
        }
        
        [TearDown]
        public void TearDown ()
        {
            MeetName.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            Assert.IsNull (MeetName.GetId ("NCAA National Championships"));
            Assert.IsNull (MeetName.GetId ("Willamette Invitational"));
            foreach (MeetName meet in Meets)
            {
                Assert.AreEqual (meet.Id, MeetName.GetId (meet.Name));
            }
        }
    }
}
