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
            

            //for testing, remove after
            int id = 1;
            var character = CharacterManager.CreateCharacter(id);

            Console.Clear();
            Console.WriteLine($"Name: {character.Name}\n");
            Console.WriteLine($"Strength: {character.Strength}");
            Console.WriteLine($"Constitution: {character.Constitution}");
            Console.WriteLine($"Dexterity: {character.Dexterity}");
            Console.WriteLine($"Intelligence: {character.Intelligence}");
            Console.WriteLine($"Wisdom: {character.Wisdom}");
            Console.WriteLine($"Charisma: {character.Charisma}\n");
        }
    }
}