using Gtk;
using System;
using System.Collections.Generic;
using XCAnalyze.Hytek;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// A text buffer that proviedes the model for a view of a race.
    /// </summary>
    public class RaceResultsBuffer : TextBuffer
    {
        /// <summary>
        /// The default tag used for the text.
        /// </summary>
        TextTag DefaultTag { get; set; }
        
        /// <summary>
        /// The object used to format the race results.
        /// </summary>
        RaceFormatter Formatter { get; set; }
    
        protected internal RaceResultsBuffer () : base(null)
        {
            DefaultTag = new TextTag ("default");
            DefaultTag.Font = "courier";
            TagTable.Add (DefaultTag);
            Formatter = new RaceFormatter ();
        }
        
        public RaceResultsBuffer (IDataSelection<Race> selection) : this()
        {
            selection.SelectionChanged += OnSelectionChanged;
        }
        
        public void OnSelectionChanged (object sender,
            SelectionChangedArgs<Race> arguments)
        {
            Race race = arguments.Selected;
            if (race != null)
            {
                race.Score ();
                Text = "\n" + String.Join ("\n",
                new List<string> (Formatter.Format (race)).ToArray ());
                ApplyTag (DefaultTag, StartIter, EndIter);
            }
        }
    }
}
