using System;
using DatabaseLogic;
using DatabaseUtility;

namespace UserHandler
{
    ///<summary>
    ///This class will handle all user related interactions with the database
    ///</summary>
    class Input
    {
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
                            DatabaseFunctions.MainMenuOptions(isInputValid.Item2);
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
                string keepDatabaseOpen = UserConfirmation("Y/N: ");

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

        ///<summary>
        ///Prompts user to confirm the selection of an action
        ///</summary>
        ///<param name="prompt">prompts user for input of 'y' or 'n'</param>
        ///<returns>string with value of "yes" or "no"</returns>
        private static string UserConfirmation(string prompt)
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
    }
}