using System;

namespace CharacterJob
{
    ///<summary>
    ///To assign one of the current available jobs.
    ///</summary>
    public class Job
    {
        #region Abstract Class
        public abstract class JobBasics
        {
            public abstract string JobName {get; set;}
            public abstract int JobId {get; set;}
            public abstract string Ability {get; set;}
            public abstract string Skill1 {get; set;}
            public abstract string Skill2 {get; set;}
            public abstract string Skill3 {get; set;}
        }
        #endregion

        #region Current Available Job Classes
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
                JobName = "Fighter";
                JobId = 1;
                Ability = "[Parry]";

                Skill1 = character != null && character.Dexterity >= 14 ? "[Deflect Missile]" : "[-----]"; 
                Skill2 = character != null && character.Strength >= 16 ? "[Multi-Slash]" : "[-----]";
                Skill3 = character != null && character.Constitution >= 20 ? "[Withstand Deathblow]" : "[-----]";
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
                JobName = "Rogue";
                JobId = 2;
                Ability = "[Steal(Battle/Shop)]";

                Skill1 = character != null && character.Strength >= 14 ? "[Deathblow]" : "[-----]";
                Skill2 = character != null && character.Wisdom >= 16 ? "[Detect Traps]" : "[-----]";
                Skill3 = character != null && character.Dexterity >= 20 ? "[Uncanny Dodge]" : "[-----]";
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
                JobName = "Spellcaster";
                JobId = 3;
                Ability = "[Fireball]";

                Skill1 = character != null && character.Dexterity >= 16 ? "[Quick Chanter]" : "[-----]";
                Skill2 = character != null && character.Constitution >= 18 ? "[Unshakable Caster]" : "[-----]";
                Skill3 = character != null && character.Intelligence >= 20 ? "[Meteor]" : "[-----]";
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
                JobName = "Priest";
                JobId = 4;
                Ability = "[Heal]";

                Skill1 = character != null && character.Intelligence >= 14 ? "[Expand Healing Radius]" : "[-----]";
                Skill2 = character != null && character.Constitution >= 18 ? "[Deathblow Immunity]" : "[-----]";
                Skill3 = character != null && character.Wisdom >= 20 ? "[Resurrection]" : "[-----]";
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
                JobName = "Bard";
                JobId = 5;
                Ability = "[Inspire]";

                Skill1 = character != null && character.Dexterity >= 16 ? "[Evade & Parry]" : "[-----]";
                Skill2 = character != null && character.Intelligence >= 18 ? "[Song of Bravery]" : "[-----]";
                Skill3 = character != null && character.Charisma >= 20 ? "[Charm]" : "[-----]";
            }
        }
        #endregion
    }
}