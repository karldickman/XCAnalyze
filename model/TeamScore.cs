using NUnit.Framework;
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
		
		public TeamScore(School school) : this(school, new List<Performance>()) {}

		internal TeamScore (School school, List<Performance> runners)
		{
			this.school = school;
			this.runners = runners;
		}
		
		/// <summary>
		/// Compares the breakAt runner of this team with the the breakAt runner of the other team.
		/// </summary>
		protected internal static int BreakTie (TeamScore item1, TeamScore item2, int breakAt)
		{
			if (item1.Runners.Count < breakAt && item2.Runners.Count < breakAt) {
				return 0;
			}
			if (item1.Runners.Count < breakAt) {
				return 1;
			}
			if (item2.Runners.Count < breakAt) {
				return -1;
			}
			return item1.Runners[breakAt - 1].Points.Value.CompareTo (item2.Runners[breakAt - 1].Points.Value);
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
			comparison = BreakTie (this, other, 6);
			if (comparison != 0)
			{
				return comparison;
			}
			comparison = BreakTie (this, other, 7);
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
		
		/// <summary>
		/// The averate time of the top 5 runners on the team.
		/// </summary>
		public double TopFiveAverate ()
		{
			return TopXAverage (5);
		}
		
		/// <summary>
		/// The average time of the top 7 runners on the team.
		/// </summary>
		public double TopSevenAverate ()
		{
			return TopXAverage (7);
		}
		
		/// <summary>
		/// The average time of the top x runners on the team.
		/// </summary>
		protected internal double TopXAverage(int x)
		{
			double sum = 0.0;
			int number;
			if (Runners.Count < x) 
			{
				number = Runners.Count;
			}
			else
			{
				number = x;
			}
			for (int i = 0; i < number; i++)
			{
				sum += Runners[i].Time;
			}
			return sum / number;
		}
	}
	
	[TestFixture]
	public class TestTeamScore
	{
		[Test]
		public void TestBreakTie ()
		{
			int breakAt = 5;
			List<Performance>[] performances = new List<Performance>[2] { new List<Performance> (), new List<Performance> () };
			Performance[] fifthMen = new Performance[2];
			Race race = new Race(null, DateTime.Now, null, 8000, null, null, null);
			fifthMen[0] = new Performance (null, race, 1500);
			fifthMen[1] = new Performance (null, race, 1600);
			fifthMen[0].Points = 5;
			fifthMen[1].Points = 9;
			TeamScore[] scores = new TeamScore[2] { new TeamScore (null, performances[0]), new TeamScore (null, performances[1]) };
			for (int i = 0; i < breakAt - 1; i++) 
			{
				performances[0].Add (null);
				performances[1].Add (null);
			}
			Assert.AreEqual (0, TeamScore.BreakTie (scores[0], scores[1], breakAt));
			performances[0].Add (fifthMen[0]);
			Assert.AreEqual (-1, TeamScore.BreakTie (scores[0], scores[1], breakAt));
			Assert.AreEqual (1, TeamScore.BreakTie (scores[1], scores[0], breakAt));
			performances[1].Add (fifthMen[1]);
			Assert.AreEqual (fifthMen[0].CompareTo (fifthMen[1]), TeamScore.BreakTie (scores[0], scores[1], breakAt));
			Assert.AreEqual(fifthMen[1].CompareTo (fifthMen[0]), TeamScore.BreakTie(scores[1], scores[0], breakAt));
		}
	}
}