using System;

namespace PartyDatabase
{
    public class Vocation
    {
        public abstract class VocationAbility { public abstract void BasicAbility(Character character); }

        public class Fighter : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation Skill [Second Wind]");
                Console.WriteLine("Skills Unlocked:");

                Console.WriteLine(character.Strength >= 16 ? "- [Shield Bash]" : "- ????");
                Console.WriteLine(character.Constitution >= 18 ? "- [Deflect Attack]" : "- ????");
            }
        }

        class Barbarian : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation Skill [Rage]");
                Console.WriteLine("Skills Unlocked:");

                Console.WriteLine(character.Strength >= 18 ? "- [Frenzy]" : "- ????");
                Console.WriteLine(character.Dexterity >= 16 ? "- [Sprint]" : "- ????");
            }
        }
    }
}