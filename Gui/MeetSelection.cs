using System;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// Keeps track of which meet is currently selected in the meet browser.
    /// </summary>
    public class MeetSelection
    {        
        /// <summary>
        /// The delegate used when the meet selection changes.
        /// </summary>
        public delegate void MeetSelector(Meet meet);
        
        /// <summary>
        /// The event that is fired when the meet selection changes.
        /// </summary>
        public event MeetSelector SelectionChanged;
         
        private Meet _selected;
        
        /// <summary>
        /// The meet that is currently selected.
        /// </summary>
        public Meet Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SelectionChanged (_selected);
            }
        }
    }
}
