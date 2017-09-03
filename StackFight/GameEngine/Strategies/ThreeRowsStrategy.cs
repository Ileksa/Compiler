using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.GameEngine.Strategies
{
    /// <summary>
    /// Стратегия сражения для армий, выстроенных в три шеренги.
    /// </summary>
    public class ThreeRowsStrategy : IStrategy
    {
        public IEnumerable<ActionResult> Fight(IArmy firstArmy, IArmy secondArmy, Random random = null)
        {
            if (random == null)
                random = new Random();

            firstArmy.Width = 3;
            secondArmy.Width = 3;

            List<ActionResult> results = new List<ActionResult>();

            int count = Math.Min(firstArmy.Count, secondArmy.Count);
            count = Math.Min(count, 3);
            for (int i = 0; i < count; i++)
            {
                IUnit first = firstArmy.GetUnit(i);
                IUnit second = secondArmy.GetUnit(i);

                int turnRound = random.Next(0, 2); //чей ход должен быть следующим: 0 - 1-го игрока, 1 - 2-го игрока

                if (turnRound == 0)
                    results.AddRange(Attack(i, first, second, firstArmy, secondArmy));
                else if (turnRound == 1)
                    results.AddRange(Attack(i, second, first, secondArmy, firstArmy));
            }

            results.AddRange(DoSpecialActions(count, firstArmy, secondArmy, random));

            return results;
        }

        //юниты атакут друг друга по-очереди: сначала первый второго, затем второй - первого
        private IEnumerable<ActionResult> Attack(int position, IUnit first, IUnit second, IArmy firstArmy, IArmy secondArmy)
        {
            List<ActionResult> results = new List<ActionResult>();

            IUnit resultUnit = second.TakeDamage(first.Power);

            if (resultUnit != second)
                secondArmy.ReplaceUnit(resultUnit, position);
            if (first.Power > 0)
                results.Add(new ActionResult(first, second, ActionResultType.Attack));

            second = secondArmy.GetUnit(position);
            if (second.Health > 0)
            {
                resultUnit = first.TakeDamage(second.Power);
                if (resultUnit != first)
                    firstArmy.ReplaceUnit(resultUnit, position);

                if (second.Power > 0)
                    results.Add(new ActionResult(second, first, ActionResultType.Attack));
            }

            return results;
        }

        private IEnumerable<ActionResult> DoSpecialActions(int start, IArmy firstArmy, IArmy secondArmy, Random random)
        {
            List<ActionResult> results = new List<ActionResult>();
            int firstSpecialActions = random.Next(0, 2); //чей ход должен быть первым: 0 - 1-го игрока, 1 - 2-го игрока

            if (firstSpecialActions == 0)
            {
                results.AddRange(DoSpecialActionsForArmy(start, firstArmy, secondArmy, random));
                results.AddRange(DoSpecialActionsForArmy(start, secondArmy, firstArmy, random));
            }
            else
            {
                results.AddRange(DoSpecialActionsForArmy(start, secondArmy, firstArmy, random));
                results.AddRange(DoSpecialActionsForArmy(start, firstArmy, secondArmy, random));
            }

            return results;
        }

        private IEnumerable<ActionResult> DoSpecialActionsForArmy(int start, IArmy myArmy, IArmy enemyArmy, Random random)
        {
            List<ActionResult> results = new List<ActionResult>();
            for (int i = start; i < myArmy.Count; i++)
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
