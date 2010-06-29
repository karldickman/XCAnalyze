using System;
using System.Collections;
using System.Collections.Generic;

using TextFormat;
using XCAnalyze.Model;

namespace XCAnalyze.Hytek
{
    /// <summary>
    /// A formatter for the list of times in a race.
    /// </summary>
    public class ResultsFormatter : HytekFormatter, IFormatter<IList<Performance>>
    {
        /// <summary>
        /// Create a new formatter.
        /// </summary>
        public ResultsFormatter ()
            : base(new string[] { null, "Name", "Year", "School", "Finals",
                "Points" }) {}

        /// <summary>
        /// The alignment used for points.
        /// </summary>
        /// <param name="points">
        /// A <see cref="System.Object"/>.  The points the runner got.
        /// </param>
        /// <param name="width">
        /// A <see cref="System.Int32"/>.  The width of the desired string.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>.  If points is wider than width, this
        /// is the same as points.ToString().
        /// </returns>
        public static string AlignPoints (object points, int width)
        {
            return StringFormatting.RightPadded (points, 3) + "   ";
        }

        /// <summary>
        /// Format the results into a table.
        /// </summary>
        /// <param name="results">
        /// A <see cref="IList<Performance>"/> of results.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines in the table.
        /// </returns>
        public IList<string> Format (IList<Performance> results)
        {
            Alignment[] alignments = new Alignment[] {
                StringFormatting.RightJustified, null, null, null, null,
                AlignPoints };
            IList<IList> values = new List<IList> ();
            for (int i = 0; i < results.Count; i++) {
                object[] valueRow = new object[Header.Count];
                int n = 0;
                valueRow[n++] = (i + 1).ToString ();
                valueRow[n++] = results[i].Runner.Name;
                valueRow[n++] = results[i].Runner.Year.ToString ();
                if (results[i].School == null)
                {
                    valueRow[n++] = "";
                }
                else
                {
                    valueRow[n++] = results[i].School.Name;
                }
                valueRow[n++] = FormatTime(results[i].Time);
                valueRow[n++] = results[i].Points.ToString ();
                values.Add (valueRow);
            }
            return base.Format (values, alignments);
        }

        /// <summary>
        /// Format results in the table.
        /// </summary>
        /// <param name="distance">
        /// A <see cref="System.Int32"/>.  The length of the race.
        /// </param>
        /// <param name="results">
        /// A <see cref="IList<Performance>"/> of results.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines in the table.
        /// </returns>
        public IList<string> Format (int distance, IList<Performance> results)
        {
            Title = distance + " m run CC";
            return Format (results);
        }
    }
}
