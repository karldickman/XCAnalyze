using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{    
    /// <summary>
    /// A way to browse the meets in the database.
    /// </summary>
    public class MeetBrowser : HPaned
    {
        /// <summary>
        /// The list of meets.
        /// </summary>
        protected internal MeetsList Browser { get; set; }
        
        /// <summary>
        /// A detailed display of meet info.
        /// </summary>
        protected internal MeetDetail Detail { get; set; }
        
        /// <summary>
        /// An object that keeps track of the currently selected meet.
        /// </summary>
        protected internal IDataSelection<Meet> MeetSelection { get; set; }
        
        public MeetBrowser (IList<Meet> meets)
        {
            MeetSelection = new DataSelection<Meet> ();
            Browser = new MeetsList (MeetSelection, meets);
            Detail = new MeetDetail (MeetSelection);
            Add (Browser);
            Add (Detail);
        }
    }
}
