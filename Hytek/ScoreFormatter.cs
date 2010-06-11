using System;
using System.Collections;
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
            List<IList> values = new List<IList> ();
            for (int i = 0; i < scores.Count; i++)
            {
                IList valueRow = new ArrayList ();
                if (scores[i].Score () != null)
                {
                    valueRow.Add (i + 1);
                }
                else
                {
                    valueRow.Add (null);
                }
                valueRow.Add (scores[i].School.Name);
                valueRow.Add (scores[i].Score ());
                for (int j = 0; j < 7; j++) {
                    if (j < scores[i].Runners.Count)
                    {
                        runner = scores[i].Runners[j];
                        valueRow.Add (runner.Points);
                    }
                    else
                    {
                        valueRow.Add (null);
                    }
                }
                values.Add (valueRow);
                valueRow = new object[Header.Count];
                valueRow[1] = "  Top 5 Avg: " + scores[i].TopFiveAverage ().Time;
                values.Add (valueRow);
                if (scores[i].Runners.Count > 5)
                {
                    valueRow = new object[Header.Count];
                    valueRow[1] = "  Top 7 Avg: " + scores[i].TopSevenAverage ().Time;
                    values.Add (valueRow);
                }
            }
            return base.Format (values, alignments);
        }
    }
}
