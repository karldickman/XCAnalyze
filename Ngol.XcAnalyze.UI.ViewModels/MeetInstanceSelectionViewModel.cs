using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ngol.XcAnalyze.Model;

namespace Ngol.XcAnalyze.UI.ViewModels
{
    /// <summary>
    /// View model for browsing and selecting <see cref="MeetInstance" />s.
    /// </summary>
    public class MeetInstanceSelectionViewModel : ObservableCollection<MeetInstance>
    {
        #region Properties

        #region Physical implementation

        private MeetInstance _selectedMeetInstance;

        #endregion

        /// <summary>
        /// The currently selected <see cref="MeetInstance" />.
        /// </summary>
        public MeetInstance SelectedMeetInstance
        {
            get { return _selectedMeetInstance; }

            set
            {
                if(SelectedMeetInstance != value)
                {
                    _selectedMeetInstance = value;
                    OnPropertyChanged("SelectedMeetInstance");
                }
            }
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public new event PropertyChangedEventHandler PropertyChanged
        {
            add { base.PropertyChanged += value; }
            
            remove { base.PropertyChanged -= value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetInstanceSelectionViewModel" />.
        /// </summary>
        /// <param name="meetInstances">
        /// The list of <see cref="MeetInstance" />s to expose.
        /// </param>
        public MeetInstanceSelectionViewModel(IEnumerable<MeetInstance> meetInstances) : base(meetInstances.ToList())
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}