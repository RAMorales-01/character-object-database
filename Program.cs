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
            
            Console.Clear();
            Console.WriteLine("Welcome user! let's begin with the creation of your party of 6 characters. Press any key to continue.\n");
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

            int id2 = 2;
            var character2 = CharacterManager.CreateCharacter(id);

            Console.WriteLine($"Name: {character2.Name}\n");
            Console.WriteLine($"Strength: {character2.Strength}");
            Console.WriteLine($"Constitution: {character2.Constitution}");
            Console.WriteLine($"Dexterity: {character2.Dexterity}");
            Console.WriteLine($"Intelligence: {character2.Intelligence}");
            Console.WriteLine($"Wisdom: {character2.Wisdom}");
            Console.WriteLine($"Charisma: {character2.Charisma}");
        }
    }
}