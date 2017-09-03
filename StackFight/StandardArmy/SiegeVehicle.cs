using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecialUnits;

namespace StackFight.StandardArmy
{
    //адаптер гуляй-города
    public sealed class SiegeVehicle : IUnit
    {
        private readonly GulyayGorod _gorod;

        private const int UnitCost = 45;

        private const int MaxHealth = 100;
        private const int MaxDefence = 0;

        public int Cost { get { return _gorod.GetCost(); } }
        public int Defence {
            get { return _gorod.GetDefence(); }
            set { }
        }
        public int Health { get { return _gorod.GetCurrentHealth(); } }

        public int Power
        {
            get { return _gorod.GetStrength(); }
            set { }
        }

        private const string DefaultName = "Siege Vehicle";

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = string.IsNullOrEmpty(value) ? DefaultName : value; }
        }

        public SiegeVehicle(string name = null)
        {
             _gorod = new GulyayGorod(MaxHealth, MaxDefence, UnitCost);
            Name = name;
        }

        private SiegeVehicle(SiegeVehicle siegeVehicle)
        {
            _gorod = new GulyayGorod(siegeVehicle.Health, siegeVehicle.Defence, siegeVehicle.Cost);
            Name = siegeVehicle.Name;
        }

        public IUnit Clone()
        {
            return new SiegeVehicle(this);
        }

        public bool IsAlive()
        {
            return !(_gorod.IsDead);
        }

        public IUnit TakeDamage(int damage)
        {
            if (!_gorod.IsDead)
                _gorod.TakeDamage(damage);
            return this;
        }

        public override string ToString()
        {
            return Name + ". Health: " + Health + ". Defence: " + Defence + ". Power: " + Power;
        }
    }
}
