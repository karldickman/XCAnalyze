using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class SqlMeet
    {
        protected internal static IDictionary<int, SqlMeet> IdMap = new Dictionary<int, SqlMeet>();
        
        public static IList<SqlMeet> List
        {
            get { return new List<SqlMeet> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the meet.  Example: Lewis & Clark Invitational.
        /// </summary>
        public string Name { get; protected internal set; }

        public SqlMeet (int id, string name)
        {
            Id = id;
            Name = name;
            IdMap[id] = this;
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static SqlMeet Get (int id)
        {
            return IdMap[id];
        }
    }
}
