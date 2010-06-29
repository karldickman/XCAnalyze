using System;

using Gtk;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class MeetDetail : VBox
    {            
        /// <summary>
        /// Some information about the race.
        /// </summary>
        protected Label Info { get; set; }
        
        /// <summary>
        /// The widget used to display the mens race.
        /// </summary>
        protected Container MensRace { get; set; }
        
        /// <summary>
        /// The container that holds the two races.
        /// </summary>
        protected Box RaceContainer { get; set; }
        
        /// <summary>
        /// The widget used to display the womens race.
        /// </summary>
        protected Container WomensRace { get; set; }
        
        protected MeetDetail ()
        {
            //Create the heading label
            Info = new Label ();
            PackStart (Info, false, false, 10);
            Info.Justify = Justification.Center;
            //Create container for races
            RaceContainer = new VBox ();
            Add (RaceContainer);
            RaceContainer.Spacing = 20;
            //Create mens race container
            MensRace = new ScrolledWindow ();
            RaceContainer.Add (MensRace);
            //Create womens race container
            WomensRace = new ScrolledWindow ();
            RaceContainer.Add (WomensRace);
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
            MensRace.Add (new TextView(mensBuffer));
            WomensRace.Add (new TextView(womensBuffer));
        }
        
        public void OnSelectionChanged (object sender, EventArgs arguments)
        {
            Meet meet = ((SelectionChangedArgs<Meet>)arguments).Selected;
            Info.Text = string.Format("{0}\n{1:yyyy/MM/dd}\n{2}", meet.Name,
                meet.Date, meet.Location);
        }
    }
}
