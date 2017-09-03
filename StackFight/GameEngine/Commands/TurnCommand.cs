using System;
using System.Collections.Generic;
using StackFight.GameEngine.Strategies;

namespace StackFight.GameEngine.Commands
{
    /// <summary>
    /// Команда - выполнение одного хода игры.
    /// </summary>
    public sealed class TurnCommand : ICommand
    {
        //---------------------------------------------------------------------
        private bool _isCommandExecuted = false; //показывает, выполнялась ли команда уже

        private IArmy _firstArmyBefore;
        private IArmy _secondArmyBefore;

        private IEnumerable<ActionResult> _results;
        private IArmy _firstArmyAfter;
        private IArmy _secondArmyAfter;

        //---------------------------------------------------------------------
        private const int MaxEmptyTurns = 50;

        private readonly Random _random;
        private readonly IGameEngine _gameEngine;
        private readonly IStrategy _strategy;

        public TurnCommand(IGameEngine gameEngine, IStrategy strategy, Random random = null)
        {
            _gameEngine = gameEngine;
            _strategy = strategy;
            _random = random ?? new Random();
        }

        public IEnumerable<ActionResult> Execute()
        {
            if (_isCommandExecuted)
            {
                _gameEngine.FirstArmy = _firstArmyAfter;
                _gameEngine.SecondArmy = _secondArmyAfter;
                return _results;
            }

            _firstArmyBefore = _gameEngine.FirstArmy.Clone();
            _secondArmyBefore = _gameEngine.SecondArmy.Clone();

            _results = Fight(_random);
            _isCommandExecuted = true;

            _firstArmyAfter = _gameEngine.FirstArmy.Clone();
            _secondArmyAfter = _gameEngine.SecondArmy.Clone();

            return _results;
        }

        public ActionResult Undo()
        {
            if (!_isCommandExecuted)
                return new ActionResult(ActionResultType.Error, "Невозможно отменить операцию, которая не была выполнена.");

            _gameEngine.FirstArmy = _firstArmyBefore;
            _gameEngine.SecondArmy = _secondArmyBefore;
            return new ActionResult(ActionResultType.Ok, "Операция успешно отменена!");
        }

        private IEnumerable<ActionResult> Fight(Random random)
        {
            IArmy firstArmy = _gameEngine.FirstArmy;
            IArmy secondArmy = _gameEngine.SecondArmy;

            List<ActionResult> results = new List<ActionResult>();
            results.AddRange(_strategy.Fight(firstArmy, secondArmy, random));
            results.AddRange(CollectUnits(firstArmy, secondArmy));

            return results;
        }

        private IEnumerable<ActionResult> CollectUnits(IArmy firstArmy, IArmy secondArmy)
        {
            List<ActionResult> collectedUnits = firstArmy.CollectKilledUnits();
            collectedUnits.AddRange(secondArmy.CollectKilledUnits());

            if (collectedUnits.Count == 0)
                _gameEngine.EmptyTurns++;
            else
                _gameEngine.EmptyTurns = 0;       

            if (_gameEngine.EmptyTurns == MaxEmptyTurns)
                _gameEngine.IsDeadHeat = true;

            return collectedUnits;
        }
    }
}
