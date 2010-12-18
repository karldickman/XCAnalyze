using System;
using System.Collections.Generic;

using NUnit.Framework;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public partial class RaceDisplayModel
    {
        #if DEBUG
        [TestFixture]
        public class Test
        {
            protected IDataSelection<MeetInstance> CurrentMeet { get; set; }
            protected bool MHandlerCalled { get; set; }
            protected bool WHandlerCalled { get; set; }
            protected IList<MeetInstance> Meets { get; set; }
            protected RaceDisplayModel MModel { get; set; }
            protected RaceDisplayModel WModel { get; set; }
            protected Race MSelected { get; set; }
            protected Race WSelected { get; set; }
            protected RaceDisplayModel MSender { get; set; }
            protected RaceDisplayModel WSender { get; set; }

            [SetUp]
            public void SetUp()
            {
                Meets = new List<MeetInstance> { SampleData.LCInvite10, SampleData.PluInvite10, SampleData.NwcChampionships10 };
                CurrentMeet = new DataSelection<MeetInstance>();
            }

            [Test]
            public void TestSelect()
            {
                Assert.That(!MHandlerCalled);
                Assert.That(!WHandlerCalled);
                //Assert.IsNull (WModel.Selected);
                foreach(int i in new int[] {
                    2,
                    0,
                    1
                }) {
                    CurrentMeet.Select(Meets[i]);
                    Assert.That(MHandlerCalled);
                    Assert.That(WHandlerCalled);
                    //Assert.AreEqual (Meets[i].MensRace, MSelected);
                    //Assert.AreEqual (Meets[i].WomensRace, WSelected);
                    MHandlerCalled = false;
                    WHandlerCalled = false;
                }
            }

            public void MHandler(object sender, SelectionChangedArgs<Race> arguments)
            {
                MHandlerCalled = true;
                MSender = (RaceDisplayModel)sender;
                MSelected = arguments.Selected;
            }

            public void WHandler(object sender, SelectionChangedArgs<Race> arguments)
            {
                WHandlerCalled = true;
                WSender = (RaceDisplayModel)sender;
                WSelected = arguments.Selected;
            }
        }
        #endif
    }
}
