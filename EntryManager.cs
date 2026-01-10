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
                    Name TEXT NOT NULL, Trait TEXT NOT NULL)";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXIST Jobs (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name TEXT NOT NULL, Ability TEXT NOT NULL, Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, Skill3 TEXT NOT NULL)";
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

        ///<summary>
        ///Check Characters table contains entries, if entries equals 0 returns false.
        ///</summary>
        ///<returns>bool, false if table has 0 entries else returns true</returns> 
        private static bool VerifyTableBeforeOperation()
        {
            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                using(SqliteCommand checkCommand = connection.CreateCommand())
                {
                    checkCommand.CommandText = @"SELECT COUNT(*) FROM Characters";

                    long count = (long)checkCommand.ExecuteScalar();

                    if(count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
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
                    insertCommand.CommandText = @"INSERT INTO Races (Id, Name, Trait) VALUES (@id, @name, @trait)";

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
            insertCommand.Parameters.AddWithValue("@name", race);
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
                    insertCommand.CommandText = @"INSERT INTO Jobs (Id, Name, Ability, Skill1, Skill2, Skill3) 
                    VALUES (@id, @name, @ability, s1, s2, s3)";

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
            insertCommand.Parameters.AddWithValue("@name", job);
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
                int choosenRaceId = UserHandler.SelectRaceAndJob("race", name);
                int choosenJobId = UserHandler.SelectRaceAndJob("job", name);

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
        private static void DeleteCharacterFromDatabase(int characterId)
        {
            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                SqliteCommand deleteCharacterCommand = connection.CreateCommand();
                deleteCharacterCommand.CommandText = @"DELETE FROM Characters WHERE id = @id";
                deleteCharacterCommand.Parameters.AddWithValue("@id", characterId);
                deleteCharacterCommand.ExecuteNonQuery();
            }
        }

        ///<summary>
        ///Main entry point for the deletion process of an existing entry on the Characters table.
        ///</summary>
        public static void DeleteAnEntryVerification()
        {
            var isValid = VerifyTableBeforeOperation();

            if(isValid == false)
            {
                Console.WriteLine("\nThere are currently no character.\n");
            }
            else
            {
                DisplayCharacterTable(GetIdAndName("characters"));
                var (idExist, selectedId) = UserHandler.IsSelectedIdValid("Delete: ");

                if(idExist == true)
                {
                    DeleteCharacterFromDatabase(selectedId);
                }
                else
                {
                    Console.WriteLine($"ERROR: selected id {selectedId} does not belong to any existing character. Press any key to try again.");
                    Console.ReadKey();
                }    
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

        #region Search, Retrieve and Display 
        ///<summary>
        ///Retreives id and name of selected table.
        ///</summary>
        ///<returns>A dictionary with the id and name of all the current character in the database</returns>
        public static Dictionary<int, string> GetIdAndName(string selectedTable)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            string tableName = selectedTable.ToLower() switch
            {
                "characters" => "Characters",
                "races" => "Races",
                "jobs" => "Jobs",
                _ => throw new ArgumentException("Invalid table selection.")
            };

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retrieveCommand = connection.CreateCommand();
                retrieveCommand.CommandText = $"SELECT Id, Name FROM {tableName};";

                using(SqliteDataReader reader = retrieveCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        
                        dictionary.Add(id, name);
                    }
                }
            }

            return dictionary;
        }

        ///<summary>
        ///Using the primary key, retrieves the 6 main stat values to display on the character sheet.
        ///</summary>
        ///<param name="characterId">primary key of each existing entry on the table Characters</param>
        ///<returns>a List of Tuple, string for the column name(stat name) and int for the value of each column</returns> 
        private static List<Tuple<string, string>> GetStatsFromId(int characterId)
        {
            List<Tuple<string, string>> characterStats = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retrieveStatsCommand = connection.CreateCommand();
                retrieveStatsCommand.CommandText = @"SELECT Strength, Constitution, Dexterity, Intelligence, Wisdom,
                Charisma FROM Characters WHERE id = @id";
                retrieveStatsCommand.Parameters.AddWithValue("@id", characterId);
                
                using(SqliteDataReader reader = retrieveStatsCommand.ExecuteReader())
                {
                   while(reader.Read())
                   {
                        for(int i = 0; i < reader.FieldCount; i++)//FieldCount gets the number of columns in the current row.
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = Convert.ToString(reader.GetValue(i));

                            characterStats.Add(new Tuple<string, string>(columnName, columnValue));
                        }
                   }
                }
            }

            return characterStats;
        }

        ///<summary>
        ///Using primary key retrives the race information assigned to an existing entry on the Characters table.  
        ///</summary>
        ///<param name="characterId">primary key of each existing entry on the table Characters</param>
        ///<returns>a List of Tuple string for the column name(in this case is "Race") and string for the value inside the column</returns>
        private static List<Tuple<string, string>> GetRaceFromId(int characterId)
        {
            List<Tuple<string, string>> characterRaceInfo = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retrieveRace = connection.CreateCommand();
                retrieveRace.CommandText = @"SELECT Race, Trait FROM Characters WHERE id = @id";
                retrieveRace.Parameters.AddWithValue("@id", characterId);

                using(SqliteDataReader reader = retrieveRace.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);//gets name of column --> Race
                            string columnValue = reader.GetString(i);//gets value inside the column Race(for example "Human")

                            characterRaceInfo.Add(new Tuple<string, string>(columnName, columnValue));
                        }
                    }
                }
            }

            return characterRaceInfo;
        }

        ///<summary>
        ///Using primary key retrives the job information assigned to an existing entry on the Characters table.
        ///</summary>
        ///<param name="characterId">primary key of each existing entry on the table Characters</param>
        ///<returns>a List of Tuple string for the column name("Job name", "Ability", "Skill1" and so on) and string for the value inside the column</returns>
        private static List<Tuple<string, string>> GetJobFromId(int characterId)
        {
            List<Tuple<string, string>> characterJobInfo = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retrieveJob = connection.CreateCommand();
                retrieveJob.CommandText = @"SELECT Job, Ability, Skill1, Skill2, Skill3 FROM Characters WHERE id = @id";
                retrieveJob.Parameters.AddWithValue("@id", characterId);

                using(SqliteDataReader reader = retrieveJob.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = reader.GetString(i);

                            characterJobInfo.Add(new Tuple<string, string>(ColumnName, columnValue));
                        }
                    }
                }
            }

            return characterJobInfo;
        }

        ///<summary>
        ///Display all the current existing entries in Characters table.
        ///</summary>
        ///<param name="characterList">dictionary with id(primary key) and name of character(value)</param>
        public static void DisplayCharacterTable(Dictionary<int, string> characterList)
        {
            if(characterList.Count == 0)
            {
                Console.WriteLine("\nThere are currently no characters.\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\n ----- Characters -----\n");

                foreach(KeyValuePair<int, string> kvp in characterList)
                {
                    Console.WriteLine($"ID: {kvp.Key} - Name: {kvp.Value}");
                }
            }
        }

        ///<summary>
        ///Display all the info of the Races table.
        ///</summary>
        public static void DisplayRaceTable()
        {
            Console.Clear();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using(SqliteCommand retrieveRaces = connection.CreateCommand())
                {
                    retrieveRaces.CommandText = @"SELECT id, Name, Trait FROM Races";

                    using(SqliteDataReader reader = retrieveRaces.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string trait = reader.GetString(2);

                            Console.WriteLine($"id: {id} | Race: {name} | Trait: {trait}\n");
                        }
                    }
                }
            }
        }

        ///<summary>
        ///Display all info of the Jobs table.
        ///</summary>
        public static void DisplayJobTable()
        {
            Console.Clear();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using(SqliteCommand retrieveJobs = connection.CreateCommand())
                {
                    retrieveJobs.CommandText = @"SELECT ALL id, Name, Ability, Skill1, Skill2, Skill3 FROM Jobs";

                    using(SqliteDataReader reader = retrieveJobs.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string ability = reader.GetString(2);
                            string s1 = reader.GetString(3);
                            string s2 = reader.GetString(4);
                            string s3 = reader.GetString(5);

                        Console.WriteLine($"id: {id} | Job: {name} | Ability: {ability}");
                        Console.WriteLine($"Skills: {s1} - {s2} - {s3}\n");
                        }
                    }
                }
            }
        }

        ///<summary>
        ///Display existing entry information(character name, race, job and 6 main stats).
        ///</summary>
        ///<param name="characterSheet">List of tuple, string(column name) and string(column value)</param>
        private static void DisplayCharacterSheet(List<Tuple<string, string>> characterSheet)
        {
            Console.Clear();

            foreach(var sheet in characterSheet)
            {
                Console.WriteLine($"{sheet.Item1} -- {sheet.Item2}");
            }

            Console.WriteLine("\n");
        }

        ///<summary>
        ///Main entry point for the process of viewing stats, race and job info of an existing Character entry.
        ///</summary>
        public static void ViewAnEntryVerification()
        {
            var isValid = VerifyTableBeforeOperation();

            if(isValid == false)
            {
                Console.WriteLine("\nThere are currently no character.\n");
            }
            else
            {
                DisplayCharacterTable(GetIdAndName("characters")); 
                var (idExist, selectedId) = UserHandler.IsSelectedIdValid("Select: ");  

                 if(idExist == true)
                {
                    DisplayCharacterSheet(GetStatsFromId(selectedId));
                    DisplayCharacterSheet(GetRaceFromId(selectedId));
                    DisplayCharacterSheet(GetJobFromId(selectedId));
                }
                else
                {
                    Console.WriteLine($"ERROR: selected id {selectedId} does not belong to any existing character. Press any key to try again.");
                    Console.ReadKey();
                } 
            }
        }
        #endregion
    }
}