using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public interface IArmyFactory
    {
        IArmy GetArmy (int cost);

        /// <summary>
        /// Получить юнита, чья стоимость не больше заданной.
        /// </summary>
        /// <param name="cost">Максимальная стоимость юнита.</param>
        IUnit GetRandomUnit(int cost);
    }
}
