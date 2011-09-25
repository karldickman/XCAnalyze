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

        public static readonly Conference Nwc;
        public static readonly Conference Sciac;
        public static readonly Conference Scac;
        public static readonly Conference Pac12;

        public static readonly IEnumerable<Conference> Conferences;

        #endregion
        #region Teams

        public static readonly Team LewisAndClark;
        public static readonly Team Willamette;
        public static readonly Team PugetSound;
        public static readonly Team ClaremontMuddScripps;
        public static readonly Team Pomona;
        public static readonly Team ColoradoCollege;
        public static readonly Team Chapman;
        public static readonly Team Whitman;
        public static readonly Team UniversityOfWashington;
        public static readonly Team PacificLutheran;

        public static readonly IEnumerable<Team> Teams;

        #endregion
        #region States

        public static readonly State California;
        public static readonly State Colorado;
        public static readonly State Oregon;
        public static readonly State Washington;

        public static readonly IEnumerable<State> States;

        #endregion
        #region Cities

        public static readonly City Estacada;
        public static readonly City Seattle;
        public static readonly City Claremont;
        public static readonly City WallaWalla;
        public static readonly City Chino;
        public static readonly City Salem;
        public static readonly City Tacoma;
        public static readonly City Portland;

        public static readonly IEnumerable<City> Cities;

        #endregion
        #region Venues

        public static readonly Venue McIver;
        public static readonly Venue BushPark;
        public static readonly Venue VeteransGolfCourse;
        public static readonly Venue PomonaCampus;
        public static readonly Venue LincolnPark;
        public static readonly Venue PradoPark;
        public static readonly Venue PluGolfCourse;
        public static readonly Venue FortSteilacoom;

        public static readonly IEnumerable<Venue> Venues;

        #endregion
        #region Runners

        public static readonly Runner Karl;
        public static readonly Runner Hannah;
        public static readonly Runner Richie;
        public static readonly Runner Keith;
        public static readonly Runner Leo;
        public static readonly Runner Francis;
        public static readonly Runner Florian;
        public static readonly Runner Jackson;

        public static readonly IEnumerable<Runner> Runners;

        #endregion
        #region Meets

        public static readonly Meet LCInvite;
        public static readonly Meet CharlesBowles;
        public static readonly Meet NwcChampionships;
        public static readonly Meet SciacMultiDuals;
        public static readonly Meet Sundodger;
        public static readonly Meet Regionals;
        public static readonly Meet PluInvite;

        public static readonly IEnumerable<Meet> Meets;

        #endregion
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

        public static readonly IEnumerable<MeetInstance> MeetInstances;

        #endregion
        #region Races

        public static readonly IDictionary<MeetInstance, Gender, Race> Races;

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
            // Conferences
            Nwc = new Conference("Northwest Conference", "NWC");
            Sciac = new Conference("Southern California Intercollegiate Athletic Conference", "SCIAC");
            Scac = new Conference("Southern Collegiate Athletic Conference", "SCAC");
            Pac12 = new Conference("Pacific 12", "PAC-12");
            Conferences = new List<Conference> { Nwc, Sciac, Scac, Pac12 };
            // Teams
            LewisAndClark = new Team("Lewis & Clark") { Conference = Nwc };
            Willamette = new Team("Willamette") { Conference = Nwc };
            PugetSound = new Team("Puget Sound") { Conference = Nwc };
            ClaremontMuddScripps = new Team("Claremont-Mudd-Scripps") { Conference = Sciac };
            Pomona = new Team("Pomona-Pitzer") { Conference = Sciac };
            ColoradoCollege = new Team("Colorado") { Conference = Scac };
            Chapman = new Team("Chapman");
            Whitman = new Team("Whitman") { Conference = Nwc };
            UniversityOfWashington = new Team("Washington") { Conference = Pac12 };
            PacificLutheran = new Team("Pacific Lutheran") { Conference = Nwc };
            Teams = new List<Team> { LewisAndClark, Willamette, PugetSound, ClaremontMuddScripps, Pomona, ColoradoCollege, Chapman, Whitman, UniversityOfWashington, PacificLutheran, };
            // States
            California = new State("CA", "California");
            Colorado = new State("CO", "Colorado");
            Oregon = new State("OR", "Oregon");
            Washington = new State("WA", "Washington");
            States = new List<State> { California, Colorado, Oregon, Washington };
            // Cities
            Estacada = new City("Estacada", Oregon);
            Seattle = new City("Seattle", Washington);
            Claremont = new City("Claremont", California);
            WallaWalla = new City("Walla Walla", Washington);
            Chino = new City("Chino", California);
            Salem = new City("Salem", Oregon);
            Tacoma = new City("Tacoma", Washington);
            Portland = new City("Portland", Oregon);
            Cities = new List<City> { Estacada, Seattle, Claremont, WallaWalla, Chino, Salem, Tacoma, Portland };
            // Venues
            McIver = new Venue("Milo McIver State Park", Estacada);
            BushPark = new Venue("Bush Pasture Park", Salem);
            VeteransGolfCourse = new Venue("Veteran's Memorial Golf Course", WallaWalla);
            PomonaCampus = new Venue("Pomona College Campus", Claremont);
            LincolnPark = new Venue("Lincoln Park", Seattle);
            PradoPark = new Venue("Prado Park", Chino);
            PluGolfCourse = new Venue("PLU Golf Course", Tacoma);
            FortSteilacoom = new Venue("Fort Steilacoom Park", Tacoma);
            Venues = new List<Venue> { McIver, BushPark, VeteransGolfCourse, PomonaCampus, LincolnPark, PradoPark, PluGolfCourse, FortSteilacoom };
            // Runners
            Karl = new Runner("Dickman", "Karl", Gender.Male) { EnrollmentYear = 2006 };
            Hannah = new Runner("Palmer", "Hannah", Gender.Female) { EnrollmentYear = 2006 };
            Richie = new Runner("LeDonne", "Richie", Gender.Male) { EnrollmentYear = 2007 };
            Keith = new Runner("Woodard", "Keith", Gender.Male) { EnrollmentYear = 1969 };
            Leo = new Runner("Castillo", "Leo", Gender.Male) { EnrollmentYear = 2012 };
            Francis = new Runner("Reynolds", "Francis", Gender.Male) { EnrollmentYear = 2010 };
            Florian = new Runner("Scheulen", "Florian", Gender.Male) { EnrollmentYear = 2010 };
            Jackson = new Runner("Brainerd", "Jackson", Gender.Male) { EnrollmentYear = 2012 };
            Runners = new List<Runner> { Karl, Hannah, Richie, Keith, Leo, Francis, Florian, Jackson };
            // Affiliations
            IEnumerable<Runner> runners = new List<Runner> { Florian, Karl, Francis, Richie, Leo, Jackson };
            IEnumerable<Team> teams = new List<Team> { ClaremontMuddScripps, LewisAndClark, PugetSound, LewisAndClark, Willamette, ColoradoCollege };
            IEnumerable<int> years = new List<int> { 2005, 2006, 2006, 2007, 2008, 2008 };
            IEnumerableExtensions.ForEachEqual(runners, teams, years, (runner, team, year) =>
            {
                for(int i = 0; i < 4; i++)
                {
                    runner.Affiliations[year + i] = team.ID;
                }
            });
            foreach(int year in new List<int> { 2006, 2008, 2009, 2010 })
            {
                Hannah.Affiliations[year] = LewisAndClark.ID;
            }
            // Meets
            LCInvite = new Meet("Lewis & Clark Invitational");
            CharlesBowles = new Meet("Charles Bowles Invitational");
            NwcChampionships = new Meet("Northwest Conference Championship");
            SciacMultiDuals = new Meet("SCIAC Multi-Duals");
            Sundodger = new Meet("Sundodger Invitational");
            Regionals = new Meet("NCAA West Region Championship");
            PluInvite = new Meet("Pacific Lutheran Invitational");
            Meets = new List<Meet> { LCInvite, CharlesBowles, NwcChampionships, SciacMultiDuals, Sundodger, Regionals, PluInvite };
            // Meet instances
            LCInvite09 = new MeetInstance(LCInvite, new DateTime(2009, 9, 12), McIver, LewisAndClark);
            LCInvite10 = new MeetInstance(LCInvite, new DateTime(2010, 9, 27), McIver, LewisAndClark);
            CharlesBowles09 = new MeetInstance(CharlesBowles, new DateTime(2009, 10, 3), BushPark, Willamette);
            NwcChampionships10 = new MeetInstance(NwcChampionships, new DateTime(2010, 10, 30), FortSteilacoom, PugetSound);
            NwcChampionships09 = new MeetInstance(NwcChampionships, new DateTime(2009, 10, 31), McIver, LewisAndClark);
            NwcChampionships08 = new MeetInstance(NwcChampionships, new DateTime(2008, 11, 1), VeteransGolfCourse, Whitman);
            PluInvite10 = new MeetInstance(PluInvite, new DateTime(2010, 10, 16), PluGolfCourse, PacificLutheran);
            SciacMultiDuals09 = new MeetInstance(SciacMultiDuals, new DateTime(2009, 10, 17), PradoPark, ClaremontMuddScripps);
            Sundodger09 = new MeetInstance(Sundodger, new DateTime(2009, 9, 19), LincolnPark, UniversityOfWashington);
            Regionals08 = new MeetInstance(Regionals, new DateTime(2009, 11, 15), BushPark, Willamette);
            Regionals09 = new MeetInstance(Regionals, new DateTime(2009, 11, 14), PomonaCampus, Pomona);
            MeetInstances = new List<MeetInstance> { LCInvite09, LCInvite10, CharlesBowles09, NwcChampionships10, NwcChampionships09, NwcChampionships08, PluInvite10, SciacMultiDuals09, Sundodger09, Regionals08,
            Regionals09 };
            // Races
            Races = new Dictionary<MeetInstance, Gender, Race>();
            foreach(MeetInstance meetInstance in MeetInstances)
            {
                int womensDistance = meetInstance.Meet == CharlesBowles ? 5000 : 6000;
                Races[meetInstance, Gender.Male] = new Race(meetInstance, Gender.Male, 8000);
                Races[meetInstance, Gender.Female] = new Race(meetInstance, Gender.Female, womensDistance);
            }
            // Performances
            KarlAtSundodger = new Performance(Karl, Races[Sundodger09, Gender.Male], 24 * 60 + 55);
            KarlAtWillamette = new Performance(Karl, Races[CharlesBowles09, Gender.Male], 24 * 60 + 44);
            HannahsPerformance = new Performance(Hannah, Races[Regionals08, Gender.Female], 22 * 60 + 3);
            FrancisPerformance = new Performance(Francis, Races[NwcChampionships08, Gender.Male], 24 * 60 + 30);
            Performances = new List<Performance> { KarlAtSundodger, KarlAtWillamette, HannahsPerformance, FrancisPerformance };
        }
    }
}

