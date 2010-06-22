using System;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class GlobalData : IDataSelection<Meet>
    {
        public event Selector<Meet> SelectionChanged;
        
        /// <summary>
        /// The currently selected meet.
        /// </summary>
        public Meet Selected { get; protected internal set; }
        
        /// <summary>
        /// The data this application handles.
        /// </summary>
        protected internal XcData Data { get; set; }
        
        public GlobalData (XcData data)
        {
            Data = data;
        }
        
        /// <summary>
        /// Select a meet to have focus.
        /// </summary>
        /// <param name="item">
        /// The <see cref="Meet"/> to select.
        /// </param>
        public void Select (Meet item)
        {
            Selected = item;
            SelectionChanged (this, new SelectionChangedArgs<Meet> (item));
        }
    }
}
