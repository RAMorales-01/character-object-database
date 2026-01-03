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
                        case 1: SetRaceParameters(insertCommand, id, "Human", "+5 on initiative");
                        break;

                        case 2: SetRaceParameters(insertCommand, id, "Eleven", "+15 healing power");
                        break;

                        case 3: SetRaceParameters(insertCommand, id, "Fiendblood", "+10 fire resistance");
                        break;

                        case 4: SetRaceParameters(insertCommand, id, "Beastfolk", "+15 on evasion");
                        break;
                    }

                    insertCommand.ExecuteNonQuery();
                }
                
            }
        }

        ///<summary>
        ///Inserts each value in the Races table
        ///</summary>
        private static void SetRaceParameters(SqliteCommand insertCommand, int id, string race, string trait)
        {
            insertCommand.Parameters.AddWithValue("@id", id);
            insertCommand.Parameters.AddWithValue("@race", race);
            insertCommand.Parameters.AddWithValue("@trait", trait);
        }

        ///<summary>
        ///First checks if the table for jobs is already filled, if not proceeds to insert each row in table 
        ///the values for each column.
        ///</summary>
        ///<param name="connection">open connection from method VerifyDatabaseIsCreated</param>
        private static void AddJobsToTable(SqliteConnection connection)
        {
            using(SqliteCommand checkCommand = connection.CreateCommand())
            {
                checkCommand.CommandText = @"SELECT COUNT(*) FROM Jobs";

                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0){ return; }
            }

            int[] jobsId = { 1, 2, 3, 4, 5 };

            foreach(int id in jobsId)
            {
                using(SqliteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = @"INSERT INTO Jobs (Id, Job, Ability, Skill1, Skill2, Skill3) 
                    VALUES (@id, @job, @ability, s1, s2, s3)";

                    switch(id)
                    {
                        case 1: SetJobParameters(insertCommand, id, "Fighter", "Parry", "Multi-Slice", "Withstand Deathblow", "Deflect Missile");
                        break;

                        case 2: SetJobParameters(insertCommand, id, "Rouge", "Pilfer", "Deathblow", "Disarm", "Evasion");
                        break;

                        case 3: SetJobParameters(insertCommand, id, "Spellcaster", "Fireball", "Quick-Chanter", "Unshakable Caster", "Meteor-Strike");
                        break;

                        case 4: SetJobParameters(insertCommand, id, "Priest", "Heal", "Protect", "Holy-Smite", "Resurrection");
                        break;

                        case 5: SetJobParameters(insertCommand, id, "Bard", "Inspire", "Thunder-Strike", "Song of Bravery", "Charm");
                        break;
                    }

                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        ///<summary>
        ///Inserts each value in the Jobs table
        ///</summary>
        private static void SetJobParameters(SqliteCommand insertCommand, int id, string job, string ability, string s1, string s2, string s3)
        {
            insertCommand.Parameters.AddWithValue("@id", id);
            insertCommand.Parameters.AddWithValue("@job", job);
            insertCommand.Parameters.AddWithValue("@ability", ability);
            insertCommand.Parameters.AddWithValue("@s1", s1);
            insertCommand.Parameters.AddWithValue("@s2", s2);
            insertCommand.Parameters.AddWithValue("@s3", s3);
        }
    }
}