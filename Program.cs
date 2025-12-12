using System;

namespace PartyDatabase
{
    class Program
    {
        static void Main(string[] args)
        {  
            Console.Clear();
            Console.WriteLine("This program let the user create 6 characters(or accounts) each with its own stats.");
            Console.WriteLine("Finally the user will be able to add the character(or account) to the database, delete an existing one or create a new one.\n");
            Console.WriteLine("\nPress any key to begin.");
            Console.ReadKey(true);
            
            //for testing remove after
            Console.WriteLine("--Database Open--");
            Console.WriteLine("\nDo you want to create and insert a character?");
            Console.Write("Y/N: ");
            string userInput = Console.ReadLine();
            
            if(userInput == "y")
            {
                CharacterManager.VerifyDatabaseIsCreated();
            }
            else
            {
                Console.WriteLine("\nGoodbye!");
            }
        }
    }
}