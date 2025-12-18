using System;
using System.Threading;

namespace PartyDatabase
{
    ///<summary>
    ///The objective of this program is to test the interaction of the user with a database
    ///the user can view the current entries inside the database(in this case are characters in a dnd style)
    ///each entry has columns representing stats from the tabletop game.
    ///The user can create new characters and delete them. this program is more of practice for myself
    ///using a database inside c#
    ///</summary>
    class Program
    {
        static void Main(string[] args)
        {  
            /*Console.Clear();
            Console.WriteLine("This program let the user create characters each with its own stats. Delete existing ones, view stats and so on.");
            Console.WriteLine("\nPress any key to begin the test.");
            Console.ReadKey(true);

            //The Thread.Sleep is just for show, to simulate the database is loading.
            Console.Clear();
            Console.WriteLine("----- ACCESSING DATABASE -----");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.WriteLine("\n----- ACCESS GRANTED -----");
            Thread.Sleep(1500);
            Console.Clear();
            
            UserInputHandler.SelectionScreen();
        
            Console.Clear();
            Console.WriteLine("----- EXITING DATABASE -----");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write("GOODBYE!");*/

            
            /*Console.Clear();
            Character character = new Character("Alex", 16, 18, 10, 10, 10, 10);
            Console.WriteLine($"Name: {character.Name}");
            character.ChoosenVocation = new Vocation.Fighter();
            character.ChoosenVocation.BasicAbility(character);
            Console.WriteLine();*/

            //test 2, remove after
            Character character = new Character("Alex", 16, 18, 10, 10, 10, 10);
            character.AssignedVocation = new Vocation.Fighter(character);
            Console.WriteLine($"Name: {character.Name}");
            Console.WriteLine($"Skill: {character.AssignedVocation.DefaultSkill}");
            Console.WriteLine($"Skill: {character.AssignedVocation.SkillLowLevel}");
            Console.WriteLine($"Skill: {character.AssignedVocation.SkillMediumLevel}");
            Console.WriteLine($"Skill: {character.AssignedVocation.SkillHighLevel}");
            character.AssignedVocation.VocationInfo();
        }
    }
}