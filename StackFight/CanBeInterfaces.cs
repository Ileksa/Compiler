using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public interface ICanBeHealed
    {
        void TakeHealth(int health);
    }

    public interface ICanBeCloned
    {
        IUnit Clone();
    }
}
