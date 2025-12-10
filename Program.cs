using System;

namespace PartyDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Character character = new Character(1, "Tanis Half-Elven", 10, 10, 10, 10, 10, 10);

            //For testing, remove after
            Console.WriteLine($"\nId: {character.Id}");
            Console.WriteLine($"Name: {character.Name}");
            Console.WriteLine($"Str: {character.Strenght}");
            Console.WriteLine($"Con: {character.Constitution}");
            Console.WriteLine($"Dex: {character.Dexterity}");
            Console.WriteLine($"Int: {character.Intelligence}");
            Console.WriteLine($"Wis: {character.Wisdom}");
            Console.WriteLine($"Cha: {character.Charisma}\n");
        }
    }
}