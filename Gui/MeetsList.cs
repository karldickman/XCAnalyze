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
            : base(typeof(Meet), typeof(string), typeof(string), typeof(Venue),
                typeof(int), typeof(int))
        {
            meets = new List<Meet> (meets);
            ((List<Meet>)meets).Sort ();
            foreach (Meet meet in meets)
            {
                AppendValues (meet, meet.Name, meet.Date.ToString (),
                    meet.Venue, meet.MensDistance, meet.WomensDistance);
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
        protected internal IDataSelection<Meet> MeetSelection { get; set; }
        
        protected internal MeetsView (IDataSelection<Meet> selection)
        {
            MeetSelection = selection;
            AppendColumn("Name", new CellRendererText(), "text", 1);
            AppendColumn("Date", new CellRendererText(), "text", 2);
            RowActivated += HandleRowActivated;
        }
        
        public MeetsView (IDataSelection<Meet> selection, ListStore model)
            : this(selection)
        {
            Model = model;
        }
        
        public void HandleRowActivated (object object_, RowActivatedArgs args)
        {
            TreeIter iter = new TreeIter ();
            if (Model.GetIter (out iter, args.Path))
            {
                MeetSelection.Select ((Meet)Model.GetValue(iter, 0));
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
        public MeetsList (IDataSelection<Meet> selection, IList<Meet> meets)
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
