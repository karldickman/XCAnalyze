using System;
using System.Collections.Generic;

namespace XCAnalyze.Io.Sql.Tables
{
    /// <summary>
    /// A representation of an entry in the affiliations table of the database.
    /// </summary>
    public class Affiliation : Model.Affiliation
    {
        /// <summary>
        /// A registry of all instances of the class (i.e. rows in the table) by
        /// id number.
        /// </summary>
        protected internal static IDictionary<int, Model.Affiliation> IdMap =
            new Dictionary<int, Model.Affiliation>();
        
        /// <summary>
        /// Get all instances of the class.
        /// </summary>
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
                if(Tables.Runner.Exists(RunnerId))
                {
                    return Tables.Runner.Get(RunnerId);
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
        
        override public Model.School School
        {
            get
            {
                if(Tables.School.Exists(SchoolId))
                {
                    return Tables.School.Get(SchoolId);
                }
                return base.School;
            }
            
            protected internal set
            {
                if(value is School)
                {
                    SchoolId = ((School)value).Id;
                }
                else
                {
                    base.School = value;
                }
            }
        }
        
        /// <summary>
        /// The row id of the school.
        /// </summary>
        public int SchoolId { get; protected internal set; }

        /// <summary>
        /// Create a new affiliation between a runner and school.
        /// </summary>
        /// <param name="id">
        /// The id number of the affilation.
        /// </param>
        /// <param name="runnerId">
        /// The id number of the runner.
        /// </param>
        /// <param name="schoolId">
        /// The id number of the school.
        /// </param>
        /// <param name="year">
        /// The year in which the affiliation occurred.
        /// </param>
        public Affiliation (int id, int runnerId, int schoolId, int year)
            : base(year)
        {
            Id = id;
            RunnerId = runnerId;
            SchoolId = schoolId;
            IdMap[id] = this;
        }
    }
}
