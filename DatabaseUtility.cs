using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using UserHandler;
using CharacterCreation;
using RaceSelection;
using JobSelection;

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
            string? folder = Path.GetDirectoryName(_databasePathFile);

            if(!string.IsNullOrEmpty(folder))
            { 
                Directory.CreateDirectory(folder); 
            }

            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                using(SqliteCommand createCommand = connection.CreateCommand())
                {
                    createCommand.CommandText = @"PRAGMA foreign_keys = ON";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Races (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL, Trait TEXT NOT NULL)";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Jobs (Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name TEXT NOT NULL, Ability TEXT NOT NULL, Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, Skill3 TEXT NOT NULL)";
                    createCommand.ExecuteNonQuery();

                    createCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Characters (Id INTEGER PRIMARY KEY AUTOINCREMENT,
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
        ///Checks Characters table for existing entries, if entries equals 0 returns false.
        ///</summary>
        ///<returns>bool, false if table has 0 entries else returns true</returns> 
        public static bool VerifyTableBeforeOperation()
        {
            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                using(SqliteCommand checkCommand = connection.CreateCommand())
                {
                    checkCommand.CommandText = @"SELECT COUNT(*) FROM Characters";

                    long count = (long?)checkCommand.ExecuteScalar() ?? 0;//Safely handles null and uses the proper SQLite long type

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

                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0)
                { 
                    return; 
                }
            }

            List<Race.RaceBasics> availableRaces = new List<Race.RaceBasics>
            {
                new Race.Human(),
                new Race.Elven(),
                new Race.Fiendblood(),
                new Race.Beastfolk()
            };

            foreach(var race in availableRaces)
            {
                using(SqliteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = @"INSERT INTO Races (Id, Name, Trait) VALUES (@id, @name, @trait)";

                    insertCommand.Parameters.AddWithValue("@id", race.RaceId);
                    insertCommand.Parameters.AddWithValue("@name", race.RaceName);
                    insertCommand.Parameters.AddWithValue("@trait", race.RaceTrait);           

                    insertCommand.ExecuteNonQuery();
                }
            }
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

                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0)
                { 
                    return; 
                }
            }
            
            List<Job.JobBasics> availableJobs = new List<Job.JobBasics>
            {
                new Job.Fighter(),
                new Job.Rogue(),
                new Job.Spellcaster(),
                new Job.Priest(),
                new Job.Bard(),
            };

            foreach(var job in availableJobs)
            {
                using(SqliteCommand insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = @"INSERT INTO Jobs (Id, Name, Ability, Skill1, Skill2, Skill3) 
                    VALUES (@id, @name, @ability, @s1, @s2, @s3)";

                    insertCommand.Parameters.AddWithValue("@id", job.JobId);
                    insertCommand.Parameters.AddWithValue("@name", job.JobName);
                    insertCommand.Parameters.AddWithValue("@ability", job.Ability);
                    insertCommand.Parameters.AddWithValue("@s1", job.Skill1);
                    insertCommand.Parameters.AddWithValue("@s2", job.Skill2);
                    insertCommand.Parameters.AddWithValue("@s3", job.Skill3);
                    
                    insertCommand.ExecuteNonQuery();
                }
            }
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
                
                string name = Input.AddName("Name: ");
                int choosenRaceId = Input.SelectRaceAndJob("race", name);
                int choosenJobId = Input.SelectRaceAndJob("job", name);

                int strength = Input.AddStatValue("Add points: ", "Strength", Character._minStatValue, Character._maxStatValue, ref points);
                int constitution = Input.AddStatValue("Add points: ", "Constitution", Character._minStatValue, Character._maxStatValue, ref points);
                int dexterity = Input.AddStatValue("Add points: ", "Dexterity", Character._minStatValue, Character._maxStatValue, ref points);
                int intelligence = Input.AddStatValue("Add points: ", "Intelligence", Character._minStatValue, Character._maxStatValue, ref points);
                int wisdom = Input.AddStatValue("Add points: ", "Wisdom", Character._minStatValue, Character._maxStatValue, ref points);
                int charisma = Input.AddStatValue("Add points: ", "Charisma", Character._minStatValue, Character._maxStatValue, ref points);

                //before creating the instance of the character class the user will confirm the created character is correct.
                bool proceed = Input.CharacterConfirmation(name, choosenRaceId, choosenJobId, strength, constitution, dexterity, intelligence, wisdom, charisma, ref points);

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

                using(SqliteCommand addCharacter = connection.CreateCommand())
                {
                    addCharacter.CommandText = @"INSERT INTO Characters (Name, Strength, Constitution, Dexterity, Intelligence, Wisdom, Charisma,
                    RaceId, Race, Trait, 
                    JobId, Job, Ability, Skill1, Skill2, Skill3)
                    VALUES (@name, @strength, @constitution, @dexterity, @intelligence, @wisdom, @charisma,
                    @raceId, @race, @trait,
                    @jobId, @job, @ability, @s1, @s2, @s3)";
                    addCharacter.Parameters.AddWithValue("@name", character.Name);
                    addCharacter.Parameters.AddWithValue("@strength", character.Strength);
                    addCharacter.Parameters.AddWithValue("@constitution", character.Constitution);
                    addCharacter.Parameters.AddWithValue("@dexterity", character.Dexterity);
                    addCharacter.Parameters.AddWithValue("@intelligence", character.Intelligence);
                    addCharacter.Parameters.AddWithValue("@wisdom", character.Wisdom);
                    addCharacter.Parameters.AddWithValue("@charisma", character.Charisma);
                    addCharacter.Parameters.AddWithValue("@raceId", character.RaceId);
                    addCharacter.Parameters.AddWithValue("@race", character.AssignedRace.RaceName);
                    addCharacter.Parameters.AddWithValue("@trait", character.AssignedRace.RaceTrait);
                    addCharacter.Parameters.AddWithValue("@jobId", character.JobId);
                    addCharacter.Parameters.AddWithValue("@job", character.AssignedJob.JobName);
                    addCharacter.Parameters.AddWithValue("@ability", character.AssignedJob.Ability);
                    addCharacter.Parameters.AddWithValue("@s1", character.AssignedJob.Skill1);
                    addCharacter.Parameters.AddWithValue("@s2", character.AssignedJob.Skill2);
                    addCharacter.Parameters.AddWithValue("@s3", character.AssignedJob.Skill3);

                    addCharacter.ExecuteNonQuery();
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
                var characterList = GetIdAndName("characters");
                DisplayCharacterTable(characterList);
                var (idExist, selectedId) = Input.IsSelectedIdValid("Delete id: ", characterList);

                string confirm = Input.ChoiceConfirmation($"\nWARNING: Delete character with id {selectedId}? [Y/N]: ");

                if(confirm == "yes")
                {
                    if(idExist == true)
                    {
                        DeleteCharacterFromDatabase(selectedId);
                        
                        Console.Clear();
                        Console.WriteLine("\n\nSelected character has been successfully deleted!. Press any key to go back to main menu.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: selected id {selectedId} does not belong to any existing character. Press any key to try again.");
                        Console.ReadKey();
                    }    
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
                case 1: character.SetJob(new Job.Fighter(character));
                break;

                case 2: character.SetJob(new Job.Rogue(character));
                break;

                case 3: character.SetJob(new Job.Spellcaster(character));
                break;

                case 4: character.SetJob(new Job.Priest(character));
                break;

                case 5: character.SetJob(new Job.Bard(character));
                break;

                default: Console.WriteLine("\nERROR: Invalid input selected option does not exist.\n");
                break;
            }
        }
        #endregion

        #region Search and Retrieve 
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

            using(SqliteConnection connection = new SqliteConnection(_connection))
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
        public static List<Tuple<string, string>> GetStatsFromId(int characterId)
        {
            List<Tuple<string, string>> characterStats = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                SqliteCommand retrieveStats = connection.CreateCommand();
                retrieveStats.CommandText = @"SELECT Strength, Constitution, Dexterity, Intelligence, Wisdom,
                Charisma FROM Characters WHERE id = @id";
                retrieveStats.Parameters.AddWithValue("@id", characterId);
                
                using(SqliteDataReader reader = retrieveStats.ExecuteReader())
                {
                   if(reader.Read())
                   {
                        for(int i = 0; i < reader.FieldCount; i++)//FieldCount gets the number of columns in the current row.
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = reader.GetValue(i)?.ToString() ?? string.Empty;

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
        public static List<Tuple<string, string>> GetRaceFromId(int characterId)
        {
            List<Tuple<string, string>> characterRaceInfo = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                SqliteCommand retrieveRace = connection.CreateCommand();
                retrieveRace.CommandText = @"SELECT Race, Trait FROM Characters WHERE id = @id";
                retrieveRace.Parameters.AddWithValue("@id", characterId);

                using(SqliteDataReader reader = retrieveRace.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = reader.GetValue(i)?.ToString() ?? string.Empty;

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
        public static List<Tuple<string, string>> GetJobFromId(int characterId)
        {
            List<Tuple<string, string>> characterJobInfo = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connection))
            {
                connection.Open();

                SqliteCommand retrieveJob = connection.CreateCommand();
                retrieveJob.CommandText = @"SELECT Job, Ability, Skill1, Skill2, Skill3 FROM Characters WHERE id = @id";
                retrieveJob.Parameters.AddWithValue("@id", characterId);

                using(SqliteDataReader reader = retrieveJob.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            string columnValue = reader.GetValue(i)?.ToString() ?? string.Empty;

                            characterJobInfo.Add(new Tuple<string, string>(columnName, columnValue));
                        }
                    }
                }
            }

            return characterJobInfo;
        }
        #endregion

        #region Display Tables
        ///<summary>
        ///Display all the current existing entries in Characters table.
        ///</summary>
        ///<param name="characterList">dictionary with id(primary key) and name of character(value)</param>
        public static void DisplayCharacterTable(Dictionary<int, string> characterList)
        {
            if(characterList.Count == 0)
            {
                Console.WriteLine("\nThere are currently no characters. Press any key to try again.");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Select character id.");
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
        private static void DisplayRaceTable()
        {
            Console.Clear();

            using(SqliteConnection connection = new SqliteConnection(_connection))
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

            using(SqliteConnection connection = new SqliteConnection(_connection))
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

            Console.WriteLine("To go back and select another option press any key.");
            Console.ReadKey();
        }

        ///<summary>
        ///Display existing entry information(character name, race, job and 6 main stats).
        ///</summary>
        ///<param name="characterSheet">List of tuple, string(column name) and string(column value)</param>
        public static void DisplayCharacterSheet(List<Tuple<string, string>> characterSheet)
        {
            foreach(var sheet in characterSheet)
            {
                Console.WriteLine($"{sheet.Item1} -- {sheet.Item2}");
            }

            Console.WriteLine();
        }

        ///<summary>
        ///Main entry point for the process of viewing stats, race and job info of an existing Character entry.
        ///</summary>
        public static void ViewAnEntryVerification()
        {
            var isValid = VerifyTableBeforeOperation();

            if(isValid == false)
            {
                Console.WriteLine("\nThere are currently no character. Press any key to continue.\n");
                Console.ReadKey();
            }
            else
            {
                var characterList = GetIdAndName("characters");
                DisplayCharacterTable(characterList); 
                var (idExist, selectedId) = Input.IsSelectedIdValid("View id: ", characterList);  

                if(idExist == true)
                {
                    Console.Clear();

                    DisplayCharacterSheet(GetStatsFromId(selectedId));
                    DisplayCharacterSheet(GetRaceFromId(selectedId));
                    DisplayCharacterSheet(GetJobFromId(selectedId));
                    
                    Console.WriteLine("\nPress any key to return to main menu.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"ERROR: selected id {selectedId} does not belong to any existing character. Press any key to try again.");
                    Console.ReadKey();
                } 
            }
        }

        public static void ViewRaceInformation()
        {
            while(true)
            {
                Console.Clear();
                DisplayRaceTable();
                Input.ShowSubmenuAdditionalOptions();

                string confirm = Input.ChoiceConfirmation($"\nView information of another id? [Y/N]: ");

                if(confirm == "no")
                {
                    break;
                }
            }
        }
        #endregion
    }
}