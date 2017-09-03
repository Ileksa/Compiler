using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight
{
    public enum StrategyTypes { OneToOne, WallToWall, ThreeRows }

    public interface IGameEngine
    {
        IArmy FirstArmy { get; set; }
        IArmy SecondArmy { get; set; }
        void NewGame(int cost, string first = "First Army", string second = "Second Army");

        IEnumerable<ActionResult> Turn();
        IEnumerable<ActionResult> TurnToEnd();
        ActionResult Undo();
        IEnumerable<ActionResult> Reundo();

        void SetStrategy(StrategyTypes type);
        StrategyTypes GetStrategy();

        bool IsGameEnded();
        bool IsDeadHeat { get; set; }
        int EmptyTurns { get; set; }
    }
}
