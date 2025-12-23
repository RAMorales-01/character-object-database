using System;

namespace PartyDatabase
{
    public class Vocation
    {
        public abstract class VocationBasics
        {
            public abstract string VocationName {get; set;}
            public abstract int VocationId {get; set;}
            public abstract string DefaultSkill {get; set;}
            public abstract string SkillLowLevel {get; set;}
            public abstract string SkillMediumLevel {get; set;}
            public abstract string SkillHighLevel {get; set;}

            public abstract void VocationInfo();
        }

        public class Fighter : VocationBasics
        {
            public override string VocationName {get; set;}
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Fighter(Character character)
            {
                VocationName = "Fighter";
                VocationId = 1;
                DefaultSkill = "[Parry]";

                SkillLowLevel = character != null && character.Dexterity >= 14 ? "[Deflect Missile]" : "[Locked]"; 
                SkillMediumLevel = character != null && character.Strength >= 16 ? "[Multi-Slash]" : "[Locked]";
                SkillHighLevel = character != null && character.Constitution >= 20 ? "[Withstand Deathblow]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Fighter: A balanced vocation and easy to use.");
                Console.WriteLine("Recommended stat distribution:");
                Console.WriteLine("Strength (Priority Medium), Constitution (Priority High), Dexterity (Priority Low)");
            }
        }

        public class Rouge : VocationBasics
        {
            public override string VocationName {get; set;}
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Rouge(Character character)
            {
                VocationName = "Rouge";
                VocationId = 2;
                DefaultSkill = "[Steal(Battle/Shop)]";

                SkillLowLevel = character != null && character.Strength >= 14 ? "[Deathblow]" : "[Locked]";
                SkillMediumLevel = character != null && character.Wisdom >= 16 ? "[Detect Traps]" : "[Locked]";
                SkillHighLevel = character != null && character.Dexterity >= 20 ? "[Uncanny Dodge]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Rouge: A vocation focused on skill and utility, medium difficulty.");
                Console.WriteLine("Recommended stat distribution:");
                Console.WriteLine("Strength (Priority Low), Dexterity (Priority High), Wisdom (Priority Medium)");
            }
        }

        public class Sorcerer : VocationBasics
        {
            public override string VocationName {get; set;}
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}
                
            public Sorcerer(Character character)
            {
                VocationName = "Sorcerer";
                VocationId = 3;
                DefaultSkill = "[Fireball]";

                SkillLowLevel = character != null && character.Dexterity >= 16 ? "[Quick Chanter]" : "[Locked]";
                SkillMediumLevel = character != null && character.Constitution >= 18 ? "[Unshakable Caster]" : "[Locked]";
                SkillHighLevel = character != null && character.Intelligence >= 20 ? "[Meteor]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Sorcerer: A vocation focused on magic and casting, hard to use.");
                Console.WriteLine("Recommended stat distribution:");
                Console.WriteLine("Constitution (Priority High), Dexterity (Priority Medium), Intelligence (Priority High)");
            }
        }

        public class Healer : VocationBasics
        {
            public override string VocationName {get; set;}
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Healer(Character character)
            {
                VocationName = "Healer";
                VocationId = 4;
                DefaultSkill = "[Heal]";

                SkillLowLevel = character != null && character.Intelligence >= 14 ? "[Expand Healing Radius]" : "[Locked]";
                SkillMediumLevel = character != null && character.Constitution >= 18 ? "[Deathblow Immunity]" : "[Locked]";
                SkillHighLevel = character != null && character.Wisdom >= 20 ? "[Resurrection]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Healer: A vocation focused on protecting allies by healing magic, very hard to use.");
                Console.WriteLine("Recommended stat distribution:");
                Console.WriteLine("Constitution (Priority High), Intelligence (Priority Low), Wisdom (Priority High)");
            }
        }

        public class Bard : VocationBasics
        {
            public override string VocationName {get; set;}
            public override int VocationId {get; set;}
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Bard(Character character)
            {
                VocationName = "Bard";
                VocationId = 5;
                DefaultSkill = "[Inspire]";

                SkillLowLevel = character != null && character.Dexterity >= 16 ? "[Evade & Parry]" : "[Locked]";
                SkillMediumLevel = character != null && character.Intelligence >= 18 ? "[Song of Bravery]" : "[Locked]";
                SkillHighLevel = character != null && character.Charisma >= 20 ? "[Charm]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Bard: A vocation focused on buffing using music to inspire allies, medium difficulty.");
                Console.WriteLine("Recommended stat distribution:");
                Console.WriteLine("Constitution (Priority High), Intelligence (Priority Low), Wisdom (Priority High)");
            }
        }
    }
}