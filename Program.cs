using System;
using DatabaseMainEntry;
//for testing remove after.
using CharacterCreation;
using RaceSelection;
using JobSelection;

namespace CRUDTest
{
    ///<summary>
    ///The objective of this program is to test the interaction of the user using a basic CRUD database
    ///the user can view the current entries inside the database(in this case are characters in a dnd style)
    ///each entry has columns representing stats from the tabletop game.
    ///The user can create new characters and delete them. this program is more of test on how to use basic SQL in c#.
    ///</summary>
    class Program
    {
        static void Main(string[] args)
        {  
            Console.Clear();
            Console.WriteLine("This program let the user create characters assign stats, select race and job.\nDelete existing ones, view stats and so on.\n");
            Console.WriteLine("\nPress any key to begin the test.");
            Console.ReadKey(true);

            DatabaseOptions.OpenDatabase();

            /*For testing remove after.
            Console.WriteLine();
            Character test = new Character("Alex", 16, 20, 14, 10, 10, 10);
            test.SetRace(new Race.Human(test));
            test.SetJob(new Job.Fighter(test));

            Console.WriteLine($"{test.Name}\n{test.AssignedRace.RaceName}\n{test.AssignedRace.RaceTrait}\n{test.AssignedJob.JobName}\n{test.AssignedJob.Ability}\n{test.AssignedJob.Skill1}\n{test.AssignedJob.Skill2}\n{test.AssignedJob.Skill3}");
            Console.WriteLine($"{test.Strength}\n{test.Constitution}\n{test.Dexterity}\n{test.Intelligence}\n{test.Wisdom}\n{test.Charisma}");
            Console.ReadKey();*/
        }
    }
}