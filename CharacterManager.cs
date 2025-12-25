using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace PartyDatabase
{
    class CharacterManager
    {  
        private static readonly string _databasePathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "characters.db");
        private static readonly string _connectionString = $"Data Source={_databasePathFile}; Foreign Keys=True;";

        ///<summary>
        ///Ensures the database and folder exist
        ///</summary>
        public static void VerifyDatabaseIsCreated()
        {
            string folder = Path.GetDirectoryName(_databasePathFile);
            if(!string.IsNullOrEmpty(folder)) Directory.CreateDirectory(folder);

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using(SqliteCommand command = connection.CreateCommand())//Ensure foreign keys are enforced
                {
                    command.CommandText = @"PRAGMA foreign_keys = ON";
                    command.ExecuteNonQuery();
                    
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Vocations (id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name TEXT NOT NULL, Ability TEXT NOT NULL, Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, Skill3 TEXT NOT NULL);";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Characters (id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    VocationId INTEGER, Name TEXT NOT NULL, Strength INTEGER NOT NULL, Constitution INTEGER NOT NULL, 
                    Dexterity INTEGER NOT NULL, Intelligence INTEGER NOT NULL, Wisdom INTEGER NOT NULL, Charisma INTEGER NOT NULL, 
                    Vocation TEXT NOT NULL, Ability TEXT NOT NULL, Skill1 TEXT NOT NULL, Skill2 TEXT NOT NULL, 
                    Skill3 TEXT NOT NULL, FOREIGN KEY (VocationId) REFERENCES Vocations(id) ON DELETE SET NULL);";
                    command.ExecuteNonQuery();
                }
                
                SeedVocations(connection);
            }
        }
        
        ///<summary>
        ///Inserts every class vocation in to its own table(Vocations)
        ///</summary>
        ///<param name="connection">instruction to open connection to the database</param>
        private static void SeedVocations(SqliteConnection connection)
        {
           using(SqliteCommand checkCommand = connection.CreateCommand())//to verify if the vocations are already on table
            {
                checkCommand.CommandText = @"SELECT COUNT(*) FROM Vocations";
                if(Convert.ToInt32(checkCommand.ExecuteScalar()) > 0) return;
            } 

            int[] vocationId = {1, 2, 3, 4, 5};

            foreach(int id in vocationId)
            {
                using(SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Vocations (id, Name, Ability, Skill1, Skill2, Skill3)
                    VALUES (@id, @name, @def, @s1, @s2, @s3)";

                    switch(id)
                    {
                        case 1: SetVocationParameters(command, 1, "Fighter", "[Parry]", "[Deflect Missile]", "[Multi-Slash]", "[Withstand Deathblow]");
                        break;

                        case 2: SetVocationParameters(command, 2, "Rouge", "[Steal(Battle/Shop)]", "[Deathblow]", "[Detect Traps]", "[Uncanny Dodge]");
                        break;

                        case 3: SetVocationParameters(command, 3, "Sorcerer", "[Fireball]", "[Quick Chanter]", "[Unshakable Caster]", "[Meteor]");
                        break;

                        case 4: SetVocationParameters(command, 4, "Healer", "[Heal]", "[Expand Healing Radius]", "[Deathblow Immunity]", "[Resurrection]");
                        break;

                        case 5: SetVocationParameters(command, 5, "Bard", "[Inspire]", "[Evade & Parry]", "[Song of Bravery]", "[Charm]");
                        break;
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        ///<summary>
        ///Helper method for SeedVocations, inserts each entry value
        ///</summary>
        private static void SetVocationParameters(SqliteCommand command, int id, string name, string def, string s1, string s2, string s3)
        {
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@def", def);
            command.Parameters.AddWithValue("@s1", s1);
            command.Parameters.AddWithValue("@s2", s2);
            command.Parameters.AddWithValue("@s3", s3);
        }

        ///<summary>
        ///Displays each row and columns for the existing vocations
        ///</summary>
        public static void DisplayVocations()
        {
            Console.Clear();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using(SqliteCommand retrieveVocations = connection.CreateCommand())
                {
                    retrieveVocations.CommandText = @"SELECT ALL id, Name, Ability, Skill1, Skill2, Skill3 FROM Vocations";

                    using(SqliteDataReader reader = retrieveVocations.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string def = reader.GetString(2);
                            string s1 = reader.GetString(3);
                            string s2 = reader.GetString(4);
                            string s3 = reader.GetString(5);

                        Console.WriteLine($"id: {id} | Vocation: {name} | Ability: {def}");
                        Console.WriteLine($"Unlockable Skills: {s1} - {s2} - {s3}\n");
                        }
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
                addCharacterCommand.CommandText = @"INSERT INTO Characters (Name, Strength, Constitution, Dexterity, Intelligence, Wisdom, Charisma, VocationId,
                Vocation, Ability, Skill1, Skill2, Skill3) 
                VALUES (@name, @strength, @constitution, @dexterity, @intelligence, @wisdom, @charisma, @vocationId, @VName, @def, @s1, @s2, @s3)";
                addCharacterCommand.Parameters.AddWithValue("@name", character.Name);
                addCharacterCommand.Parameters.AddWithValue("@strength", character.Strength);
                addCharacterCommand.Parameters.AddWithValue("@constitution", character.Constitution);
                addCharacterCommand.Parameters.AddWithValue("@dexterity", character.Dexterity);
                addCharacterCommand.Parameters.AddWithValue("@intelligence", character.Intelligence);
                addCharacterCommand.Parameters.AddWithValue("@wisdom", character.Wisdom);
                addCharacterCommand.Parameters.AddWithValue("@charisma", character.Charisma);
                addCharacterCommand.Parameters.AddWithValue("@vocationId", character.VocationId > 0 ? character.VocationId : DBNull.Value);
                addCharacterCommand.Parameters.AddWithValue("@VName", character.AssignedVocation.VocationName);
                addCharacterCommand.Parameters.AddWithValue("@def", character.AssignedVocation.DefaultSkill);
                addCharacterCommand.Parameters.AddWithValue("@s1", character.AssignedVocation.SkillLowLevel);
                addCharacterCommand.Parameters.AddWithValue("@s2", character.AssignedVocation.SkillMediumLevel);
                addCharacterCommand.Parameters.AddWithValue("@s3", character.AssignedVocation.SkillHighLevel);
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
            var choosenVocationId = UserInputHandler.ChooseVocation("\nSelect a vocation: ", name);

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
                    AssignVocation(character, choosenVocationId);
                    return character;
                }
            } 
        }
        
        ///<summary>
        ///Helper method to assign the choosen vocation to the character instance.
        ///</summary>
        ///<param name="character">instance of the character created</param>
        ///<param name="vocationId">a representation of the selected id to assign the vocation</param>
        private static void AssignVocation(Character character,  int vocationId)
        {
            switch(vocationId)
            {
                case 1: character.SetVocation(new Vocation.Fighter(character));
                break;

                case 2: character.SetVocation(new Vocation.Rouge(character));
                break;

                case 3: character.SetVocation(new Vocation.Sorcerer(character));
                break;

                case 4: character.SetVocation(new Vocation.Healer(character));
                break;

                case 5: character.SetVocation(new Vocation.Bard(character));
                break;

                default: Console.WriteLine("\nERROR: Invalid input selected option does not exist.\n");
                break;
            }
        }

        ///<summary>
        ///Retreives all the values from the column.
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

        ///Testing
        public static List<Tuple<string, string>> GetSetVocationInfoFromId(int characterId)
        {
            List<Tuple<string, string>> characterVocationInfo = new List<Tuple<string, string>>();

            using(SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand retrieveSetVInfo = connection.CreateCommand();
                retrieveSetVInfo.CommandText = @"SELECT Vocation, Ability, Skill1, Skill2, Skill3 FROM Characters WHERE id = @id";
                retrieveSetVInfo.Parameters.AddWithValue("@id", characterId);

                using(SqliteDataReader reader = retrieveSetVInfo.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            string vColumnName = reader.GetName(i);
                            string vColumnValue = reader.GetString(i);

                            characterVocationInfo.Add(new Tuple<string, string>(vColumnName, vColumnValue));
                        }
                    }
                }
            }

            return characterVocationInfo;
        }
    }
}