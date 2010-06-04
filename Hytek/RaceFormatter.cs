using System;
using System.Collections.Generic;
using TextFormat;
using XCAnalyze.Model;

namespace XCAnalyze.Hytek
{
    /// <summary>
    /// A formatter for races.
    /// </summary>
    public class RaceFormatter : IFormatter<Race>
    {
        /// <summary>
        /// The formatter for the results.
        /// </summary>
        protected internal ResultsFormatter ResultsFormatter { get; set; }
        
        /// <summary>
        /// The formatter for the scores.
        /// </summary>
        protected internal ScoreFormatter ScoreFormatter { get; set; }
        
        /// <summary>
        /// Create a new formatter.
        /// </summary>
        public RaceFormatter ()
        {
            ResultsFormatter = new ResultsFormatter ();
            ScoreFormatter = new ScoreFormatter ();
        }
        
        /// <summary>
        /// Format a race.
        /// </summary>
        /// <param name="race">
        /// A <see cref="Race"/> to be formatted.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines of the table.
        /// </returns>
        public IList<string> Format (Race race)
        {
            IList<string> resultsLines = ResultsFormatter.Format (race.Distance,
                race.Results);
            List<string> lines = new List<string> ();
            int width = resultsLines[0].Length;
            lines.Add (StringFormatting.Centered (race.Meet, width));
            lines.Add (StringFormatting.Centered (race.Date.ToString(), width));
            lines.Add (StringFormatting.Centered (race.Location, width));
            lines.Add ("");
            lines.AddRange(resultsLines);
            lines.Add ("");
            lines.AddRange (ScoreFormatter.Format (race.Scores));
            return (IList<string>)lines;
        }
    }
}
