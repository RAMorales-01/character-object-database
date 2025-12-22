using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection; //for class scanning
using System.Linq; //for filtering
using Microsoft.Data.Sqlite;

namespace PartyDatabase
{
    class CharacterManager
    {  
        private static readonly string _databasePathFile = Path.Combine(Directory.GetCurrentDirectory(), "Data", "characters.db");
        private static readonly string _connectionString = $"Data Source={_databasePathFile}; Foreign Keys=True;";

        ///<summary>
        ///Ensures the database and folder exist
        ///</summary>
        public static void VerifyDatabaseIsCreated()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_databasePathFile));

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using(SqliteCommand pragmaCommand = connection.CreateCommand())//Ensure foreign keys are enforced
                {
                    pragmaCommand.CommandText = @"PRAGMA foreign_keys = ON";
                    pragmaCommand.ExecuteNonQuery();
                }
                using(SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Vocations (id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name TEXT NOT NULL, DefaultSkill TEXT NOT NULL, Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, Skill3 TEXT NOT NULL);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Characters (id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    VocationId INTEGER, Name TEXT NOT NULL, Strength INTEGER NOT NULL, Constitution INTEGER NOT NULL, 
                    Dexterity INTEGER NOT NULL, Intelligence INTEGER NOT NULL, Wisdom INTEGER NOT NULL, Charisma INTEGER NOT NULL, 
                    FOREIGN KEY (VocationId) REFERENCES Vocations(id) ON DELETE SET NULL);";
                    command.ExecuteNonQuery();"
                } 

                SeedVocations();
            }
        }

        private static void SeedVocations()
        {
            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand checkCommand = connection.CreateCommand();
                checkCommand.CommandText = @"SELECT * FROM Vocations";

                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0) return; 

                //we use Reflection to scan all the classes that are not abstract that inherits form VocationBasics                
                var vocationTypes = typeof(Vocation).GetNestedTypes().Where(t => t.IsSubClassOf(typeof(Vocation.VocationBasics)) && !t.IsAbstract);
                /*typeof(Vocation).GetNestedTypes() looks inside the Vocation class and makes a list
                then with the method Where() a lambda expression where 't' is a list of all the classes that inherit from Vocation.VocationBasics
                and finally !t.IsAbstract ignores VocationBasics which is an abstract class.*/

                foreach(var types in vocationTypes)
                {
                    var instance = (Vocation.VocationBasics)Activator.CreateInstance(type, new object[] { null });
                    /*Activator creates an object by knowing its type, then by using this type takes the arguments in an array 
                    and pass the argument of the constructor as null. So basically what happens is: Take this type, find its constructor, 
                    and run it using null as the input. Then, take that new object and treat it as a VocationBasics 
                    so I can read its Name and Skills."*/

                    using(SqliteCommand addVocationCommand = connection.CreateCommand())
                    {
                        addVocationCommand.CommandText = @"INSERT INTO Vocations (id, Name, DefaultSkill, Skill1, Skill2, Skill3)
                        VALUES (@id, @name, @def, @s1, @s2, @s3)";
                        addVocationCommand.Parameters.AddWithValue("@id", instance.VocationId);
                        addVocationCommand.Parameters.AddWithValue("@name", instance.VocationName);
                        addVocationCommand.Parameters.AddWithValue("@def", instance.DefaultSkill);
                        addVocationCommand.Parameters.AddWithValue("@s1", instance.SkillLowLevel);
                        addVocationCommand.Parameters.AddWithValue("@s2", instance.SkillMediumLevel);
                        addVocationCommand.Parameters.AddWithValue("@s3", instance.SkillHighLevel);
                        addVocationCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        ///<summary>
        ///Insert the character object to the database
        ///</summary>
        ///<param name="character">current character object to be inserted to the database</param>
        public static void InsertCharacter(Character character)
        {
            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open(); 

                SqliteCommand addCharacterCommand = connection.CreateCommand();
                addCharacterCommand.CommandText = @"INSERT INTO Characters (Name, Strength, Constitution, Dexterity, Intelligence, Wisdom, Charisma, VocationId) 
                VALUES (@name, @strength, @constitution, @dexterity, @intelligence, @wisdom, @charisma, @vocationId)";
                addCharacterCommand.Parameters.AddWithValue("@name", character.Name);
                addCharacterCommand.Parameters.AddWithValue("@strength", character.Strength);
                addCharacterCommand.Parameters.AddWithValue("@constitution", character.Constitution);
                addCharacterCommand.Parameters.AddWithValue("@dexterity", character.Dexterity);
                addCharacterCommand.Parameters.AddWithValue("@intelligence", character.Intelligence);
                addCharacterCommand.Parameters.AddWithValue("@wisdom", character.Wisdom);
                addCharacterCommand.Parameters.AddWithValue("@charisma", character.Charisma);
                addCharacterCommand.Parameters.AddWithValue("@vocationId", character.VocationId > 0 ? character.VocationId : DBNull.Value);
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

                bool proceed = UserInputHandler.StatsConfirmation(name, strength, constitution, dexterity, intelligence, wisdom, charisma, ref points);

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