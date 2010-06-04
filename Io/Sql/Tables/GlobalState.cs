using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XCAnalyze.Io.Sql.Tables
{
    public class GlobalState : Model.GlobalState
    {
        private IList<string> conferences;
        private IList<string> meets;
        private IList<string[]> venues;
        
        override public IList<string> Conferences
        {
            get
            {
                if(conferences == null)
                {
                    conferences = new List<string>(from conference in SqlConferences
                        select conference.Name);
                }
                return conferences;
            }
        }
        
        override public IList<string> Meets
        {
            get
            {
                if(meets == null)
                {
                    meets = new List<string>(from meet in SqlMeets
                        select meet.Name);
                }
                return meets;
            }
        }
        
        override public IList<string[]> Venues
        {
            get
            {
                if(venues == null)
                {
                    venues = new List<string[]>(from venue in SqlVenues
                        select new string[] {venue.Name, venue.City, venue.State});
                }
                return venues;
            }
        }
        
        public IList<Conference> SqlConferences { get; protected internal set; }
        public IList<Meet> SqlMeets { get; protected internal set; }
        public IList<Venue> SqlVenues { get; protected internal set; }

        protected internal GlobalState(IList<Model.Affiliation> affiliations,
            IList<Conference> conferences, IList<string> conferenceNames,
            IList<Meet> meets, IList<string> meetNames,
            IList<Model.Performance> performances, IList<Model.Race> races,
            IList<Model.Runner> runners, IList<Model.School> schools,
            IList<Venue> venues, IList<string[]> venueNames)
            : base(affiliations, conferenceNames, meetNames, performances,
                races, runners, schools, venueNames)
        {
            SqlConferences = conferences;
            SqlMeets = meets;
            SqlVenues = venues;
        }

        public static GlobalState NewInstance (
            IList<Model.Affiliation> affiliations,
            IList<Conference> conferences, IList<Meet> meets,
            IList<Model.Performance> performances, IList<Model.Race> races,
            IList<Model.Runner> runners, IList<Model.School> schools,
            IList<Venue> venues)
        {
            IList<string> conferenceNames = new List<string> ();
            IList<string> meetNames = new List<string> ();
            IList<string> venueNames = new List<string> ();
            IList<string[]> venueInfo = new List<string[]> ();
            foreach (Conference conference in conferences)
            {
                if (!conferenceNames.Contains (conference.Name))
                {
                    conferenceNames.Add (conference.Name);
                }
            }
            foreach (Meet meet in meets)
            {
                if (!meetNames.Contains (meet.Name))
                {
                    meetNames.Add (meet.Name);
                }
            }
            foreach (Venue venue in venues)
            {
                if (!venueNames.Contains (venue.Name))
                {
                    venueNames.Add (venue.Name);
                    venueInfo.Add (new string[] { venue.Name, venue.City, venue.State });
                }
            }
            return new GlobalState (affiliations, conferences, conferenceNames,
                meets, meetNames, performances, races, runners, schools, venues,
            venueInfo);
        }
    }
}
