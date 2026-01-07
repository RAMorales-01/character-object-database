using System;

namespace CharacterJob
{
    public class Job
    {
        public abstract class JobBasics
        {
            public abstract string JobName {get; set;}
            public abstract int JobId {get; set;}
            public abstract string Ability {get; set;}
            public abstract string Skill1 {get; set;}
            public abstract string Skill2 {get; set;}
            public abstract string Skill3 {get; set;}
        }

        public class Fighter : JobBasics
        {
            public override string JobName {get; set;}
            public override int JobId {get; set;}
            public override string Ability {get; set;}
            public override string Skill1 {get; set;}
            public override string Skill2 {get; set;}
            public override string Skill3 {get; set;}

            public Fighter(Character character)
            {
                VocationName = "Fighter";
                VocationId = 1;
                DefaultSkill = "[Parry]";

                SkillLowLevel = character != null && character.Dexterity >= 14 ? "[Deflect Missile]" : "[-----]"; 
                SkillMediumLevel = character != null && character.Strength >= 16 ? "[Multi-Slash]" : "[-----]";
                SkillHighLevel = character != null && character.Constitution >= 20 ? "[Withstand Deathblow]" : "[-----]";
            }
        }

        public class Rogue : JobBasics
        {
            public override string JobName {get; set;}
            public override int JobId {get; set;}
            public override string Ability {get; set;}
            public override string Skill1 {get; set;}
            public override string Skill2 {get; set;}
            public override string Skill3 {get; set;}

            public Rogue(Character character)
            {
                VocationName = "Rogue";
                VocationId = 2;
                DefaultSkill = "[Steal(Battle/Shop)]";

                SkillLowLevel = character != null && character.Strength >= 14 ? "[Deathblow]" : "[-----]";
                SkillMediumLevel = character != null && character.Wisdom >= 16 ? "[Detect Traps]" : "[-----]";
                SkillHighLevel = character != null && character.Dexterity >= 20 ? "[Uncanny Dodge]" : "[-----]";
            }
        }

        public class Spellcaster : JobBasics
        {
            public override string JobName {get; set;}
            public override int JobId {get; set;}
            public override string Ability {get; set;}
            public override string Skill1 {get; set;}
            public override string Skill2 {get; set;}
            public override string Skill3 {get; set;}
                
            public Spellcaster(Character character)
            {
                VocationName = "Spellcaster";
                VocationId = 3;
                DefaultSkill = "[Fireball]";

                SkillLowLevel = character != null && character.Dexterity >= 16 ? "[Quick Chanter]" : "[-----]";
                SkillMediumLevel = character != null && character.Constitution >= 18 ? "[Unshakable Caster]" : "[-----]";
                SkillHighLevel = character != null && character.Intelligence >= 20 ? "[Meteor]" : "[-----]";
            }
        }

        public class Priest : JobBasics
        {
            public override string JobName {get; set;}
            public override int JobId {get; set;}
            public override string Ability {get; set;}
            public override string Skill1 {get; set;}
            public override string Skill2 {get; set;}
            public override string Skill3 {get; set;}

            public Priest(Character character)
            {
                VocationName = "Priest";
                VocationId = 4;
                DefaultSkill = "[Heal]";

                SkillLowLevel = character != null && character.Intelligence >= 14 ? "[Expand Healing Radius]" : "[-----]";
                SkillMediumLevel = character != null && character.Constitution >= 18 ? "[Deathblow Immunity]" : "[-----]";
                SkillHighLevel = character != null && character.Wisdom >= 20 ? "[Resurrection]" : "[-----]";
            }
        }

        public class Bard : JobBasics
        {
            public override string JobName {get; set;}
            public override int JobId {get; set;}
            public override string Ability {get; set;}
            public override string Skill1 {get; set;}
            public override string Skill2 {get; set;}
            public override string Skill3 {get; set;}

            public Bard(Character character)
            {
                VocationName = "Bard";
                VocationId = 5;
                DefaultSkill = "[Inspire]";

                SkillLowLevel = character != null && character.Dexterity >= 16 ? "[Evade & Parry]" : "[-----]";
                SkillMediumLevel = character != null && character.Intelligence >= 18 ? "[Song of Bravery]" : "[-----]";
                SkillHighLevel = character != null && character.Charisma >= 20 ? "[Charm]" : "[-----]";
            }
        }
    }
}