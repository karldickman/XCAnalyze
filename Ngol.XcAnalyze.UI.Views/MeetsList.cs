using System;
using System.Linq;
using Gtk;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.UI.ViewModels;

namespace Ngol.XcAnalyze.UI.Views.Gtk
{
    /// <summary>
    /// A widget to display a list of meets.
    /// </summary>
    public class MeetsList : TreeView
    {
        #region Properties

        /// <summary>
        /// View model that controls interactions.
        /// </summary>
        protected MeetInstanceSelectionViewModel ViewModel
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetsList" />.
        /// </summary>
        /// <param name="viewModel">
        /// The view model to use.
        /// </param>
        /// <param name="model">
        /// The <see cref="ListStore"/> to use.
        /// </param>
        public MeetsList(MeetInstanceSelectionViewModel viewModel, ListStore model)
        {
            ViewModel = viewModel;
            Model = model;
            AppendColumn("Name", new CellRendererText(), "text", 1);
            AppendColumn("Date", new CellRendererText(), "text", 2);
            RowActivated += HandleRowActivated;
        }

        #endregion

        #region Event handlers

        private void HandleRowActivated(object sender, RowActivatedArgs e)
        {
            TreeIter iter = new TreeIter();
            if(Model.GetIter(out iter, e.Path))
            {
                ViewModel.SelectedMeetInstance = Model.GetValue(iter, 0) as MeetInstance;
            }
        }

        #endregion
    }
}
