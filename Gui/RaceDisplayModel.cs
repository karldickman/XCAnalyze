using NUnit.Framework;
using System;
using System.Collections.Generic;
using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    /// <summary>
    /// The model used by race visualizers to determine which race to draw.
    /// </summary>
    public class RaceDisplayModel : DataSelection<Race>
    {
        /// <summary>
        /// Is this model supposed to display a men's race or a women's race?
        /// </summary>
        public Gender Gender { get; set; }
        
        public RaceDisplayModel (Gender gender,
            IDataSelection<Meet> meetSelection)
        {
            Gender = gender;
            meetSelection.SelectionChanged += OnSelectionChanged;
        }
        
        /// <summary>
        /// Callback to handle a changed to the currently selected meet.
        /// </summary>
        public void OnSelectionChanged (object sender,
            SelectionChangedArgs<Meet> arguments)
        {
            Meet meet = arguments.Selected;
            Select (meet.Race (Gender));
        }
    }
    
    [TestFixture]
    public class TestRaceDisplayModel
    {
        protected internal IDataSelection<Meet> CurrentMeet { get; set; }
        protected internal bool HandlerCalled { get; set; }
        protected internal IList<Meet> Meets { get; set; }
        protected internal RaceDisplayModel MModel { get; set; }
        protected internal RaceDisplayModel WModel { get; set; }
        protected internal Race Selected { get; set; }
        protected internal RaceDisplayModel Sender { get; set; }
        
        [SetUp]
        public void SetUp ()
        {
            Meets = new List<Meet> ();
            Meets.Add (new Meet ("Lewis & Clark Invitational", new Date (2010, 9, 27), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("Pacific Lutheran Invitational", new Date (2010, 10, 23), null, new Race (null, 8000), new Race (null, 6000)));
            Meets.Add (new Meet ("NWC Championship", new Date (2010, 11, 1), null, new Race (null, 8000), new Race (null, 6000)));
            CurrentMeet = new DataSelection<Meet> ();
            MModel = new RaceDisplayModel (Gender.MALE, CurrentMeet);
            WModel = new RaceDisplayModel (Gender.FEMALE, CurrentMeet);
            WModel.SelectionChanged += Handler;
        }
        
        [Test]
        public void TestSelect ()
        {
            Assert.That (!HandlerCalled);
            //Assert.IsNull (WModel.Selected);
            foreach (int i in new int[] { 2, 0, 1 })
            {
                CurrentMeet.Select (Meets[i]);
                Assert.That (HandlerCalled);
                Assert.AreEqual (Meets[i].MensRace, Selected);
                Assert.AreEqual (Meets[i].WomensRace, Selected);
                HandlerCalled = false;
            }
        }
        
        public void Handler (object sender, EventArgs arguments)
        {
            HandlerCalled = true;
            Sender = (RaceDisplayModel)sender;
            Selected = ((SelectionChangedArgs<Race>)arguments).Selected;
        }
    }
}
