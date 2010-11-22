using System;
using System.Collections.Generic;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public class GlobalData : IDataSelection<MeetInstance>
    {
        #region IDataSelection[XCAnalyze.Model.Meet] implementation
        public event ContentModifier<MeetInstance> ContentReplaced;
        
        public event ContentModifier<MeetInstance> ItemsAdded;
        
        public event ContentModifier<MeetInstance> ItemsDeleted;
        
        public event Selector<MeetInstance> SelectionChanged;
        
        public void Add (MeetInstance meet)
        {
            //Data.Add (meet);
            ItemsAdded (this, new ContentModifiedArgs<MeetInstance> (meet));
        }
        
        public void Add (IList<MeetInstance> meets)
        {
            //Data.Add (meets);
            ItemsAdded (this, new ContentModifiedArgs<MeetInstance> (meets));
        }
        
        public void Delete (MeetInstance meet)
        {
            //Data.Remove (meet);
            ItemsDeleted (this, new ContentModifiedArgs<MeetInstance> (meet));
        }
        
        public void Delete (IList<MeetInstance> meets)
        {
            //Data.Remove (meets);
            ItemsDeleted (this, new ContentModifiedArgs<MeetInstance> (meets));
        }
        
        public void Select (MeetInstance meet)
        {
            SelectionChanged (this, new SelectionChangedArgs<MeetInstance> (meet));
        }
        #endregion
        
        private DataContext _data;
        
        /// <summary>
        /// The data this application handles.
        /// </summary>
        public DataContext Data
        {
            get { return _data; }
            
            set
            {
                _data = value;
                ContentReplaced (this,
                    new ContentModifiedArgs<MeetInstance> (value.MeetInstances));
                if (value.MeetInstances.Count > 0)
                {
                    Select (value.MeetInstances[0]);
                }
            }
        }
    }
}
