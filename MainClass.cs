using System;
using XCAnalyze.Gui;
using XCAnalyze.Io;
using XCAnalyze.Model;

namespace XCAnalyze
{
    public class MainClass
    {
        public static void Main (string[] args)
        {
            Gtk.Application.Init ();
            IReader<XcData> reader;
            reader = new XcaReader (SupportFiles.GetPath ("example.xca"));
            XcData data = reader.Read ();
            GlobalData model = new GlobalData ();
            MainWindow application = new MainWindow (model);            
            model.Data = data;
            application.ShowAll ();
            Gtk.Application.Run ();
        }
    }
}
