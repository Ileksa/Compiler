using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public abstract class HeavyInfantryDecorator : IHeavyInfantry
    {
        protected IHeavyInfantry HeavyUnit;
        protected double RemoveProbability = 0.1; //вероятность потерять улучшение

        protected HeavyInfantryDecorator(IHeavyInfantry heavyInfantry)
        {
            if (heavyInfantry == null)
                throw new ArgumentNullException();
            HeavyUnit = heavyInfantry;
        }


        public string Name
        {
            get { return HeavyUnit.Name/*GetImprovements()*/; }
            set { HeavyUnit.Name = value; }
        }

        public int Health { get { return HeavyUnit.Health; } }

        public virtual int Power
        {
            get { return HeavyUnit.Power; }
            set { HeavyUnit.Power = value; }
        }

        public virtual int Defence
        {
            get { return HeavyUnit.Defence; }
            set { HeavyUnit.Defence = value; }
        }

        public virtual int Cost { get { return HeavyUnit.Cost; } }

        public virtual IUnit TakeDamage(int damage)
        {
            Random random = new Random();

            if (random.NextDouble() < RemoveProbability)
            {
                HeavyUnit = (IHeavyInfantry) HeavyUnit.TakeDamage(damage);
                return HeavyUnit;
            }

            return this;
        }

        public bool IsAlive() { return HeavyUnit.IsAlive(); }

        public virtual IUnit Clone() { return HeavyUnit.Clone(); }

        public virtual string GetImprovements() { return HeavyUnit.GetImprovements(); }
        public virtual bool ContainsImprovement(Type type) { return HeavyUnit.ContainsImprovement(type); }
    }
}
