using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class Venue
    {
        protected internal static IDictionary<int, Venue> IdMap = new Dictionary<int, Venue>();
        
        public static IList<Venue> List
        {
            get { return new List<Venue> (IdMap.Values); }
        }
            
        public int Id { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string City { get; protected internal set; }
        public string State { get; protected internal set; }
        public int? Elevation { get; protected internal set; }

        public Venue (int id, string name, string city, string state,
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
        
        public static Venue Get (int id)
        {
            return IdMap[id];
        }
    }
}
