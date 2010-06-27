using System;

using NUnit.Framework;

using XCAnalyze.Collections;

namespace XCAnalyze.Model
{
    [TestFixture]
    public class TestTeamScore
    {
        [Test]
        public void TestBreakTie ()
        {
            int breakAt = 5;
            IXList<Performance>[] performances = new IXList<Performance>[2] { new XList<Performance> (), new XList<Performance> () };
            Performance[] fifthMen = new Performance[2];
            Race race = new Race (null, 8000);
            fifthMen[0] = new Performance (null, race, 1500);
            fifthMen[1] = new Performance (null, race, 1600);
            fifthMen[0].Points = 5;
            fifthMen[1].Points = 9;
            TeamScore[] scores = new TeamScore[2] { new TeamScore (null, null, performances[0]), new TeamScore (null, null, performances[1]) };
            for (int i = 0; i < breakAt - 1; i++) {
                performances[0].Add (null);
                performances[1].Add (null);
            }
            Assert.AreEqual (0, TeamScore.BreakTie (scores[0], scores[1], breakAt));
            performances[0].Add (fifthMen[0]);
            Assert.AreEqual (-1, TeamScore.BreakTie (scores[0], scores[1], breakAt));
            Assert.AreEqual (1, TeamScore.BreakTie (scores[1], scores[0], breakAt));
            performances[1].Add (fifthMen[1]);
            Assert.AreEqual (fifthMen[0].CompareTo (fifthMen[1]), TeamScore.BreakTie (scores[0], scores[1], breakAt));
            Assert.AreEqual (fifthMen[1].CompareTo (fifthMen[0]), TeamScore.BreakTie (scores[1], scores[0], breakAt));
        }
    }
}
