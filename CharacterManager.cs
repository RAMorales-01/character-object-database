using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace PartyDatabase
{
    class CharacterManager
    {
        private static readonly int _maxCapacity = 6;//defines the maximum permited capacity for the characters inside the table  
        private static readonly string _databasePathFile = Path.Combine(Directory.GetCurrentDirectory(), "Data", "characters.db");
        private static readonly string _connectionString = $"Data Source={_databasePathFile}";

        ///<summary>
        ///Ensures the database and folder exist
        ///</summary>
        public static void VerifyDatabaseIsCreated()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_databasePathFile));

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Characters (id INTEGER PRIMARY KEY AUTOINCREMENT, 
                Name TEXT NOT NULL, Strength INTEGER NOT NULL, Constitution INTEGER NOT NULL, Dexterity INTEGER NOT NULL, 
                Intelligence INTEGER NOT NULL, Wisdom INTEGER NOT NULL, Charisma INTEGER NOT NULL);";
                command.ExecuteNonQuery();


                /*Console.WriteLine("\nDelete character?");
                Console.Write("id: ");
                int.TryParse(Console.ReadLine(), out int characterId);
                DeleteCharacter(characterId);*/

                Console.WriteLine("Want to see his stats?");
                Console.Write("id: ");
                int.TryParse(Console.ReadLine(), out int characterId);
                var characterStats = GetStatsFromId(characterId);

                Console.WriteLine();

                foreach(var stats in characterStats)
                {
                    Console.WriteLine($"{stats.Item1} --> {stats.Item2}");
                }

                Console.ReadKey();
            }
        }

        ///<summary>
        ///Insert the character object to the database
        ///</summary>
        ///<param name="character">current character object to be inserted to the database</param>
        private static void InsertCharacter(Character character)
        {
            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); 

                SqliteCommand addCharacterCommand = connection.CreateCommand();
                addCharacterCommand.CommandText = @"INSERT INTO Characters (Name, Strength, Constitution, Dexterity, Intelligence, Wisdom, Charisma) 
                VALUES (@name, @strength, @constitution, @dexterity, @intelligence, @wisdom, @charisma)";
                addCharacterCommand.Parameters.AddWithValue("@name", character.Name);
                addCharacterCommand.Parameters.AddWithValue("@strength", character.Strength);
                addCharacterCommand.Parameters.AddWithValue("@constitution", character.Constitution);
                addCharacterCommand.Parameters.AddWithValue("@dexterity", character.Dexterity);
                addCharacterCommand.Parameters.AddWithValue("@intelligence", character.Intelligence);
                addCharacterCommand.Parameters.AddWithValue("@wisdom", character.Wisdom);
                addCharacterCommand.Parameters.AddWithValue("@charisma", character.Charisma);
                addCharacterCommand.ExecuteNonQuery();
            }
        }

        ///<summary>
        ///To create an instance of the Character class after all the parameters have been confirmed by user.
        ///</summary>
        ///<returns>an instance of the Character class</returns>
        public static Character CreateCharacter()
        {
            var name = UserInputHandler.AddName("Name: ");

            while(true)
            {
                int points = 30;//each new character have a total of 30 points to distribute between the 6 stats.

                var strength = UserInputHandler.AddStatValue("Add points: ", "Strength", Character._minStatValue, Character._maxStatValue, ref points);
                var constitution = UserInputHandler.AddStatValue("Add points: ", "Constitution", Character._minStatValue, Character._maxStatValue, ref points);
                var dexterity = UserInputHandler.AddStatValue("Add points: ", "Dexterity", Character._minStatValue, Character._maxStatValue, ref points);
                var intelligence = UserInputHandler.AddStatValue("Add points: ", "Intelligence", Character._minStatValue, Character._maxStatValue, ref points);
                var wisdom = UserInputHandler.AddStatValue("Add points: ", "Wisdom", Character._minStatValue, Character._maxStatValue, ref points);
                var charisma = UserInputHandler.AddStatValue("Add points: ", "Charisma", Character._minStatValue, Character._maxStatValue, ref points);

                bool proceed = UserInputHandler.StatsConfirmation(strength, constitution, dexterity, intelligence, wisdom, charisma, ref points);

                if(proceed == true)
                {
                    Character character = new Character(name, strength, constitution, dexterity, intelligence, wisdom, charisma);
                    return character;
                }
            } 
        }
        
        ///<summary>
        ///Retreives all the values from the column name from the database.
        ///</summary>
        ///<returns>A dictionary with the id and name of all the current character in the database</returns>
        public static Dictionary<int, string> GetIdAndName()
        {
            Dictionary<int, string> characters = new Dictionary<int, string>();//To stores all the names form the database

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retrieveCharacters = connection.CreateCommand();
                retrieveCharacters.CommandText = @"SELECT ALL id, Name FROM Characters;";

                using(SqliteDataReader reader = retrieveCharacters.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = reader.GetInt32(0);//reads column index 0
                        string name = reader.GetString(1);//reads column index 1
                        characters.Add(id, name);
                    }
                }
            }

            return characters;
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

        ///<summary>
        ///Retrieves all the columns and values from a row using the primary key
        ///</summary>
        ///<param name="characterId">primary key form each row</param>
        ///<returns>a List of Tuple string for the column name and int for the value on each column</returns> 
        public static List<Tuple<string, int>> GetStatsFromId(int characterId)
        {
            List<Tuple<string, int>> characterStats = new List<Tuple<string, int>>();

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
                            string statName = reader.GetName(i);
                            int statValue = Convert.ToInt32(reader.GetValue(i));

                            characterStats.Add(new Tuple<string, int>(statName, statValue));
                        }
                   }
                }
            }

            return characterStats;
        }
    }
}