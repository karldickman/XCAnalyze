using System;

namespace XcAnalyze.Model
{

    public class Date
    {
        private DateTime date;
        
        public int Year
        {
            get { return date.Year; }
        }
        
        public int Month
        {
            get { return date.Month; }
        }
        
        public int Day
        {
            get { return date.Day; }
        }
        
        public Date (int year, int month, int day) : this(new DateTime(year, month, day)) {}

        public Date (DateTime date)
        {
            this.date = date;
        }
        
        public override string ToString ()
        {
            return Year + "/" + Month + "/" + Day;
        }
    }
}
