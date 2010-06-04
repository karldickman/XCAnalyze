using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class Runner : Model.Runner
    {
        protected internal static IDictionary<int, Model.Runner> IdMap = new Dictionary<int, Model.Runner>();
        
        public static IList<Model.Runner> List
        {
            get { return new List<Model.Runner> (IdMap.Values); }
        }
        
        public int Id { get; protected internal set; }
        public string[] Nicknames { get; protected internal set; }
        
        public Runner (int id, string surname, string givenName,
            string[] nicknames, Model.Gender gender, int? year)
            : base(surname, givenName, gender, year)
        {
            Id = id;
            Nicknames = nicknames;
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
        
        public static Model.Runner Get(int id)
        {
            return IdMap[id];
        }
    }
}
