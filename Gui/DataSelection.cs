using System;
using System.Collections.Generic;

namespace XCAnalyze.Gui
{    
    public class DataSelection<T> : IDataSelection<T>
    {
        #region IDataSelection[XCAnalyze.Gui.DataSelection.T] implementation
        public event ContentModifier<T> ContentReplaced;
        
        public event ContentModifier<T> ItemsAdded;
        
        public event ContentModifier<T> ItemsDeleted;
        
        public event Selector<T> SelectionChanged;
        
        public void Add (T item)
        {
            IList<T> items = new List<T> ();
            items.Add (item);
            Add (items);
        }
        
        public void Add (IList<T> items)
        {
            if (ItemsAdded != null)
            {
                ItemsAdded (this, new ContentModifiedArgs<T> (items));
            }
        }
        
        public void Delete (T item)
        {
            IList<T> items = new List<T> ();
            items.Add (item);
            Delete (items);
        }
        
        public void Delete (IList<T> items)
        {
            if (ItemsDeleted != null) 
            {
                ItemsDeleted (this, new ContentModifiedArgs<T> (items));
            }
        }
        
        public void Select (T item)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged (this, new SelectionChangedArgs<T> (item));
            }
        }        
        #endregion
    }
}
