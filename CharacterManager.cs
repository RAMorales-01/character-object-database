using System;
using System.IO;
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
            }
        }

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
        //TODO: GetAllCharacters method
    }
}