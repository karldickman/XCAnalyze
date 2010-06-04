namespace XCAnalyze.Model
{
    /// <summary>
    /// The time it took to run a race.
    /// </summary>
    public class Time
    {
        public double Seconds { get; protected internal set; }

        /// <summary>
        /// Create a new time instance.
        /// </summary>
        /// <param name="seconds">
        /// The number of seconds it took to run the race.
        /// </param>
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