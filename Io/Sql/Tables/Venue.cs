using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class SqlVenue
    {
        protected internal static IDictionary<int, SqlVenue> IdMap = new Dictionary<int, SqlVenue>();
        
        public static IList<SqlVenue> List
        {
            get { return new List<SqlVenue> (IdMap.Values); }
        }
            
        public int Id { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string City { get; protected internal set; }
        public string State { get; protected internal set; }
        public int? Elevation { get; protected internal set; }

        public SqlVenue (int id, string name, string city, string state,
            int? elevation)
        {
            Id = id;
            Name = name;
            City = city;
            State = state;
            Elevation = elevation;
            IdMap[id] = this;
        }       
        
        public static bool Exists (int id)
        {
            return IdMap.ContainsKey (id);
        }
        
        public static SqlVenue Get (int id)
        {
            return IdMap[id];
        }
    }
}
