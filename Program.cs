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
        }
    }
}