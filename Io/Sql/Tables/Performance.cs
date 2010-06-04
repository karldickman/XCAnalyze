using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class Performance : Model.Performance
    {
        protected internal static IDictionary<int, Model.Performance> IdMap = new Dictionary<int, Model.Performance>();
        
        public static IList<Model.Performance> List
        {
            get { return new List<Model.Performance>(IdMap.Values); }
        }
        
        /// <summary>
        /// The row id.
        /// </summary>
        public int Id { get; protected internal set; }
        
        override public Model.Runner Runner
        {
            get
            {
                if (Tables.Runner.Exists (RunnerId))
                {
                    return Tables.Runner.Get (RunnerId);
                }
                return base.Runner;
            }
            
            protected internal set
            {
                if(value is Runner)
                {
                    RunnerId = ((Runner)value).Id;
                }
                else
                {
                    base.Runner = value;
                }
            }
        }
        
        /// <summary>
        /// The row id of the runner.
        /// </summary>
        public int RunnerId { get; protected internal set; }
        
        override public Model.Race Race
        {
            get
            {
                if(Tables.Race.Exists(RaceId))
                {
                    return Tables.Race.Get(RaceId);
                }
                return base.Race;
            }
            
            protected internal set
            {
                if(value is Race)
                {
                    RaceId = ((Race)value).Id;
                }
                else
                {
                    base.Race = value;
                }
            }
        }
        
        /// <summary>
        /// The row id of the race.
        /// </summary>
        public int RaceId { get; protected internal set; }
        
        public Performance (int id, int runnerId, int raceId, Model.Time time)
            : base(time)
        {
            Id = id;
            RunnerId = runnerId;
            RaceId = raceId;
        }
    }
}
