using System;
using XCAnalyze.Gui;
using XCAnalyze.Io;
using XCAnalyze.Model;

namespace XCAnalyze
{
    public class XCAnalyze
    {
        public static int Main (string[] args)
        {
            Gtk.Application.Init ();
            IReader<XcData> reader;
            reader = XcaReader.NewInstance (SupportFiles.GetPath ("example.xca"));
            XcData data = reader.Read ();
            MainWindow application = new MainWindow (data);
            application.ShowAll ();
            Gtk.Application.Run ();
            return 0;
        }
    }
}
