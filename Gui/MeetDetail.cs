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
        /// The container in which the content of the men's race is displayed.
        /// </summary>
        protected Container MensRace { get; set; }

        /// <summary>
        /// The widget used to display the men's race.
        /// </summary>
        protected Widget MensRaceContent { get; set; }

        /// <summary>
        /// The container that holds the two races.
        /// </summary>
        protected VPaned RaceContainer { get; set; }

        /// <summary>
        /// The container in which the content of the women's race is displayed.
        /// </summary>
        protected Container WomensRace { get; set; }

        /// <summary>
        /// The widget used to display the women's race.
        /// </summary>
        protected Widget WomensRaceContent { get; set; }
        
        protected MeetDetail ()
        {
            //Create the heading label
            Info = new Label ();
            PackStart (Info, false, false, 10);
            Info.Justify = Justification.Center;
            //Create container for races
            RaceContainer = new VPaned ();
            Add (RaceContainer);
            //Create mens race container
            MensRace = new ScrolledWindow ();
            RaceContainer.Pack1 (MensRace, true, true);
            //Create womens race container
            WomensRace = new ScrolledWindow ();
            RaceContainer.Pack2 (WomensRace, true, true);
        }
        
        /// <summary>
        /// Create a new meet viewer to display a particular meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to display.
        /// </param>
        public MeetDetail (IDataSelection<MeetInstance> meetSelection) : this()
        {
            meetSelection.SelectionChanged += OnSelectionChanged;
            RaceDisplayModel mensRaceModel =
                new RaceDisplayModel (Gender.Male, meetSelection);
            RaceDisplayModel womensRaceModel =
                new RaceDisplayModel (Gender.Female, meetSelection);
            RaceResultsBuffer mensBuffer = new RaceResultsBuffer (mensRaceModel);
            RaceResultsBuffer womensBuffer =
                new RaceResultsBuffer (womensRaceModel);
            MensRaceContent = new TextView (mensBuffer);
            WomensRaceContent = new TextView (womensBuffer);
            MensRace.Add (MensRaceContent);
            WomensRace.Add (WomensRaceContent);
            MensRaceContent.SizeRequested += OnRaceContentSizeRequested;
            WomensRaceContent.SizeRequested += OnRaceContentSizeRequested;
        }        
        
        /// <summary>
        /// Handler for when the race content changes its size request.
        /// </summary>
        protected void OnRaceContentSizeRequested (object sender,
            SizeRequestedArgs arguments)
        {
            int width = arguments.Requisition.Width + 30;
            if (sender == MensRaceContent)
            {
                MensRace.SetSizeRequest (width, -1);
            }
            else
            {
                WomensRace.SetSizeRequest (width, -1);
            }
        }
        
        /// <summary>
        /// Handler for when the selected <see cref="Meet"/> changes.
        /// </summary>
        public void OnSelectionChanged (object sender,
            SelectionChangedArgs<MeetInstance> arguments)
        {
            MeetInstance meet = arguments.Selected;
            Info.Text = string.Format("{0}\n{1:yyyy/MM/dd}\n{2}", meet.Name,
                meet.Date, meet.Venue);
        }
    }
}
