using System;
using System.IO;

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
        #region Properties
        
        /// <summary>
        /// The container wherein all the content resides.
        /// </summary>
        protected Box Content { get; set; }

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
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Create a new main window for the application.
        /// </summary>
        public MainWindow () : base(WindowType.Toplevel)
        {
            Title = "XCAnalyze";
            SetPosition (WindowPosition.Center);
            SetSizeRequest((int)(Screen.Width * 0.5), (int)(Screen.Height * 0.75));
            AllowShrink = true;
            Model = new GlobalData ();
            //Create the content container
            Content = new VBox ();
            Add (Content);
            //Add the menu
            MainMenu = CreateMenu();
            Content.PackStart(MainMenu, false, false, 0);
            //Create the meet browser
            Meets = new MeetBrowser (Model);
            Content.Add (Meets);
            string sessionFile = SupportFiles.GetPath(".xcanalyze_session");
            if(File.Exists(sessionFile))
            {
                string fileName = File.ReadAllText(sessionFile);
                using(XcaReader reader = new XcaReader(fileName))
                {
                    Model.Data = reader.Read();
                }
            }
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
            //Create open item in file menu
            MenuItem openItem = new MenuItem("Open");
            fileMenu.Append(openItem);
            openItem.Activated += OpenItem_Activated;
            //Add separator
            SeparatorMenuItem separator = new SeparatorMenuItem();
            fileMenu.Append(separator);
            //Create quit item in file menu
            MenuItem quitItem = new MenuItem("Quit");
            fileMenu.Append(quitItem);
            quitItem.Activated += QuitItem_Activated;
            return menu;
        }
         
        #endregion
        
        #region Event handlers
        
        protected void OpenItem_Activated (object sender, EventArgs arguments)
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
                    File.WriteAllText (SupportFiles.GetPath
                        (".xcanalyze_session"), fileName);
                }
            }
            dialog.Destroy ();
        }
        
        protected void QuitItem_Activated(object sender, EventArgs arguments)
        {
            Destroy();
        }
        
        #endregion
    }
}
