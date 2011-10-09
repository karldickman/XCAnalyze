using System;
using Gtk;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.UI.ViewModels;
using Ngol.XcAnalyze.UI.Views.Gtk.ViewModels;

namespace Ngol.XcAnalyze.UI.Views.Gtk
{
    /// <summary>
    /// User interface for displaying details about a <see cref="Race" />.
    /// </summary>
    public class RaceDetail : VBox
    {
        #region Properties

        #region Physical implementation

        private Race _race;

        #endregion

        /// <summary>
        /// Some information about the race.
        /// </summary>
        protected readonly Label Info;

        /// <summary>
        /// The <see cref="TextView" /> used to look at the results of the <see cref="RaceDetail.Race" />.
        /// </summary>
        protected readonly TextView ResultsView;

        /// <summary>
        /// The <see cref="ScrolledWindow" /> that contains the <see cref="ResultsView" />.
        /// </summary>
        protected readonly ScrolledWindow Scroller;

        /// <summary>
        /// The <see cref="Race" /> whose details to display.
        /// </summary>
        public Race Race
        {
            get { return _race; }

            set
            {
                _race = value;
                Info.Text = string.Format("{0}\n{1:yyyy/MM/dd}\n{2}", Race.MeetInstance.Name, Race.Date, Race.Venue);
                ResultsView.Buffer = new RaceResultsBuffer { Race = Race };
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="RaceDetail" />.
        /// </summary>
        public RaceDetail()
        {
            // Create the heading label
            Info = new Label();
            PackStart(Info, false, false, 10);
            Info.Justify = Justification.Center;
            // Create the scroller
            Scroller = new ScrolledWindow();
            Add(Scroller);
            // Create the races view
            ResultsView = new TextView();
            Scroller.Add(ResultsView);
        }

        #endregion
    }
}

