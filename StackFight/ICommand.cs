using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public interface ICommand
    {
        IEnumerable<ActionResult> Execute();
        ActionResult Undo();
    }
}
