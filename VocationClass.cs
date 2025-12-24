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

                SkillLowLevel = character != null && character.Dexterity >= 14 ? "[Deflect Missile]" : "[-----]"; 
                SkillMediumLevel = character != null && character.Strength >= 16 ? "[Multi-Slash]" : "[-----]";
                SkillHighLevel = character != null && character.Constitution >= 20 ? "[Withstand Deathblow]" : "[-----]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("A balanced vocation and easy to use.");
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

                SkillLowLevel = character != null && character.Strength >= 14 ? "[Deathblow]" : "[-----]";
                SkillMediumLevel = character != null && character.Wisdom >= 16 ? "[Detect Traps]" : "[-----]";
                SkillHighLevel = character != null && character.Dexterity >= 20 ? "[Uncanny Dodge]" : "[-----]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("A vocation focused on skill and utility, medium difficulty.");
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

                SkillLowLevel = character != null && character.Dexterity >= 16 ? "[Quick Chanter]" : "[-----]";
                SkillMediumLevel = character != null && character.Constitution >= 18 ? "[Unshakable Caster]" : "[-----]";
                SkillHighLevel = character != null && character.Intelligence >= 20 ? "[Meteor]" : "[-----]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("A vocation focused on magic and casting, hard to use.");
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

                SkillLowLevel = character != null && character.Intelligence >= 14 ? "[Expand Healing Radius]" : "[-----]";
                SkillMediumLevel = character != null && character.Constitution >= 18 ? "[Deathblow Immunity]" : "[-----]";
                SkillHighLevel = character != null && character.Wisdom >= 20 ? "[Resurrection]" : "[-----]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("A vocation focused on protecting allies by healing magic, very hard to use.");
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

                SkillLowLevel = character != null && character.Dexterity >= 16 ? "[Evade & Parry]" : "[-----]";
                SkillMediumLevel = character != null && character.Intelligence >= 18 ? "[Song of Bravery]" : "[-----]";
                SkillHighLevel = character != null && character.Charisma >= 20 ? "[Charm]" : "[-----]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("A vocation focused on buffing using music to inspire allies, medium difficulty.");
                Console.WriteLine("Recommended stat distribution:");
                Console.WriteLine("Constitution (Priority High), Intelligence (Priority Low), Wisdom (Priority High)");
            }
        }
    }
}