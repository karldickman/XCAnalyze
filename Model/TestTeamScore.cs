using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace XCAnalyze.Model
{
    public partial class TeamScore
    {
#if DEBUG
        [TestFixture]
        public class Test
        {
            Race Race
            {
                get { return SampleData.RaceLookup[SampleData.CharlesBowles09][Gender.Male]; }
            }
            
            [Test]
            public void TestBreakTie ()
            {
                int breakAt = 5;
                //Create a pair of performance lists
                IList<Performance>[] performances = new IList<Performance>[2] {
                    new List<Performance> (), new List<Performance> () };
                //Create a pair of fifth men
                Performance[] fifthMen = new Performance[2];
                //Team 0: Willamette
                fifthMen[0] = new Performance (SampleData.Leo, Race, 1600);
                //Team 1: Lewis & Clark
                fifthMen[1] = new Performance (SampleData.Karl, Race, 1500);
                fifthMen[0].Points = 9;
                fifthMen[1].Points = 5;
                //Add scores for the first four men
                for (int i = 0; i < breakAt - 1; i++) {
                    performances[0].Add (null);
                    performances[1].Add (null);
                }
                //Create scores for the two teams
                TeamScore[] scores = new TeamScore[2] {
                    new TeamScore (Race, SampleData.Willamette, performances[0]),
                    new TeamScore (Race, SampleData.LewisAndClark, performances[1])
                };
                //If only four men have scores, the scores are equal (incomplete)
                Assert.AreEqual (0, TeamScore.BreakTie (scores[0], scores[1], breakAt));
                //If Willamette has a fifth man, Willamette is better than Lewis & Clark
                scores[0].AddRunner (fifthMen[0]);
                Assert.AreEqual (-1, TeamScore.BreakTie (scores[0], scores[1], breakAt));
                Assert.AreEqual (1, TeamScore.BreakTie (scores[1], scores[0], breakAt));
                //If both teams have a fifth man, Lewis & Clark is better than Willamette
                scores[1].AddRunner (fifthMen[1]);
                Assert.AreEqual (fifthMen[0].CompareTo (fifthMen[1]), TeamScore.BreakTie (scores[0], scores[1], breakAt));
                Assert.AreEqual (fifthMen[1].CompareTo (fifthMen[0]), TeamScore.BreakTie (scores[1], scores[0], breakAt));
            }
        }
#endif
    }
}
