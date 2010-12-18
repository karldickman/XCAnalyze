using System;
using Gtk;
using XCAnalyze.Collections;
using XCAnalyze.Hytek;
using XCAnalyze.Model;
using System.Collections.Generic;


namespace XCAnalyze.Gui
{
    /// <summary>
    /// A text buffer that proviedes the model for a view of a race.
    /// </summary>
    public class RaceResultsBuffer : TextBuffer
    {
        #region Properties
        
        /// <summary>
        /// The default tag used for the text.
        /// </summary>
        TextTag DefaultTag { get; set; }

        /// <summary>
        /// The object used to format the race results.
        /// </summary>
        RaceFormatter Formatter { get; set; }

        #endregion
        
        #region Constructors
        
        protected RaceResultsBuffer() : base(null)
        {
            DefaultTag = new TextTag("default");
            DefaultTag.Font = "courier";
            TagTable.Add(DefaultTag);
            Formatter = new RaceFormatter();
        }
        
        public RaceResultsBuffer(IEnumerable<Race> races) : this()
        {
            if(races == null)
            {
                throw new ArgumentNullException("race");
            }
            foreach(Race race in races)
            {
                race.Score();
                Text += "\n" + String.Join("\n", Formatter.Format(race).ToArray());
            }
            ApplyTag(DefaultTag, StartIter, EndIter);
        }
        
        #endregion
    }
}
