using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// Representation of a row in the schools table of the database.
    /// </summary>
    public class School : Model.School
    {
        /// <summary>
        /// A registry of all instances (i.e. rows) by id number.
        /// </summary>
        protected internal static IDictionary<int, Model.School> IdMap =
            new Dictionary<int, Model.School>();
        
        /// <summary>
        /// Get all instances of this class.
        /// </summary>
        public static IList<Model.School> List
        {
            get { return new List<Model.School> (IdMap.Values); }
        }
        
        override public string Conference
        {
            get
            {
                if (ConferenceId == null)
                {
                    return null;
                }
                if (Tables.Conference.Exists (ConferenceId.Value))
                {
                    return Tables.Conference.Get (ConferenceId.Value).Name;
                }
                return null;
            }
        }
        
        /// <summary>
        /// The id number of the conference this school is affiliated with.
        /// </summary>
        public int? ConferenceId { get; protected internal set; }
        
        /// <summary>
        /// The id number of this school.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The nicknames of this school.
        /// </summary>
        public string[] Nicknames { get; protected internal set; }
        
        /// <summary>
        /// Create a new school.
        /// </summary>
        /// <param name="id">
        /// The id number of the school.
        /// </param>
        /// <param name="name">
        /// The name of the school (Linfield, Willamette, Pacific Lutheran, etc.)
        /// </param>
        /// <param name="nicknames">
        /// The nicknames of the school.
        /// </param>
        /// <param name="type">
        /// College, University, Institude of Technology, etc.
        /// </param>
        /// <param name="nameFirst">
        /// Should the name of the school come first (as with Linfield College)
        /// or not (as with University of Puget Sound).
        /// </param>
        /// <param name="conferenceId">
        /// The id number of the conference with which this school is
        /// affiliated.  If none, then null.
        /// </param>
        public School (int id, string name, string[] nicknames,
            string type, bool nameFirst, int? conferenceId)
            : base(name, type, nameFirst)
        {
            Id = id;
            Nicknames = nicknames;
            ConferenceId = conferenceId;
            IdMap[id] = this;
        }
        
        /// <summary>
        /// Clear all instances of this class from the registry.
        /// </summary>
        public static void Clear ()
        {
            IdMap.Clear ();
        }
        
        /// <summary>
        /// Check whether an instance with a particular id number exists.
        /// </summary>
        /// <param name="id">
        /// The id number to search for.
        /// </param>
        /// <returns>
        /// True if an instance exists; false if not.
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
        /// The <see cref="Model.School"/> with the requested id number.
        /// </returns>
        public static Model.School Get (int id)
        {
            return IdMap[id];
        }
        
        /// <summary>
        /// Get the id number of a particular school.
        /// </summary>
        /// <param name="school">
        /// The <see cref="Model.School"/> to search for.
        /// </param>
        /// <returns>
        /// The id number of the school.
        /// </returns>
        public static int? GetId (Model.School school)
        {
            if (school is School)
            {
                return ((School)school).Id;
            }
            foreach (KeyValuePair<int, Model.School> entry in IdMap)
            {
                if (entry.Value.Equals (school)) 
                {
                    return entry.Key;
                }
            }
            return null;
        }
    }
    
    [TestFixture]
    public class TestSchool
    {
        protected internal Conference Nwc { get; set; }
        protected internal School Linfield { get; set; }
        protected internal School Willamette { get; set; }
        protected internal School Chapman { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Conference.Clear ();
            School.Clear ();
            Nwc = new Conference (1, "Northwest Conference", "NWC");
            Linfield = new School (1, "Linfield", null, "College", true, 1);
            Willamette = new School (2, "Willamette", null, "University", true, 1);
            Chapman = new School (3, "Chapman", null, "University", true, null);
        }
        
        [TearDown]
        public void TearDown ()
        {
            Conference.Clear ();
            School.Clear ();
        }
        
        [Test]
        public void TestGetId ()
        {
            Model.School[] clones = new Model.School[3];
            clones[0] = new Model.School (Linfield.Name, Linfield.Type, Linfield.NameFirst, Nwc.Name);
            clones[1] = new Model.School (Willamette.Name, Willamette.Type, Willamette.NameFirst, Nwc.Name);
            clones[2] = new Model.School (Chapman.Name, Chapman.Type, Chapman.NameFirst, null);
            Model.School off = new Model.School (Willamette.Name, Willamette.Type, Willamette.NameFirst);
            Model.School nxst = new Model.School ("Puget Sound", "University", false, Nwc.Name);
            Assert.IsNull (School.GetId (nxst));
            Assert.IsNull (School.GetId (off));
            for (int i = 0; i < clones.Length; i++)
            {
                Assert.AreEqual (i + 1, School.GetId (clones[i]));
            }
        }
    }
}
