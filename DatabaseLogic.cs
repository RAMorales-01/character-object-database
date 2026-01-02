using System;

namespace DatabaseLogic
{
    class DatabaseFuntions
    {
        ///<summary>
        ///Handles the options for the database functions for the main menu
        ///</summary>
        ///<param name="selectedOption">int between 1 and 5 for the current available options in the main menu</param>
        public static void MainMenuOptions(int selectedOption)
        {

            switch(selectedOption)
            {
                case 1: //TODO: EntryManager --- Display list of available entries
                break;

                case 2: //TODO: EntryManager --- Display Submenu
                break;

                case 3: //TODO: EntryManager --- Create entry
                break;
                
                case 4: //TODO: EntryManager --- Delete entry
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
            switch(selectedOption)
            {
                case 1: //TODO: EntryManger --- Display entry info
                break;

                case 2: //TODO: Go back to main menu
                break;

                default: throw new ArgumentException($"Invalid operation selected", nameof(selectedOption));
            }
        }
    }
}