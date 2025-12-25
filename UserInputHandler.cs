using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace PartyDatabase
{
    class UserInputHandler
    {
        private enum DatabaseOptions
        {
            DisplayList = 'L',
            CreateCharacter = 'C',
            DeleteCharacter = 'D',
            ViewStats = 'S',
            Vocations = 'V',
            GoBack = 'B'
        }

        private static Dictionary<char, DatabaseOptions> operations = new Dictionary<char, DatabaseOptions>
        {
            { 'L', DatabaseOptions.DisplayList },
            { 'C', DatabaseOptions.CreateCharacter },
            { 'D', DatabaseOptions.DeleteCharacter },
            { 'S', DatabaseOptions.ViewStats },
            { 'V', DatabaseOptions.Vocations },
            { 'B', DatabaseOptions.GoBack }
        };

        private enum Vocations
        {
            Fighter = 1,
            Rouge, 
            Sorcerer,
            Healer,
            Bard
        }

        ///<summary>
        ///Selection screen for the available options for the user depending of on
        ///what screen is currently.
        ///</summary>
        public static void SelectionScreen()
        {
            CharacterManager.VerifyDatabaseIsCreated();
            
            while(true)
            {
                Console.Clear();
                Console.WriteLine("\nWhat do you want to do?.\n");
                Thread.Sleep(1000);
                Console.WriteLine("L- List Characters\nC- Create Character\nD- Delete Character\nS- Stats Character\nV- Display Vocations");
                Thread.Sleep(1000);
                Console.Write("\nSelect: ");
                string userInput = Console.ReadLine().ToUpper();

                if(Char.TryParse(userInput, out char selectedOption) && operations.ContainsKey(selectedOption))
                {
                    try
                    {
                        DatabaseFunctions(selectedOption);
                    }
                    catch(ArgumentException ex)
                    {
                        Console.WriteLine($"\nAn error occurred: {ex.Message}");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"\nA general error occurred: {ex.Message}");
                    }
                }
                else
                {
                        Console.WriteLine($"\nERROR: Invalid input expected char. Press any key to try again.");
                        Console.ReadKey();
                }

                Console.WriteLine("\nIs there another operation you wish to do?");
                string closeDatabase = UserInputHandler.YesNoConfirmation("Y/N: ");

                if(closeDatabase == "no")
                {
                    break;
                }
            }
        }

        ///<summary>
        ///Helper method for SelectionScreen, manage 4 current selectable operations the database can do.
        ///Display list of the current entries or Create a new entry, delete an entry or view the stats of the characters..
        ///of an entry.
        ///</summary>
        ///<param name="selectedOption">int between 1 and 4 for the current available enums</param>
        private static void DatabaseFunctions(char selectedOption)
        {

            switch(selectedOption)
            {
                case 'L': HandleListSubMenu();
                break;

                case 'C': CharacterManager.InsertCharacter(CharacterManager.CreateCharacter());
                break;
                
                case 'V': CharacterManager.DisplayVocations();
                break;

                default: throw new ArgumentException($"Invalid operation selected", nameof(selectedOption));
            }
        }

        ///<summary>
        ///Extension for the method SelectionScreeen, display the options the user can choose when the 
        ///list of the current entries is already displayed, Delete Character, Stats Character and Return to the 
        ///main menu.
        ///</summary>
        private static void HandleListSubMenu()
        {
            while(true)
            {
                DisplayCharacterList(CharacterManager.GetIdAndName());

                Console.WriteLine("\nD- Delete Character\nS- Stats Character\nB- Go back to main menu");
                Console.Write("\nSelect: ");
                string userInput = Console.ReadLine().ToUpper();

                if(Char.TryParse(userInput, out char selectedOption) && operations.ContainsKey(selectedOption))
                {
                    switch(selectedOption)
                    {
                        case 'D':
                            var verificationDelete = IsValidId("Delete: ", CharacterManager.GetIdAndName()); 

                            if(verificationDelete.exist == true)
                            {
                                CharacterManager.DeleteCharacter(verificationDelete.characterId);
                                Console.WriteLine("\nCharacter deleted. Press any key to continue.");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("\nINVALID: Choosen id does not exist inside the database. Press any key to return.");
                                Console.ReadKey();
                            }
                        break;

                        case 'S':
                            var verificationStats = IsValidId("Show stats: ", CharacterManager.GetIdAndName());
                            
                            if(verificationStats.exist == true)
                            {
                                DisplayCharacterStats(CharacterManager.GetStatsFromId(verificationStats.characterId), CharacterManager.GetSetVocationInfoFromId(verificationStats.characterId));
                            }
                             else
                            {
                                Console.WriteLine("\nINVALID: Choosen id does not exist inside the database. Press any key to return.");
                                Console.ReadKey();
                            }
                        break;

                        case 'B': return; 

                        default:
                                Console.WriteLine($"\nERROR: Invalid input for this submenu. Expected 'D', 'S', or 'B'. Press any key to try again.");
                                Console.ReadKey();
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"\nERROR: Invalid input expected char. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        ///<summary>
        ///Helper method to display the a list with the current characters inside the database
        ///Displays Name and id(primary key).
        ///</summary>
        ///<param name="characterList">dictionary with id(primary key) and name of character</param>
        private static void DisplayCharacterList(Dictionary<int, string> characterList)
        {
            if(characterList.Count == 0)
            {
                Console.WriteLine("\nThere are currently no characters\n");
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

            /*Console.WriteLine("\nPress any key to display the sub menu.");
            Console.ReadKey();*/
        }

        ///<summary>
        ///Helper method to display the stats of the selected character
        ///Displays Strength, Constitution, Dexterity, Intelligence, Wisdom and Charisma
        ///</summary>
        ///<param name="characterStats">List of tuple, string with the name of the column and int with the value of the column</param>
        private static void DisplayCharacterStats(List<Tuple<string, int>> characterStats, List<Tuple<string, string>> characterSkills)
        {
            Console.WriteLine("\n----- Character Stats -----");

            foreach(var stats in characterStats)
            {
                Console.WriteLine($"{stats.Item1} --> {stats.Item2}");
            }

            Console.WriteLine("----- Character Skills -----");

            foreach(var skills in characterSkills)
            {
                Console.WriteLine($"- {skills.Item1} --- {skills.Item2}");
            }

            Console.WriteLine("\nPress any key to return to main menu.\n");
            Console.ReadKey();
        }

        ///<summary>
        ///Helper method to validate that the user choosen id exist within the database
        ///</summary>
        ///<param name="prompt">prompts user the selected operation</param>
        ///<param name="characterList">Dictionary with primary key(character id) and name of the character</param>
        ///<returns>Tupel, boolean if true id exist else id doesn't, integer corresponding to the id choosen</returns>
        private static (bool exist, int characterId) IsValidId(string prompt, Dictionary<int, string> characterList)
        {
            while(true)
            {
                Console.WriteLine("\nEnter the id of the character to...");
                Console.Write(prompt);

                if(int.TryParse(Console.ReadLine(), out int userInput))
                {
                    if(characterList.ContainsKey(userInput))
                    {
                        return (true, userInput);
                    }
                    else
                    {
                        return (false, userInput);
                    }
                }
                else
                {
                    Console.WriteLine("\nERROR:invalid input expected positive integer(Character Id). Press any key to try again.");
                }
            }
        }

        ///<summary>
        ///To enter the name of each character or account.
        ///</summary>
        ///<param name="prompt">prompts user to enter a name</param>
        ///<returns>string for the name of the character</returns>
        public static string AddName(string prompt)
        {
            const int nameMaxLength = 10;

            string nameInput = string.Empty;

            while(true)
            {
                Console.Clear();
                Console.WriteLine($"NOTE: Character name cannot have more than {nameMaxLength} character in length.\n");
                Console.WriteLine("Enter the name of this new character.");
                Console.Write(prompt);
                nameInput = Console.ReadLine();

                if(!String.IsNullOrWhiteSpace(nameInput) && nameInput.Length <= nameMaxLength)
                {
                    return nameInput;
                }
                else
                {
                    Console.WriteLine($"\nERROR: Invalid input, name cannot be Null or Empty and must have less than {nameMaxLength} characters.\n");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        ///<summary>
        ///To add points to each stat value(starts at 10 to a maximum of 20 for each stat)
        ///</summary>
        ///<param name="prompt">prompts user to add the desire points to the current stat</param>
        ///<param name="statName">name of the current stat</param>
        ///<param name="minValue">limits the minimum value each stat can have</param>
        ///<param name="maxValue">limits the maximum value each stat can have</param>
        ///<param name="points">quantity of points the user can add to each character</param>
        ///<returns>int which is the final stat value after the allocation of the available points</returns>
        public static int AddStatValue(string prompt, string statName, int minValue, int maxValue, ref int points)
        {
            if(points <= 0)//if no points left, return the minimum value(10) for the remaining stats
            {
                return minValue;
            }

            while(true)
            {
                int statTotal = 0;

                Console.Clear();
                Console.WriteLine($"NOTE: Each stat value cannot be less than {minValue} and greater than {maxValue}.");
                Console.WriteLine($"HINT: If you don't want to add points to the current stat just add 0.\n");
                Console.WriteLine($"You have {points} points remaining\n");
                Console.WriteLine($"Current {statName} --> {minValue}");
                Console.Write(prompt);

                if(int.TryParse(Console.ReadLine(), out int statInput))
                {
                    bool userConfirmation = PointsConfirmation("Y/N: ", statName, statInput, ref points);

                    if(userConfirmation == true)
                    {
                        statTotal = minValue + statInput;
                        
                        if(statTotal <= maxValue)
                        {
                            points -= statInput;
                            return statTotal;
                        }
                        else
                        {
                            Console.WriteLine($"\nERROR: Invalid input, {statName} cannot pass the limit of 20. Press any key to try again.\n");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine($"\nERROR: Invalid input, expected an integer. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        ///<summary>
        ///Helper method for AddStatValue to confirm the added points are correct.
        ///</summary>
        ///<param name="prompt">ask user to confirm is the points allocated to the current stat is correct</param>
        ///<param name="statName">name of the current stat at work</param>
        ///<param name="statInput">int input for how many points the user choose to add to the current stat</param>
        ///<param name="points">currently available points to distribute across all stats</param>
        ///<returns>boolean, if true user confirm stat allocation as correct else user can change the points allocated</returns>
        private static bool PointsConfirmation(string prompt, string statName, int statInput, ref int points)
        {
            if(statInput <= points)
            {
                while(true)
                {
                    Console.WriteLine($"\nYou added +{statInput} points to the stat of {statName}, is this correct?");
                    string confirm = YesNoConfirmation("Y/N: ");

                    if(String.Equals(confirm, "yes"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine($"\nERROR: Invalid input, you cannot add more points than you currently have available --> {points}");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
                
                return false;
            }
        }

        ///<summary>
        ///Displays to the user the current stats for the characters and to confirm the distribution of points
        ///is correct before creating the instance of the character class.
        ///</summary>
        ///<param name="strength">current strength stat of the character</param>
        ///<param name="constitution">current constitution stat of the character</param>
        ///<param name="dexterity">current dexterity stat of the character</param>
        ///<param name="intelligence">current intelligence stat of the character</param>
        ///<param name="wisdom">current wisdom stat of the character</param>
        ///<param name="charisma">current charisma stat of the character</param>
        ///<returns>boolean to confirm the current stats before creating the character instance</returns>
        public static bool StatsConfirmation(string characterName, int strength, int constitution, int dexterity, int intelligence, int wisdom, int charisma, ref int points)
        {
            while(true)
            {
                Console.Clear();

                if(points > 0)
                {
                    Console.WriteLine($"WARNING: You have {points} points unassiged, these points will be lost if you dont use them.");
                }

                Console.WriteLine($"Name: {characterName}");
                Console.WriteLine("\nProceed with this stats?\n");
                Console.WriteLine($"Str: {strength}");
                Console.WriteLine($"Con: {constitution}");
                Console.WriteLine($"Dex: {dexterity}");
                Console.WriteLine($"Int: {intelligence}");
                Console.WriteLine($"Wis: {wisdom}");
                Console.WriteLine($"Cha: {charisma}\n");
                
                string confirm = YesNoConfirmation("Y/N: ");

                if(String.Equals(confirm, "yes"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        ///<summary>
        ///Display to the user the currently available vocations for the character.
        ///</summary>
        ///<param name="prompt">prompts the user to select 1 of the available vocations</param>
        ///<param name="name">choosen name of the character to be created</param>
        ///<returns>integer that represents the vocation id</returns>
        public static int ChooseVocation(string prompt, string name)
        {
            List<Vocations> vocationList = Enum.GetValues(typeof(Vocations)).Cast<Vocations>().ToList();
            Vocations firstIdValue = Vocations.Fighter;
            int minValue = (int)firstIdValue;
            int maxValue = vocationList.Count;

            while(true)
            {
                int bulletList = 1;

                Console.Clear();
                Console.WriteLine($"Now lets choose {name} vocation.\n");

                foreach(Vocations vocation in vocationList)
                {
                    Console.WriteLine($"{bulletList} - {vocation}");
                    bulletList++;
                }
                
                Console.Write(prompt);

                if(int.TryParse(Console.ReadLine(), out int selectedId) && selectedId >= minValue || selectedId <= maxValue)
                {
                    return selectedId;
                }
                else
                {
                    Console.WriteLine($"ERROR: Invalid input expected positive integer between {minValue} and {maxValue}. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        ///<summary>
        ///Prompts user for confirmation for an action
        ///</summary>
        ///<param name="prompt">prompts user for input of 'y' or 'n'</param>
        ///<returns>string with value of "yes" or "no"</returns>
        private static string YesNoConfirmation(string prompt)
        {
            while(true)
            {
                Console.Write(prompt);
                string userInput = Console.ReadLine().ToLower();

                if(string.Equals(userInput, "y", StringComparison.OrdinalIgnoreCase) || string.Equals(userInput, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    return "yes";
                }
                else if(string.Equals(userInput, "n", StringComparison.OrdinalIgnoreCase) || string.Equals(userInput, "no", StringComparison.OrdinalIgnoreCase))
                {
                    return "no";
                }
                else
                {
                    Console.WriteLine("\nERROR: Invalid input, expected 'y' for yes or 'n' for no. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }
    }
}