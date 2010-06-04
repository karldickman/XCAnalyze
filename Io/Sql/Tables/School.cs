using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class School : Model.School
    {
        protected internal static IDictionary<int, Model.School> IdMap = new Dictionary<int, Model.School>();
        
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
        
        public int? ConferenceId { get; protected internal set; }
        public int Id { get; protected internal set; }
        public string[] Nicknames { get; protected internal set; }
        
        public School (int id, string name, string[] nicknames,
            string type, bool nameFirst, int? conferenceId)
            : base(name, type, nameFirst)
        {
            Id = id;
            Nicknames = nicknames;
            ConferenceId = conferenceId;
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
        
        public static Model.School Get(int id)
        {
            return IdMap[id];
        }
    }
}
