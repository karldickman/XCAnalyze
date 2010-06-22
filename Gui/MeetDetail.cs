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
        protected internal Container MensRace { get; set; }
        
        /// <summary>
        /// The widget used to display the womens race.
        /// </summary>
        protected internal Container WomensRace { get; set; }
        
        protected internal MeetDetail()
        {
            Spacing = 20;
            Info = new Label();
            Info.Justify = Justification.Center;
        }
        
        /// <summary>
        /// Create a new meet viewer to display a particular meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to display.
        /// </param>
        public MeetDetail (IDataSelection<Meet> meetSelection) : this()
        {
            meetSelection.SelectionChanged += OnSelectionChanged;
            RaceDisplayModel mensRaceModel =
                new RaceDisplayModel (Gender.MALE, meetSelection);
            RaceDisplayModel womensRaceModel =
                new RaceDisplayModel (Gender.FEMALE, meetSelection);
            RaceResultsBuffer mensBuffer = new RaceResultsBuffer (mensRaceModel);
            RaceResultsBuffer womensBuffer =
                new RaceResultsBuffer (womensRaceModel);
            TextView mensTextView = new TextView (mensBuffer);
            TextView womensTextView = new TextView (womensBuffer);
            MensRace = new ScrolledWindow ();
            WomensRace = new ScrolledWindow ();
            MensRace.Add (mensTextView);
            WomensRace.Add (womensTextView);
            Add (MensRace);
            Add (WomensRace);
        }
        
        public void OnSelectionChanged (object sender, EventArgs arguments)
        {
            Meet meet = ((SelectionChangedArgs<Meet>)arguments).Selected;
            Info.Text = meet.Name + "\n" + meet.Date + "\n" + meet.Venue;
        }
    }
}
