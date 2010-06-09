using Gtk;
using System;
using System.Collections.Generic;
using XCAnalyze.Hytek;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class RaceResultsViewer : ScrolledWindow
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
    
        /// <summary>
        /// Create a new race viewer with no associated race.
        /// </summary>
        RaceResultsViewer ()
        {
            DefaultTag = new TextTag ("default");
            DefaultTag.Font = "courier";
            Buffer = new TextBuffer (null);
            Buffer.TagTable.Add (DefaultTag);
            TextWidget = new TextView (Buffer);
            TextWidget.Editable = false;
            Add (TextWidget);
        }
    
        /// <summary>
        /// Create a new race viewer to look at a particular race.
        /// </summary>
        /// <param name="race">
        /// The <see cref="Race"/> to view.
        /// </param>
        public RaceResultsViewer (Race race) : this()
        {
            Race = race;
            race.Score ();
            Formatter = new RaceFormatter ();
            Text = "\n" + String.Join ("\n", new List<string> (Formatter.Format (race)).ToArray ());
            Buffer.ApplyTag ("default", Buffer.StartIter, Buffer.EndIter);
        }
        
        /// <summary>
        /// Set the default dimensions of this widget and all its children.
        /// </summary>
        public void SetSizeRequest ()
        {
            double factor = 6.0;//For ten point font
            int width = 0, height;
            height = Screen.Height * 2 / 3;
            foreach(string line in Text.Split('\n'))
            {
                if(line.Length > width)
                {
                    width = line.Length;
                }
            }
            SetSizeRequest((int)(width * factor + 20), height);
        }
        
        new public void SetSizeRequest (int width, int height)
        {
            base.SetSizeRequest (width, height);
        }
    }
}