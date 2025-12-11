using System;

namespace PartyDatabase
{
    class UserInputHandler
    {
        ///<summary>
        ///To enter the name of each character, length limit to 10 characters long.
        ///</summary>
        ///<param name="prompt">prompts user to enter a name</param>
        ///<returns>string for the name of the character</returns>
        public static string AddName(string prompt)
        {
            const int nameLengthLimit = 10;//name length limit

            string nameInput = string.Empty;

            while(true)
            {
                Console.Clear();
                Console.WriteLine("NOTE: Character name cannot have more than 10 character in length.");
                Console.Write(prompt);
                nameInput = Console.ReadLine();

                if(!String.IsNullOrWhiteSpace(nameInput) && nameInput.Length <= nameLengthLimit)
                {
                    return nameInput;
                }
                else
                {
                    Console.WriteLine("\nERROR: Invalid input, name cannot be Null or Empty and must have less than 10 characters.\n");
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
            while(true)
            {
                int statTotal = 0;

                Console.Clear();
                Console.WriteLine("NOTE: Stat cannot be less than 10 and greater than 20.\n");
                Console.WriteLine($"Available points: {points}");
                Console.WriteLine($"Current {statName}: {minValue}\n");
                Console.Write(prompt);

                if(int.TryParse(Console.ReadLine(), out int statInput))
                {
                    statTotal = minValue + statInput;

                    bool userConfirmation = PointsConfirmation("Y/N: ", statInput, statName);

                    if(userConfirmation == true)
                    {
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
        ///Helper method for AddStatValue, to confirm the added points
        ///</summary>
        ///<param name="prompt">ask user to confirm is the points allocated to the current stat is correct</param>
        ///<param name="statInput">int input for how many points the user choose to add to the current stat</param>
        ///<param name="statName">name of the current stat at work</param>
        ///<returns>boolean, if true user confirm stat allocation as correct else user can change the points allocated</returns>
        private static bool PointsConfirmation(string prompt, int statInput, string statName)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine($"You added {statInput} points to the stat of {statName}, is this correct?");
                Console.Write(prompt);
                string userInput = Console.ReadLine().ToLower();

                if(string.Equals(userInput, "y", StringComparison.OrdinalIgnoreCase) || string.Equals(userInput, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if(string.Equals(userInput, "n", StringComparison.OrdinalIgnoreCase) || string.Equals(userInput, "no", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
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