using System;

namespace XCAnalyze.Model
{
    public class Date
    {
        protected internal DateTime TheDate { get; set; }
        
        public int Year
        {
            get { return TheDate.Year; }
        }
        
        public int Month
        {
            get { return TheDate.Month; }
        }
        
        public int Day
        {
            get { return TheDate.Day; }
        }
        
        public Date (int year, int month, int day) : this(new DateTime(year, month, day)) {}

        public Date (DateTime date)
        {
            TheDate = date;
        }
        
        public override string ToString ()
        {
            return Year + "/" + Month + "/" + Day;
        }
    }
}
