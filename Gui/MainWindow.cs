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
        protected internal RaceResultsWidget RaceResults { get; set; }
        
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
        
        protected internal MainWindow() : base("XCAnalyze v 0.1")
        {
            SetDefaultSize(600, 600);
        }
        
        /// <summary>
        /// Create a new application to work with a particular piece of data.
        /// </summary>
        /// <param name="data">
        /// The <see cref="GlobalState"/> with which to work.
        /// </param>
        public MainWindow (XcData data) : this()
        {
            XcData = data;
            Race wRegM;
            wRegM = (from race in data.Races
                where (race.Name != null
                    && race.Name.Equals ("NCAA West Region Championship")
                    && race.Year == 2009)
                select race).First ();
            wRegM.Score ();
            RaceResults = new RaceResultsWidget (wRegM);
            Add (RaceResults);
        }
    }
}
