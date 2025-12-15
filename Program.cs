using System;

namespace PartyDatabase
{
    class Program
    {
        static void Main(string[] args)
        {  
            Console.Clear();
            Console.WriteLine("This program let the user create characters each with its own stats.");
            Console.WriteLine("Finally the user will be able to add the character to the database, delete an existing one or create or display stats.\n");
            Console.WriteLine("\nPress any key to begin.");
            Console.ReadKey(true);

            //testing, remove after
            CharacterManager.VerifyDatabaseIsCreated();
            UserInputHandler.MainScreen();
        }
    }
}