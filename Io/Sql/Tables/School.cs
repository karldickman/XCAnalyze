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
        public static Model.School Get(int id)
        {
            return IdMap[id];
        }
    }
}
