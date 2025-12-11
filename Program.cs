using System;

namespace PartyDatabase
{
    class Program
    {
        static void Main(string[] args)
        {  
            Console.Clear();
            Console.WriteLine("\nThis program let the user create 6 characters each with its own stats and then add them to a database.\n");
            Console.WriteLine("\nPress any key to begin.");
            Console.ReadKey(true);
            
            //For testing, remove after
            Console.Clear();
            int points = 30;
            Console.WriteLine("Lets add points to the character.");
            var name = UserInputHandler.AddName("Enter name: ");
            int assignedStat = UserInputHandler.AddStatValue("Add points: ", nameof(Character.Strength), Character._minStatValue, Character._maxStatValue, ref points);
            
            Console.WriteLine($"Character name is: {name}");
            Console.WriteLine($"Character Strength is: {assignedStat}");
            Console.WriteLine($"Remaining points: {points}");
        }
    }
}