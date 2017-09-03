using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public enum ActionResultType { Ok, Error, Attack, Death, Healing, Improvement, Cloning, Shot }
    public class ActionResult
    {
        public static readonly ActionResult Empty = new ActionResult(null, null, ActionResultType.Ok);

        public IUnit Subject { get; protected set; }
        public IUnit Object { get; protected set; }
        public ActionResultType Action { get; protected set; }
        public string Message { get; protected set; }

        /// <param name="subject">Юнит, совершивший действие.</param>
        /// <param name="obj">Юнит, к которому применено действие.</param>
        /// <param name="action">Тип воздействия.</param>
        public ActionResult(IUnit subject, IUnit obj, ActionResultType action)
        {
            Subject = subject;
            Object = obj;
            Action = action;
            Message = String.Empty;
        }

        /// <summary>
        /// Конструктор для состояний Ok и Error, то есть для случаев, когда необходимо вывести сообщение.
        /// </summary>
        /// <param name="action">Состояние Ok или Error.</param>
        /// <param name="message">Текст сообщения.</param>
        public ActionResult(ActionResultType action, string message)
        {
            if (action != ActionResultType.Ok && action != ActionResultType.Error)
                throw new ArgumentException("Expected ActionResultType.Ok or ActionResultType.Error", nameof(action));

            Subject = null;
            Object = null;
            Action = action;
            Message = message;
        }
    }
}
