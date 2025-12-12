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

                InsertCharacter(CreateCharacter());
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
        
        public static List<string> GetAllCharacters()
        {
            List<string> characters = new List<string>();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retriveCharacters = connection.CreateCommand();
                retriveCharacters.CommandText = @"SELECT ALL (Name) FROM (Characters);";

                using(SqliteDataReader reader = retriveCharacters.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        characters.Add(reader.GetString(0));
                    }
                }
            }

            return characters;
        }
    }
}