using System;
using System.Collections.Generic;

namespace XcAnalyze.Model
{

	public class Model
	{
		private List<Affiliation> affiliations;
		private List<Performance> performances;
		private List<Race> races;
		private List<Runner> runners;
		private List<School> schools;

		public List<Affiliation> Affiliations {
			get { return affiliations; }
		}

		public List<Performance> Performances {
			get { return performances; }
		}

		public List<Race> Races {
			get { return races; }
		}

		public List<Runner> Runners {
			get { return runners; }
		}

		public List<School> Schools {
			get { return schools; }
		}

		public Model (List<Affiliation> affiliations, List<Performance> performances, List<Race> races, List<Runner> runners, List<School> schools)
		{
			this.affiliations = affiliations;
			this.performances = performances;
			this.races = races;
			this.runners = runners;
			this.schools = schools;
			foreach (Affiliation affiliation in affiliations) {
				Affiliate (affiliation);
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
			Affiliations.Add (affiliation);
			Affiliate (affiliation);
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
