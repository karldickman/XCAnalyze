using System;
using System.Collections.Generic;

using XCAnalyze.Model;

namespace XCAnalyze.IO
{
    abstract public class AbstractXcDataWriter : IWriter<XcData>
    {
        abstract public void Dispose();
        
        /// <summary>
        /// Write data to the database.
        /// </summary>
        /// <param name="data">
        /// The <see cref="Data"/> to be written.
        /// </param>
        public void Write (XcData data)
        {
            WriteConferences (data.Conferences);
            WriteRunners (data.Runners);
            WriteSchools (data.Schools, data.Conferences);
            WriteAffiliations (data.Affiliations, data.Runners, data.Schools);
            WriteMeetNames (data.MeetNames);
            WriteRaces (data.Races);
            WriteVenues (data.Venues);
            WriteMeets (data.Meets, data.MeetNames, data.Races, data.Venues);
            WritePerformances (data.Performances, data.Races, data.Runners);
        }

        abstract public void WriteAffiliations(IList<Affiliation> affiliations,
            IList<Runner> runnerIds, IList<School> schoolIds);                 
                
        abstract public void WriteConferences(IList<string> conferences);
                
        abstract public void WriteMeetNames(IList<string> meetNames);
        
        abstract public void WriteMeets(IList<Meet> meets,
            IList<string> meetNames, IList<Race> races, IList<Venue> venues);
        
        abstract public void WritePerformances(IList<Performance> performances,
            IList<Race> races, IList<Runner> runners);
        
        abstract public void WriteRaces(IList<Race> races);
                
        abstract public void WriteRunners(IList<Runner> runners);
                
        abstract public void WriteSchools(IList<School> schoolIds,
            IList<string> conferences);
                
        abstract public void WriteVenues(IList<Venue> venues);
    }
}
