using System;
using System.IO;
using Gtk;
using Ngol.XcAnalyze.Model;
using Ngol.XcAnalyze.UI.ViewModels;
using Ngol.XcAnalyze.UI.Views.Gtk;
using Ngol.XcAnalyze.Persistence.Collections;

namespace Ngol.XcAnalyze
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    public class MainWindow : Window
    {
        #region Properties

        /// <summary>
        /// The container wherein all the content resides.
        /// </summary>
        protected Box Content
        {
            get;
            set;
        }

        /// <summary>
        /// The applications main menu.
        /// </summary>
        protected MenuBar MainMenu
        {
            get;
            set;
        }

        /// <summary>
        /// The browser for the meets.
        /// </summary>
        protected Widget Meets
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new main window for the application.
        /// </summary>
        public MainWindow(PersistenceContainer container) : base(WindowType.Toplevel)
        {
            Title = "XCAnalyze";
            SetPosition(WindowPosition.Center);
            SetSizeRequest((int)(Screen.Width * 0.4), (int)(Screen.Height * 0.75));
            AllowShrink = true;
            //Create the content container
            Content = new VBox();
            Add(Content);
            //Add the menu
            MainMenu = CreateMenu();
            Content.PackStart(MainMenu, false, false, 0);
            //Create the meet browser
            Meets = new MeetBrowser(new MeetInstanceSelectionViewModel(container.MeetInstances));
            Content.Add(Meets);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create the menu
        /// </summary>
        protected MenuBar CreateMenu()
        {
            //Create the main menu
            MenuBar menu = new MenuBar();
            MenuItem fileItem = new MenuItem("File");
            menu.Append(fileItem);
            //Create file menu
            Menu fileMenu = new Menu();
            fileItem.Submenu = fileMenu;
            //Create quit item in file menu
            MenuItem quitItem = new MenuItem("Quit");
            fileMenu.Append(quitItem);
            quitItem.Activated += HandleQuitItemActivated;
            return menu;
        }

        #endregion

        #region Event handlers

        private void HandleQuitItemActivated(object sender, EventArgs arguments)
        {
            Application.Quit();
        }
        
        #endregion
    }
}
