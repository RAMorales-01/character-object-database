using System;
using DataEntry;

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
                RaceTrait = "+15 on all Physical and Offenive Magic";
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
                RaceTrait = "+20 on all Magic and +5 on all Healing Magic";
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
                RaceTrait = "+30 on Offenive Fire and Dark Magic";
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
                RaceTrait = "+10 on Initiative and +25 in Agility";
            }
        }
        #endregion
    }
}