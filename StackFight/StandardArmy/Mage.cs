using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy
{
    public class Mage : IUnit, ISpecialAbility, ICanBeHealed
    {
        private const int UnitCost = 55;
        private const int UnitPower = 12;
        private const int UnitDefence = 5;
        private const int MaxHealth = 30;

        private const double MagicProbability = 0.05;

        private const string DefaultName = "Mage";

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

        public Mage(string name = null)
        {
            _currentHealth = MaxHealth;
            _currentDefence = UnitDefence;
            _currentPower = UnitPower;
            Name = name;
        }

        private Mage(Mage mage)
        {
            Health = mage.Health;
            Defence = mage.Defence;
            Power = mage.Power;
            Name = mage.Name;
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

        public void TakeHealth(int health)
        {
            Health += health;
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
            return new Mage(this);
        }

        public override string ToString()
        {
            return Name + ". Health: " + Health + ". Defence: " + Defence + ". Power: " + Power +
                   " with magic power";
        }

        public ActionResult DoSpecialAction(int position, IArmy myArmy, IArmy enemyArmy, Random random = null)
        {
            if (random == null)
                random = new Random();

            double probability = random.NextDouble();

            //для лечения сгенерированная вероятность должна принадлежать промежутку [0, HealingProbability)
            if (probability >= MagicProbability)
                return ActionResult.Empty;

            List<ICanBeCloned> availibleForCloning = GetAvailibleForCloning(position, myArmy);

            if (availibleForCloning.Count == 0)
                return ActionResult.Empty;

            IUnit unitForCloning = (IUnit)availibleForCloning[random.Next(0, availibleForCloning.Count)];
            myArmy.AddUnit(unitForCloning, position + 1);

            return new ActionResult(this, unitForCloning, ActionResultType.Cloning);
        }

        List<ICanBeCloned> GetAvailibleForCloning(int position, IArmy myArmy)
        {
            List<ICanBeCloned> availibleForCloning = new List<ICanBeCloned>(5);

            ICanBeCloned unit;
            if (position % myArmy.Width > 0 && position > 0) //есть ли юниты вверху шеренги
            {
                unit = myArmy.GetUnit(position - 1) as ICanBeCloned;
                if (unit != null)
                    availibleForCloning.Add(unit);
            }

            if ((position + 1) % myArmy.Width > 0 && position < myArmy.Count - 1) //есть ли юниты внизу шеренги
            {
                unit = myArmy.GetUnit(position + 1) as ICanBeCloned;
                if (unit != null)
                    availibleForCloning.Add(unit);
            }

            if (position + myArmy.Width < myArmy.Count) //есть ли юниты по бокам
            {
                unit = myArmy.GetUnit(position + myArmy.Width) as ICanBeCloned;
                if (unit != null)
                    availibleForCloning.Add(unit);
            }

            if (position - myArmy.Width >= 0)
            {
                unit = myArmy.GetUnit(position - myArmy.Width) as ICanBeCloned;
                if (unit != null)
                    availibleForCloning.Add(unit);
            }

            return availibleForCloning;
        }
    }
}
