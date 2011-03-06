using System;
using System.Collections.Generic;
using XCAnalyze.Model;

namespace XCAnalyze.Data
{
#if DEBUG
    public static class SampleData
    {
        #region Runners
        
        public static readonly IList<IRunner> Runners;
        
        public static readonly IRunner Karl = new PersistentRunner(1, "Dickman", "Karl");//, Gender.Male, 2010);
        public static readonly IRunner Hannah = new PersistentRunner(2, "Palmer", "Hannah");//, Gender.Female, 2010);
        public static readonly IRunner Richie = new PersistentRunner(3, "LeDonne", "Richie");//, Gender.Male, 2011);
        public static readonly IRunner Keith = new PersistentRunner(4, "Woodard", "Keith");//, Gender.Male, null);
        public static readonly IRunner Leo = new PersistentRunner(5, "Castillo", "Leo");//, Gender.Male, 2012);
        public static readonly IRunner Francis = new PersistentRunner(6, "Reynolds", "Francis");//, Gender.Male, 2010);
        public static readonly IRunner Florian = new PersistentRunner(7, "Scheulen", "Florian");//, Gender.Male, 2010);
        public static readonly IRunner Jackson = new PersistentRunner(8, "Brainerd", "Jackson");//, Gender.Male, 2012);

        #endregion
        
        static SampleData()
        {
            #region Runners
            Runners = new List<IRunner>();
            Runners.Add(Karl);
            Runners.Add(Hannah);
            Runners.Add(Richie);
            Runners.Add(Keith);
            Runners.Add(Leo);
            Runners.Add(Francis);
            Runners.Add(Florian);
            Runners.Add(Jackson);
            #endregion
        }
    }
#endif
}

