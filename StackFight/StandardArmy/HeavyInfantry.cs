using System;

namespace StackFight.StandardArmy
{
    /// <summary>
    /// Тяжелая пехота.
    /// </summary>
    public sealed class HeavyInfantry : ICanBeCloned, IHeavyInfantry
    {

        private const int UnitCost = 50;
        private const int UnitPower = 35;
        private const int UnitDefence = 15;
        private const int MaxHealth = 50;

        private const string DefaultName = "Heavy";

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = string.IsNullOrEmpty(value) ? DefaultName : value; }
        }

        private int _currentHealth;
        public int Health
        {
            get { return _currentHealth; }
            private set
            {
                if (value < 0)
                    _currentHealth = 0;
                else if (value > MaxHealth)
                    _currentHealth = MaxHealth;
                else
                    _currentHealth = value;
            }
        }

        private int _currentDefence;
        public int Defence
        {
            get { return _currentDefence; }
            set
            {
                if (value < 0)
                    _currentDefence = 0;
                else
                    _currentDefence = value;
            }
        }

        private int _currentPower;
        public int Power
        {
            get { return _currentPower; }
            set
            {
                if (value < 0)
                    _currentPower = 0;
                else
                    _currentPower = value;
            }
        }
        public int Cost { get { return UnitCost; } }

        public HeavyInfantry(string name = null)
        {
            _currentHealth = MaxHealth;
            _currentDefence = UnitDefence;
            _currentPower = UnitPower;
            Name = name;
        }

        private HeavyInfantry(HeavyInfantry heavyInfantry)
        {
            Health = heavyInfantry.Health;
            Defence = heavyInfantry.Defence;
            Power = heavyInfantry.Power;
            Name = heavyInfantry.Name;
        }

        public IUnit TakeDamage(int damage)
        {
            if (Defence > 0)
            {
                int defence = Defence;
                Defence -= damage;
                damage -= defence;
                if (damage <= 0)
                    return this;
            }

            Health -= damage;
            return this;
        }
        public bool IsAlive()
        {
            if (Health > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Осуществляет клонирование объекта с копированием текущего состояния копируемого объекта.
        /// </summary>
        public IUnit Clone()
        {
            return new HeavyInfantry(this);
        }

        public string GetImprovements() { return String.Empty; }
        public bool ContainsImprovement(Type type) { return false; }

        public override string ToString()
        {
            return Name + " " + GetImprovements() + ". Health: " + Health + ". Defence: " + Defence + ". Power: " + Power;
        }
    }
}
