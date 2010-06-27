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
        /// Format a race the way Hytek Meet Manger does.
        /// </summary>
        /// <param name="race">
        /// A <see cref="Race"/> to be formatted.
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines of the table.
        /// </returns>
        public IList<string> Format (Race race)
        {
            return Format (race, false);
        }
        
        /// <summary>
        /// Format a race the way Hytek Meet Manger does.
        /// </summary>
        /// <param name="race">
        /// A <see cref="Race"/> to be formatted.
        /// </param>
        /// <param name="showHeader">
        /// Should a header describing race location be shown or not?
        /// </param>
        /// <returns>
        /// A <see cref="IList<System.String>"/> of lines in the table.
        /// </returns>
        public IList<string> Format (Race race, bool showHeader)
        {
            IList<string> resultsLines = ResultsFormatter.Format (race.Distance,
                race.Results);
            List<string> lines = new List<string> ();
            int width = resultsLines[0].Length;
            if (showHeader)
            {
                lines.Add (StringFormatting.Centered (race.Name, width));
                lines.Add (StringFormatting.Centered (race.Date.ToString (), width));
                lines.Add (StringFormatting.Centered (race.Location, width));
                lines.Add ("");
            }
            lines.AddRange(resultsLines);
            lines.Add ("");
            lines.AddRange (ScoreFormatter.Format (race.Scores));
            return (IList<string>)lines;
        }
    }
}
