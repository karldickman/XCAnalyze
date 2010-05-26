using MySql.Data.MySqlClient;
using System;
using System.Data;
using XcAnalyze.Hytek;
using XcAnalyze.Io.Sql;
using XcAnalyze.Model;

namespace XcAnalyze.Cli
{
    public class ShowRace
    {
        public static void Main (string[] args)
        {
            IDbConnection connection;
            int raceId;
            string connectionString;
            if (args.Length != 1)
            {
                Console.WriteLine ("Error, must have exactly one arg.");
                Environment.Exit (1);
            }
            connectionString = "Server=localhost;";
            connectionString += "Database=xcanalyze;";
            connectionString += "User ID=xcanalyze;";
            connectionString += "Pooling=false;";
            connection = new MySqlConnection (connectionString);
            connection.Open ();
            raceId = int.Parse (args[0]);
            DatabaseReader reader = new DatabaseReader (connection);
            Data data = reader.Read ();
            Race race = data.GetRace (raceId);
            race.Score ();
            RaceFormatter formatter = new RaceFormatter ();
            foreach(string line in formatter.Format (race))
            {
                Console.WriteLine(line);
            }
        }
    }
}
