using System;
using System.Collections.Generic;
using StackFight.GameEngine.Commands;
using StackFight.GameEngine.Observers;
using StackFight.GameEngine.Strategies;
using StackFight.StandardArmy;

namespace StackFight.GameEngine
{
    public sealed class GameEngine : IGameEngine
    {
        private IArmyFactory _armyFactory;
        public IArmyFactory ArmyFactory
        {
            get { return _armyFactory; }
            set { if (value != null) _armyFactory = value; }
        }
        private IArmy _firstArmy;
        private IArmy _secondArmy;
        public IArmy FirstArmy
        {
            get { return _firstArmy; }
            set { if (value != null) _firstArmy = value; }
        }
        public IArmy SecondArmy
        {
            get { return _secondArmy; }
            set { if (value != null) _secondArmy = value; }
        }

        public int EmptyTurns { get; set; }
        public bool IsDeadHeat { get; set; }
        private Random _random;
        private Invoker _commandInvoker;
        public Invoker CommandInvoker
        {
            get { return _commandInvoker; }
            set { if (value != null) _commandInvoker = value; }
        }

        private IStrategy _strategy;
        public void SetStrategy(StrategyTypes type)
        {
            switch (type)
            {
                case StrategyTypes.OneToOne:
                    _strategy = new OneToOneStrategy();
                    break;
                case StrategyTypes.WallToWall:
                    _strategy = new WallToWallStrategy();
                    break;
                case StrategyTypes.ThreeRows:
                    _strategy = new ThreeRowsStrategy();
                    break;
            }
        }
        public StrategyTypes GetStrategy()
        {
            if (_strategy is OneToOneStrategy)
                return StrategyTypes.OneToOne;
            if (_strategy is WallToWallStrategy)
                return StrategyTypes.WallToWall;
            if (_strategy is ThreeRowsStrategy)
                return StrategyTypes.ThreeRows;

            return 0;
        }

        public static GameEngine GetGameEngine()
        {
            if (_gameEngine == null)
                _gameEngine = new GameEngine();
            return _gameEngine;
        }
        private static GameEngine _gameEngine;
        private GameEngine()
        {
            ArmyFactory = new StandardArmyFactory();
            _firstArmy = ArmyFactory.GetArmy(0);
            _secondArmy = ArmyFactory.GetArmy(0);
            CommandInvoker = new Invoker();
        }

        public void NewGame(int cost, string first = "First Army", string second = "Second Army")
        {
            IArmyFactory armyFactory = new StandardArmyFactory();
            _firstArmy = armyFactory.GetArmy(cost);
            _firstArmy.Name = first;
            _secondArmy = armyFactory.GetArmy(cost);
            _secondArmy.Name = second;
            AddObservers();

            IsDeadHeat = false;
            _random = new Random();
            SetStrategy(StrategyTypes.OneToOne);
        }

        public IEnumerable<ActionResult> Turn()
        {
            if (IsGameEnded())
                return new List<ActionResult>(1) { new ActionResult(ActionResultType.Ok, GetEndMessage()) };

            return CommandInvoker.Execute(new TurnCommand(this, _strategy, _random));
        }

        public IEnumerable<ActionResult> TurnToEnd()
        {
            List<ActionResult> results = new List<ActionResult>();
            while (!IsGameEnded())
                results.AddRange(Turn());

            results.AddRange(Turn()); //чтобы вывести финальное сообщение
            return results;
        }

        public ActionResult Undo()
        {
            return _commandInvoker.Undo();
        }

        public IEnumerable<ActionResult> Reundo()
        {
            return _commandInvoker.Reundo();
        }

        public bool IsGameEnded()
        {
            if (_firstArmy.IsEmpty() || _secondArmy.IsEmpty() || IsDeadHeat)
                return true;
            return false;
        }

        //создает сообщение о конце игры
        private string GetEndMessage()
        {
            if (_firstArmy.IsEmpty())
                return "Игра окончена! Победила армия \"" + _secondArmy.Name + "\"";
            if (_secondArmy.IsEmpty())
                return "Игра окончена! Победила армия \"" + _firstArmy.Name + "\"";
            if (IsDeadHeat)
                return "Игра окончена! Ничья!";

            return String.Empty;
        }

        private void AddObservers()
        {
            _firstArmy.Attach(new BeepObserver(400, 401));
            _secondArmy.Attach(new BeepObserver(600, 601));
            _firstArmy.Attach(new DeadLogObserver());
            _secondArmy.Attach(new DeadLogObserver());

        }

    }
}
