using System;
using Gtk;

namespace Ngol.XcAnalyze
{
    /// <summary>
    /// The application's main down.
    /// </summary>
    public partial class MainWindow : Gtk.Window
    {
        /// <summary>
        /// Construct a new main window.
        /// </summary>
        public MainWindow()
            : base(Gtk.WindowType.Toplevel)
        {
            Build();
        }

        /// <summary>
        /// Handler for the delete event.
        /// </summary>
        protected void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }
    }
}
