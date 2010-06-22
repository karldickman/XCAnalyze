using NUnit.Framework;
using System;
using System.Collections.Generic;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The signature of callbacks for a SelectionChanged event.
    /// </summary>
    public delegate void Selector<T>(object sender,
        SelectionChangedArgs<T> arguments);
    
    /// <summary>
    /// The arguments passed by a SelectionChanged event.
    /// </summary>
    public class SelectionChangedArgs<T> : EventArgs
    {
        /// <summary>
        /// The item currently selected.
        /// </summary>
        public T Selected { get; protected internal set; }
        
        public SelectionChangedArgs (T selected)
        {
            Selected = selected;
        }
    }
    
    public interface IDataSelection<T>
    {
        event Selector<T> SelectionChanged;
        T Selected { get; }
        void Select(T item);
    }
    
    public class DataSelection<T> : IDataSelection<T>
    {
        public event Selector<T> SelectionChanged;
        
        public T Selected { get; protected internal set; }
        
        public void Select (T item)
        {
            Selected = item;
            if (SelectionChanged != null)
            {
                SelectionChanged (this, new SelectionChangedArgs<T> (item));
            }
        }           
    }
    
    [TestFixture]
    public class TestDataSelection
    {
        protected internal bool HandlerCalled { get; set; }
        protected internal IDataSelection<Meet> CurrentMeet { get; set; }
        protected internal IList<Meet> Meets { get; set; }
        protected internal Meet Selected { get; set; } 
        protected internal IDataSelection<Meet> Sender { get; set; }
        
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
            Assert.IsNull (CurrentMeet.Selected);
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
