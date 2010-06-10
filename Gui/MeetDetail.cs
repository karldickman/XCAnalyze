using Gtk;
using System;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class MeetDetail : VBox
    {
        /// <summary>
        /// Some information about the race.
        /// </summary>
        protected internal Label Info { get; set; }
        
        /// <summary>
        /// The widget used to display the mens race.
        /// </summary>
        protected internal RaceResults MensRace { get; set; }
        
        /// <summary>
        /// The widget used to display the womens race.
        /// </summary>
        protected internal RaceResults WomensRace { get; set; }
        
        protected internal MeetDetail()
        {
            Spacing = 20;
        }
        
        /// <summary>
        /// Create a new meet viewer to display a particular meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to display.
        /// </param>
        public MeetDetail (Meet meet) : this()
        {
            MensRace = new RaceResults (meet.MensRace);
            WomensRace = new RaceResults (meet.WomensRace);
            Info = new Label (meet.Name + "\n" + meet.Date + "\n" + meet.Venue);
            Info.Justify = Justification.Center;
            Add (Info);
            Add (MensRace);
            Add (WomensRace);
        }
        
        /// <summary>
        /// Set the default dimensions of all children of this widget.
        /// </summary>
        public void SetSizeRequest ()
        {
            if(Children.Length == 0)
            {
                SetSizeRequest(Screen.Width / 4, Screen.Height / 3);
                return;
            }
            int infoHeight = Info.SizeRequest().Height;
            int height, width;
            MensRace.SetSizeRequest ();
            WomensRace.SetSizeRequest ();
            Requisition mensSize = MensRace.SizeRequest ();
            Requisition womensSize = WomensRace.SizeRequest ();
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
            MensRace.SetSizeRequest(width, height);
            WomensRace.SetSizeRequest(width, height);
        }
    }
}
