using Gtk;
using System;
using System.Collections.Generic;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The arguments passed by a ContentChanged, ItemsAdded, or ItemsDeleted
    /// event.
    /// </summary>
    public class ContentModifiedArgs<T> : EventArgs
    {
        public IEnumerable<T> Items { get; protected set; }
        
        public ContentModifiedArgs (T item)
        {
            Items = new List<T> ();
            ((List<T>)Items).Add (item);
        }
        
        public ContentModifiedArgs (IEnumerable<T> items)
        {
            Items = items;
        }
    }
    
    /// <summary>
    /// The arguments passed by a SelectionChanged event.
    /// </summary>
    public class SelectionChangedArgs<T> : EventArgs
    {
        /// <summary>
        /// The item currently selected.
        /// </summary>
        public T Selected { get; protected set; }
        
        public SelectionChangedArgs (T selected)
        {
            Selected = selected;
        }
    }
}
