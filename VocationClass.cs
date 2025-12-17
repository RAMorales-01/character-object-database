using System;

namespace PartyDatabase
{
    public class Vocation
    {
        public abstract class VocationAbility { public abstract void BasicAbility(Character character); }

        class Fighter : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("- Default class ability: Second Wind.");

                if(strength >= 16)
                {
                    Console.WriteLine("- Unlocked ability: Shield Bash.");
                }

                if(constitution >= 18)
                {
                    Console.WriteLine("- Unlocked ability: Deflect Attack.");
                }
            }
        }

        class Barbarian : VocationAbility
        {
            public override void BasicAbility(Character character)
            {
                Console.WriteLine("- Default class ability: Rage.");

                if(strength >= 18)
                {
                    Console.WriteLine("- Unlocked ability: Frenzy.");
                }

                if(dexterity >= 16)
                {
                    Console.WriteLine("- Unlocked ability: Sprint.");
                }
            }
        }
    }
}