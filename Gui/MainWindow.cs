using Gtk;
using System;
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
        protected internal Widget Meets { get; set; }
        
        /// <summary>
        /// The data used by this application.
        /// </summary>
        protected internal GlobalData Model { get; set; }
        
        protected internal MainWindow() : base("XCAnalyze v 0.0") {}
        
        /// <summary>
        /// Create a new application to work with a particular piece of data.
        /// </summary>
        /// <param name="data">
        /// The <see cref="GlobalState"/> with which to work.
        /// </param>
        public MainWindow (GlobalData model) : this()
        {
            Model = model;
            Meets = new MeetBrowser (Model, Model.Data.Meets);
            Add(Meets);
        }
    }
}
