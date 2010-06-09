using Gtk;
using System;
using System.Linq;
using XCAnalyze.Io;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class MainWindow : Window
    {
        /// <summary>
        /// The widget where race results are displayed.
        /// </summary>
        protected internal MeetViewer RaceInfo { get; set; }
        
        /// <summary>
        /// The data with which the application will be working.
        /// </summary>
        protected internal XcData XcData { get; set; }
        
        public static int Main (string[] args)
        {
            Application.Init ();
            IReader<XcData> reader;
            reader = XcaReader.NewInstance (SupportFiles.GetPath("example.xca"));
            XcData data = reader.Read ();
            MainWindow application = new MainWindow (data);
            application.ShowAll ();
            Application.Run ();
            return 0;
        }
        
        protected internal MainWindow() : base("XCAnalyze v 0.1") {}
        
        /// <summary>
        /// Create a new application to work with a particular piece of data.
        /// </summary>
        /// <param name="data">
        /// The <see cref="GlobalState"/> with which to work.
        /// </param>
        public MainWindow (XcData data) : this()
        {
            XcData = data;
            Meet wReg = (from meet in data.Meets
                where ("NCAA West Region Championship".Equals (meet.Name)
                    && 2009.Equals (meet.Date.Year))
                select meet).First ();
            RaceInfo = new MeetViewer (wReg);
            Add (RaceInfo);
            SetSizeRequest ();
        }
        
        /// <summary>
        /// Set the default dimensions of all the children.
        /// </summary>
        public void SetSizeRequest ()
        {
            RaceInfo.SetSizeRequest ();
        }
    }
}
