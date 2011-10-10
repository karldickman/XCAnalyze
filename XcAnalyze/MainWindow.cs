using System;
using System.Linq;
using Gtk;
using Ngol.XcAnalyze.Persistence.Collections;
using Ngol.XcAnalyze.UI.ViewModels;
using Ngol.XcAnalyze.UI.Views.Gtk;

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
            SetSizeRequest(Convert.ToInt32(Screen.Width * 0.4), Convert.ToInt32(Screen.Height * 0.75));
            AllowShrink = true;
            //Create the content container
            Content = new VBox();
            Add(Content);
            //Add the menu
            MainMenu = CreateMenu();
            Content.PackStart(MainMenu, false, false, 0);
            //Create the meet browser
            var viewModel = new MeetInstanceSelectionViewModel(container.MeetInstances);
            viewModel.SelectedMeetInstance = container.MeetInstances.FirstOrDefault();
            Meets = new MeetBrowser(viewModel);
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
