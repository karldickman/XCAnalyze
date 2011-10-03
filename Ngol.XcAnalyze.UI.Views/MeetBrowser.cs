using System;
using Gtk;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.UI.ViewModels;
using Ngol.XcAnalyze.UI.Views.Gtk.ViewModels;

namespace Ngol.XcAnalyze.UI.Views.Gtk
{
    /// <summary>
    /// Provides a way to browse all the meets currently loaded.
    /// </summary>
    public class MeetBrowser : HPaned
    {
        #region Properties

        /// <summary>
        /// The pane where the list of meets is shown.
        /// </summary>
        protected Widget Browser
        {
            get;
            set;
        }

        /// <summary>
        /// The browser pane should be scrollable.
        /// </summary>
        protected Container BrowserWindow
        {
            get;
            set;
        }

        /// <summary>
        /// The pane where detailed information about the selected meet is
        /// shown.
        /// </summary>
        protected Widget Detail
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new <see cref="MeetBrowser" />.
        /// </summary>
        /// <param name="viewModel">
        /// The <see cref="MeetInstanceSelectionViewModel"/> to use as the view model.
        /// </param>
        public MeetBrowser(MeetInstanceSelectionViewModel viewModel)
        {
            BrowserWindow = new ScrolledWindow();
            Add(BrowserWindow);
            MeetsListStore listStore = new MeetsListStore(viewModel);
            Browser = new MeetsList(viewModel, listStore);
            BrowserWindow.Add(Browser);
            Browser.SizeRequested += HandleBrowserWindowSizeRequested;
            Detail = new MeetDetail(viewModel);
            Add(Detail);
        }

        #endregion

        #region Event handlers

        private void HandleBrowserWindowSizeRequested(object sender, SizeRequestedArgs arguments)
        {
            int width = arguments.Requisition.Width + 15;
            if(width < Screen.Width / 2)
            {
                BrowserWindow.SetSizeRequest(width, BrowserWindow.HeightRequest);
            }
        }

        #endregion
    }
}
