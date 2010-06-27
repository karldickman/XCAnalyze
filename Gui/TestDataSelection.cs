using System;
using System.Collections.Generic;

using NUnit.Framework;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
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
            Meets.Add (new Meet ("Lewis & Clark Invitational", new DateTime (2010, 9, 27), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("Pacific Lutheran Invitational", new DateTime (2010, 10, 23), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("NWC Championship", new DateTime (2010, 11, 1), null, new Race (null, 8000), new Race (null, 6000)));
            HandlerCalled = false;
            CurrentMeet = new DataSelection<Meet> ();
            CurrentMeet.SelectionChanged += Handler;
        }

        [Test]
        public void TestSelect ()
        {
            Assert.That (!HandlerCalled);
            //Assert.IsNull (CurrentMeet.Selected);
            foreach (int i in new int[] { 2, 0, 1 }) {
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
