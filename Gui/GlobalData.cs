using System;
using System.Collections.Generic;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class GlobalData : IDataSelection<Meet>
    {
        #region IDataSelection[XCAnalyze.Model.Meet] implementation
        public event ContentModifier<Meet> ContentReplaced;
        
        public event ContentModifier<Meet> ItemsAdded;
        
        public event ContentModifier<Meet> ItemsDeleted;
        
        public event Selector<Meet> SelectionChanged;
        
        public void Add (Meet meet)
        {
            Data.Add (meet);
            ItemsAdded (this, new ContentModifiedArgs<Meet> (meet));
        }
        
        public void Add (IList<Meet> meets)
        {
            Data.Add (meets);
            ItemsAdded (this, new ContentModifiedArgs<Meet> (meets));
        }
        
        public void Delete (Meet meet)
        {
            Data.Remove (meet);
            ItemsDeleted (this, new ContentModifiedArgs<Meet> (meet));
        }
        
        public void Delete (IList<Meet> meets)
        {
            Data.Remove (meets);
            ItemsDeleted (this, new ContentModifiedArgs<Meet> (meets));
        }
        
        public void Select (Meet meet)
        {
            SelectionChanged (this, new SelectionChangedArgs<Meet> (meet));
        }
        #endregion
        
        private XcData _data;
        
        /// <summary>
        /// The data this application handles.
        /// </summary>
        public XcData Data
        {
            get { return _data; }
            
            set
            {
                _data = value;
                ContentReplaced (this,
                    new ContentModifiedArgs<Meet> (value.Meets));
                if (value.Meets.Count > 0)
                {
                    Select (value.Meets[0]);
                }
            }
        }
    }
}
