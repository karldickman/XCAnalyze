using System;
using System.Collections.Generic;

using NUnit.Framework;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    [TestFixture]
    public class TestRaceDisplayModel
    {
        protected IDataSelection<Meet> CurrentMeet { get; set; }
        protected bool MHandlerCalled { get; set; }
        protected bool WHandlerCalled { get; set; }
        protected IList<Meet> Meets { get; set; }
        protected RaceDisplayModel MModel { get; set; }
        protected RaceDisplayModel WModel { get; set; }
        protected Race MSelected { get; set; }
        protected Race WSelected { get; set; }
        protected RaceDisplayModel MSender { get; set; }
        protected RaceDisplayModel WSender { get; set; }

        [SetUp]
        public void SetUp ()
        {
            Meets = new List<Meet> ();
            Meets.Add (new Meet ("Lewis & Clark Invitational", new DateTime (2010, 9, 27), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("Pacific Lutheran Invitational", new DateTime (2010, 10, 23), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("NWC Championship", new DateTime (2010, 11, 1), null, new Race (null, 8000), new Race (null, 6000)));
            CurrentMeet = new DataSelection<Meet> ();
            MModel = new RaceDisplayModel (Gender.MALE, CurrentMeet);
            MModel.SelectionChanged += MHandler;
            WModel = new RaceDisplayModel (Gender.FEMALE, CurrentMeet);
            WModel.SelectionChanged += WHandler;
        }

        [Test]
        public void TestSelect ()
        {
            Assert.That (!MHandlerCalled);
            Assert.That (!WHandlerCalled);
            //Assert.IsNull (WModel.Selected);
            foreach (int i in new int[] { 2, 0, 1 }) {
                CurrentMeet.Select (Meets[i]);
                Assert.That (MHandlerCalled);
                Assert.That (WHandlerCalled);
                Assert.AreEqual (Meets[i].MensRace, MSelected);
                Assert.AreEqual (Meets[i].WomensRace, WSelected);
                MHandlerCalled = false;
                WHandlerCalled = false;
            }
        }

        public void MHandler (object sender, SelectionChangedArgs<Race> arguments)
        {
            MHandlerCalled = true;
            MSender = (RaceDisplayModel)sender;
            MSelected = arguments.Selected;
        }

        public void WHandler (object sender, SelectionChangedArgs<Race> arguments)
        {
            WHandlerCalled = true;
            WSender = (RaceDisplayModel)sender;
            WSelected = arguments.Selected;
        }
    }
}
