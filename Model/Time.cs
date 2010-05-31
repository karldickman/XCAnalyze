namespace XCAnalyze.Model
{
    public class Time
    {
        public double Seconds { get; protected internal set; }

        public Time (double seconds)
        {
            Seconds = seconds;
        }

        override public string ToString ()
        {
            int minutes = ((int)Seconds) / 60;
            double seconds = Seconds - minutes * 60;
            return string.Format ("{0}:{1:00.00}", minutes, seconds);
        }
    }
}