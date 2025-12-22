using System;

namespace PartyDatabase
{
    public class Vocation
    {
        public abstract class VocationBasics
        {
            public abstract int VocationId {get; set;}
            public abstract string DefaultSkill {get; set;}
            public abstract string SkillLowLevel {get; set;}
            public abstract string SkillMediumLevel {get; set;}
            public abstract string SkillHighLevel {get; set;}

            public abstract void VocationInfo();
        }

        public class Fighter : VocationBasics
        {
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Fighter(Character character)
            {
                VocationId = 01;
                DefaultSkill = "[Parry]";

                SkillLowLevel = character.Dexterity >= 14 ? "[Deflect Missile]" : "[Locked]"; 
                SkillMediumLevel = character.Strength >= 16 ? "[Multi-Slash]" : "[Locked]";
                SkillHighLevel = character.Constitution >= 20 ? "[Withstand Mortal Blow]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Fighter: A balanced vocation and easy to use.");
                Console.WriteLine("Recommended stats:");
                Console.WriteLine("Strength (Priority Medium), Constitution (Priority High), Dexterity (Priority Low)");
            }
        }

        public class Rouge : VocationBasics
        {
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Rouge(Character character)
            {
                VocationId = 02;
                DefaultSkill = "[Steal(Battle/Shop)]";

                SkillLowLevel = character.Strength >= 14 ? "[Deathblow]" : "[Locked]";
                SkillMediumLevel = character.Wisdom >= 16 ? "[Detect Traps]" : "[Locked]";
                SkillHighLevel = character.Dexterity >= 20 ? "[Uncanny Dodge]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Rouge: A vocation focused on skill and utility, medium difficulty.");
                Console.WriteLine("Recommended stats:");
                Console.WriteLine("Strength (Priority Low), Dexterity (Priority High), Wisdom (Priority Medium)");
            }
        }

        public class Sorcerer : VocationBasics
        {
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}
                
            public Sorcerer(Character character)
            {
                VocationId = 03;
                DefaultSkill = "[Fireball]";

                SkillLowLevel = character.Dexterity >= 16 ? "[Quick Chanter]" : "[Locked]";
                SkillMediumLevel = character.Constitution >= 18 ? "[Unshakable Caster]" : "[Locked]";
                SkillHighLevel = character.Intelligence >= 20 ? "[Meteor]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Sorcerer: A vocation focused on magic and casting, hard to use.");
                Console.WriteLine("Recommended stats:");
                Console.WriteLine("Constitution (Priority High), Dexterity (Priority Medium), Intelligence (Priority High)");
            }
        }

        public class Healer : VocationBasics
        {
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Healer(Character character)
            {
                VocationId = 04;
                DefaultSkill = "[Heal]";

                SkillLowLevel = character.Intelligence >= 14 ? "[Expand Healing Radius]" : "[Locked]";
                SkillLowLevel = character.Constitution >= 18 ? "[Deathblow Immunity]" : "[Locked]";
                SkillLowLevel = character.Wisdom >= 20 ? "[Resurrection]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Healer: A vocation focused on protecting allies by healing magic, very hard to use.");
                Console.WriteLine("Recommended stats:");
                Console.WriteLine("Constitution (Priority High), Intelligence (Priority Low), Wisdom (Priority High)");
            }
        }

        public class Bard : VocationBasics
        {
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Bard(Character character)
            {
                VocationId = 05;
                DefaultSkill = "[Inspire]";

                SkillLowLevel = character.Dexterity >= 16 ? "[Evade & Parry]" : "[Locked]";
                SkillLowLevel = character.Intelligence >= 18 ? "[Song of Bravery]" : "[Locked]";
                SkillLowLevel = character.Charisma >= 20 ? "[Charm]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Bard: A vocation focused on buffing using music to inspire allies, medium difficulty.");
                Console.WriteLine("Recommended stats:");
                Console.WriteLine("Constitution (Priority High), Intelligence (Priority Low), Wisdom (Priority High)");
            }
        }
    }
}