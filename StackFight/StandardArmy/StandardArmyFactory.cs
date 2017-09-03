using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StackFight.StandardArmy
{
    public sealed class StandardArmyFactory : IArmyFactory
    {
        private readonly IUnit[] _units =
        {
            new LightInfantry(), new HeavyInfantry(), new Cleric(),
            new ArcherProxy(new Archer()), new Mage(), new SiegeVehicle(),
        };
        private readonly double[] _cumulativeProbabilities; //массив накопленных вероятностей, используемый для генерации армии
        private readonly int _minCost; //наименьшая стоимость юнита из доступных
        private readonly Random _random = new Random();

        public StandardArmyFactory()
        {
            int sumCost = 0; //общая стоимость всех доступных юнитов
            _minCost = Int32.MaxValue; //наименьшая стоимость

            foreach (IUnit unit in _units)
            {
                sumCost += unit.Cost;
                if (unit.Cost < _minCost)
                    _minCost = unit.Cost;
            }

            //определение накопленной вероятности
            _cumulativeProbabilities = new double[_units.Length];
            double cumulariveProbability = 0; //накопленная вероятность

            for (int i = 0; i < _cumulativeProbabilities.Length; i++)
            {
                _cumulativeProbabilities[i] = cumulariveProbability;
                cumulariveProbability += (1 - (double)(_units[i].Cost)/sumCost)/(_cumulativeProbabilities.Length - 1); //вероятности, обратно пропорциональные стоимости
            }
        }

        /// <summary>
        /// Возвращает армию из случайных юнитов, стоимость которой не больше переданной в параметре.
        /// </summary>
        /// <param name="cost">Максимальная стоимость армии.</param>
        public IArmy GetArmy(int cost)
        {
            List<IUnit> units = new List<IUnit>();

            while (cost >= _minCost)
            {
                IUnit unit = GetRandomUnit(cost);

                if (unit == null) //если не удалось создать юнита, вернем армию в текущем состоянии
                    break;

                units.Add(unit);
                cost -= unit.Cost;
            }

            return new StandardArmy(units);
        }

        /// <summary>
        /// Возвращает случайного юнита, стоимость которого не больше переданной величины cost.
        /// </summary>
        /// <param name="cost">Максимальная стоимость юнита.</param>
        /// <returns>Случайный юнит; null - если невозможно создать.</returns>
        public IUnit GetRandomUnit(int cost)
        {
            if (cost < _minCost)
                return null;

            while (true)
            {
                double generated = _random.NextDouble();

                int i = 0;
                while (i < _units.Length - 1 && _cumulativeProbabilities[i + 1] < generated)
                    i++;

                if (_units[i].Cost <= cost)
                {
                    IUnit unit = _units[i].Clone();
                    unit.Name = unit.Name + " №" +  _random.Next(0, 100);
                    return unit;
                }
            }
        }
    }
}
