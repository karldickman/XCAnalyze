using Gtk;
using System;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class MeetDetail : VBox
    {
        protected internal static readonly RaceResults NULL_RACE =
            new RaceResults();
            
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
            Info = new Label (meet.Name + "\n" + meet.Date + "\n" + meet.Venue);
            Info.Justify = Justification.Center;
            Add (Info);
            if (meet.MensRace != null)
            {
                MensRace = new RaceResults (meet.MensRace);
                Add (MensRace);
            }
            else
            {
                MensRace = NULL_RACE;
            }
            if (meet.WomensRace != null)
            {
                WomensRace = new RaceResults (meet.WomensRace);
                Add (WomensRace);
            }
            else
            {
                WomensRace = NULL_RACE;
            }
        }
        
        /// <summary>
        /// Set the default dimensions of all children of this widget.
        /// </summary>
        public void SetSizeRequest ()
        {
            int height, width;
            Requisition mensSize, womensSize;
            MensRace.SetSizeRequest ();
            WomensRace.SetSizeRequest ();
            mensSize = MensRace.SizeRequest ();
            womensSize = WomensRace.SizeRequest ();
            if(MensRace == NULL_RACE || WomensRace == NULL_RACE)
            {
                height = Screen.Height * 2/3;
            }
            else
            {
                height = Math.Max(mensSize.Height, womensSize.Height) / 2;
            }
            width = Math.Max(mensSize.Width, womensSize.Width);
            MensRace.SetSizeRequest(width, height);
            WomensRace.SetSizeRequest(width, height);
        }
    }
}
