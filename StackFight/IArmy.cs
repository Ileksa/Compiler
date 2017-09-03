using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public interface IArmy : ISubject
    {
        string Name { get; set; }
        int Count { get; }
        int Width { get; set; }
   
        IUnit GetUnit(int position);

        void AddUnit(IUnit unit, int position);
        void ReplaceUnit(IUnit newUnit, int position);
        void ReplaceUnit(IUnit newUnit, IUnit oldUnit);
        bool IsEmpty();
        List<ActionResult> CollectKilledUnits();

        IArmy Clone();


    }
}
