using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy.HeavyInfantryDecorators
{
    public class SwordDecorator : HeavyInfantryDecorator
    {
        public const int SwordPower = 5;
        public const string SwordName = "steel sword";

        public SwordDecorator(IHeavyInfantry heavyInfantry) : base(heavyInfantry)
        {
            HeavyUnit.Power += SwordPower;
        }

        public override int Power { get { return base.Power + SwordPower; } }

        public override string GetImprovements()
        {
            return HeavyUnit.GetImprovements() + "[" + SwordName + "] ";
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
            return new SwordDecorator((IHeavyInfantry)HeavyUnit.Clone());
        }
    }
}
