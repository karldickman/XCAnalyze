using Gtk;
using System;
using System.Collections.Generic;
using XCAnalyze.Hytek;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class RaceResultsWidget : ScrolledWindow
    {
        /// <summary>
        /// The default tag used for the text.
        /// </summary>
        TextTag DefaultTag { get; set; }
    
        /// <summary>
        /// The text buffer.
        /// </summary>
        TextBuffer Buffer { get; set; }
    
        /// <summary>
        /// The object used to format the race results.
        /// </summary>
        RaceFormatter Formatter { get; set; }
    
        /// <summary>
        /// The race this widget is supposed to display.
        /// </summary>
        Race Race { get; set; }
    
        /// <summary>
        /// The text stored in the text buffer.
        /// </summary>
        string Text {
            get { return TextWidget.Buffer.Text; }
            set { TextWidget.Buffer.Text = value; }
        }
    
        /// <summary>
        /// The TextView widget that displays the race results.
        /// </summary>
        TextView TextWidget { get; set; }
    
        RaceResultsWidget ()
        {
            TextTag defaultTag = new TextTag ("default");
            defaultTag.Family = "courier";
            //defaultTag.Font = "monospace 10";
            Buffer = new TextBuffer (null);
            Buffer.TagTable.Add (defaultTag);
            TextWidget = new TextView (Buffer);
            Add (TextWidget);
        }
    
        public RaceResultsWidget (Race race) : this()
        {
            Race = race;
            race.Score ();
            Formatter = new RaceFormatter ();
            Text = String.Join ("\n", new List<string>(Formatter.Format (race)).ToArray());
            Buffer.ApplyTag ("default", Buffer.StartIter, Buffer.EndIter);
        }
    }
}