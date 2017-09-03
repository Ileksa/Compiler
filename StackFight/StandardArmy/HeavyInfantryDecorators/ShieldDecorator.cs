using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy.HeavyInfantryDecorators
{
    public class ShieldDecorator : HeavyInfantryDecorator
    {
        public const int ShieldDefence = 5;
        public const string ShieldName = "steel shield";

        public ShieldDecorator(IHeavyInfantry heavyInfantry) : base(heavyInfantry)
        { }

        public override IUnit TakeDamage(int damage)
        {
            return base.TakeDamage(Math.Max(damage - ShieldDefence, 0));
        }

        public override string GetImprovements()
        {
            return HeavyUnit.GetImprovements() + "[" + ShieldName + "] ";
        }

        public override bool ContainsImprovement(Type type)
        {
            if (this.GetType() == type)
                return true;

            return HeavyUnit.ContainsImprovement(type);
        }

        public override string ToString()
        {
            return GetImprovements() + ". Health: " + Health + ". Defence: " + Defence + ". Power: " + Power;
        }

        public override IUnit Clone()
        {
            return new ShieldDecorator((IHeavyInfantry)HeavyUnit.Clone());
        }
    }
}
