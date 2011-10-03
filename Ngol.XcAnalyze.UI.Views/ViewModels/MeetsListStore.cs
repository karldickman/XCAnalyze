using System;
using System.Collections.Generic;
using Gtk;
using Ngol.Utilities.Collections.Extensions;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.UI.ViewModels;

namespace Ngol.XcAnalyze.UI.Views.Gtk.ViewModels
{
    /// <summary>
    /// A list store that provides the model for a list of meets.
    /// </summary>
    public class MeetsListStore : ListStore
    {
        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetsListStore" />
        /// </summary>
        /// <param name="viewModel">
        /// The view model to use.
        /// </param>
        public MeetsListStore(MeetInstanceSelectionViewModel viewModel) : base(typeof(MeetInstance), typeof(string), typeof(string), typeof(Venue), typeof(int), typeof(int))
        {
            IEnumerable<MeetInstance> meets = viewModel.Sorted(new MeetComparer());
            foreach(MeetInstance meet in meets)
            {
                AppendValues(meet, meet.Name, string.Format("{0:yyyy/MM/dd}", meet.Date), meet.Venue);
            }
        }

        #endregion

        #region Inner classes

        /// <summary>
        /// A comparer used to sort meets in this list store.
        /// </summary>
        public class MeetComparer : IComparer<MeetInstance>
        {
            #region IComparer[Meet] implementation
            /// <summary>
            /// Meets are compared first by date, then by name.  If both are
            /// identical, then it is the same meet.
            /// </summary>
            public int Compare(MeetInstance first, MeetInstance second)
            {
                int comparison;
                if(first == second)
                {
                    return 0;
                }
                comparison = first.Date.CompareTo(second.Date);
                if(comparison != 0)
                {
                    return comparison;
                }
                if(first.Name == second.Name)
                {
                    return 0;
                }
                if(first.Name == null)
                {
                    return 1;
                }
                if(second.Name == null)
                {
                    return 0;
                }
                return first.Name.CompareTo(second.Name);
            }
            #endregion
        }

        #endregion
    }
}
