using System;
using System.Collections.Generic;
using StackFight.StandardArmy.HeavyInfantryDecorators;

namespace StackFight.StandardArmy
{
    /// <summary>
    /// Легкая пехота, отличается слабой защитой и низкой стоимостью.
    /// </summary>
    public sealed class LightInfantry : IUnit, ICanBeHealed, ICanBeCloned, ISpecialAbility
    {
        private const int UnitCost = 29;
        private const int UnitPower = 20;
        private const int UnitDefence = 5;
        private const int MaxHealth = 40;

        private const double ImprovementHeavyUnitProbability = 0.1;

        private const string DefaultName = "Light";

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

        public LightInfantry(string name = null)
        {
            _currentHealth = MaxHealth;
            _currentDefence = UnitDefence;
            _currentPower = UnitPower;
            Name = name;
        }

        private LightInfantry(LightInfantry lightInfantry)
        {
            Health = lightInfantry.Health;
            Defence = lightInfantry.Defence;
            Power = lightInfantry.Power;
            Name = lightInfantry.Name;
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
            return new LightInfantry(this);
        }

        public ActionResult DoSpecialAction(int position, IArmy myArmy, IArmy enemyArmy, Random random = null)
        {
            if (random == null)
                random = new Random();

            double probability = random.NextDouble();

            //для улучшения сгенерированная вероятность должна принадлежать промежутку [0, ImprovementHeavyUnitProbability)
            if (probability >= ImprovementHeavyUnitProbability)
                return ActionResult.Empty;

            int[] availibleForImprovement = GetAvailibleForImprovement(position, myArmy).ToArray();
            
            if (availibleForImprovement.Length == 0)
                return ActionResult.Empty;

            int index = availibleForImprovement[random.Next(0, availibleForImprovement.Length)];
            int action = random.Next(0, 4);

            IHeavyInfantry unit = myArmy.GetUnit(index) as IHeavyInfantry;

            switch (action)
            {
                case 0:
                    if (unit.ContainsImprovement(typeof (HelmetDecorator)))
                        return ActionResult.Empty;
                    unit = new HelmetDecorator(unit);
                    myArmy.ReplaceUnit(unit, index);
                    break;
                case 1:
                    if (unit.ContainsImprovement(typeof(HorseDecorator)))
                        return ActionResult.Empty;
                    unit = new HorseDecorator(unit);
                    myArmy.ReplaceUnit(unit, index);
                    break;
                case 2:
                    if (unit.ContainsImprovement(typeof(ShieldDecorator)))
                        return ActionResult.Empty;
                    unit = new ShieldDecorator(unit);
                    myArmy.ReplaceUnit(unit, index);
                    break;
                case 3:
                    if (unit.ContainsImprovement(typeof(SwordDecorator)))
                        return ActionResult.Empty;
                    unit = new SwordDecorator(unit);
                    myArmy.ReplaceUnit(unit, index);
                    break;
                default:
                    return ActionResult.Empty;
            }
            return new ActionResult(this, unit, ActionResultType.Improvement);
        }

        List<int> GetAvailibleForImprovement(int position, IArmy myArmy)
        {
            List<int> availibleForImprovement = new List<int>(4);

            IHeavyInfantry unit;
            if (position % myArmy.Width > 0 && position > 0) //есть ли юниты вверху шеренги
            {
                unit = myArmy.GetUnit(position - 1) as IHeavyInfantry;
                if (unit != null)
                    availibleForImprovement.Add(position - 1);
            }

            if ((position + 1) % myArmy.Width > 0 && position < myArmy.Count - 1) //есть ли юниты внизу шеренги
            {
                unit = myArmy.GetUnit(position + 1) as IHeavyInfantry;
                if (unit != null)
                    availibleForImprovement.Add(position + 1);
            }

            if (position + myArmy.Width < myArmy.Count) //есть ли юниты по бокам
            {
                unit = myArmy.GetUnit(position + myArmy.Width) as IHeavyInfantry;
                if (unit != null)
                    availibleForImprovement.Add(position + myArmy.Width);
            }

            if (position - myArmy.Width >= 0)
            {
                unit = myArmy.GetUnit(position - myArmy.Width) as IHeavyInfantry;
                if (unit != null)
                    availibleForImprovement.Add(position - myArmy.Width);
            }

            return availibleForImprovement;
        }

        public override string ToString()
        {
            return Name + ". Health: " + Health + ". Defence: " + Defence + ". Power: " + Power;
        }
    }
}
