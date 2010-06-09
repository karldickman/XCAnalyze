using Gtk;
using System;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class MeetViewer : VBox
    {
        /// <summary>
        /// Some information about the race.
        /// </summary>
        protected internal Label Info { get; set; }
        
        /// <summary>
        /// The meet this viewer displays.
        /// </summary>
        protected internal Meet Meet { get; set; }
        
        /// <summary>
        /// The widget used to display the mens race.
        /// </summary>
        protected internal RaceResultsViewer MensRaceViewer { get; set; }
        
        /// <summary>
        /// The widget used to display the womens race.
        /// </summary>
        protected internal RaceResultsViewer WomensRaceViewer { get; set; }
        
        /// <summary>
        /// Create a new meet viewer to display a particular meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to display.
        /// </param>
        public MeetViewer (Meet meet)
        {
            Spacing = 20;
            Meet = meet;
            MensRaceViewer = new RaceResultsViewer (Meet.MensRace);
            WomensRaceViewer = new RaceResultsViewer (Meet.WomensRace);
            Info = new Label (Meet.Name + "\n" + Meet.Date + "\n" + Meet.Venue);
            Info.Justify = Justification.Center;
            Add (Info);
            Add (MensRaceViewer);
            Add (WomensRaceViewer);
        }
        
        /// <summary>
        /// Set the default dimensions of all children of this widget.
        /// </summary>
        public void SetSizeRequest ()
        {
            int infoHeight = Info.SizeRequest().Height;
            int height, width;
            MensRaceViewer.SetSizeRequest ();
            WomensRaceViewer.SetSizeRequest ();
            Requisition mensSize = MensRaceViewer.SizeRequest ();
            Requisition womensSize = WomensRaceViewer.SizeRequest ();
            height = mensSize.Height / 2;
            width = mensSize.Width;
            if(womensSize.Width > width)
            {
                width = womensSize.Width;
            }
            if(infoHeight + (height + Spacing)* 2 > Screen.Height)
            {
                height = (Screen.Height - infoHeight - 2*Spacing) / 2;
            }
            MensRaceViewer.SetSizeRequest(width, height);
            WomensRaceViewer.SetSizeRequest(width, height);
        }
    }
}
