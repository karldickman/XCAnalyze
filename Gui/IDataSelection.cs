using System;
using System.Collections.Generic;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The interface that any interface handling selection of items from a list
    /// of data must implement.
    /// </summary>
    public interface IDataSelection<T>
    {
        /// <summary>
        /// The event that occurs when the content changes.
        /// </summary>
        event ContentModifier<T> ContentReplaced;

        /// <summary>
        /// The event that occurs when items are added.
        /// </summary>
        event ContentModifier<T> ItemsAdded;

        /// <summary>
        /// The event that occurs when items are deleted.
        /// </summary>
        event ContentModifier<T> ItemsDeleted;

        /// <summary>
        /// The event that occurs when the selected item changes.
        /// </summary>
        event Selector<T> SelectionChanged;

        /// <summary>
        /// Add one item.
        /// </summary>
        /// <param name="item">
        /// The item to add.
        /// </param>
        void Add (T item);

        /// <summary>
        /// Add a bunch of items.
        /// </summary>
        /// <param name="items">
        /// The items to add.
        /// </param>
        void Add (IList<T> items);

        /// <summary>
        /// Delete one item.
        /// </summary>
        /// <param name="item">
        /// The item to delete.
        /// </param>
        void Delete (T item);

        /// <summary>
        /// Delete a bunch of items.
        /// </summary>
        /// <param name="items">
        /// The items to delete.
        /// </param>
        void Delete (IList<T> items);

        /// <summary>
        /// Choose an item to be selected.
        /// </summary>
        /// <param name="item">
        /// The item to select.
        /// </param>
        void Select (T item);
    }
}
