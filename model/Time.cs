namespace XcAnalyze.Model
{
    public class Time
    {
        private double seconds;

        public double Seconds {
            get { return seconds; }
        }

        public Time (double seconds)
        {
            this.seconds = seconds;
        }

        public override string ToString ()
        {
            int minutes = ((int)this.seconds) / 60;
            double seconds = this.seconds - minutes * 60;
            return string.Format ("{0}:{1:00.00}", minutes, seconds);
        }
    }
}