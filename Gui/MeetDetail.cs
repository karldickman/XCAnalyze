using System;
using Gtk;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class MeetDetail : VBox
    {
        #region Properties

        /// <summary>
        /// Some information about the race.
        /// </summary>
        protected Label Info { get; set; }

        protected TextView RacesView { get; set; }
        
        /// <summary>
        /// The scrolled window that contains all the races.
        /// </summary>
        protected ScrolledWindow Scroller { get; set; }
        
        #endregion

        #region Constructors

        protected MeetDetail()
        {
            //Create the heading label
            Info = new Label();
            PackStart(Info, false, false, 10);
            Info.Justify = Justification.Center;
            Scroller = new ScrolledWindow();
            Add(Scroller);
            RacesView = new TextView();
            Scroller.Add(RacesView);
        }

        /// <summary>
        /// Create a new meet viewer to display a particular meet.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to display.
        /// </param>
        public MeetDetail(IDataSelection<MeetInstance> meetSelection) : this()
        {
            meetSelection.SelectionChanged += OnSelectionChanged;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handler for when the selected <see cref="Meet"/> changes.
        /// </summary>
        public void OnSelectionChanged(object sender, SelectionChangedArgs<MeetInstance> arguments)
        {
            MeetInstance meetInstance = arguments.Selected;
            Info.Text = string.Format("{0}\n{1:yyyy/MM/dd}\n{2}", meetInstance.Name, meetInstance.Date, meetInstance.Venue);
            RacesView.Buffer = new RaceResultsBuffer(meetInstance.Races);
        }
        
        #endregion
    }
}
