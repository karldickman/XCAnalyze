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
        
        public MeetBrowser (IDataSelection<Meet> meetSelection, IList<Meet> meets)
        {
            Browser = new MeetsList (meetSelection, meets);
            Detail = new MeetDetail (meetSelection);
            Add (Browser);
            Add (Detail);
        }
    }
}
