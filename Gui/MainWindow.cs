using System;

using Gtk;

using XCAnalyze.IO;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    public class MainWindow : Window
    {       
        /// <summary>
        /// Create a new main window for the application.
        /// </summary>
        public MainWindow () : base("XCAnalyze")
        {
            Model = new GlobalData ();
            //Create the content container
            Content = new VBox ();
            Add (Content);
            //Create the main menu
            MainMenu = new MenuBar ();
            Content.Add (MainMenu);
            MenuItem fileItem = new MenuItem ("File");
            MainMenu.Append (fileItem);
            //Create file menu
            Menu fileMenu = new Menu ();
            fileItem.Submenu = fileMenu;
            //Create open item in file menu
            MenuItem openItem = new MenuItem ("Open");
            fileMenu.Append (openItem);
            openItem.Activated += OnOpenItemActivated;
            //Add separator
            SeparatorMenuItem separator = new SeparatorMenuItem ();
            fileMenu.Append (separator);
            //Create quit item in file menu
            MenuItem quitItem = new MenuItem ("Quit");
            fileMenu.Append (quitItem);
            quitItem.Activated += OnQuitItemActivated;
            //Create the meet browser
            Meets = new MeetBrowser (Model);
            Content.Add (Meets);
        }
        
        /// <summary>
        /// The container wherein all the content resides.
        /// </summary>
        protected Container Content { get; set; }

        /// <summary>
        /// The applications main menu.
        /// </summary>
        protected MenuBar MainMenu { get; set; }

        /// <summary>
        /// The browser for the meets.
        /// </summary>
        protected Widget Meets { get; set; }

        /// <summary>
        /// The data used by this application.
        /// </summary>
        protected GlobalData Model { get; set; }
          
        protected void OnOpenItemActivated (object sender, EventArgs arguments)
        {
            string fileName;
            FileChooserDialog dialog;
            dialog = new FileChooserDialog ("Choose a file to open", this,
                FileChooserAction.Open, "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);
            dialog.Filter = new FileFilter ();
            dialog.Filter.AddPattern ("*.xca");
            if (dialog.Run () == (int)ResponseType.Accept)
            {
                fileName = dialog.Filename;
                using (XcaReader reader = new XcaReader (fileName)) 
                {
                    Model.Data = reader.Read ();
                }
            }
            dialog.Destroy ();
        }
        
        protected void OnQuitItemActivated (object sender, EventArgs arguments)
        {
            Destroy ();
        }
    }
}
