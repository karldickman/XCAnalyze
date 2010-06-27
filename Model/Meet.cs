using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class Meet
    {
        /// <summary>
        /// Create a new meet.
        /// </summary>
        /// <param name="name">
        /// The name of the meet.
        /// </param>
        /// <param name="date">
        /// The date on which the meet was held.
        /// </param>
        /// <param name="venue">
        /// The venue whereat the meet was held.
        /// </param>
        /// <param name="mensRace">
        /// The men's <see cref="Race"/>.
        /// </param>
        /// <param name="womensRace">
        /// The women's <see cref="Race"/>.
        /// </param>
        /// <exception>
        /// A <see cref="ArgumentNullException"/> is thrown when both men's and
        /// women's races are null.
        /// </exception>
        public Meet (string name, DateTime date, Venue location, Race mensRace,
            Race womensRace)
        {
            Name = name;
            Date = date;
            Location = location;
            MensRace = mensRace;
            WomensRace = womensRace;
            if (mensRace != null)
            {
                mensRace.Meet = this;
            }
            if (womensRace != null)
            {
                womensRace.Meet = this;
            }
            if (mensRace == null && womensRace == null) {
                throw new ArgumentNullException (
                    "A meet must have either a men's race or a women's race.");
            }
        }
        
        /// <summary>
        /// The city where the meet was held.
        /// </summary>
        public string City { get { return Location.City; } }
       
        /// <summary>
        /// The date on which this meet was held.
        /// </summary>
        public DateTime Date { get; protected set; }
        
        /// <summary>
        /// The distance of the men's race.
        /// </summary>
        public int? MensDistance
        {
            get
            {
                if (MensRace == null)
                {
                    return null;
                }
                return MensRace.Distance;
            }
        }
        
        /// <summary>
        /// This meet's men's race.
        /// </summary>
        public Race MensRace { get; protected set; }
        
        /// <summary>
        /// The state in which the meet was held.
        /// </summary>
        public string State { get { return Location.State; } }
        
        /// <summary>
        /// The name of the meet.
        /// </summary>
        public string Name { get; protected set; }
        
        /// <summary>
        /// The venue whereat this meet was held.
        /// </summary>
        public Venue Location { get; protected set; }
        
        /// <summary>
        /// The length of the women's race.
        /// </summary>
        public int? WomensDistance
        {
            get
            {
                if (WomensRace == null)
                {
                    return null;
                }
                return WomensRace.Distance;
            }
        }
        
        /// <summary>
        /// This meet's women's race.
        /// </summary>
        public Race WomensRace { get; protected set; }
        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Meet)
            {
                return Equals((Meet)other);
            }
            return false;
        }
        
        /// <summary>
        /// If two meets have the same date and same name, they are the same
        /// meet.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Meet"/> with which to compare.
        /// </param>
        protected bool Equals (Meet other)
        {
            return Date.Year == other.Date.Year &&
                Date.Month == other.Date.Month &&
                Date.Day == other.Date.Day &&
                (Name == other.Name ||
                    Name != null && Name.Equals(other.Name));
        }
        
        override public int GetHashCode()
        {
            return ("" + Date + Name + Location).GetHashCode();
        }
        
        public Race Race (Gender gender)
        {
            if (gender.IsMale) 
            {
                return MensRace;
            }
            return WomensRace;
        }
    }
}
