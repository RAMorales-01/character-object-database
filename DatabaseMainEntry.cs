using System;
using UserHandler;
using DatabaseUtility;
using JobSelection;

namespace DatabaseMainEntry
{
    ///<summary>
    ///This class handles the main entry and the options the user
    ///can select in the Main menu and submenu.
    ///</summary>
    class DatabaseOptions
    {
        ///<summary>
        ///Main entry for the progaram
        ///</summary>
        public static void OpenDatabase()
        {
            //The Thread.Sleep is just for show, to simulate the database is loading.
            Console.Clear();
            Console.WriteLine("----- ACCESSING DATABASE -----");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.WriteLine("\n----- ACCESS GRANTED -----");
            Thread.Sleep(1500);
            Console.Clear();
            
            Input.ShowMainMenu();
            
            Console.Clear();
            Console.Write("\n\nGOODBYE!\n");
        }

        ///<summary>
        ///Handles the options for the database functions for the main menu
        ///</summary>
        ///<param name="selectedOption">int between 1 and 5 for the current available options in the main menu</param>
        public static void MainMenuOptions(int selectedOption)
        {

            switch(selectedOption)
            {
                case 1: DatabaseHandler.ViewAnEntryVerification();
                break;

                case 2: Input.ShowSubmenu();
                break;

                case 3: DatabaseHandler.InsertCharacterToDatabase(DatabaseHandler.CreateCharacter());
                break;
                
                case 4: DatabaseHandler.DeleteAnEntryVerification();
                break;

                default: throw new ArgumentException($"Invalid operation selected, please select only one of the presented options", nameof(selectedOption));
            }
        }

        ///<summary>
        ///Handles the options for the database functions for the submenu
        ///</summary>
        ///<param name="selectedOption">int between 1 and 2 for the current available options in the submenu</param>
        public static void SubmenuOptions(int selectedOption)
        {
            switch(selectedOption)
            {
                case 1: DatabaseHandler.ViewRaceInformation();
                break;

                case 2: DatabaseHandler.DisplayJobTable(); 
                break;

                case 3: Input.ShowMainMenu();
                break;

                default: throw new ArgumentException($"Invalid operation selected, please select only one of the presented options", nameof(selectedOption));
            }
        }
    }
}