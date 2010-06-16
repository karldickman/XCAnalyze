using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A meet has a mens race and a womens race and occurs at a particular time.
    /// </summary>
    public class Meet : IComparable<Meet>
    {
        public string City { get { return Venue.City; } }
       
        /// <summary>
        /// The date on which this meet was held.
        /// </summary>
        public Date Date { get; protected internal set; }
        
        /// <summary>
        /// The distance of the men's race.
        /// </summary>
        public int MensDistance { get { return MensRace.Distance; } }
        
        /// <summary>
        /// This meet's men's race.
        /// </summary>
        public Race MensRace { get; protected internal set; }
        
        public string State { get { return Venue.State; } }
        
        /// <summary>
        /// The name of the meet.
        /// </summary>
        public string Name { get; protected internal set; }
        
        /// <summary>
        /// The venue whereat this meet was held.
        /// </summary>
        public Venue Venue { get; protected internal set; }
        
        /// <summary>
        /// The length of the women's race.
        /// </summary>
        public int WomensDistance { get { return WomensRace.Distance; } }
        
        /// <summary>
        /// This meet's women's race.
        /// </summary>
        public Race WomensRace { get; protected internal set; }
        
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
        public Meet (string name, Date date, Venue venue,
            Race mensRace, Race womensRace)
        {
            Name = name;
            Date = date;
            Venue = venue;
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
            if (mensRace == null && womensRace == null)
            {
                throw new ArgumentNullException (
                    "A meet must have either a men's race or a women's race.");
            }
        }
        
        /// <summary>
        /// Meets are compared first by date, then by name.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Meet"/> with which to compare.
        /// </param>
        public int CompareTo (Meet other)
        {
            int comparison;
            if (this == other)
            {
                return 0;
            }
            comparison = Date.CompareTo (other.Date);
            if (comparison != 0) 
            {
                return comparison;
            }
            return ObjectComparer<string>.Compare(Name, other.Name, 1);
        }
        
        override public bool Equals(object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Meet)
            {
                return 0 == CompareTo((Meet)other);
            }
            return false;
        }
        
        override public int GetHashCode()
        {
            return ("" + Date + Name + Venue).GetHashCode();
        }
        
        override public string ToString()
        {
            return Name + " (" + Date + ")";
        }
    }
}
