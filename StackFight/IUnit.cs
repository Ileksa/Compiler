using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public interface IUnit
    {
        string Name { get; set; }
        int Health { get; }
        int Power { get; set; }
        int Defence { get; set; }
        int Cost { get; }

        IUnit TakeDamage(int damage);
        bool IsAlive();

        IUnit Clone();
    }
}
