using System;
using System.Collections.Generic;
using System.Linq;
using Ngol.XcAnalyze.Model;
using Ngol.Utilities.Collections.Extensions;

namespace Ngol.XcAnalyze.Model.Tests
{
    public static class SampleData
    {
        #region Conferences

        public static readonly Conference Nwc = new Conference(1, "Northwest Conference", "NWC");
        public static readonly Conference Sciac = new Conference(2, "Southern California Intercollegiate Athletic Conference", "SCIAC");
        public static readonly Conference Scac = new Conference(3, "Southern Collegiate Athletic Conference", "SCAC");

        public static readonly IEnumerable<Conference> Conferences = new List<Conference> { Nwc, Sciac, Scac };

        #endregion
        #region Teams

        public static readonly Team LewisAndClark = new Team(1, "Lewis & Clark", Nwc);
        public static readonly Team Willamette = new Team(2, "Willamette", Nwc);
        public static readonly Team PugetSound = new Team(3, "PugetSound", Nwc);
        public static readonly Team ClaremontMuddScripps = new Team(4, "Claremont-Mudd-Scripps", Sciac);
        public static readonly Team Pomona = new Team(5, "Pomona-Pitzer", Sciac);
        public static readonly Team ColoradoCollege = new Team(6, "Colorado", Scac);
        public static readonly Team Chapman = new Team(7, "Chapman");

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

        public static IDictionary<Runner, IDictionary<int, Affiliation>> AffiliationDictionary;

        public static IEnumerable<Affiliation> Affiliations
        {
            get
            {
                return AffiliationDictionary.Values.SelectMany(entry => entry.Values);
            }
        }

        #endregion
        #region Meets

        public static readonly Meet LCInvite = new Meet(1, "Lewis & Clark Invitational");
        public static readonly Meet CharlesBowles = new Meet(2, "Charles Bowles Invitational");
        public static readonly Meet NwcChampionships = new Meet(3, "Northwest Conference Championship");
        public static readonly Meet SciacMultiDuals = new Meet(4, "SCIAC Multi-Duals");
        public static readonly Meet Sundodger = new Meet(5, "Sundodger Invitational");
        public static readonly Meet Regionals = new Meet(6, "NCAA West Region Championship");
        public static readonly Meet PluInvite = new Meet(7, "Pacific Lutheran Invitational");

        public static readonly IEnumerable<Meet> Meets = new List<Meet> { LCInvite, CharlesBowles, NwcChampionships, SciacMultiDuals, Sundodger, Regionals, PluInvite, };

        #endregion

        static SampleData()
        {
            AffiliationDictionary = new Dictionary<Runner, IDictionary<int, Affiliation>>();
            IEnumerable<Runner> runners = new List<Runner> { Florian, Karl, Francis, Richie, Leo, Jackson };
            IEnumerable<Team> teams = new List<Team> { ClaremontMuddScripps, LewisAndClark, PugetSound, LewisAndClark, Willamette, ColoradoCollege };
            IEnumerable<int> years = new List<int> { 2005, 2006, 2006, 2007, 2008, 2008 };
            IEnumerableExtensions.ForEachEqual(runners, teams, years, (runner, team, year) =>
            {
                AffiliationDictionary[runner] = new Dictionary<int, Affiliation>(4);
                for(int i = 0; i < 4; i++)
                {
                    AffiliationDictionary[runner][year + i] = new Affiliation(runner, team, year + i);
                }
            });
            AffiliationDictionary[Hannah] = new Dictionary<int, Affiliation>(4);
            foreach(int year in new List<int> { 2006, 2008, 2009, 2010 })
            {
                AffiliationDictionary[Hannah][year] = new Affiliation(Hannah, LewisAndClark, year);
            }
        }
    }
        /*
         * #region Properties

        #region MeetInstances

        public static readonly MeetInstance LCInvite09;
        public static readonly MeetInstance LCInvite10;
        public static readonly MeetInstance CharlesBowles09;
        public static readonly MeetInstance NwcChampionships10;
        public static readonly MeetInstance NwcChampionships09;
        public static readonly MeetInstance NwcChampionships08;
        public static readonly MeetInstance PluInvite10;
        public static readonly MeetInstance SciacMultiDuals09;
        public static readonly MeetInstance Sundodger09;
        public static readonly MeetInstance Regionals08;
        public static readonly MeetInstance Regionals09;

        #endregion


        #endregion

        static SampleData()
        {
            #region MeetInstances
            IList<MeetInstance> meetInstances = new List<MeetInstance>();
            LCInvite09 = new MeetInstance(LCInvite, new DateTime(2009, 9, 12), McIver);
            LCInvite10 = new MeetInstance(LCInvite, new DateTime(2010, 9, 27), McIver);
            CharlesBowles09 = new MeetInstance(CharlesBowles, new DateTime(2009, 10, 3), BushPark);
            NwcChampionships10 = new MeetInstance(NwcChampionships, new DateTime(2010, 10, 30), FortSteilacoom);
            NwcChampionships09 = new MeetInstance(NwcChampionships, new DateTime(2009, 10, 31), McIver);
            NwcChampionships08 = new MeetInstance(NwcChampionships, new DateTime(2008, 11, 1), VeteransGolfCourse);
            PluInvite10 = new MeetInstance(PluInvite, new DateTime(2010, 10, 16), PluGolfCourse);
            SciacMultiDuals09 = new MeetInstance(SciacMultiDuals, new DateTime(2009, 10, 17), PradoPark);
            Sundodger09 = new MeetInstance(Sundodger, new DateTime(2009, 9, 19), LincolnPark);
            Regionals08 = new MeetInstance(Regionals, new DateTime(2009, 11, 15), BushPark);
            Regionals09 = new MeetInstance(Regionals, new DateTime(2009, 11, 14), PomonaCampus);
            meetInstances.Add(LCInvite09);
            meetInstances.Add(LCInvite10);
            meetInstances.Add(CharlesBowles09);
            meetInstances.Add(NwcChampionships10);
            meetInstances.Add(NwcChampionships09);
            meetInstances.Add(NwcChampionships08);
            meetInstances.Add(PluInvite10);
            meetInstances.Add(SciacMultiDuals09);
            meetInstances.Add(Sundodger09);
            meetInstances.Add(Regionals08);
            meetInstances.Add(Regionals09);
            #endregion
            #region Races
            RaceLookup = new Dictionary<MeetInstance, IDictionary<Gender, Race>>();
            IList<Race> races = new List<Race>();
            int womensDistance;
            Race mensRace, womensRace;
            foreach(MeetInstance meetInstance in meetInstances) {
                mensRace = new Race(meetInstance, Gender.Male, 8000);
                womensDistance = meetInstance.Meet == CharlesBowles ? 5000 : 6000;
                womensRace = new Race(meetInstance, Gender.Female, womensDistance);
                IDictionary<Gender, Race> lookup = new Dictionary<Gender, Race>();
                lookup[Gender.Male] = mensRace;
                lookup[Gender.Female] = womensRace;
                RaceLookup.Add(meetInstance, lookup);
                races.Add(mensRace);
                races.Add(womensRace);
            }
            #endregion
            #region Performances
            IList<Performance> performances = new List<Performance>();
            performances.Add(new Performance(Karl, RaceLookup[Sundodger09][Gender.Male], 24 * 60 + 55));
            performances.Add(new Performance(Karl, RaceLookup[CharlesBowles09][Gender.Male], 24 * 60 + 44));
            performances.Add(new Performance(Hannah, RaceLookup[Regionals08][Gender.Female], 22 * 60 + 3));
            performances.Add(new Performance(Francis, RaceLookup[NwcChampionships08][Gender.Male], 24 * 60 + 30));
            #endregion
            Data = new DataContext(affiliations, cities, conferences, meetInstances, meets, performances, races, runners, states, teams,
            venues);
        }*/        
    }

