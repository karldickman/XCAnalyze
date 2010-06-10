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
        
        public MeetBrowser (IList<Meet> meets)
        {
            Browser = new MeetsList (meets);
            Meet wReg = (from meet in meets
                where ("NCAA West Region Championship".Equals (meet.Name)
                    && 2009.Equals (meet.Date.Year))
                select meet).First ();
            Detail = new MeetDetail (wReg);
            Add (Browser);
            Add (Detail);
        }
        
        public void SetSizeRequest ()
        {
            Browser.SetSizeRequest ();
            Detail.SetSizeRequest ();
        }
    }
}
