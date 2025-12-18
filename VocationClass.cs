using System;

namespace PartyDatabase
{
    public class Vocation
    {
        public abstract class VocationBasics
        {
            public abstract string DefaultSkill {get; set;}
            public abstract string SkillLowLevel {get; set;}
            public abstract string SkillMediumLevel {get; set;}
            public abstract string SkillHighLevel {get; set;}

            public abstract void VocationInfo();
        }

        public class Fighter : VocationBasics
        {
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Fighter(Character character)
            {
                DefaultSkill = "[Parry]";

                SkillMediumLevel = character.Strength >= 16 ? "[Multi-Slash]" : "[Locked]";
                SkillHighLevel = character.Constitution >= 20 ? "[Withstand Mortal Blow]" : "[Locked]";
                SkillLowLevel = character.Dexterity >= 14 ? "[Deflect Missile]" : "[Locked]"; 
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
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}

            public Rouge(Character character)
            {
                DefaultSkill = "[Steal(Battle/Shop)]";

                SkillLowLevel = character.Strength >= 14 ? "[DeathBlow]" : "[Locked]";
                SkillHighLevel = character.Dexterity >= 20 ? "[Uncanny Dodge]" : "[Locked]";
                SkillMediumLevel = character.Wisdom >= 16 ? "[Detect Traps]" : "[Locked]";
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
            public override string DefaultSkill {get; set;}
            public override string SkillLowLevel {get; set;}
            public override string SkillMediumLevel {get; set;}
            public override string SkillHighLevel {get; set;}
                
            public Sorcerer(Character character)
            {
                DefaultSkill = "[Fireball]";

                SkillMediumLevel = character.Constitution >= 18 ? "[Unshakable Caster]" : "[Locked]";
                SkillLowLevel = character.Dexterity >= 16 ? "[Quick Chanter]" : "[Locked]";
                SkillHighLevel = character.Intelligence >= 20 ? "[Meteor]" : "[Locked]";
            }

            public override void VocationInfo()
            {
                Console.WriteLine("Sorcerer: A vocation focused on magic and casting, hard to use.");
                Console.WriteLine("Recommended stats:");
                Console.WriteLine("Constitution (Priority High), Dexterity (Priority Medium), Intelligence (Priority High)");
            }
        }
    }
}