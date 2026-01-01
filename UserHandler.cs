using System;

namespace UserHandler
{
    ///<summary>
    ///This class will handle all user related interactions with the database
    ///</summary>
    class Input
    {
        //Representation of the Initial menu options 
        private enum MainMenu
        {
            DisplayEntry = 1,
            DisplaySubmenu,
            CreateEntry,             
            DeleteEntry, 
            ExitDatabase
        }

        ///<summary>
        ///Selection screen for initial menu
        ///</summary>
        public static void MainMenu()
        {
            CharacterManager.VerifyDatabaseIsCreated();//Verifies the database exist, if not is created along with all the tables.

            //Takes the int values of the first element and last element of the enum, to limit the user options.
            int minPermited = (int)MainMenu.DisplayEntry;
            int maxPermited = (int)MainMenu.ExitDatabase;
            
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the main menu, please select an option\n");
                Console.WriteLine("1- Display list of entries\n2- Display submenu\n3- Create entry\n4- Delete entry\n5- Exit");
                Console.Write("\n: ");

                if(int.TryParse(Console.ReadLine(), out int selectedOption) && selectedOption >= minPermited || selectedOption <= maxPermited)
                {
                    if(selectedOption == maxPermited){ break; }

                    else
                    {
                        try
                        {
                            DatabaseMainMenuOptions();
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
        ///Prompts user to confirm a selected action
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
    }
    }
}