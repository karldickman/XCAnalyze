using System;
using System.IO;

using NUnit.Framework;

using XCAnalyze.Model;
using XCAnalyze.IO.Sql;

namespace XCAnalyze.IO
{
    
    [TestFixture]
    public class TestXcaWriter
    {
        protected internal static readonly string EXAMPLE_FILE = SupportFiles.GetPath("example.xca");
        protected internal const string TEST_FILE = "test.xca";
            
        protected internal XcData Data { get; set; }
        protected internal XcaReader Reader { get; set; }
        protected internal XcaWriter Writer { get; set; }
                
        public static bool AreDataEqual(XcData item1, XcData item2)
        {
            if(item1.Affiliations.Count != item2.Affiliations.Count)
            {
                return false;
            }
            foreach(Affiliation affiliation in item1.Affiliations)
            {
                if(!item2.Affiliations.Contains(affiliation))
                {
                    return false;
                }
            }
            if(item1.Conferences.Count != item2.Conferences.Count)
            {
                return false;
            }
            foreach(string conference in item1.Conferences)
            {
                if(!item2.Conferences.Contains(conference))
                {
                    return false;
                }
            }
            if(item1.Meets.Count != item2.Meets.Count)
            {
                return false;
            }
            foreach(string meetName in item1.MeetNames)
            {
                if(!item2.MeetNames.Contains(meetName))
                {
                    return false;
                }
            }
            if(item1.Performances.Count != item2.Performances.Count)
            {
                return false;
            }
            foreach(Performance performance in item1.Performances)
            {
                if(!item2.Performances.Contains(performance))
                {
                    return false;
                }
            }
            if(item1.Races.Count != item2.Races.Count)
            {
                return false;
            }
            foreach(Race race in item1.Races)
            {
                if(!item2.Races.Contains(race))
                {
                    return false;
                }
            }
            if(item1.Runners.Count != item2.Runners.Count)
            {
                return false;
            }
            foreach(Runner runner in item1.Runners)
            {
                if(!item2.Runners.Contains(runner))
                {
                    return false;
                }
            }
            if(item1.Schools.Count != item2.Schools.Count)
            {
                return false;
            }
            foreach(School school in item1.Schools)
            {
                if(!item2.Schools.Contains(school))
                {
                    return false;
                }
            }
            if(item1.Venues.Count != item2.Venues.Count)
            {
                return false;
            }
            foreach(Venue venue in item1.Venues)
            {
                if(!item2.Venues.Contains(venue))
                {
                    return false;
                }
            }
            return true;
        }
        
        [SetUp]
        public void SetUp ()
        {
            Reader = new XcaReader (EXAMPLE_FILE);
            Data = Reader.Read ();
            Reader.Dispose ();         
            Writer = new XcaWriter (TEST_FILE); 
        }
        
        [TearDown]
        public void TearDown ()
        {
            Writer.Dispose ();
            File.Delete (TEST_FILE);
        }
        
        [Test]
        public void TestWrite ()
        {
            XcData actual;
            for (int i = 0; i < 3; i++)
            {
                Writer.Write (Data);
                Writer.Dispose ();
                Reader = new XcaReader (TEST_FILE);
                actual = Reader.Read ();
                Reader.Dispose ();
                Assert.That (AreDataEqual (Data, actual));
                Data = actual;
                Writer = new XcaWriter (TEST_FILE);
            }
        }
    }
}
