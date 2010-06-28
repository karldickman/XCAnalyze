using System;

using Gtk;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    public class MainWindow : Window
    {
        /// <summary>
        /// The browser for the meets.
        /// </summary>
        protected Widget Meets { get; set; }
        
        /// <summary>
        /// The data used by this application.
        /// </summary>
        protected GlobalData Model { get; set; }
        
        /// <summary>
        /// Create a new main window for the application.
        /// </summary>
        public MainWindow () : base("XCAnalyze")
        {
            Model = new GlobalData ();
            Meets = new MeetBrowser (Model);
            Add (Meets);
        }
    }
}
