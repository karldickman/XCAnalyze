using System;
using System.Collections.Generic;

using XCAnalyze.Model;

namespace XCAnalyze.IO
{
    abstract public class AbstractXcDataWriter
    {        
        /// <summary>
        /// Write data to the database.
        /// </summary>
        /// <param name="data">
        /// The <see cref="Data"/> to be written.
        /// </param>
        public void Write (DataContext data)
        {
            WriteConferences (data.Conferences);
            WriteRunners (data.Runners);
            WriteTeams (data.Teams);
            WriteAffiliations (data.Affiliations);
            WriteStates (data.States);
            WriteCities (data.Cities);
            WriteVenues (data.Venues);
            WriteMeets (data.Meets);
            WriteMeetInstances (data.MeetInstances);
            WriteRaces (data.Races);
            WritePerformances (data.Performances);
        }

        abstract public void WriteAffiliations(IEnumerable<Affiliation> affiliations);                 
                
        abstract public void WriteCities(IEnumerable<City> cities);
            
        abstract public void WriteConferences(IEnumerable<Conference> conferences);
                
        abstract public void WriteMeets(IEnumerable<Meet> meets);
        
        abstract public void WriteMeetInstances(
            IEnumerable<MeetInstance> meetInstances);
        
        abstract public void WritePerformances(
            IEnumerable<Performance> performances);
        
        abstract public void WriteRaces(IEnumerable<Race> races);
                
        abstract public void WriteRunners(IEnumerable<Runner> runners);
        
        abstract public void WriteStates(IEnumerable<State> states);
        
        abstract public void WriteTeams(IEnumerable<Team> teams);
                
        abstract public void WriteVenues(IEnumerable<Venue> venues);
    }
}
