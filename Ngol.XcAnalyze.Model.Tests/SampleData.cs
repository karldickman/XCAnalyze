using System;
using System.Collections.Generic;
using Ngol.XcAnalyze.Model;

namespace Ngol.XcAnalyze.Model.Tests
{
    public static class SampleData
    {
        #region States

        public static readonly State California = new State("CA", "California");
        public static readonly State Colorado = new State("CO", "Colorado");
        public static readonly State Oregon = new State("OR", "Oregon");
        public static readonly State Washington = new State("WA", "Washington");

        public static readonly IEnumerable<State> States = new List<State> { California, Colorado, Oregon, Washington, };

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

        public static readonly IEnumerable<City> Cities = new List<City> { Estacada, Seattle, Claremont, WallaWalla, Chino, Salem, Tacoma, Portland, };

        #endregion
        #region Runners
        
        public static readonly Runner Karl = new Runner(1, "Dickman", "Karl");//, Gender.Male, 2010);
        public static readonly Runner Hannah = new Runner(2, "Palmer", "Hannah");//, Gender.Female, 2010);
        public static readonly Runner Richie = new Runner(3, "LeDonne", "Richie");//, Gender.Male, 2011);
        public static readonly Runner Keith = new Runner(4, "Woodard", "Keith");//, Gender.Male, null);
        public static readonly Runner Leo = new Runner(5, "Castillo", "Leo");//, Gender.Male, 2012);
        public static readonly Runner Francis = new Runner(6, "Reynolds", "Francis");//, Gender.Male, 2010);
        public static readonly Runner Florian = new Runner(7, "Scheulen", "Florian");//, Gender.Male, 2010);
        public static readonly Runner Jackson = new Runner(8, "Brainerd", "Jackson");//, Gender.Male, 2012);

        public static readonly IEnumerable<Runner> Runners = new List<Runner> { Karl, Hannah, Richie, Keith, Leo, Francis, Florian, Jackson, };

        #endregion
        /*
         * #region Properties

        #region Cities

        public static readonly City Estacada;
        public static readonly City Seattle;
        public static readonly City Claremont;
        public static readonly City WallaWalla;
        public static readonly City Chino;
        public static readonly City Salem;
        public static readonly City Tacoma;

        #endregion

        #region Conferences

        public static readonly Conference Nwc;
        public static readonly Conference Sciac;
        public static readonly Conference Scac;

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

        #endregion

        #region Meets

        public static readonly Meet LCInvite = new Meet("Lewis & Clark Invitational");
        public static readonly Meet CharlesBowles = new Meet("Charles Bowles Invitational");
        public static readonly Meet NwcChampionships = new Meet("Northwest Conference Championship");
        public static readonly Meet SciacMultiDuals = new Meet("SCIAC Multi-Duals");
        public static readonly Meet Sundodger = new Meet("Sundodger Invitational");
        public static readonly Meet Regionals = new Meet("NCAA West Region Championship");
        public static readonly Meet PluInvite = new Meet("Pacific Lutheran Invitational");

        #endregion

        #region Runners

        public static readonly Runner Karl = new Runner("Dickman", "Karl", Gender.Male, 2010);
        public static readonly Runner Hannah = new Runner("Palmer", "Hannah", Gender.Female, 2010);
        public static readonly Runner Richie = new Runner("LeDonne", "Richie", Gender.Male, 2011);
        public static readonly Runner Keith = new Runner("Woodard", "Keith", Gender.Male, null);
        public static readonly Runner Leo = new Runner("Castillo", "Leo", Gender.Male, 2012);
        public static readonly Runner Francis = new Runner("Reynolds", "Francis", Gender.Male, 2010);
        public static readonly Runner Florian = new Runner("Scheulen", "Florian", Gender.Male, 2010);
        public static readonly Runner Jackson = new Runner("Brainerd", "Jackson", Gender.Male, 2012);

        #endregion

        #region States

        public static readonly State Oregon = new State("OR", "Oregon");
        public static readonly State California = new State("CA", "California");
        public static readonly State Colorado = new State("CO", "Colorado");
        public static readonly State Texas = new State("TX", "Texas");
        public static readonly State Washington = new State("WA", "Washington");

        #endregion

        #region Teams

        public static readonly Team LewisAndClark;
        public static readonly Team Willamette;
        public static readonly Team PugetSound;
        public static readonly Team ClaremontMuddScripps;
        public static readonly Team Pomona;
        public static readonly Team ColoradoCollege;
        public static readonly Team Chapman;

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

        #endregion

        public static IList<City> Cities {
            get { return Data.Cities; }
        }

        public static IList<Conference> Conferences {
            get { return Data.Conferences; }
        }

        public static IList<Affiliation> Affiliations {
            get { return Data.Affiliations; }
        }

        public static readonly DataContext Data;

        public static IList<MeetInstance> MeetInstances {
            get { return Data.MeetInstances; }
        }

        public static IList<Meet> Meets {
            get { return Data.Meets; }
        }

        public static IList<Performance> Performances {
            get { return Data.Performances; }
        }

        public static readonly IDictionary<MeetInstance, IDictionary<Gender, Race>> RaceLookup;

        public static IList<Race> Races {
            get { return Data.Races; }
        }

        public static IList<Runner> Runners {
            get { return Data.Runners; }
        }

        public static IList<State> States {
            get { return Data.States; }
        }

        public static IList<Team> Teams {
            get { return Data.Teams; }
        }

        public static IList<Venue> Venues {
            get { return Data.Venues; }
        }

        #endregion

        static SampleData()
        {
            #region Conferences
            IList<Conference> conferences = new List<Conference>();
            Nwc = new Conference("Northwest Conference", "NWC");
            Sciac = new Conference("Southern California Intercollegiate Athletic Conference", "SCIAC");
            Scac = new Conference("Southern Collegiate Athletic Conference", "SCAC");
            conferences.Add(Nwc);
            conferences.Add(Sciac);
            conferences.Add(Scac);
            #endregion
            #region Teams
            IList<Team> teams = new List<Team>();
            LewisAndClark = new Team("Lewis & Clark", Nwc);
            Willamette = new Team("Willamette", Nwc);
            PugetSound = new Team("PugetSound", Nwc);
            ClaremontMuddScripps = new Team("Claremont-Mudd-Scripps", Sciac);
            Pomona = new Team("Pomona-Pitzer", Sciac);
            ColoradoCollege = new Team("Colorado", Scac);
            Chapman = new Team("Chapman");
            teams.Add(LewisAndClark);
            teams.Add(Willamette);
            teams.Add(PugetSound);
            teams.Add(ClaremontMuddScripps);
            teams.Add(Pomona);
            teams.Add(ColoradoCollege);
            teams.Add(Chapman);
            #endregion
            #region Runners
            IList<Runner> runners = new List<Runner>();
            runners.Add(Karl);
            runners.Add(Hannah);
            runners.Add(Richie);
            runners.Add(Keith);
            runners.Add(Leo);
            runners.Add(Francis);
            runners.Add(Florian);
            runners.Add(Jackson);
            #endregion
            #region Affiliations
            IList<Affiliation> affiliations = new List<Affiliation>();
            int y;
            for(y = 0; y < 4; y++) {
                affiliations.Add(new Affiliation(Florian, ClaremontMuddScripps, y + 2005));
                affiliations.Add(new Affiliation(Karl, LewisAndClark, y + 2006));
                affiliations.Add(new Affiliation(Francis, PugetSound, y + 2006));
                affiliations.Add(new Affiliation(Richie, LewisAndClark, y + 2007));
                affiliations.Add(new Affiliation(Leo, Willamette, y + 2008));
                affiliations.Add(new Affiliation(Jackson, ColoradoCollege, y + 2008));
            }
            affiliations.Add(new Affiliation(Hannah, LewisAndClark, 2006));
            affiliations.Add(new Affiliation(Hannah, LewisAndClark, 2008));
            affiliations.Add(new Affiliation(Hannah, LewisAndClark, 2009));
            affiliations.Add(new Affiliation(Hannah, LewisAndClark, 2010));
            #endregion
            #region States
            IList<State> states = new List<State>();
            states.Add(Oregon);
            states.Add(Washington);
            states.Add(California);
            states.Add(Colorado);
            states.Add(Texas);
            #endregion
            #region Cities
            IList<City> cities = new List<City>();
            Estacada = new City("Estacada", Oregon);
            Seattle = new City("Seattle", Washington);
            Claremont = new City("Claremont", California);
            WallaWalla = new City("Walla Walla", Washington);
            Chino = new City("Chino", California);
            Salem = new City("Salem", Oregon);
            Tacoma = new City("Tacoma", Washington);
            cities.Add(Estacada);
            cities.Add(Seattle);
            cities.Add(Claremont);
            cities.Add(WallaWalla);
            cities.Add(Chino);
            cities.Add(Salem);
            cities.Add(Tacoma);
            #endregion
            #region Venues
            IList<Venue> venues = new List<Venue>();
            McIver = new Venue("Milo McIver State Park", Estacada);
            BushPark = new Venue("Bush Pasture Park", Salem);
            VeteransGolfCourse = new Venue("Veteran's Memorial Golf Course", WallaWalla);
            PomonaCampus = new Venue("Pomona College Campus", Claremont);
            LincolnPark = new Venue("Lincoln Park", Seattle);
            PradoPark = new Venue("Prado Park", Chino);
            PluGolfCourse = new Venue("PLU Golf Course", Tacoma);
            FortSteilacoom = new Venue("Fort Steilacoom Park", Tacoma);
            venues.Add(McIver);
            venues.Add(BushPark);
            venues.Add(VeteransGolfCourse);
            venues.Add(PomonaCampus);
            venues.Add(LincolnPark);
            venues.Add(PradoPark);
            venues.Add(PluGolfCourse);
            venues.Add(FortSteilacoom);
            #endregion
            #region Meets
            IList<Meet> meets = new List<Meet>();
            meets.Add(LCInvite);
            meets.Add(CharlesBowles);
            meets.Add(NwcChampionships);
            meets.Add(SciacMultiDuals);
            meets.Add(Sundodger);
            meets.Add(Regionals);
            meets.Add(PluInvite);
            #endregion
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
}

