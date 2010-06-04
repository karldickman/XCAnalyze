using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class Meet
    {
        protected internal static IDictionary<int, Meet> IdMap = new Dictionary<int, Meet>();
        
        public static IList<Meet> List
        {
            get { return new List<Meet> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        /// <summary>
        /// The name of the meet.  Example: Lewis & Clark Invitational.
        /// </summary>
        public string Name { get; protected internal set; }

        public Meet (int id, string name)
        {
            Id = id;
            Name = name;
            IdMap[id] = this;
        }
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static Meet Get (int id)
        {
            return IdMap[id];
        }
    }
}
