using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    public class SqlAffiliation : Model.Affiliation
    {
        protected internal static IDictionary<int, Model.Affiliation> IdMap = new Dictionary<int, Model.Affiliation>();
        
        public static IList<Model.Affiliation> List
        {
            get { return new List<Model.Affiliation> (IdMap.Values); }
        }
        
        /// <summary>
        /// The row id of the affiliation.
        /// </summary>
        public int Id { get; protected internal set; }
        
        override public Model.Runner Runner
        {
            get
            {
                if(SqlRunner.Exists(RunnerId))
                {
                    return SqlRunner.Get(RunnerId);
                }
                return base.Runner;
            }
            
            protected internal set
            {
                if(value is SqlRunner)
                {
                    RunnerId = ((SqlRunner)value).Id;
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
        
        override public Model.School School
        {
            get { return SqlSchool.Get(SchoolId); }
        }
        
        /// <summary>
        /// The row id of the school.
        /// </summary>
        public int SchoolId { get; protected internal set; }

        public SqlAffiliation (int id, int runnerId, int schoolId, int year)
            : base(year)
        {
            Id = id;
            RunnerId = runnerId;
            SchoolId = schoolId;
        }
    }
}
