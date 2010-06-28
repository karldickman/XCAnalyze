using System;

using XCAnalyze.Gui;
using XCAnalyze.IO;
using XCAnalyze.Model;

namespace XCAnalyze
{
    public class MainClass
    {
        public static void Main (string[] args)
        {
            Gtk.Application.Init ();
            MainWindow application = new MainWindow ();
            application.ShowAll ();
            Gtk.Application.Run ();
        }
    }
}
