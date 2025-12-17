using System;

namespace PartyDatabase
{
    public class Character
    {
        //character stat must have a min value of 10 and a max value of 20
        public const int _minStatValue = 10;
        public const int _maxStatValue = 20;
        
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Character name cannot be null or empty.");
                }
                
                _name = value;
            }
        }
        
        public int Id {get; set;}
        public int Strength {get; private set;}
        public int Constitution {get; private set;}
        public int Dexterity {get; private set;}
        public int Intelligence {get; private set;}
        public int Wisdom {get; private set;}
        public int Charisma {get; private set;}
        public Vocation.VocationAbility ChoosenVocation {get; set;}

        public Character(string name, int strength, int constitution, int dexterity, int intelligence, int wisdom, int charisma)
        {
            Name = name;

            Strength = ValidateStat(strength, nameof(Strength));
            Constitution = ValidateStat(constitution, nameof(Constitution));
            Dexterity = ValidateStat(dexterity, nameof(Dexterity));
            Intelligence = ValidateStat(intelligence, nameof(Intelligence));
            Wisdom = ValidateStat(wisdom, nameof(Wisdom));
            Charisma = ValidateStat(charisma, nameof(Charisma));
        }

        ///<summary>
        ///Helper method to validate that the stat value stays between the permited limits(min 10, max 20)
        ///</summary>
        private int ValidateStat(int statValue,  string statName)
        {
            if(statValue < _minStatValue || statValue > _maxStatValue)
            {
                throw new ArgumentOutOfRangeException(statName,  $"Stat value cannot be less than {_minStatValue} or greater than {_maxStatValue}.");
            }

            return statValue;
        }
    }
}