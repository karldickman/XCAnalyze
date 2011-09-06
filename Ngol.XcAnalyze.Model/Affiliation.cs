using System;
using System.Collections.Generic;

namespace XCAnalyze.Model
{
    /// <summary>
    /// Describes in which Year a Runner ran for a particular Team.
    /// </summary>
    public class Affiliation
    {
        #region Properties
        
        #region Constants
       
        /// <summary>
        /// The <see cref="Runner"/> affiliated with a Team.
        /// </summary>
        public readonly Runner Runner;
        
        /// <summary>
        /// The Year in which the Runner was affiliated with the Team.
        /// </summary>
        public readonly int Season;
        
        #endregion
        
        /// <summary>
        /// True if the affiliation is stored in the database, false otherwise.
        /// </summary>
        public bool IsAttached { get; set; }
        
        /// <summary>
        /// The number that identifies the runner in the affiliation.
        /// </summary>
        public int RunnerID
        {
            get { return Runner.ID; }
        }
        
        /// <summary>
        /// The number that identifies the team in the affiliation.
        /// </summary>
        public int TeamID
        {
            get { return Team.ID; }
        }
        
        /// <summary>
        /// The <see cref="Team"/> with which a Runner is affiliated.
        /// </summary>
        public Team Team { get; set; }
        
        #endregion
        
        #region Constructors
        
        /// <summary>
        /// Create a new affiliation.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who is affiliated with the team.
        /// </param>
        /// <param name="team">
        /// The <see cref="Team"/> the runner ran for.
        /// </param>
        /// <param name="year">
        /// The season in which the affiliation occurred.
        /// </param>
        public Affiliation (Runner runner, Team team, int season)
        {
            if (runner == null) {
                throw new ArgumentNullException (
                    "Property Runner cannot be null.");
            }
            Runner = runner;
            if (team == null) {
                throw new ArgumentNullException (
                    "Property Team cannot be null.");
            }
            Team = team;
            Season = season;
            IsAttached = false;
        }
        
        /// <summary>
        /// Create a new affiliation.
        /// </summary>
        /// <param name="runner">
        /// The <see cref="Runner"/> who is affiliated with the team.
        /// </param>
        /// <param name="team">
        /// The <see cref="Team"/> the runner ran for.
        /// </param>
        /// <param name="year">
        /// The season in which the affiliation occurred.
        /// </param>
        public static Affiliation NewEntity (Runner runner, Team team,
            int season)
        {
            Affiliation newAffiliation = new Affiliation (runner, team, season);
            newAffiliation.IsAttached = true;
            return newAffiliation;
        }
        
        #endregion
        
        #region Inherited methods
        
        override public bool Equals (object other)
        {
            if (this == other) 
            {
                return true;
            }
            if (other is Affiliation) 
            {
                return Equals((Affiliation)other);
            }
            return false;
        }
        
        protected bool Equals (Affiliation other)
        {
            return Team.Equals(other.Team) && Runner.Equals(other.Runner) &&
                    Season == other.Season;
        }
        
        override public int GetHashCode ()
        {
            return ToString().GetHashCode ();
        }
        
        override public string ToString ()
        {
            return Runner.Name + ", " + Team.Name + " " + Season;
        }
        
        #endregion
    }
}