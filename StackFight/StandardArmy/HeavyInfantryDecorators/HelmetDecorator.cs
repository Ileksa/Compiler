using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy.HeavyInfantryDecorators
{
    public class HelmetDecorator : HeavyInfantryDecorator
    {
        public const int HelmetDefence = 4;

        public HelmetDecorator(IHeavyInfantry heavyInfantry) : base(heavyInfantry)
        { }

        public override IUnit TakeDamage(int damage)
        {
            return base.TakeDamage(Math.Max(damage - HelmetDefence, 0));
        }

        public override string GetImprovements()
        {
            return HeavyUnit.GetImprovements() + "[helmet] ";
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
            return new HelmetDecorator((IHeavyInfantry) HeavyUnit.Clone());
        }
    }
}
