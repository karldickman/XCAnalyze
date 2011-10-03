using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Ngol.Utilities.Collections.ObjectModel;
using Ngol.XcAnalyze.Model;

namespace Ngol.XcAnalyze.UI.ViewModels
{
    /// <summary>
    /// View model for browsing and selecting <see cref="MeetInstance" />s.
    /// </summary>
    public class MeetInstanceSelectionViewModel : IEnumerable<MeetInstance>, INotifyPropertyChanged
    {
        #region

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

        /// <summary>
        /// The <see cref="MeetInstance" /> in this view model.
        /// </summary>
        protected readonly IEnumerable<MeetInstance> InnerEnumerable;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetInstanceSelectionViewModel" />.
        /// </summary>
        /// <param name="meetInstances">
        /// The list of <see cref="MeetInstance" />s to expose.
        /// </param>
        public MeetInstanceSelectionViewModel(IEnumerable<MeetInstance> meetInstances)
        {
            if(meetInstances == null)
                throw new ArgumentNullException("meetInstances");
            InnerEnumerable = meetInstances;
        }

        #endregion

        #region IEnumerable[MeetInstance]

        /// <inheritdoc />
        public IEnumerator<MeetInstance> GetEnumerator()
        {
            return InnerEnumerable.GetEnumerator();
        }

        #region IEnumerable implementation

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged implementation

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoker for the <see cref="PropertyChanged" /> event.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Invoker for the <see cref="PropertyChanged" /> event.
        /// </summary>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}

