using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public interface ISpecialAbility
    {
        /// <summary>
        /// Выполняет специальное действие с учетом того, что юнит находится на позиции position армии myArmy.
        /// </summary>
        ActionResult DoSpecialAction(int position, IArmy myArmy, IArmy enemyArmy, Random random = null);
    }
}
