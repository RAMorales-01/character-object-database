using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseLogic;
using DatabaseUtility;

namespace UserHandler
{
    ///<summary>
    ///This class will handle all user related interactions with the database
    ///</summary>
    class Input
    {
        #region Enums
        //Representation for the main menu current options 
        private enum MainMenu
        {
            DisplayEntry = 1,
            DisplaySubmenu,
            CreateEntry,             
            DeleteEntry, 
            ExitDatabase
        }

        //Representation for the submenu current options
        private enum Submenu
        {
            DisplayEntryInfo = 1,
            GoBackToMain
        }

        //Representation for the current available races for the character
        private enum Races
        {
            Human = 1,
            Elven,
            Fiendblood,
            Beastfolk
        }
        
        //Representation for the current available jobs for the character
        private enum Jobs
        {
            Fighter = 1,
            Rogue,
            Spellcaster,
            Priest,
            Bard
        }
        #endregion

        #region Main Menu and Submenu
        ///<summary>
        ///Selection screen for initial menu
        ///</summary>
        public static void MainMenu()
        {
            EntryManager.VerifyDatabaseIsCreated();//Verifies the database exist, if not is created along with all the tables.

            //Takes the int values of the first element and last element of the enum, to limit the user options.
            int minPermited = (int)MainMenu.DisplayEntry;
            int maxPermited = (int)MainMenu.ExitDatabase;
            
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the main menu, please select an option\n");
                Console.WriteLine("1- Display list of entries\n2- Display submenu\n3- Create entry\n4- Delete entry\n5- Exit");
                Console.Write("\n: ");
                string userInput = Console.ReadLine();

                var isInputValid = ValidateSelectedOption(userInput, minPermited, maxPermited);

                if(isInputValid.Item1 == true)
                {
                    if(isInputValid.Item2 == maxPermited){ break; }

                    else
                    {
                        try
                        {
                            DatabaseOptions.MainMenuOptions(isInputValid.Item2);
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
                }
                else
                {
                    Console.WriteLine($"\nINVALID INPUT: expected positive integer between {minPermited} and {maxPermited}. Press any key to go back and try again.");
                    Console.ReadKey();
                }

                Console.WriteLine("\nKeep working with the database?");
                string keepDatabaseOpen = ChoiceConfirmation("Y/N: ");

                if(keepDatabaseOpen == "no")
                {
                    break;
                }
            }
        }

        ///<summary>
        ///Display the submenu options to the user
        ///</summary>
        private static void Submenu()
        {
            int minPermited = (int)Submenu.DisplayEntryInfo;
            int maxPermited = (int)Submenu.GoBackToMain;

            while(true)
            {
                DisplayEntriesList(EntryManager.GetIdAndName());

                Console.WriteLine("\n1- Display character info\n2- Go back to main menu");
                Console.Write("\n: ");
                string userInput = Console.ReadLine();
                
                var isInputValid = ValidateSelectedOption(userInput, minPermited, maxPermited);

                if(isInputValid.Item1 == true)
                {
                    if(isInputValid.Item2 == maxPermited){ break; }

                    else
                    {
                        DatabaseFunctions.SubmenuOptions(isInputValid.Item2);
                    }
                }
                else
                {
                    Console.WriteLine($"\nINVALID INPUT: expected positive integer between {minPermited} and {maxPermited}. Press any key to go back and try again.");
                    Console.ReadKey();
                }
            }
        }
        #endregion
        
        #region Add Name and Stats 
        ///<summary>
        ///Method for the validation of the character name to insert in the Characters table.
        ///</summary>
        ///<param name="prompt">prompts user to enter a name</param>
        ///<returns>string value for the name of the character</returns>
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
        ///The user can add points to each stat value(min stat value of 10 to a maximum of 20 in total)
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
                int statTotal = 0;//shows total stat at the end(min value of 10 plus the choosen points to add to the current stat).

                Console.Clear();
                Console.WriteLine($"NOTE: Each stat value cannot be less than {minValue} and greater than {maxValue}.");
                Console.WriteLine($"HINT: If you don't want to add points to the current stat just add 0.\n");
                Console.WriteLine($"You have {points} points remaining\n");
                Console.WriteLine($"Current {statName} --> {minValue}");
                Console.Write(prompt);
                string userInput = Console.ReadLine();

                var isInputValid = ValidateSelectedOption(userInput, minValue, maxValue);

                if(isInputValid.Item1 == true)
                {
                    bool userConfirmation = PointsConfirmation("Y/N: ", statName, isInputValid.Item2, ref points);

                    if(userConfirmation == true)
                    {
                        statTotal = minValue + isInputValid.Item2;

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
                    Console.WriteLine($"\nINVALID INPUT: expected positive integer. Press any key to go back and try again.");
                    Console.ReadKey();
                }
            }
        }
        #endregion

        #region Select Race and Job
        ///<summary>
        ///Display to the user the currently available races and job options for the character.
        ///</summary>
        ///<param name="prompt">prompts the user to select 1 of the available options(race or job)</param>
        ///<param name="name">choosen name for the created character</param>
        ///<returns>integer that represents the id of the selected optiion for race or job</returns>
        public static int SelectRaceAndJob(string typeInfo, string name)
        {
            string toConfirm = typeInfo.ToLower();

            if(toConfirm == "race")
            {
                var raceList = RequestedList(toConfirm);
            }
            if(toConfirm == "job")
            {
                var jobList = RequestedList(toConfirm);
            }
           
        }

        ///<summary>
        ///Converts all elements of an enum type into a List of strings.
        ///</summary>
        ///<returns>a list of strings with the names of all the elements inside the selected Enum</returns>
        private static List<string> RequestedList<TEnum>() where TEnum : Enum
        {
            return Enum.GetNames(typeof(TEnum)).ToList();
        }

        ///<summary>
        ///Displays the available options for races or jobs.
        ///</summary>
        ///<param name="list">selected list to be displayed</param>
        private static void DisplayListOptions(List<string> list)
        {
            int listNum = 1;

            foreach(string option in list)
            {
                Console.WriteLine($"{listNum} - {option}");
                listNum++;
            }
        }
        #endregion

        #region User Input Utility Methods 
        ///<summary>
        ///Validate user input, if its a positive integer within the limits(minPermited, maxPermited) returns true and the int value
        ///else returns false and the int value
        ///</summary>
        ///<param name="userInput">user selected option</param>
        ///<param name="minPermited">min selected option permited</param>
        ///<param name="maxPermited">max selected option permited</param>
        ///<returns>Tuple with bool and integer values</returns>
        private static (bool, int) ValidateSelectedOption(string userInput, int minPermited, int maxPermited)
        {
            if(int.TryParse(userInput, out int selectedOption) && selectedOption >= minPermited || selectedOption <= maxPermited)
            {
                return (true, selectedOption);
            }
            else
            {
                return (false, selectedOption);
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
                    string confirm = ChoiceConfirmation("Y/N: ");

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
        ///Prompts user to confirm the selection of an action
        ///</summary>
        ///<param name="prompt">prompts user for input of 'y' or 'n'</param>
        ///<returns>string with value of "yes" or "no"</returns>
        private static string ChoiceConfirmation(string prompt)
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
                    Console.WriteLine("\nINVALID INPUT: expected 'y' for yes or 'n' for no. Press any key to go back and try again.");
                    Console.ReadKey();
                }
            }
        }

        ///<summary>
        ///Displays to the user the current stats for the characters and to confirm the distribution of points
        ///is correct before creating the instance of the character class.
        ///</summary>
        ///<param name="characterName">name of the entry choosen by the user</param>
        ///<param name="raceId">integer that represent one of the current available races to choose</param>
        ///<param name="jobId">integer that represent one of the current available jobs to choose</param>
        ///<param name="strength">current strength stat of the character</param>
        ///<param name="constitution">current constitution stat of the character</param>
        ///<param name="dexterity">current dexterity stat of the character</param>
        ///<param name="intelligence">current intelligence stat of the character</param>
        ///<param name="wisdom">current wisdom stat of the character</param>
        ///<param name="charisma">current charisma stat of the character</param>
        ///<returns>boolean to confirm the current stats before creating the character instance</returns>
        public static bool CharacterConfirmation(string characterName, int raceId, int jobId, int strength, int constitution, int dexterity, int intelligence, int wisdom, int charisma, ref int points)
        {
            //Confirms the choosen race and job by id and returns the string value(name) of selected race and job.
            string choosenRace = RaceAndJobConfirmation("Race", raceId);
            string choosenJob = RaceAndJobConfirmation("Job", jobId);

            while(true)
            {
                Console.Clear();

                if(points > 0)
                {
                    Console.WriteLine($"WARNING: You have {points} points unassiged, these points will be lost if you dont use them.");
                }

                Console.WriteLine($"Name: {characterName}");
                Console.WriteLine($"Race: {choosenRace}");
                Console.WriteLine($"Job: {choosenJob}");
                Console.WriteLine("\nProceed with this stats?\n");
                Console.WriteLine($"Str: {strength}");
                Console.WriteLine($"Con: {constitution}");
                Console.WriteLine($"Dex: {dexterity}");
                Console.WriteLine($"Int: {intelligence}");
                Console.WriteLine($"Wis: {wisdom}");
                Console.WriteLine($"Cha: {charisma}\n");
                
                string confirm = ChoiceConfirmation("Y/N: ");

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
        ///Helper method to validate the id of the job or race selected
        ///</summary>
        ///<param name="typeInfo">a string arguement to return the type of race or job</param>
        ///<param name="id">an integer that represents an id that is tied to its equivalent value.</param>
        ///<returns>string with a value of race or job</returns>
        private static string RaceAndJobConfirmation(string typeInfo, int id)
        {
            string toConfirm = typeInfo.ToLower();

            if(toConfirm == "race")
            {
                return id switch
                {
                    1 => "Human",
                    2 => "Elven",
                    3 => "Fiendblood",
                    4 => "Beastfolk",
                    _ => throw new ArgumentException("Invalid race id selected", nameof(id))
                };
            }
            if(toConfirm == "job")
            {
                return id switch
                {
                    1 => "Fighter",
                    2 => "Rogue", 
                    3 => "Spellcaster",
                    4 => "Priest",
                    5 => "Bard",
                    _ => throw new ArgumentException("Invalid job id selected", nameof(id))
                };
            }

            throw new ArgumentException("Type must be 'race' or 'job'", nameof(typeInfo));
        }

        ///<summary>
        ///Validates that the selected character entry exist in the Characters table.
        ///</summary>
        ///<param name="prompt">prompts user to enter the id for the selected operation</param>
        ///<param name="characterList">Dictionary with character id(key) and name of the character(value)</param>
        ///<returns>Tupel, boolean if true id exist inside the table else returns false, integer of selected the id</returns>
        public static (bool exist, int characterId) IsSelectedIdValid(string prompt, Dictionary<int, string> characterList)
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
        #endregion
    }
}