using System;
using XCAnalyze.Gui;
using XCAnalyze.Io;
using XCAnalyze.Model;

namespace XCAnalyze
{
    public class XCAnalyze
    {
        public static void Main (string[] args)
        {
            Gtk.Application.Init ();
            IReader<XcData> reader;
            reader = new XcaReader (SupportFiles.GetPath ("example.xca"));
            XcData data = reader.Read ();
            MainWindow application = new MainWindow (data);
            application.ShowAll ();
            Gtk.Application.Run ();
        }
    }
}
