using System;

namespace XCAnalyze.Model
{
    /// <summary>
    /// All the information about a runner.
    /// </summary>
    public interface IRunner
    {
        #region Properties

        /// <summary>
        /// The runner's given or Christian name.
        /// </summary>
        string GivenName { get; set; }

        /// <summary>
        /// The runner's surname.
        /// </summary>
        string Surname { get; set; }

        #endregion
    }
    
    public static class IRunnerExtensions
    {
        public static string FullName(this IRunner self)
        {
            return string.Format("{0} {1}", self.GivenName, self.Surname);
        }
    }
}

