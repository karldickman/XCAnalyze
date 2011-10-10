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

        #region Physical implementation

        private MeetInstanceSelectionViewModel _viewModel;

        #endregion

        /// <summary>
        /// View model that controls interactions.
        /// </summary>
        protected MeetInstanceSelectionViewModel ViewModel
        {
            get { return _viewModel; }

            set
            {
                if(ViewModel != value)
                {
                    _viewModel = value;
                    SelectMeetInstance(ViewModel.SelectedMeetInstance);
                }
            }
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
            Model = model;
            Selection.Mode = SelectionMode.Single;
            ViewModel = viewModel;
            AppendColumn("Name", new CellRendererText(), "text", 1);
            AppendColumn("Date", new CellRendererText(), "text", 2);
            RowActivated += HandleRowActivated;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Select the specified <see cref="MeetInstance" />.
        /// </summary>
        /// <param name="meetInstance">
        /// The <see cref="MeetInstance" /> to select.
        /// </param>
        protected void SelectMeetInstance(MeetInstance meetInstance)
        {
            if(meetInstance != null)
            {
                Model.Foreach((model, path, iter) =>
                {
                    if(model.GetValue(iter, 0) == meetInstance)
                    {
                        Selection.SelectPath(path);
                        return true;
                    }
                    return false;
                });
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Pass the new selection to the view model.
        /// </summary>
        private void HandleRowActivated(object sender, RowActivatedArgs e)
        {
            TreeIter iter;
            if(Model.GetIter(out iter, e.Path))
            {
                ViewModel.SelectedMeetInstance = Model.GetValue(iter, 0) as MeetInstance;
            }
        }

        #endregion
    }
}
