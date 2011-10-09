using System;
using System.Collections;
using System.Collections.Generic;
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
    public class MeetDetail : Notebook
    {
        #region Properties

        /// <summary>
        /// The view model for this control.
        /// </summary>
        protected readonly MeetInstanceSelectionViewModel ViewModel;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetDetail" />.
        /// </summary>
        /// <param name="viewModel">
        /// The view model to use.
        /// </param>
        public MeetDetail(MeetInstanceSelectionViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.PropertyChanged += HandleViewModelPropertyChanged;
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
                IEnumerable<Race > races = meetInstance.Races;
                while(NPages > 0)
                {
                    RemovePage(0);
                }
                foreach(Race race in races)
                {
                    RaceDetail raceDetail = new RaceDetail { Race = race };
                    AppendPage(raceDetail, new Label(race.Name));
                    raceDetail.ShowAll();
                }
            }
        }
        
        #endregion
    }
}
