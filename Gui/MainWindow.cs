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
        protected internal MeetBrowser Meets { get; set; }
        
        protected internal MainWindow() : base("XCAnalyze v 0.1") {}
        
        /// <summary>
        /// Create a new application to work with a particular piece of data.
        /// </summary>
        /// <param name="data">
        /// The <see cref="GlobalState"/> with which to work.
        /// </param>
        public MainWindow (XcData data) : this()
        {
            Meets = new MeetBrowser(data.Meets);
            Add(Meets);
            UsePreferredSize ();
        }
        
        /// <summary>
        /// Set the size request to its ideal value.
        /// </summary>
        public void UsePreferredSize ()
        {
            Meets.UsePreferredSize ();//Defer to child
        }
    }
}
