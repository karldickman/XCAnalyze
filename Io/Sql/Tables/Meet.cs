using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XCAnalyze.Io.Sql.Tables
{
    public class Meet
    {
        /// <summary>
        /// Create a list of meets representing a particular list of races.
        /// </summary>
        /// <param name="races">
        /// The <see cref="IList<Model.Race>"/> from which to create the meets.
        /// </param>
        /// <returns>
        /// A <see cref="IList<Model.Meet>"/> of all the meets containing the
        /// given races.
        /// </returns>
        public static IList<Model.Meet> MeetsList (IList<Model.Race> races)
        {
            IList<Model.Meet> meets = new List<Model.Meet> ();
            IList<Race[]> pairs = new List<Race[]> ();
            var mensRaces = from race in races
                where race.Gender.IsMale
                select race;
            var womensRaces = from race in races
                where race.Gender.IsFemale
                select race;
            IList<Model.Race> unpairedMensRaces = new List<Model.Race> (mensRaces);
            IList<Model.Race> unpairedWomensRaces = new List<Model.Race> (womensRaces);
            foreach (Race mensRace in mensRaces)
            {
                /*
                Race womensRace = (from Race race in unpairedWomensRaces
                    where (mensRace.Date.Equals (race.Date)
                        && mensRace.MeetId == race.MeetId)
                    select race).FirstOrDefault ();*/
                Race womensRace = null;
                foreach(Race race in unpairedWomensRaces)
                {
                    if(mensRace.Date.Equals(race.Date)
                        && mensRace.MeetNameId == race.MeetNameId)
                    {
                        womensRace = race;
                        break;
                    }
                }
                if (womensRace != null) 
                {
                    pairs.Add (new Race[] { mensRace, womensRace });
                    unpairedMensRaces.Remove (mensRace);
                    unpairedWomensRaces.Remove (womensRace);
                }
            }
            foreach (Race race in unpairedMensRaces)
            {
                pairs.Add (new Race[] { race, null });
            }
            foreach (Race race in unpairedWomensRaces)
            {
                pairs.Add (new Race[] { null, race });
            }
            foreach (Race[] pair in pairs)
            {
                Race race = (pair[0] == null) ? pair[1] : pair[0];
                meets.Add (new Model.Meet (race.Name, race.Date, race.Venue,
                        pair[0], pair[1]));
            }
            return meets;
        }
    }
    
    [TestFixture]
    public class TestMeet
    {
        [TearDown]
        public void TearDown ()
        {
            Race.Clear ();
            MeetName.Clear ();
        }
        
        [Test]
        public void TestMeetsList ()
        {
            int nr = 0;
            IList<MeetName> meetNames = new List<MeetName> ();
            meetNames.Add (new MeetName (1, "Lewis & Clark Invitational"));
            meetNames.Add (new MeetName (2, "Charles Bowles Invitational"));
            meetNames.Add (new MeetName (3, "Sundodger Invitational"));
            meetNames.Add (new MeetName (4, "Linfield Invitational"));
            meetNames.Add (new MeetName (5, "Prefontaine Classic"));
            meetNames.Add (new MeetName (6, "NCAA National Championship"));
            IList<Model.Meet> meets;
            IList<Model.Race> races = new List<Model.Race> ();
            races.Add (new Race (nr++, meetNames[0].Id, new Model.Date (2009, 9, 5), 17, Model.Gender.MALE, 8000));
            races.Add (new Race (nr++, meetNames[1].Id, new Model.Date (2009, 9, 17), 9, Model.Gender.MALE, 8000));
            races.Add (new Race (nr++, meetNames[2].Id, new Model.Date (2009, 10, 1), 3, Model.Gender.MALE, 8000));
            races.Add (new Race (nr++, meetNames[3].Id, new Model.Date (2009, 11, 2), 1, Model.Gender.MALE, 8000));
            int maleRaces = races.Count;
            for (int i = 0; i < maleRaces; i++) 
            {
                Race race = (Race)races[i];
                races.Add(new Race (nr++, race.MeetNameId, race.Date, race.VenueId, Model.Gender.FEMALE, 6000));
            }
            races.Add(new Race (nr++, meetNames[4].Id, new Model.Date (2009, 9, 5), 42, Model.Gender.MALE, 8000));
            races.Add(new Race (nr++, meetNames[5].Id, new Model.Date (2009, 9, 15), 4, Model.Gender.FEMALE, 6000));
            meets = Meet.MeetsList (races);
            Assert.AreEqual (6, meets.Count);
        }
    }
}
