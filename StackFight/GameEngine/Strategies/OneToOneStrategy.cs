using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.GameEngine.Strategies
{
    /// <summary>
    /// Стратегия сражения 1 к 1.
    /// </summary>
    public class OneToOneStrategy : IStrategy
    {
        public IEnumerable<ActionResult> Fight(IArmy firstArmy, IArmy secondArmy, Random random = null)
        {
            firstArmy.Width = 1;
            secondArmy.Width = 1;

            random = random ?? new Random();

            IUnit first = firstArmy.GetUnit(0);
            IUnit second = secondArmy.GetUnit(0);

            List<ActionResult> results = new List<ActionResult>();

            int turnRound = random.Next(0, 2); //чей ход должен быть следующим: 0 - 1-го игрока, 1 - 2-го игрока

            if (turnRound == 0)
                results.AddRange(Attack(first, second, firstArmy, secondArmy));
            else if (turnRound == 1)
                results.AddRange(Attack(second, first, secondArmy, firstArmy));
                
            results.AddRange(DoSpecialActions(firstArmy, secondArmy, random));

            return results;
        }

        //юниты атакут друг друга по-очереди: сначала первый второго, затем второй - первого
        private IEnumerable<ActionResult> Attack(IUnit first, IUnit second, IArmy firstArmy, IArmy secondArmy)
        {
            List<ActionResult> results = new List<ActionResult>();

            IUnit resultUnit = second.TakeDamage(first.Power);

            if (resultUnit != second)
                secondArmy.ReplaceUnit(resultUnit, 0);
            if (first.Power > 0)
                results.Add(new ActionResult(first, second, ActionResultType.Attack));

            second = secondArmy.GetUnit(0);
            if (second.Health > 0)
            {
                resultUnit = first.TakeDamage(second.Power);
                if (resultUnit != first)
                    firstArmy.ReplaceUnit(resultUnit, 0);

                if (second.Power > 0)
                    results.Add(new ActionResult(second, first, ActionResultType.Attack));
            }

            return results;
        }

        private IEnumerable<ActionResult> DoSpecialActions(IArmy firstArmy, IArmy secondArmy, Random random)
        {
            List<ActionResult> results = new List<ActionResult>();
            int firstSpecialActions = random.Next(0, 2); //чей ход должен быть первым: 0 - 1-го игрока, 1 - 2-го игрока

            if (firstSpecialActions == 0)
            {
                results.AddRange(DoSpecialActionsForArmy(firstArmy, secondArmy, random));
                results.AddRange(DoSpecialActionsForArmy(secondArmy, firstArmy, random));
            }
            else
            {
                results.AddRange(DoSpecialActionsForArmy(secondArmy, firstArmy, random));
                results.AddRange(DoSpecialActionsForArmy(firstArmy, secondArmy, random));
            }

            return results;
        }

        //специальные действия выполняют все юниты, начиная с первого (нулевой в дуэли сражается)
        private IEnumerable<ActionResult> DoSpecialActionsForArmy(IArmy myArmy, IArmy enemyArmy, Random random)
        {
            List<ActionResult> results = new List<ActionResult>();
            for (int i = 1; i < myArmy.Count; i++)
                if (myArmy.GetUnit(i).IsAlive())
                {
                    ISpecialAbility unit = myArmy.GetUnit(i) as ISpecialAbility;
                    if (unit != null)
                        results.Add(unit.DoSpecialAction(i, myArmy, enemyArmy, random));
                }
            return results;
        }
    }
}
