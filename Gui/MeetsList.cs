using Gtk;
using System;
using System.Collections.Generic;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{    
    /// <summary>
    /// A list store that provides the model for a list of meets.
    /// </summary>
    public class MeetsListStore : ListStore
    {
        public MeetsListStore (IList<Meet> meets) : base(typeof(string))
        {
            foreach (Meet meet in meets)
            {
                AppendValues (meet.Name);
            }
        }
    }
    
    /// <summary>
    /// A widget to display a list of meets.
    /// </summary>
    public class MeetsView : TreeView
    {
        /// <summary>
        /// The renderer for cells in the name column.
        /// </summary>
        protected internal CellRenderer NameCell { get; set; }
        
        /// <summary>
        /// The column that contains the name of the meet.
        /// </summary>
        protected internal TreeViewColumn NameColumn { get; set; }
        
        protected internal MeetsView ()
        {
            NameColumn = new TreeViewColumn ();
            NameColumn.Title = "Name";
            NameCell = new CellRendererText ();
            NameColumn.PackStart (NameCell, true);
            NameColumn.AddAttribute (NameCell, "text", 0);
            AppendColumn (NameColumn);
        }
        
        public MeetsView (ListStore model) : this()
        {
            Model = model;
        }
    }
    
    /// <summary>
    /// A widget to display a list of meets in a scrollable window.
    /// </summary>
    public class MeetsList : ScrolledWindow
    {
        /// <summary>
        /// Widget used to display the list of meets.
        /// </summary>
        protected internal Widget View { get; set; }
        
        /// <summary>
        /// Create a new widget associated with a particular list of meets.
        /// </summary>
        /// <param name="meets">
        /// The <see cref="IList<Meet>"/> to display.
        /// </param>
        public MeetsList (IList<Meet> meets)
        {
            ListStore model = new MeetsListStore (meets);
            View = new MeetsView (model);
            Add (View);
        }
        
        public void SetSizeRequest ()
        {
            int width, height;
            height = Screen.Height * 2 / 3;
            width = View.SizeRequest().Width + 20;
            SetSizeRequest(width, height);
        }
    }
}
