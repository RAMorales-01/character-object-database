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
                Console.WriteLine("Vocation: Fighter");
                Console.WriteLine("Vocation Skill [Second Wind]");
                Console.WriteLine("Skills Unlocked:");

                Console.WriteLine(character.Strength >= 16 ? "- [Double Slash]" : "- ????");
                Console.WriteLine(character.Constitution >= 18 ? "- [Master Defence]" : "- ????");
                Console.WriteLine(character.Dexterity >= 14 ? "- [Deflect Arrow]" : "- ????");
            }
        }

        public class Barbarian : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation: Barbarian");
                Console.WriteLine("Vocation Skill [Rage]");
                Console.WriteLine("Skills Unlocked:");

                Console.WriteLine(character.Strength >= 18 ? "- [Whirlwind Attack]" : "- ????");
                Console.WriteLine(character.Constitution >= 14 ? "- [Rock Skin]" : "- ????");
                Console.WriteLine(character.Dexterity >= 16 ? "- [Savage Sprint]" : "- ????");
            }
        }

        public class Rouge : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation: Rouge");
                Console.WriteLine("Vocation Skill [Lockpicking]");
                Console.WriteLine("Skill Unlocked:");

                Console.WriteLine(character.Constitution >= 14 ? "- [Parry Attack]" : "- ????");
                Console.WriteLine(character.Dexterity >= 18 ? "- [Deathblow]" : "- ????");
                Console.WriteLine(character.Wisdom >= 16 ? "- [Detect Trap]" : "- ????");
            }
        }

        public class Spellcaster : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation: Spellcaster");
                Console.WriteLine("Vocation Skill [Fireball]");
                Console.WriteLine("Skill Unlocked:");

                Console.WriteLine(character.Constitution >= 16 ? "- [Absolute Concentration]" : "- ????");
                Console.WriteLine(character.Intelligence >= 18 ? "- [Firestorm]" : "- ????");
                Console.WriteLine(character.Wisdom >= 14 ? "- [Fire Mastery]" : "- ????");
            }
        }

        public class Priest : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation: Priest");
                Console.WriteLine("Vocation Skill [Heal]");
                Console.WriteLine("Skill Unlocked:");

                Console.WriteLine(character.Constitution >= 14 ? "- [Disease Immunity]" : "- ????");
                Console.WriteLine(character.Wisdom >= 18 ? "- [Heal All]" : "- ????");
                Console.WriteLine(character.Charisma >= 16 ? "- [Store Discount]" : "- ????");
            }
        }

        public class Bard : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("Vocation: Bard");
                Console.WriteLine("Vocation Skill [Song of Bravery]");
                Console.WriteLine("Skill Unlocked:");

                Console.WriteLine(character.Dexterity >= 16 ? "- [Unncany Dodge]" : "- ????");
                Console.WriteLine(character.Intelligence >= 14 ? "- [Song of Speedster]" : "- ????");
                Console.WriteLine(character.Charisma >= 16 ? "- [Charm]" : "- ????");
            }
        }
    }
}