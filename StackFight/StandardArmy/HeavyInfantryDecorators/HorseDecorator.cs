using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy.HeavyInfantryDecorators
{
    public class HorseDecorator : HeavyInfantryDecorator
    {
        public const int AttackPower = 3;
        public const int HorseDefence = 3;

        public HorseDecorator(IHeavyInfantry heavyInfantry) : base(heavyInfantry)
        {
            HeavyUnit.Power += AttackPower;
        }

        public override IUnit TakeDamage(int damage)
        {
            return base.TakeDamage(Math.Max(damage - HorseDefence, 0));
        }

        public override int Power { get { return base.Power + AttackPower; } }


        public override string GetImprovements()
        {
            return HeavyUnit.GetImprovements() + "[horse] ";
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
            return new HorseDecorator((IHeavyInfantry)HeavyUnit.Clone());
        }
    }
}
