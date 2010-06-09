using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class Meet
    {
        public string City { get { return Venue.City; } }
       
        /// <summary>
        /// The date on which this meet was held.
        /// </summary>
        virtual public Date Date { get; protected internal set; }
        
        /// <summary>
        /// The distance of the men's race.
        /// </summary>
        public int MensDistance { get { return MensRace.Distance; } }
        
        /// <summary>
        /// This meet's men's race.
        /// </summary>
        virtual public Race MensRace { get; protected internal set; }
        
        public string State { get { return Venue.State; } }
        
        /// <summary>
        /// The name of the meet.
        /// </summary>
        virtual public string Name { get; protected internal set; }
        
        /// <summary>
        /// The venue whereat this meet was held.
        /// </summary>
        virtual public Venue Venue { get; protected internal set; }
        
        /// <summary>
        /// The length of the women's race.
        /// </summary>
        public int WomensDistance { get { return WomensRace.Distance; } }
        
        /// <summary>
        /// This meet's women's race.
        /// </summary>
        virtual public Race WomensRace { get; protected internal set; }
        
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="mensRace">
        /// The men's <see cref="Race"/>.
        /// </param>
        /// <param name="womensRace">
        /// The women's <see cref="Race"/>.
        /// </param>
        protected internal Meet (string name, Date date, Venue venue,
            Race mensRace, Race womensRace)
        {
            Name = name;
            Date = date;
            Venue = venue;
            MensRace = mensRace;
            WomensRace = womensRace;
            if(mensRace != null)
            {
                mensRace.Meet = this;
            }
            if(womensRace != null)
            {
                womensRace.Meet = this;
            }
        }
        
        public Meet NewInstance (string name, Date date, Venue venue,
            Race mensRace, Race womensRace)
        {
            if (mensRace != null && mensRace.Gender.IsFemale)
            {
                throw new InvalidMeetException ("The men's race is a women's race.");
            }
            if (womensRace != null && womensRace.Gender.IsMale)
            {
                throw new InvalidMeetException ("The women's race is a men's race.");
            }
            return new Meet (name, date, venue, mensRace, womensRace);
        }
        
        override public string ToString()
        {
            return Name + " (" + Date + ")";
        }
    }
    
    public class InvalidMeetException : Exception
    {
        public InvalidMeetException(string message) : base(message) {}
    }
}
