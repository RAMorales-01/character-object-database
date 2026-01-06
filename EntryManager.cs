using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using UserHandler;

namespace DatabaseUtility
{
    class DatabaseHandler
    {
        private static readonly string _databasePathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "characterPool.db");
        private static readonly string _connection = $"Data Source={_databasePathFile}; Foreign Keys=True;";

        #region Database Initial Verification
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

                //The next methods seed the values for each table(if they aren't filled already).
                AddRacesToTable(connection);
                AddJobsToTable(connection);
            }
        }
        #endregion

        #region Populate Race and Job Tables
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

            int[] raceId = { 1, 2, 3, 4 };//This array contains the id for the current available races. 

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
            using(SqliteCommand checkCommand = connection.CreateCommand())//checks if the table is already filled, if it is then returns.
            {
                checkCommand.CommandText = @"SELECT COUNT(*) FROM Jobs";

                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0){ return; }
            }

            int[] jobsId = { 1, 2, 3, 4, 5 };//This array contains the id for the current available jobs. 

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

                        case 2: SetJobParameters(insertCommand, id, "Rogue", "Pilfer", "Deathblow", "Disarm", "Evasion");
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
        #endregion 

        #region Create, Insert or Delete Entries in Database
        ///<summary>
        ///To create an instance of the Character class after all the parameters have been confirmed by user.
        ///</summary>
        ///<returns>an instance of the Character class</returns>
        public static Character CreateCharacter()
        {
            while(true)
            {
                int points = 30;//each new character have a total of 30 points to distribute between the 6 main stats.
                
                string name = UserHandler.AddName("Name: ");
                int choosenRaceId = UserHandler.ChooseRace("\nSelect a race: ", name);
                int choosenJobId = UserHandler.ChooseJob("\nSelect a Job: ", name);

                int strength = UserHandler.AddStatValue("Add points: ", "Strength", Character._minStatValue, Character._maxStatValue, ref points);
                int constitution = UserHandler.AddStatValue("Add points: ", "Constitution", Character._minStatValue, Character._maxStatValue, ref points);
                int dexterity = UserHandler.AddStatValue("Add points: ", "Dexterity", Character._minStatValue, Character._maxStatValue, ref points);
                int intelligence = UserHandler.AddStatValue("Add points: ", "Intelligence", Character._minStatValue, Character._maxStatValue, ref points);
                int wisdom = UserHandler.AddStatValue("Add points: ", "Wisdom", Character._minStatValue, Character._maxStatValue, ref points);
                int charisma = UserHandler.AddStatValue("Add points: ", "Charisma", Character._minStatValue, Character._maxStatValue, ref points);

                //before creating the instance of the character class the user will confirm the created character is correct.
                bool proceed = UserHandler.CharacterConfirmation(name, choosenRaceId, choosenJobId, strength, constitution, dexterity, intelligence, wisdom, charisma);

                if(proceed == true)
                {
                    Character character = new Character(name, strength, constitution, dexterity, intelligence, wisdom, charisma);
                    AssignRaceToCharacter(character, choosenRaceId);
                    AssignJobToCharacter(character, choosenJobId);
                    
                    return character;
                }
            }
        }

        ///<summary>
        ///Insert the character object to the database, along with all stats, race and job information
        ///</summary>
        ///<param name="character">current character object to be inserted to the database</param>
        public static void InsertCharacterToDatabase(Character character)
        {
            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                using(SqliteCommand addCharacterCommand = connection.CreateCommand())
                {
                    addCharacterCommand.CommandText = @"INSERT INTO Characters (Name, Strength, Constitution, Dexterity, Intelligence, Wisdom, Charisma,
                    RaceId, Race, Trait, 
                    JobId, Job, Ability, Skill1, Skill2, Skill3)
                    VALUES (@name, @strength, @constitution, @dexterity, @intelligence, @wisdom, @charaisma
                    @raceId, @race, @trait,
                    @jobId, @job, @ability, @s1, @s2, @s3)";
                    addCharacterCommand.Parameters.AddWithValue("@name", character.Name);
                    addCharacterCommand.Parameters.AddWithValue("@strength", character.Strength);
                    addCharacterCommand.Parameters.AddWithValue("@constitution", character.Constitution);
                    addCharacterCommand.Parameters.AddWithValue("@dexterity", character.Dexterity);
                    addCharacterCommand.Parameters.AddWithValue("@intelligence", character.Intelligence);
                    addCharacterCommand.Parameters.AddWithValue("@wisdom", character.Wisdom);
                    addCharacterCommand.Parameters.AddWithValue("@charisma", character.Charisma);
                    addCharacterCommand.Parameters.AddWithValue("@raceId", character.RaceId);
                    addCharacterCommand.Parameters.AddWithValue("@race", character.AssignedRace.RaceName);
                    addCharacterCommand.Parameters.AddWithValue("@trait", character.AssignedRace.RaceTrait);
                    addCharacterCommand.Parameters.AddWithValue("@jobId", character.JobId);
                    addCharacterCommand.Parameters.AddWithValue("@job", character.AssignedJob.JobName);
                    addCharacterCommand.Parameters.AddWithValue("@job", character.AssignedJob.JobAbility);
                    addCharacterCommand.Parameters.AddWithValue("@job", character.AssignedJob.Skill1);
                    addCharacterCommand.Parameters.AddWithValue("@job", character.AssignedJob.Skill2);
                    addCharacterCommand.Parameters.AddWithValue("@job", character.AssignedJob.Skill3);
                    addCharacterCommand.ExecuteNonQuery();
                }
            }
        }

        ///<summary>
        ///To delete an entry inside the database using the id(primary key)
        ///</summary>
        ///<param name="characterId">the primary key of each entry inside the database</param>
        public static void DeleteCharacter(int characterId)
        {
            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand deleteCharacterCommand = connection.CreateCommand();
                deleteCharacterCommand.CommandText = @"DELETE FROM Characters WHERE id = @id";
                deleteCharacterCommand.Parameters.AddWithValue("@id", characterId);
                deleteCharacterCommand.ExecuteNonQuery();
            }
        }
        #endregion

        #region Race and Job Setter Helper Methods
        ///<summary>
        ///Helper method to assign the choosen race to the character instance.
        ///</summary>
        ///<param name="character">instance of the character created</param>
        ///<param name="raceId">a representation of the selected id to assign the race</param>
        private static void AssignRaceToCharacter(Character character, int raceId)
        {
            switch(raceId)
            {
                case 1: character.SetRace(new Race.Human(character));
                break;

                case 2: character.SetRace(new Race.Elven(character));
                break;

                case 3: character.SetRace(new Race.Fiendblood(character));
                break;

                case 4: character.SetRace(new Race.Beastfolk(character));
                break;

                default: Console.WriteLine("\nERROR: Invalid input selected option does not exist.\n");
                break;
            }
        }

        ///<summary>
        ///Helper method to assign the choosen job to the character instance.
        ///</summary>
        ///<param name="character">instance of the character created</param>
        ///<param name="jobId">a representation of the selected id to assign the job</param>
        private static void AssignJobToCharacter(Character character, int jobId)
        {
            switch(jobId)
            {
                case 1: character.SetJob(new Vocation.Fighter(character));
                break;

                case 2: character.SetJob(new Vocation.Rouge(character));
                break;

                case 3: character.SetJob(new Vocation.Sorcerer(character));
                break;

                case 4: character.SetJob(new Vocation.Healer(character));
                break;

                case 5: character.SetJob(new Vocation.Bard(character));
                break;

                default: Console.WriteLine("\nERROR: Invalid input selected option does not exist.\n");
                break;
            }
        }
        #endregion
    }
}