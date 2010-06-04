using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    
    public class SqlConference : IComparable<SqlConference>
    {
        protected internal static IDictionary<int, SqlConference> IdMap = new Dictionary<int, SqlConference>();
        
        public static IList<SqlConference> List
        {
            get { return new List<SqlConference> (IdMap.Values); }
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

        public SqlConference (int id, string name, string abbreviation)
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
        
        public static SqlConference Get (int id)
        {
            return IdMap[id];
        }
        
        public static int? GetId (string conferenceName)
        {
            if (conferenceName == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, SqlConference> entry in IdMap)
            {
                if (conferenceName.Equals (entry.Value.Name))
                {
                    return entry.Key;
                }
            }
            return null;
        }
        
        public static int? GetId (SqlConference conference)
        {
            if (conference == null)
            {
                return null;
            }
            foreach (KeyValuePair<int, SqlConference> entry in IdMap)
            {
                if (conference.Equals (entry.Value)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }

        public int CompareTo (SqlConference other)
        {
            return Name.CompareTo (other.Name);
        }
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            else if(other is SqlConference)
            {
                return 0 == CompareTo((SqlConference)other);
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
        protected internal SqlConference Nwc { get; set; }
        protected internal SqlConference Sciac { get; set; }
        protected internal SqlConference Scac { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            SqlConference.Clear();
            NwcName = "Northwest Conference";
            Nwc = new SqlConference (1, NwcName, "NWC");
            SciacName = "Southern California Intercollegiate Athletic Conference";
            Sciac = new SqlConference (2, SciacName, "SCIAC");
            ScacName = "Southern Collegiate Athletic Conference";
            Scac = new SqlConference (3, ScacName, "SCAC");
        }
        
        [TearDown]
        public void TearDown ()
        {
            SqlConference.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            Assert.IsNull (SqlConference.GetId ((string)null));
            Assert.IsNull(SqlConference.GetId((SqlConference)null));
            Assert.IsNull(SqlConference.GetId("xkcd"));
            Assert.AreEqual(1, SqlConference.GetId(Nwc));
            Assert.AreEqual(2, SqlConference.GetId(Sciac));
            Assert.AreEqual(3, SqlConference.GetId(Scac));
            Assert.AreEqual(1, SqlConference.GetId(NwcName));
            Assert.AreEqual(2, SqlConference.GetId(SciacName));
            Assert.AreEqual(3, SqlConference.GetId(ScacName));
        }
    }
}