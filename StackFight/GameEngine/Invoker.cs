using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.GameEngine
{
    public class Invoker
    {
        protected readonly Stack<ICommand> CanBeUndo = new Stack<ICommand>();
        protected readonly Stack<ICommand> CanBeReundo = new Stack<ICommand>();

        public IEnumerable<ActionResult> Execute(ICommand command)
        {
            CanBeReundo.Clear();
            CanBeUndo.Push(command);
            return command.Execute();
        }

        public ActionResult Undo()
        {
            if (CanBeUndo.Count == 0)
                return new ActionResult(ActionResultType.Error, "Нет операций, которые можно отменить.");

            ICommand command = CanBeUndo.Pop();
            ActionResult result = command.Undo();
            CanBeReundo.Push(command);

            return result;
        }

        public IEnumerable<ActionResult> Reundo()
        {
            if (CanBeReundo.Count == 0)
                return new List<ActionResult>()
                { new ActionResult(ActionResultType.Error, "Нет операций, которые можно повторить.")};

            ICommand command = CanBeReundo.Pop();
            CanBeUndo.Push(command);
            return command.Execute();
        } 
    }
}
