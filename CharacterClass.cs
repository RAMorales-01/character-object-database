using System;

namespace PartyDatabase
{
    class Character
    {
        //character stat must have a min value of 10 and a max value of 20
        public const int _minStatValue = 10;
        public const int _maxStatValue = 20;

        //permited limits for the nummbers of entries the database will contain
        public const int _minIdValue = 1;
        public const int _maxIdValue = 6;  

        private int _id;
        public int Id
        {
            get => _id;
            set 
            {
                if(value < _minIdValue || value > _maxIdValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(Id), $"Character id must be between {_minIdValue} and {_maxIdValue}.");
                }

                _id = value;
            }
        }
        
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
        
        public int Strength {get; private set;}
        public int Constitution {get; private set;}
        public int Dexterity {get; private set;}
        public int Intelligence {get; private set;}
        public int Wisdom {get; private set;}
        public int Charisma {get; private set;}

        public Character(int id, string name, int strength, int constitution, int dexterity, int intelligence, int wisdom, int charisma)
        {
            Id = id;
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