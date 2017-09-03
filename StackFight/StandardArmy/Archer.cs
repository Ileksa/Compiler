using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy
{
    public class Archer : IUnit, ISpecialAbility, ICanBeHealed, ICanBeCloned
    {
        private const int UnitCost = 35;
        private const int UnitPower = 15;
        private const int UnitDefence = 8;
        private const int MaxHealth = 30;

        private const int Range = 8;
        private const int ArcheryPower = 7;
        private const double ArcheryProbability = 0.5;

        private const string DefaultName = "Archer";

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

        public Archer(string name = null)
        {
            _currentHealth = MaxHealth;
            _currentDefence = UnitDefence;
            _currentPower = UnitPower;
            Name = name;
        }

        private Archer(Archer archer)
        {
            Health = archer.Health;
            Defence = archer.Defence;
            Power = archer.Power;
            Name = archer.Name;
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
        public IUnit Clone() { return new Archer(this); }

        public override string ToString()
        {
            return Name + ". Health: " + Health + ". Defence: " + Defence + ". Power: " + Power +
                ". Archery power: " + ArcheryPower + ". Range: " + Range;
        }

        public ActionResult DoSpecialAction(int position, IArmy myArmy, IArmy enemyArmy, Random random = null)
        {
            if (random == null)
                random = new Random();

            double probability = random.NextDouble();

            //для выстрела сгенерированная вероятность должна принадлежать промежутку [0, ArcheryProbability)
            if (probability >= ArcheryProbability)
                return ActionResult.Empty;

            int range = Range - (int) position/myArmy.Width;
            int start = position % myArmy.Width;

            if (range <= 0)
                return ActionResult.Empty;

            List<IUnit> availibleForAttack = GetAvailibleForAttackUnits(start, range, enemyArmy);
            if (availibleForAttack.Count == 0)
                return ActionResult.Empty;

            int unitForAttack = random.Next(0, availibleForAttack.Count);

            IUnit resultUnit = availibleForAttack[unitForAttack].TakeDamage(ArcheryPower);
            if (resultUnit != availibleForAttack[unitForAttack])
                enemyArmy.ReplaceUnit(resultUnit, availibleForAttack[unitForAttack]);

            return new ActionResult(this, availibleForAttack[unitForAttack], ActionResultType.Shot);
        }

        List<IUnit> GetAvailibleForAttackUnits(int start, int range, IArmy enemyArmy)
        {
            List<IUnit> availibleForAttack = new List<IUnit>(Range);
 
            //обойдем текущий ряд и выше
            int currentRange = range;
            for (int i = start; i >= 0 && currentRange > 0; i--)
            {
                for (int j = 0; j < currentRange; j++)
                {
                    int pos = i + j*enemyArmy.Width;
                    if (pos >= enemyArmy.Count)
                        break;
                    availibleForAttack.Add(enemyArmy.GetUnit(pos));
                }

                currentRange--;
            }

            //обойдем нижние ряды
            currentRange = range - 1;
            for (int i = start + 1; i < enemyArmy.Width && currentRange > 0; i++)
            {
                for (int j = 0; j < currentRange; j++)
                {
                    int pos = i + j * enemyArmy.Width;
                    if (pos >= enemyArmy.Count)
                        break;
                    availibleForAttack.Add(enemyArmy.GetUnit(pos));
                }

                currentRange--;
            }

            return availibleForAttack;
        } 
    }
}
