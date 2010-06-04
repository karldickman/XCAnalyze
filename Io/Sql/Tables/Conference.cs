using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    
    public class Conference : IComparable<Conference>
    {
        protected internal static IDictionary<int, Conference> IdMap = new Dictionary<int, Conference>();
        
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

        public Conference (int id, string name, string abbreviation)
        {
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
            IdMap[id] = this;
        }
        
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static Conference Get (int id)
        {
            return IdMap[id];
        }
        
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
    public class TestSqlConference
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