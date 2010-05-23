using System;
using System.Collections.Generic;

namespace XcAnalyze.Model
{
	
	/// <summary>
	/// A team's score at a race.
	/// </summary>
	public class TeamScore : IComparable<TeamScore>
	{
		private School school;
		private List<Performance> runners;

		/// <summary>
		/// The school which earned the score.
		/// </summary>
		public School School
		{
			get { return school; }
		}

		/// <summary>
		/// The runners who were on the team at that particular race.
		/// </summary>
		public List<Performance> Runners
		{
			get { return runners; }
		}

		public TeamScore (School school, List<Performance> runners)
		{
			this.school = school;
			this.runners = runners;
		}
		
		internal int BreakTie (TeamScore other, int breakAt)
		{
			if (Runners.Count < breakAt && other.Runners.Count < breakAt) {
				return 0;
			}
			if (Runners.Count < breakAt) {
				return 1;
			}
			if (other.Runners.Count < breakAt) {
				return -1;
			}
			return Runners[breakAt].Points.Value.CompareTo (other.Runners[breakAt].Points.Value);
		}
		
		/// <summary>
		/// Team scores are compared first by the numerical score, then by the sixth runner, then by the seventh, then
		/// by the name of the school.
		/// </summary>
		public int CompareTo (TeamScore other)
		{
			int comparison;
			int? score, otherScore;
			if (this == other)
			{
				return 0;
			}
			score = Score ();
			otherScore = other.Score ();
			comparison = Utilities.CompareNullable (score, otherScore, 1);
			if (comparison != 0) 
			{
				return comparison;
			}
			comparison = BreakTie (other, 6);
			if (comparison != 0)
			{
				return comparison;
			}
			comparison = BreakTie (other, 7);
			if (comparison != 0)
			{
				return comparison;
			}
			return School.CompareTo (other.School);
		}

		/// <summary>
		/// A teams score is the sum of the points earned by their first five runners.  Their score is incomplete if
		/// they failed to field five runners.
		/// </summary>
		public int? Score ()
		{
			if (runners.Count < 5) {
				return null;
			}
			int? score = 0;
			for (int i = 0; i < 5; i++) {
				score += runners[i].Points;
			}
			return score;
		}
	}
}