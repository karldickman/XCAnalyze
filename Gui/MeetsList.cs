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
        public MeetsListStore (IList<Meet> meets)
            : base(typeof(Meet), typeof(string))
        {
            foreach (Meet meet in meets)
            {
                AppendValues (meet, meet.Name);
            }
        }
    }
    
    /// <summary>
    /// A widget to display a list of meets.
    /// </summary>
    public class MeetsView : TreeView
    {   
        /// <summary>
        /// The meet that is currently selected.
        /// </summary>
        protected internal MeetSelection MeetSelection { get; set; }
        
        protected internal MeetsView (MeetSelection selection)
        {
            MeetSelection = selection;
            AppendColumn("Name", new CellRendererText(), "text", 1);
            RowActivated += HandleRowActivated;
        }
        
        public MeetsView (MeetSelection selection, ListStore model)
            : this(selection)
        {
            Model = model;
        }
        
        public void HandleRowActivated (object object_, RowActivatedArgs args)
        {
            TreeIter iter = new TreeIter ();
            if (Model.GetIter (out iter, args.Path))
            {
                MeetSelection.Selected = (Meet)Model.GetValue (iter, 0);
            }
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
        public MeetsList (MeetSelection selection, IList<Meet> meets)
        {
            ListStore model = new MeetsListStore (meets);
            View = new MeetsView (selection, model);
            Add (View);
        }
        
        /// <summary>
        /// Set the size request to its ideal value.
        /// </summary>
        public void UsePreferredSize ()
        {
            //Defer to children
            SetSizeRequest(View.SizeRequest().Width + 20, 0);
        }
    }
}
