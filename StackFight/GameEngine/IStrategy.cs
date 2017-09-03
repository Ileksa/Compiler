using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.GameEngine
{
    public interface IStrategy
    {
        IEnumerable<ActionResult> Fight(IArmy firstArmy, IArmy secondArmy, Random random = null);
    }
}
