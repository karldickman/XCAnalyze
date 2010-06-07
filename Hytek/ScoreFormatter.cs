using System;
using System.Collections.Generic;
using TextFormat;
using XCAnalyze.Model;

namespace XCAnalyze.Hytek
{
    /// <summary>
    /// A formatter for the team scores of a race.
    /// </summary>
    public class ScoreFormatter : HytekFormatter, IFormatter<IList<TeamScore>>
    {
        /// <summary>
        /// Create a new formatter.
        /// </summary>
        public ScoreFormatter ()
            : base("Team Scores", new string[] { "Rank", "Team", "Total",
                "   1", "   2", "   3", "   4", "   5", "  *6", "  *7" }) {}

        /// <summary>
        /// Format the team scores.
        /// </summary>
        /// <param name="scores">
        /// The <see cref="IList<TeamScore>"/> to format.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of physical lines of the table.
        /// </returns>
        public IList<string> Format (IList<TeamScore> scores)
        {
            Alignment R = StringFormatting.RightJustified;
            Alignment[] alignments = new Alignment[] { R, null, R, R, R, R, R, R, R, R };
            Performance runner;
            int? score;
            List<object[]> values = new List<object[]> ();
            for (int i = 0; i < scores.Count; i++)
            {
                int n = 0;
                object[] valueRow = new object[Header.Length];
                if (scores[i].Score () != null)
                {
                    valueRow[n++] = (i + 1).ToString ();
                }
                else
                {
                    valueRow[n++] = "";
                }
                valueRow[n++] = scores[i].School.Name;
                score = scores[i].Score ();
                if (score != null)
                {
                    valueRow[n++] = score.Value.ToString ();
                }
                else
                {
                    valueRow[n++] = null;
                }
                for (int j = 0; j < 7; j++) {
                    if (j < scores[i].Runners.Count)
                    {
                        runner = scores[i].Runners[j];
                        if (runner.Points != null)
                        {
                            valueRow[n++] = runner.Points.ToString ();
                        }
                        else
                        {
                            valueRow[n++] = null;
                        }
                    }
                    else
                    {
                        valueRow[n++] = null;
                    }
                }
                values.Add (valueRow);
                valueRow = new object[Header.Length];
                valueRow[1] = "  Top 5 Avg: " + scores[i].TopFiveAverage ().Time;
                values.Add (valueRow);
                if (scores[i].Runners.Count > 5)
                {
                    valueRow = new object[Header.Length];
                    valueRow[1] = "  Top 7 Avg: " + scores[i].TopSevenAverage ().Time;
                    values.Add (valueRow);
                }
            }
            return base.Format (values, alignments);
        }
    }
}
