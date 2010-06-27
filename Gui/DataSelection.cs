using NUnit.Framework;
using System;
using System.Collections.Generic;
using XCAnalyze.Model;

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
        void Add(T item);
        
        /// <summary>
        /// Add a bunch of items.
        /// </summary>
        /// <param name="items">
        /// The items to add.
        /// </param>
        void Add(IList<T> items);
        
        /// <summary>
        /// Delete one item.
        /// </summary>
        /// <param name="item">
        /// The item to delete.
        /// </param>
        void Delete(T item);
        
        /// <summary>
        /// Delete a bunch of items.
        /// </summary>
        /// <param name="items">
        /// The items to delete.
        /// </param>
        void Delete(IList<T> items);
        
        /// <summary>
        /// Choose an item to be selected.
        /// </summary>
        /// <param name="item">
        /// The item to select.
        /// </param>
        void Select(T item);
    }
    
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
    
    [TestFixture]
    public class TestDataSelection
    {
        protected bool HandlerCalled { get; set; }
        protected IDataSelection<Meet> CurrentMeet { get; set; }
        protected IList<Meet> Meets { get; set; }
        protected Meet Selected { get; set; } 
        protected IDataSelection<Meet> Sender { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Meets = new List<Meet> ();
            Meets.Add (new Meet ("Lewis & Clark Invitational", new Date (2010, 9, 27), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("Pacific Lutheran Invitational", new Date (2010, 10, 23), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("NWC Championship", new Date (2010, 11, 1), null, new Race (null, 8000), new Race (null, 6000)));
            HandlerCalled = false;
            CurrentMeet = new DataSelection<Meet> ();
            CurrentMeet.SelectionChanged += Handler;
        }
        
        [Test]
        public void TestSelect ()
        {
            Assert.That (!HandlerCalled);
            //Assert.IsNull (CurrentMeet.Selected);
            foreach (int i in new int[] { 2, 0, 1 })
            {
                CurrentMeet.Select (Meets[i]);
                Assert.That (HandlerCalled);
                Assert.AreEqual (CurrentMeet, Sender);
                Assert.AreEqual (Meets[i], Selected);
                HandlerCalled = false;
            }
        }
        
        public void Handler (object sender, EventArgs arguments)
        {
            HandlerCalled = true;
            Sender = (IDataSelection<Meet>)sender;
            Selected = ((SelectionChangedArgs<Meet>)arguments).Selected;
        }
    }
}
