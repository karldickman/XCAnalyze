using System;
using System.Collections.Generic;

using NUnit.Framework;

using XCAnalyze.Model;

namespace XCAnalyze.Gui
{
    public partial class DataSelection
    {
        #if DEBUG
        [TestFixture]
        public class Test
        {
            protected bool HandlerCalled { get; set; }
            protected IDataSelection<MeetInstance> CurrentMeet { get; set; }
            protected MeetInstance LCInvite10 { get { return SampleData.LCInvite10; } }
            protected IList<MeetInstance> Meets;
            protected MeetInstance NwcChampionships10 { get { return SampleData.NwcChampionships10; } }
            protected MeetInstance Plu10 { get { return SampleData.PluInvite10; } }
            protected MeetInstance Selected { get; set; }
            protected IDataSelection<MeetInstance> Sender { get; set; }

            [SetUp]
            public void SetUp()
            {
                Meets = new List<MeetInstance>() { LCInvite10, Plu10, NwcChampionships10 };
                HandlerCalled = false;
                CurrentMeet = new DataSelection<MeetInstance>();
                CurrentMeet.SelectionChanged += Handler;
            }

            [Test]
            public void TestSelect()
            {
                Assert.That(!HandlerCalled);
                foreach(int i in new int[] {
                    2,
                    0,
                    1
                }) {
                    CurrentMeet.Select(Meets[i]);
                    Assert.That(HandlerCalled);
                    Assert.AreEqual(CurrentMeet, Sender);
                    Assert.AreEqual(Meets[i], Selected);
                    HandlerCalled = false;
                }
            }

            public void Handler(object sender, EventArgs arguments)
            {
                HandlerCalled = true;
                Sender = (IDataSelection<MeetInstance>)sender;
                Selected = ((SelectionChangedArgs<MeetInstance>)arguments).Selected;
            }
        }
        #endif
    }
}
