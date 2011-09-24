using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.Utilities.Collections.Extensions;
using Ngol.Utilities.System.Collections.Generic;
using Ngol.XcAnalyze.Model;

namespace Ngol.XcAnalyze.Model.Tests
{
    public static class SampleData
    {
        #region Conferences

        public static readonly Conference Nwc = new Conference(1, "Northwest Conference", "NWC");
        public static readonly Conference Sciac = new Conference(2, "Southern California Intercollegiate Athletic Conference", "SCIAC");
        public static readonly Conference Scac = new Conference(3, "Southern Collegiate Athletic Conference", "SCAC");
        public static readonly Conference Pac12 = new Conference(4, "Pacific 12", "PAC-12");

        public static readonly IEnumerable<Conference> Conferences = new List<Conference> { Nwc, Sciac, Scac };

        #endregion
        #region Teams

        public static readonly Team LewisAndClark = new Team(1, "Lewis & Clark", Nwc);
        public static readonly Team Willamette = new Team(2, "Willamette", Nwc);
        public static readonly Team PugetSound = new Team(3, "Puget Sound", Nwc);
        public static readonly Team ClaremontMuddScripps = new Team(4, "Claremont-Mudd-Scripps", Sciac);
        public static readonly Team Pomona = new Team(5, "Pomona-Pitzer", Sciac);
        public static readonly Team ColoradoCollege = new Team(6, "Colorado", Scac);
        public static readonly Team Chapman = new Team(7, "Chapman");
        public static readonly Team Whitman = new Team(8, "Whitman", Nwc);
        public static readonly Team UniversityOfWashington = new Team(9, "Washington", Pac12);
        public static readonly Team PacificLutheran = new Team(10, "Pacific Lutheran", Nwc);

        public static readonly IEnumerable<Team> Teams = new List<Team> { LewisAndClark, Willamette, PugetSound, ClaremontMuddScripps, Pomona, ColoradoCollege, Chapman };

        #endregion
        #region States

        public static readonly State California = new State("CA", "California");
        public static readonly State Colorado = new State("CO", "Colorado");
        public static readonly State Oregon = new State("OR", "Oregon");
        public static readonly State Washington = new State("WA", "Washington");

        public static readonly IEnumerable<State> States = new List<State> { California, Colorado, Oregon, Washington };

        #endregion
        #region Cities

        public static readonly City Estacada = new City(1, "Estacada", Oregon);
        public static readonly City Seattle = new City(2, "Seattle", Washington);
        public static readonly City Claremont = new City(3, "Claremont", California);
        public static readonly City WallaWalla = new City(4, "Walla Walla", Washington);
        public static readonly City Chino = new City(5, "Chino", California);
        public static readonly City Salem = new City(6, "Salem", Oregon);
        public static readonly City Tacoma = new City(7, "Tacoma", Washington);
        public static readonly City Portland = new City(8, "Portland", Oregon);

        public static readonly IEnumerable<City> Cities = new List<City> { Estacada, Seattle, Claremont, WallaWalla, Chino, Salem, Tacoma, Portland };

        #endregion
        #region Venues

        public static readonly Venue McIver = new Venue(1, "Milo McIver State Park", Estacada);
        public static readonly Venue BushPark = new Venue(2, "Bush Pasture Park", Salem);
        public static readonly Venue VeteransGolfCourse = new Venue(3, "Veteran's Memorial Golf Course", WallaWalla);
        public static readonly Venue PomonaCampus = new Venue(4, "Pomona College Campus", Claremont);
        public static readonly Venue LincolnPark = new Venue(5, "Lincoln Park", Seattle);
        public static readonly Venue PradoPark = new Venue(6, "Prado Park", Chino);
        public static readonly Venue PluGolfCourse = new Venue(7, "PLU Golf Course", Tacoma);
        public static readonly Venue FortSteilacoom = new Venue(8, "Fort Steilacoom Park", Tacoma);

        public static readonly IEnumerable<Venue> Venues = new List<Venue> { McIver, BushPark, VeteransGolfCourse, PomonaCampus, LincolnPark, PradoPark, PluGolfCourse, FortSteilacoom };

        #endregion
        #region Runners

        public static readonly Runner Karl = new Runner(1, "Dickman", "Karl");
        //, Gender.Male, 2010);
        public static readonly Runner Hannah = new Runner(2, "Palmer", "Hannah");
        //, Gender.Female, 2010);
        public static readonly Runner Richie = new Runner(3, "LeDonne", "Richie");
        //, Gender.Male, 2011);
        public static readonly Runner Keith = new Runner(4, "Woodard", "Keith");
        //, Gender.Male, null);
        public static readonly Runner Leo = new Runner(5, "Castillo", "Leo");
        //, Gender.Male, 2012);
        public static readonly Runner Francis = new Runner(6, "Reynolds", "Francis");
        //, Gender.Male, 2010);
        public static readonly Runner Florian = new Runner(7, "Scheulen", "Florian");
        //, Gender.Male, 2010);
        public static readonly Runner Jackson = new Runner(8, "Brainerd", "Jackson");
        //, Gender.Male, 2012);
        public static readonly IEnumerable<Runner> Runners = new List<Runner> { Karl, Hannah, Richie, Keith, Leo, Francis, Florian, Jackson };

        #endregion
        #region Affiliations

        public static IDictionary<Runner, int, Affiliation> Affiliations;

        #endregion
        #region Meets

        public static readonly Meet LCInvite = new Meet(1, "Lewis & Clark Invitational");
        public static readonly Meet CharlesBowles = new Meet(2, "Charles Bowles Invitational");
        public static readonly Meet NwcChampionships = new Meet(3, "Northwest Conference Championship");
        public static readonly Meet SciacMultiDuals = new Meet(4, "SCIAC Multi-Duals");
        public static readonly Meet Sundodger = new Meet(5, "Sundodger Invitational");
        public static readonly Meet Regionals = new Meet(6, "NCAA West Region Championship");
        public static readonly Meet PluInvite = new Meet(7, "Pacific Lutheran Invitational");

        public static readonly IEnumerable<Meet> Meets = new List<Meet> { LCInvite, CharlesBowles, NwcChampionships, SciacMultiDuals, Sundodger, Regionals, PluInvite };

        #endregion
        #region MeetInstances

        public static readonly MeetInstance LCInvite09 = new MeetInstance(1, LCInvite, new DateTime(2009, 9, 12), McIver, LewisAndClark);
        public static readonly MeetInstance LCInvite10 = new MeetInstance(2, LCInvite, new DateTime(2010, 9, 27), McIver, LewisAndClark);
        public static readonly MeetInstance CharlesBowles09 = new MeetInstance(3, CharlesBowles, new DateTime(2009, 10, 3), BushPark, Willamette);
        public static readonly MeetInstance NwcChampionships10 = new MeetInstance(4, NwcChampionships, new DateTime(2010, 10, 30), FortSteilacoom, PugetSound);
        public static readonly MeetInstance NwcChampionships09 = new MeetInstance(5, NwcChampionships, new DateTime(2009, 10, 31), McIver, LewisAndClark);
        public static readonly MeetInstance NwcChampionships08 = new MeetInstance(6, NwcChampionships, new DateTime(2008, 11, 1), VeteransGolfCourse, Whitman);
        public static readonly MeetInstance PluInvite10 = new MeetInstance(7, PluInvite, new DateTime(2010, 10, 16), PluGolfCourse, PacificLutheran);
        public static readonly MeetInstance SciacMultiDuals09 = new MeetInstance(8, SciacMultiDuals, new DateTime(2009, 10, 17), PradoPark, ClaremontMuddScripps);
        public static readonly MeetInstance Sundodger09 = new MeetInstance(9, Sundodger, new DateTime(2009, 9, 19), LincolnPark, UniversityOfWashington);
        public static readonly MeetInstance Regionals08 = new MeetInstance(10, Regionals, new DateTime(2009, 11, 15), BushPark, Willamette);
        public static readonly MeetInstance Regionals09 = new MeetInstance(11, Regionals, new DateTime(2009, 11, 14), PomonaCampus, Pomona);

        public static readonly IEnumerable<MeetInstance> MeetInstances = new List<MeetInstance> { LCInvite09, LCInvite10, CharlesBowles09, NwcChampionships10, NwcChampionships09, NwcChampionships08, PluInvite10, SciacMultiDuals09, Sundodger09, Regionals08,
        Regionals09 };

        #endregion
        #region Races

        public static readonly IDictionary<MeetInstance, Gender, Race> Races = new Dictionary<MeetInstance, Gender, Race>();

        #endregion
        #region Performances

        public static readonly Performance KarlAtSundodger;
        public static readonly Performance KarlAtWillamette;
        public static readonly Performance HannahsPerformance;
        public static readonly Performance FrancisPerformance;

        public static readonly IList<Performance> Performances;

        #endregion

        static SampleData()
        {
            // Affiliations
            Affiliations = new Dictionary<Runner, int, Affiliation>();
            IEnumerable<Runner> runners = new List<Runner> { Florian, Karl, Francis, Richie, Leo, Jackson };
            IEnumerable<Team> teams = new List<Team> { ClaremontMuddScripps, LewisAndClark, PugetSound, LewisAndClark, Willamette, ColoradoCollege };
            IEnumerable<int> years = new List<int> { 2005, 2006, 2006, 2007, 2008, 2008 };
            IEnumerableExtensions.ForEachEqual(runners, teams, years, (runner, team, year) =>
            {
                for(int i = 0; i < 4; i++)
                {
                    Affiliations[runner, year + i] = new Affiliation(runner, team, year + i);
                }
            });
            foreach(int year in new List<int> { 2006, 2008, 2009, 2010 })
            {
                Affiliations[Hannah, year] = new Affiliation(Hannah, LewisAndClark, year);
            }
            // Races
            MeetInstances.ForEach(1, (meetInstance, id) =>
            {
                int womensDistance = meetInstance.Meet == CharlesBowles ? 5000 : 6000;
                Races[meetInstance, Gender.Male] = new Race(id, meetInstance, Gender.Male, 8000);
                Races[meetInstance, Gender.Female] = new Race(id, meetInstance, Gender.Female, womensDistance);
            });
            // Performances
            KarlAtSundodger = new Performance(1, Karl, Races[Sundodger09, Gender.Male], 24 * 60 + 55);
            KarlAtWillamette = new Performance(2, Karl, Races[CharlesBowles09, Gender.Male], 24 * 60 + 44);
            HannahsPerformance = new Performance(3, Hannah, Races[Regionals08, Gender.Female], 22 * 60 + 3);
            FrancisPerformance = new Performance(4, Francis, Races[NwcChampionships08, Gender.Male], 24 * 60 + 30);
            Performances = new List<Performance> { KarlAtSundodger, KarlAtWillamette, HannahsPerformance, FrancisPerformance };
        }
    }
}

