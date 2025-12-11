using System;

namespace PartyDatabase
{
    class UserInputHandler
    {
        private static int nameLengthLimit = 10;

        public static string AddName(string prompt)
        {
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