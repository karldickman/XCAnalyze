using System;

using Gtk;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{    
    /// <summary>
    /// Provides a way to browse all the meets currently loaded.
    /// </summary>
    public class MeetBrowser : HPaned
    {
        /// <summary>
        /// The pane where the list of meets is shown.
        /// </summary>
        protected Widget Browser { get; set; }
        
        /// <summary>
        /// The browser pane should be scrollable.
        /// </summary>
        protected ScrolledWindow BrowserWindow { get; set; }
        
        /// <summary>
        /// The pane where detailed information about the selected meet is
        /// shown.
        /// </summary>
        protected Widget Detail { get; set; }

        public MeetBrowser (IDataSelection<Meet> meetSelection)
        {
            BrowserWindow = new ScrolledWindow ();
            Add (BrowserWindow);
            MeetsListStore listStore = new MeetsListStore (meetSelection);
            Browser = new MeetsList (meetSelection, listStore);
            BrowserWindow.Add (Browser);
            Detail = new MeetDetail (meetSelection);
            Add (Detail);
        }
    }
}
