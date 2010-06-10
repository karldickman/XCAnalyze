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
        protected internal MeetSelection MeetSelection { get; set; }
        
        public MeetBrowser (IList<Meet> meets)
        {
            MeetSelection = new MeetSelection ();
            Browser = new MeetsList (MeetSelection, meets);
            Detail = new MeetDetail ();
            Add (Browser);
            Add (Detail);
            MeetSelection.MeetSelector selector =
                new MeetSelection.MeetSelector (SelectMeet);
            MeetSelection.SelectionChanged += selector;
        }
        
        /// <summary>
        /// Select a particular meet to be shown in the detail view.
        /// </summary>
        /// <param name="meet">
        /// The <see cref="Meet"/> to show.
        /// </param>
        public void SelectMeet (Meet meet)
        {
            Remove (Detail);
            Detail = new MeetDetail (meet);
            Add (Detail);
            SetSizeRequest ();
            ShowAll ();
        }
        
        public void SetSizeRequest ()
        {
            Browser.SetSizeRequest ();
            Detail.SetSizeRequest ();
        }
    }
}
