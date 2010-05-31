using System;
using System.Collections.Generic;

namespace XCAnalyze.Model
{

    public class Data
    {
        public List<Affiliation> Affiliations { get; protected internal set; }
        public List<Performance> Performances { get; protected internal set; }
        public List<Race> Races { get; protected internal set; }
        public List<Runner> Runners { get; protected internal set; }
        public List<School> Schools { get; protected internal set; }

        public Data (List<Affiliation> affiliations, List<Performance> performances, List<Race> races, List<Runner> runners, List<School> schools)
        {
            Affiliations = affiliations;
            Performances = performances;
            Races = races;
            Runners = runners;
            Schools = schools;
            foreach (Affiliation affiliation in affiliations)
            {
                Affiliate (affiliation);
            }
            foreach (Performance performance in performances)
            {
                RegisterPerformance (performance);
            }
        }
        
        protected void Affiliate (Affiliation affiliation)
        {
            affiliation.Runner.AddSchool (affiliation);
            affiliation.School.AddRunner (affiliation);
        }
        
        public void Affiliate (Runner runner, School school, int year)
        {
            Affiliation affiliation = new Affiliation (runner, school, year);
            Affiliations.Add (affiliation);
            Affiliate (affiliation);
        }
        
        public void RegisterPerformance (Performance performance)
        {
            performance.Race.AddResult (performance);
            performance.Runner.AddPerformance (performance);
        }
        
        public void RegisterPerformance (Race race, Runner runner, Time time)
        {
            Performance performance = new Performance (runner, race, time);
            Performances.Add (performance);
            RegisterPerformance (performance);
        }

        public List<Runner> Team (School school, int year, Gender gender)
        {
            List<Runner> found = new List<Runner> ();
            foreach (Affiliation affiliation in Affiliations) {
                if (affiliation.School == school && affiliation.Year == year && affiliation.Runner.Gender == gender) {
                    found.Add (affiliation.Runner);
                }
            }
            return found;
        }
    }
}
