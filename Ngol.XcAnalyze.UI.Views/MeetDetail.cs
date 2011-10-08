using System;
using System.ComponentModel;
using System.Linq;
using Gtk;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.UI.ViewModels;
using Ngol.XcAnalyze.UI.Views.Gtk.ViewModels;

namespace Ngol.XcAnalyze.UI.Views.Gtk
{
    /// <summary>
    /// User control to look at detailed information about a <see cref="MeetInstance" />.
    /// </summary>
    public class MeetDetail : VBox
    {
        #region Properties

        /// <summary>
        /// The view model for this control.
        /// </summary>
        public MeetInstanceSelectionViewModel ViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// Some information about the race.
        /// </summary>
        protected Label Info
        {
            get;
            set;
        }

        /// <summary>
        /// The view used to look at the races in the <see cref="MeetInstance" />.
        /// </summary>
        protected TextView RacesView
        {
            get;
            set;
        }

        /// <summary>
        /// The scrolled window that contains all the races.
        /// </summary>
        protected ScrolledWindow Scroller
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetDetail" />.
        /// </summary>
        /// <param name="viewModel">
        /// The view model to use.
        /// </param>
        public MeetDetail(MeetInstanceSelectionViewModel viewModel) : this()
        {
            ViewModel = viewModel;
            ViewModel.PropertyChanged += HandleViewModelPropertyChanged;
        }

        /// <summary>
        /// Construct a new <see cref="MeetDetail" />.
        /// </summary>
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

        #endregion

        #region Event handlers

        /// <summary>
        /// Handler for when the selected <see cref="Meet"/> changes.
        /// </summary>
        public void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "SelectedMeetInstance")
            {
                MeetInstance meetInstance = ViewModel.SelectedMeetInstance;
                Info.Text = string.Format("{0}\n{1:yyyy/MM/dd}\n{2}", meetInstance.Name, meetInstance.Date, meetInstance.Venue);
                RacesView.Buffer = new RaceResultsBuffer(meetInstance.Races);
            }
        }
        
        #endregion
    }
}
