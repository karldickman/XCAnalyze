using System;

namespace Ngol.XcAnalyze
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public class MainClass
    {
        /// <summary>
        /// The main method for the application.
        /// </summary>
        /// <param name="args">
        /// Command-line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            MainWindow application = new MainWindow();
            application.ShowAll();
            Gtk.Application.Run();
        }
    }
}
