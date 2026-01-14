using System;
using CharacterCreation;

namespace RaceSelection
{
    ///<summary>
    ///To assign one of the current available races.
    ///</summary>
    public class Race
    {
        #region Abstract Class
        public abstract class RaceBasics
        {
            public abstract string RaceName {get; set;}
            public abstract int RaceId {get; set;}
            public abstract string RaceTrait {get; set;}
        }
        #endregion

        #region Current Available Race Classes
        public class Human : RaceBasics
        {
            public override string RaceName {get; set;}
            public override int RaceId {get; set;}
            public override string RaceTrait {get; set;}

            public Human(Character character)
            {
                RaceName = "Human";
                RaceId = 1;
                RaceTrait = "+5 on initiative";
            }
            public Human()//This constructor without parameters is to populate the Races Table.
            {
                RaceName = "Human";
                RaceId = 1;
                RaceTrait = "+5 on initiative";
            }
        }

        public class Elven : RaceBasics
        {
            public override string RaceName {get; set;}
            public override int RaceId {get; set;}
            public override string RaceTrait {get; set;}

            public Elven(Character character)
            {
                RaceName = "Elven";
                RaceId = 2;
                RaceTrait = "+15 healing power";
            }
            public Elven()//This constructor without parameters is to populate the Races Table.
            {
                RaceName = "Elven";
                RaceId = 2;
                RaceTrait = "+15 healing power";
            }
        }

        public class Fiendblood: RaceBasics
        {
            public override string RaceName {get; set;}
            public override int RaceId {get; set;}
            public override string RaceTrait {get; set;}

            public Fiendblood(Character character)
            {
                RaceName = "Fiendblood";
                RaceId = 3;
                RaceTrait = "+10 fire resistance";
            }
            public Fiendblood()//This constructor without parameters is to populate the Races Table.
            {
                RaceName = "Fiendblood";
                RaceId = 3;
                RaceTrait = "+10 fire resistance";
            }
        }

        public class Beastfolk : RaceBasics
        {
            public override string RaceName {get; set;}
            public override int RaceId {get; set;}
            public override string RaceTrait {get; set;}

            public Beastfolk(Character character)
            {
                RaceName = "Beastfolk";
                RaceId = 4;
                RaceTrait = "+15 on evasion";
            }
            public Beastfolk()//This constructor without parameters is to populate the Races Table.
            {
                RaceName = "Beastfolk";
                RaceId = 4;
                RaceTrait = "+15 on evasion";
            }
        }
        #endregion
    }
}