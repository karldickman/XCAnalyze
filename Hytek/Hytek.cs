using System;
using System.Collections.Generic;
using TextFormat;
using TextFormat.Table;
using XCAnalyze.Model;

namespace XCAnalyze.Hytek
{
    public interface IFormatter<T>
    {
        IList<string> Format(T thing);
    }
    
    public class HytekFormatter
    {
        /// <summary>
        /// The Header of the hytek table.
        /// </summary>
        protected internal string[] Header { get; set; }

        /// <summary>
        /// The title of the table.
        /// </summary>
        protected internal string Title { get; set; }

        /// <summary>
        /// The formatter used internally.
        /// </summary>
        protected internal LabeledTableFormatter TableFormatter { get; set; }
        
        public HytekFormatter (string[] Header) : this(null, Header) {}
        
        public HytekFormatter (string Title, string[] Header)
        {
            this.Title = Title;
            this.Header = Header;
            TableFormatter = new LabeledTableFormatter ('\0', ' ', '\0', '=', '\0', '=', '\0', '=');
        }
        
        protected internal IList<string> Format (IList<object[]> values, Alignment[] alignments)
        {
            IList<string> tableLines = TableFormatter.Format (Header, values, alignments);
            IList<string> lines = new List<string>();
            if(Title == null)
            {
                return tableLines;
            }
            lines.Add(StringFormatting.Centered(Title, tableLines[0].Length));
            foreach(string line in tableLines)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
    
    public class ResultsFormatter : HytekFormatter, IFormatter<IList<Performance>>
    {        
        public ResultsFormatter() : base(new string[] {null, "Name", "Year", "School", "Finals", "Points"}) {}
        
        public static string AlignPoints (object points, int width)
        {
            return StringFormatting.RightPadded (points, 3) + "   ";
        }
       
        public IList<string> Format (IList<Performance> results)
        {
            Alignment[] alignments = new Alignment[] { StringFormatting.RightJustified, null, null, null, null, AlignPoints };
            IList<object[]> values = new List<object[]> ();
            for (int i = 0; i < results.Count; i++)
            {
                object[] valueRow = new object[Header.Length];
                int n = 0;
                valueRow[n++] = (i + 1).ToString ();
                valueRow[n++] = results[i].Runner.Name;
                valueRow[n++] = results[i].Runner.Year.ToString ();
                valueRow[n++] = results[i].School.Name;               
                valueRow[n++] = results[i].Time.ToString ();
                valueRow[n++] = results[i].Points.ToString ();
                values.Add (valueRow);
            }
            return base.Format (values, alignments);
        }
        
        public IList<string> Format (int distance, IList<Performance> results)
        {
            Title = distance + " m run CC";
            return Format (results);
        }
    }
    
    public class ScoreFormatter : HytekFormatter, IFormatter<IList<TeamScore>>
    {
        public ScoreFormatter() : base("Team Scores", new string[] {"Rank", "Team", "Total", "   1", "   2", "   3", "   4", "   5", "  *6", "  *7"}) {}
        
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
                valueRow[n++] = (i + 1).ToString ();
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
                for (int j = 0; j < 7; j++)
                {
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
            return base.Format(values, alignments);
        }
    }
    
    public class RaceFormatter : IFormatter<Race>
    {
        protected internal ResultsFormatter ResultsFormatter { get; set; }
        protected internal ScoreFormatter ScoreFormatter { get; set; }
        
        public RaceFormatter ()
        {
            ResultsFormatter = new ResultsFormatter ();
            ScoreFormatter = new ScoreFormatter ();
        }
        
        public IList<string> Format (Race race)
        {
            IList<string> resultsLines = ResultsFormatter.Format (race.Distance, race.Results);
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