using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StackFight.StandardArmy;

namespace StackFight
{
    public sealed class UserInterface
    {
        private readonly IGameEngine _engine;

        public UserInterface(IGameEngine gameEngine)
        {
            _engine = gameEngine;
        }

        public static int MinCost = 30;
        public static int MaxCost = 2500;
        public static int WindowWidth = 140;
        public static int WindowHeight = Console.LargestWindowHeight - 10;

        private bool _gameCreated = true;

        /// <summary>
        /// Запуск пользовательского интерфейса.
        /// </summary>
        public void Run()
        {
            Console.WindowWidth = WindowWidth;
            Console.WindowHeight = WindowHeight;

            NewGame();

            while (true)
            {
                ShowMenu();

                string action = Console.ReadLine();

                if (action == null)
                    Console.WriteLine("Необходимо ввести команду.");
                else if (action.Equals("0"))
                    return;
                else
                    DoAction(action);
            }
        }

        private void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;

            WriteLine('.', 50);
            Console.WriteLine(". {0, -47}.", "Меню:");

            WriteLine('.', 50);
            Console.WriteLine(". {0, -47}.", "0. Выход");
            Console.WriteLine(". {0, -47}.", "1. Начать новую игру");
            Console.WriteLine(". {0, -47}.", "2. Отобразить армии");
            Console.WriteLine(". {0, -47}.", "3. Сделать ход");
            Console.WriteLine(". {0, -47}.", "4. Отменить последний ход");
            Console.WriteLine(". {0, -47}.", "5. Повторить последний отмененный ход");
            Console.WriteLine(". {0, -47}.", "6. Доиграть до конца");
            Console.WriteLine(". {0, -47}.", "7. Отобразить текущую тактику игры");
            Console.WriteLine(". {0, -47}.", "8. Установить новую тактику игры");
            WriteLine('.', 50);

            Console.ResetColor();
        }

        private void DoAction(string action)
        {
            if (_gameCreated == false && !action.Equals("0") && !action.Equals("1"))
            {
                Console.WriteLine("Необходимо создать игру.");
                return;
            }

            switch (action)
            {
                case "0":
                    return;
                case "1":
                    NewGame();
                    break;
                case "2":
                    DisplayArmy(_engine.FirstArmy);
                    DisplayArmy(_engine.SecondArmy);
                    break;
                case "3":
                    Fight();
                    break;
                case "4":
                    Undo();
                    break;
                case "5":
                    Reundo();
                    break;
                case "6":
                    FightToEnd();
                    break;
                case "7":
                    GetStrategy();
                    break;
                case "8":
                    SetStrategy();
                    break;
                default:
                    Console.WriteLine("Не удалось распознать входные символы.");
                    break;
            }
        }

        private void NewGame()
        {
            Console.WriteLine("Введите стоимость армий для новой игры от " + MinCost + " до " + MaxCost + ": ");
            int cost;
            if (!Int32.TryParse(Console.ReadLine(), out cost) || cost < MinCost || cost > MaxCost)
            {
                Console.WriteLine("Не удалось получить стоимость армии.");
                _gameCreated = false;
                return;
            }

            Console.WriteLine("Нажмите y, если хотите ввести названия армий.");
            string readLine = Console.ReadLine();

            if (readLine != null && readLine.Equals("y"))
                ProcessArmyNames(cost);
            else
            {
                _engine.NewGame(cost);
                _gameCreated = true;
                Console.WriteLine("Новая игра создана.");
            }

            GetStrategy();
        }

        private void ProcessArmyNames(int cost)
        {
            Console.WriteLine("Введите название первой армии:");
            string first = Console.ReadLine();

            Console.WriteLine("Введите название второй армии:");
            string second = Console.ReadLine();

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            {
                _engine.NewGame(cost);
                Console.WriteLine("Не удалось создать армии с заданными именами. Созданы армии с базовыми именами.");
            }
            else
            {
                _engine.NewGame(cost, first, second);
                Console.WriteLine("Новая игра создана.");
            }

            _gameCreated = true;
        }

        private void DisplayArmy(IArmy army)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n " + army.Name);
            Console.ResetColor();
            OutputHeaderInDisplayUnitTable();
            for (int i = 0; i < army.Count; i++)
                OutputUnit(army.GetUnit(i));
        }

        private void Fight()
        {
            ActionResult[] results = _engine.Turn().ToArray();
            if (OutputSingleMessage(results))
                return;

            OutputHeaderInOutputResult();
            foreach (var result in results)
                OutputResult(result);
        }

        private void FightToEnd()
        {
            ActionResult[] results = _engine.TurnToEnd().ToArray();
            if (OutputSingleMessage(results))
                return;

            OutputHeaderInOutputResult();
            foreach (var result in results)
                OutputResult(result);
        }

        private void Undo()
        {
            ActionResult result = _engine.Undo();
            if (result.Action == ActionResultType.Ok || result.Action == ActionResultType.Error)
                Console.WriteLine(result.Message);
        }

        private void Reundo()
        {
            ActionResult[] results = _engine.Reundo().ToArray();
            if (OutputSingleMessage(results))
                return;

            OutputHeaderInOutputResult();
            foreach (var result in results)
                OutputResult(result);
        }

        private void SetStrategy()
        {
            Console.WriteLine("Выберите одну из доступных тактик игры:");
            Console.WriteLine("1. Стратегия \"Стековый бой\"");
            Console.WriteLine("2. Стратегия \"Стенка на стенку\"");
            Console.WriteLine("3. Стратегия \"Три дуэли\"");

            string action = Console.ReadLine();
            switch (action)
            {
                case "1":
                    _engine.SetStrategy(StrategyTypes.OneToOne);
                    break;
                case "2":
                    _engine.SetStrategy(StrategyTypes.WallToWall);
                    break;
                case "3":
                    _engine.SetStrategy(StrategyTypes.ThreeRows);
                    break;
                default:
                    Console.WriteLine("Невозможно определить желаемую тактику игры.");
                    return;
            }

            Console.WriteLine("Новая тактика успешно установлена!");
        }

        private void GetStrategy()
        {
            Console.Write("Текущая стратегия: ");
            switch (_engine.GetStrategy())
            {
                case StrategyTypes.OneToOne:
                    Console.WriteLine("\"Стековый бой\".");
                    break;
                case StrategyTypes.WallToWall:
                    Console.WriteLine("\"Стенка на стенку\".");
                    break;
                case StrategyTypes.ThreeRows:
                    Console.WriteLine("\"Три дуэли\".");
                    break;
                default:
                    Console.WriteLine("Невозможно распознать тактику");
                    return;
            }
        }

        //----------------------------------Методы отображения результата ----------------------------------------//
        /// <summary>
        /// Отображает линию длиной len из символов c
        /// </summary>
        private void WriteLine(char c, int len)
        {
            for (int i = 0; i < len; i++)
                Console.Write(c);
            Console.WriteLine();
        }

        //имя юнита нужным цветом в 25-и позициях, с обеих сторон ограниченных '|'
        private void OutputNameInOutputResult(string name, ConsoleColor color)
        {
            Console.Write("|");
            Console.ForegroundColor = color;
            Console.Write("{0, -25}", name);
            Console.ResetColor();
            Console.Write("|");
        }

        //цвет юнита по его типу
        private ConsoleColor GetColor(IUnit unit)
        {
            if (unit is IHeavyInfantry)
                return ConsoleColor.Cyan;
            if (unit is LightInfantry)
                return ConsoleColor.Yellow;
            if (unit is Cleric)
                return ConsoleColor.Green;
            if (unit is Mage)
                return ConsoleColor.Blue;
            if (unit is Archer || unit is ArcherProxy)
                return ConsoleColor.Magenta;
            if (unit is SiegeVehicle)
                return ConsoleColor.DarkMagenta;

            return ConsoleColor.Gray;
        }

        //заголовок результата боя
        private void OutputHeaderInOutputResult()
        {
            Console.ForegroundColor = ConsoleColor.White;
            OutputLineInOutputResult();
            Console.WriteLine("|{0, -25}|{1, -15}|{2, -25}|",
                "Юнит, сделавший ход", "Тип действия", "Юнит-цель действия");
            OutputLineInOutputResult();
            Console.ResetColor();
        }

        //результат боя
        private void OutputResult(ActionResult result)
        {
            if (result != ActionResult.Empty)
            {
                if (result.Action == ActionResultType.Error)
                    Console.WriteLine("Error: " + result.Message);
                else if (result.Action == ActionResultType.Ok)
                    Console.WriteLine(result.Message);
                else
                {
                    if (result.Action == ActionResultType.Death)
                    {
                        OutputNameInOutputResult("The Death", ConsoleColor.Red);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0,-15}", "Taking");
                        Console.ResetColor();
                        OutputNameInOutputResult(result.Subject.Name, ConsoleColor.Red);
                        Console.WriteLine();
                    }
                    else
                    {
                        OutputNameInOutputResult(result.Subject.Name, GetColor(result.Subject));
                        Console.Write("{0,-15}", result.Action);
                        OutputNameInOutputResult(result.Object.Name, GetColor(result.Object));
                        Console.WriteLine();
                    }
                    OutputLineInOutputResult();
                }
            }
        }

        //линия таблицы между результатами боя
        private void OutputLineInOutputResult()
        {
            Console.Write("+");
            for (int i = 0; i < 25; i++) Console.Write("-");
            Console.Write("+");
            for (int i = 0; i < 15; i++) Console.Write("-");
            Console.Write("+");
            for (int i = 0; i < 25; i++) Console.Write("-");
            Console.Write("+");
            Console.WriteLine();
        }

        //заголовок описания армии
        private void OutputHeaderInDisplayUnitTable()
        {
            Console.ForegroundColor = ConsoleColor.White;
            OutputLineInDisplayUnitTable();
            Console.WriteLine("|{0, -25}|{1, -10}|{2, -8}|{3, -5}|{4, -50}|",
                "Имя", "Здоровье", "Защита", "Сила", "Улучшения");
            OutputLineInDisplayUnitTable();
            Console.ResetColor();
        }

        //описание юнита в описании армии
        private void OutputUnit(IUnit unit)
        {
            IHeavyInfantry heavy = unit as IHeavyInfantry;
            string improvements = String.Empty;
            if (heavy != null)
                improvements = heavy.GetImprovements();

            OutputNameInOutputResult(unit.Name, GetColor(unit));
            Console.WriteLine("{0,10}|{1, 8}|{2,5}|{3, -50}|",
                 unit.Health, unit.Defence, unit.Power, improvements);
            OutputLineInDisplayUnitTable();
        }

        //линия таблицы между описаниями юнита в описании армии
        private void OutputLineInDisplayUnitTable()
        {
            Console.Write("+");
            for (int i = 0; i < 25; i++) Console.Write("-");
            Console.Write("+");
            for (int i = 0; i < 10; i++) Console.Write("-");
            Console.Write("+");
            for (int i = 0; i < 8; i++) Console.Write("-");
            Console.Write("+");
            for (int i = 0; i < 5; i++) Console.Write("-");
            Console.Write("+");
            for (int i = 0; i < 50; i++) Console.Write("-");
            Console.Write("+");
            Console.WriteLine();
        }

        //отображает спецаильное сообщение, если результаты состоят только из него и возвращает true; иначе - false
        private bool OutputSingleMessage(ActionResult[] actionResults)
        {
            if (actionResults[0].Action == ActionResultType.Error || actionResults[0].Action == ActionResultType.Ok)
            {
                Console.WriteLine(actionResults[0].Message);
                return true;
            }

            return false;
        }
    }
}
