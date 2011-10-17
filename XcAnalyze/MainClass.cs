using System;
using System.Diagnostics;
using GLib;
using Gtk;
using Ngol.XcAnalyze.Persistence.Collections;

namespace Ngol.XcAnalyze
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public class MainClass
    {
        #region Properties

        /// <summary>
        /// The application's <see cref="MainWindow" />.
        /// </summary>
        protected static MainWindow MainWindow { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The main method for the application.
        /// </summary>
        /// <param name="args">
        /// Command-line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            ExceptionManager.UnhandledException += HandleUnhandledException;
            PersistenceContainer container = new PersistenceContainer();
            Application.Init();
            MainWindow mainWindow = new MainWindow(container);
            mainWindow.ShowAll();
            Application.Run();
        }

        #endregion

        #region Event handlers

        private static void HandleUnhandledException(UnhandledExceptionArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            string message = ex == null ? e.ExceptionObject.ToString() : ex.ToString();
            string title = "Unhandled exception";
            DialogFlags flags = DialogFlags.Modal | DialogFlags.DestroyWithParent;
            Dialog dialog = new Dialog(title, MainWindow, flags);
            Label label = new Label(message);
            VBox vBox = (VBox)dialog.Child;
            vBox.Add(label);
            dialog.ShowAll();
            e.ExitApplication = false;
        }

        #endregion
    }
}
