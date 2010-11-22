using System;
using System.IO;

using NUnit.Framework;

using XCAnalyze.Model;
using XCAnalyze.IO.Sql;

namespace XCAnalyze.IO
{
    public partial class XcaWriter
    {
        #if DEBUG
        [TestFixture]
        public new class Test
        {
            #region Properties

            static readonly string ExampleFile = SupportFiles.GetPath("example.xca");

            DataContext Data { get; set; }
            
            XcaReader Reader { get; set; }
            
            const string TestFile = "test.xca";
            
            XcaWriter Writer { get; set; }

            #endregion

            public static bool AreDataEqual(DataContext item1, DataContext item2)
            {
                if(item1.Affiliations.Count != item2.Affiliations.Count) {
                    return false;
                }
                foreach(Affiliation affiliation in item1.Affiliations) {
                    if(!item2.Affiliations.Contains(affiliation)) {
                        return false;
                    }
                }
                if(item1.Conferences.Count != item2.Conferences.Count) {
                    return false;
                }
                foreach(Conference conference in item1.Conferences) {
                    if(!item2.Conferences.Contains(conference)) {
                        return false;
                    }
                }
                if(item1.MeetInstances.Count != item2.MeetInstances.Count) {
                    return false;
                }
                foreach(Meet meetName in item1.Meets) {
                    if(!item2.Meets.Contains(meetName)) {
                        return false;
                    }
                }
                if(item1.Performances.Count != item2.Performances.Count) {
                    return false;
                }
                foreach(Performance performance in item1.Performances) {
                    if(!item2.Performances.Contains(performance)) {
                        return false;
                    }
                }
                if(item1.Races.Count != item2.Races.Count) {
                    return false;
                }
                foreach(Race race in item1.Races) {
                    if(!item2.Races.Contains(race)) {
                        return false;
                    }
                }
                if(item1.Runners.Count != item2.Runners.Count) {
                    return false;
                }
                foreach(Runner runner in item1.Runners) {
                    if(!item2.Runners.Contains(runner)) {
                        return false;
                    }
                }
                if(item1.Teams.Count != item2.Teams.Count) {
                    return false;
                }
                foreach(Team school in item1.Teams) {
                    if(!item2.Teams.Contains(school)) {
                        return false;
                    }
                }
                if(item1.Venues.Count != item2.Venues.Count) {
                    return false;
                }
                foreach(Venue venue in item1.Venues) {
                    if(!item2.Venues.Contains(venue)) {
                        return false;
                    }
                }
                return true;
            }

            [SetUp]
            public void SetUp()
            {
                using(Reader = new XcaReader(ExampleFile))
                {
                    Data = Reader.Read();
                }
                Data.DetachAll();
                Writer = new XcaWriter(TestFile);
            }

            [TearDown]
            public void TearDown()
            {
                Writer.Close();
                File.Delete(TestFile);
            }

            [Test]
            public void TestWrite()
            {
                DataContext actual;
                for(int i = 0; i < 3; i++) {
                    Writer.Write(Data);
                    Writer.Close();
                    using(Reader = new XcaReader(TestFile))
                    {
                        actual = Reader.Read();
                    }
                    Assert.That(AreDataEqual(Data, actual));
                    Data = actual;
                    Writer = new XcaWriter(TestFile);
                }
            }
        }
        #endif
    }
}
