using System;
using System.Collections.Generic;

using Gtk;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{        
    /// <summary>
    /// A widget to display a list of meets.
    /// </summary>
    public class MeetsList : TreeView
    {   
        /// <summary>
        /// The meet that is currently selected.
        /// </summary>
        protected IDataSelection<Meet> MeetSelection { get; set; }
        
        public MeetsList (IDataSelection<Meet> selection, ListStore model)
        {
            MeetSelection = selection;
            AppendColumn("Name", new CellRendererText(), "text", 1);
            AppendColumn("Date", new CellRendererText(), "text", 2);
            RowActivated += OnRowActivated;
            Model = model;
        }
        
        public void OnRowActivated (object sender, RowActivatedArgs arguments)
        {
            TreeIter iter = new TreeIter ();
            if (Model.GetIter (out iter, arguments.Path))
            {
                MeetSelection.Select ((Meet)Model.GetValue(iter, 0));
            }
        }
    }
}
