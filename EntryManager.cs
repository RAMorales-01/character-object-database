using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace DatabaseUtility
{
    class DatabaseHandler
    {
        private static readonly string _databasePathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "characters.db");
        private static readonly string _connection = $"Data Source={_databasePathFile}; Foreign Keys=True;";

        ///<summary>
        ///Ensures the database and folder exist
        ///</summary>
        public static void VerifyDatabaseIsCreated()
        {
            string folder = Path.GetDirectoryName(_databasePathFile);

            if(!string.IsNullOrEmpty(folder)){ Directory.CreateDirectory(folder) }

            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                using(SqliteCommand createCommand = connection.CreateCommand())
                {
                    createCommand.CommandText = @"PRAGMA foreign_keys = ON";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXIST Races (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Race TEXT NOT NULL, Trait TEXT NOT NULL)";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXIST Jobs (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Job TEXT NOT NULL, Ability TEXT NOT NULL, Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, Skill3 TEXT NOT NULL)";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXIST Characters (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    RaceId INTEGER, JobId INTEGER,
                    Name TEXT NOT NULL,
                    Strength INTEGER NOT NULL,
                    Constitution INTEGER NOT NULL,
                    Dexterity INTEGER NOT NULL,
                    Intelligence INTEGER NOT NULL,
                    Wisdom INTEGER NOT NULL,
                    Charisma INTEGER NOT NULL,
                    Race TEXT NOT NULL,
                    Trait TEXT NOT NULL,
                    Job TEXT NOT NULL,
                    Ability TEXT NOT NULL,
                    Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, Skill3 TEXT NOT NULL,
                    FOREIGN KEY (RaceId) REFERENCES Races(id) ON DELETE SET NULL, 
                    FOREIGN KEY (JobId) REFERENCES Jobs(id) ON DELETE SET NULL);";
                    createCommand.ExecuteNonQuery();
                }

                //TODO: Method to "seed" Race table and Job table
            }
        }

        ///<summary>
        ///First checks if the table for races is already filled, if not proceeds to insert each row in table 
        ///the values for each column.
        ///</summary>
        ///<param name="connection">open connection from method VerifyDatabaseIsCreated</param>
        private static void AddRacesToTable(SqliteConnection connection)
        {
            using(SqliteCommand checkCommand = connection.CreateCommand())//checks if the table is already filled, if it is then returns.
            {
                checkCommand.CommandText = @"SELECT COUNT(*) FROM Races";

                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0){ return; }
            }

            int[] raceId = { 1, 2, 3, 4 };//This array contains the id for the current available races 

            foreach(int id in raceId)
            {
                using(SqliteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = @"INSERT INTO Races (Id, Race, Trait) VALUES (@id, @race, @trait)";

                    switch(id)
                    {
                        case 1: SetRaceParameters(command, id, "Human", "+5 on initiative");
                        break;

                        case 2: SetRaceParameters(command, id, "Eleven", "+15 healing power");
                        break;

                        case 3: SetRaceParameters(command, id, "Fiendblood", "+10 fire resistance");
                        break;

                        case 4: SetRaceParameters(command, id, "Beastfolk", "+15 on evasion");
                        break;
                    }

                    insertCommand.ExecuteNonQuery();
                }
                
            }
        }

        ///<summary>
        ///Inserts each value in the Races table
        ///</summary>
        private static void SetRaceParameters(SqliteCommand command, int id, string race, string trait)
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@race", race);
            command.Parameters.AddWithValue("@trait", trait);
        }
    }
}