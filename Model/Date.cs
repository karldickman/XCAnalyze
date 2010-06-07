using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// A class representing a particular day (YYYY-MM-DD), without specifying
    /// hours or anything smaller.
    /// </summary>
    public class Date : IComparable<Date>
    {
        /// <summary>
        /// The value used internally to represent the date.
        /// </summary>
        protected internal DateTime Date_ { get; set; }
        
        public int Day
        {
            get { return Date_.Day; }
        }
        
        public int Month
        {
            get { return Date_.Month; }
        }
        
        public int Year
        {
            get { return Date_.Year; }
        }
        
        public Date (int year, int month, int day)
        : this(new DateTime(year, month, day)) {}

        public Date (DateTime date)
        {
            Date_ = date;
        }
        
        public int CompareTo (Date other)
        {
            int comparison;
            if (this == other) 
            {
                return 0;
            }
            comparison = Year.CompareTo (other.Year);
            if (comparison != 0) 
            {
                return comparison;
            }
            comparison = Month.CompareTo (other.Month);
            if (comparison != 0) 
            {
                return comparison;
            }
            return Day.CompareTo (other.Day);
        }    
        
        override public bool Equals (object other)
        {
            if(this == other)
            {
                return true;
            }
            if(other is Date)
            {
                return 0 == CompareTo((Date)other);
            }
            return false;
        }
        
        override public int GetHashCode ()
        {
            return ToString().GetHashCode();
        }  
        
        override public string ToString ()
        {
            return Year + "/" + Month + "/" + Day;
        }
    }
}
