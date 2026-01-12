using System;
using UserHandler;
using DatabaseUtility;
using DataEntry;

namespace DatabaseMainEntry
{
    class DatabaseOptions
    {
        public static void OpenDatabase()
        {
            Console.WriteLine("Test Succesful. Press any key to continue.");
            Console.ReadKey();
        }

        ///<summary>
        ///Handles the options for the database functions for the main menu
        ///</summary>
        ///<param name="selectedOption">int between 1 and 5 for the current available options in the main menu</param>
        public static void MainMenuOptions(int selectedOption)
        {

            switch(selectedOption)
            {
                case 1: DatabaseHandler.DisplayCharacterTable(DatabaseHandler.GetIdAndName("characters"));
                break;

                case 2: Input.ShowSubmenu();
                break;

                case 3: DatabaseHandler.InsertCharacterToDatabase(DatabaseHandler.CreateCharacter());
                break;
                
                case 4: DatabaseHandler.DeleteAnEntryVerification();
                break;

                case 5: //TODO: Exit database
                break;

                default: throw new ArgumentException($"Invalid operation selected", nameof(selectedOption));
            }
        }

        ///<summary>
        ///Handles the options for the database functions for the  submenu
        ///</summary>
        ///<param name="selectedOption">int between 1 and 2 for the current available options in the submenu</param>
        public static void SubmenuOptions(int selectedOption)
        {
            Console.Clear();

            switch(selectedOption)
            {
                case 1: DatabaseHandler.ViewAnEntryVerification();
                break;

                case 2: DatabaseHandler.DisplayRaceTable();
                break;

                case 3: DatabaseHandler.DisplayJobTable(); 
                break;

                case 4: Input.ShowMainMenu();
                break;

                default: throw new ArgumentException($"Invalid operation selected", nameof(selectedOption));
            }
        }
    }
}