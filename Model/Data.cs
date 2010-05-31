using System;
using System.Collections.Generic;

namespace XCAnalyze.Model
{
    public class Data
    {
        private IList<Affiliation> affiliations;
        private IList<string> conferences;
        private IList<string> meets;
        private IList<Performance> performances;
        private IList<Race> races;
        private IList<Runner> runners;
        private IList<School> schools;
        private IList<string> venues;

        public IList<Affiliation> Affiliations
        {
            get { return affiliations; }
        }

        public IList<string> Conferences
        {
            get { return conferences; }
        }

        public IList<string> Meets
        {
            get { return meets; }
        }

        public IList<Performance> Performances
        {
            get { return performances; }
        }

        public IList<Race> Races
        {
            get { return races; }
        }

        public IList<Runner> Runners
        {
            get { return runners; }
        }

        public IList<School> Schools
        {
            get { return schools; }
        }

        public IList<string> Venues
        {
            get { return venues; }
        }

        public Data (IList<Affiliation> affiliations, IList<string> conferences, IList<string> meets, IList<Performance> performances, IList<Race> races, IList<Runner> runners, IList<School> schools, IList<string> venues)
        {
            this.affiliations = affiliations;
            this.conferences = conferences;
            this.meets = meets;
            this.performances = performances;
            this.races = races;
            this.runners = runners;
            this.schools = schools;
            this.venues = venues;
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
            affiliation.Runner.AddAffiliation (affiliation);
            affiliation.School.AddAffiliation (affiliation);
        }
        
        public void Affiliate (Runner runner, School school, int year)
        {
            Affiliation affiliation = new Affiliation (runner, school, year);
            affiliations.Add (affiliation);
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
            performances.Add (performance);
            RegisterPerformance (performance);
        }

        public List<Runner> Team (School school, int year, Gender gender)
        {
            List<Runner> found = new List<Runner> ();
            foreach (Affiliation affiliation in affiliations) {
                if (affiliation.School == school && affiliation.Year == year && affiliation.Runner.Gender == gender) {
                    found.Add (affiliation.Runner);
                }
            }
            return found;
        }
    }
}
