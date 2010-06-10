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
        }
        
        public RaceResultsBuffer (Race race) : this()
        {
            race.Score ();
            Formatter = new RaceFormatter ();
            Text = "\n" + String.Join ("\n",
                new List<string> (Formatter.Format (race)).ToArray ());
            ApplyTag ("default", StartIter, EndIter);
        }
    }
    
    /// <summary>
    /// A widget to display race results in a Hytek-style format.
    /// </summary>
    public class RaceResultsView : TextView
    {
        protected internal RaceResultsView ()
        {
            Editable = false;
        }
    
        public RaceResultsView (TextBuffer buffer) : this()
        {
            Buffer = buffer;
        }
    }
    
    /// <summary>
    /// A scrollable window containing a RaceResultsView.
    /// </summary>
    public class RaceResults : ScrolledWindow
    {
        /// <summary>
        /// The view used to show the race results.
        /// </summary>
        protected internal TextView View { get; set; }
            
        public RaceResults (Race race)
        {
            TextBuffer buffer = new RaceResultsBuffer (race);
            View = new RaceResultsView (buffer);
            Add (View);
        }
        
        /// <summary>
        /// Set the default dimensions of this widget and all its children.
        /// </summary>
        public void SetSizeRequest ()
        {
            double factor = 6.0;//For ten point font
            int width = 0, height;
            height = Screen.Height * 2 / 3;
            foreach(string line in View.Buffer.Text.Split('\n'))
            {
                if(line.Length > width)
                {
                    width = line.Length;
                }
            }
            SetSizeRequest((int)(width * factor + 20), height);
        }
    }
}