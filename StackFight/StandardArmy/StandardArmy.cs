using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy
{
    public sealed class StandardArmy : IArmy
    {
        private const string DefaultName = "Standard Army";
        private List<IObserver> _observers = new List<IObserver>();

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = string.IsNullOrEmpty(value) ? DefaultName : value; }
        }

        private List<IUnit> _units;
        public int Count { get { return _units.Count; } }

        private int _width;
        public int Width
        {
            get { return _width; }
            set
            {
                if (value < 1)
                    _width = 1;
                else if (value > Count)
                    _width = Count;
                else
                    _width = value;
            }
        }


        public StandardArmy(List<IUnit> units)
        {
            _units = units;
            _name = DefaultName;
            _width = 1;
        }

        public IUnit GetUnit(int position)
        {
            if (position >= Count)
                throw new IndexOutOfRangeException();
            return _units[position];
        }

        public void AddUnit(IUnit unit, int position)
        {
            _units.Insert(position, unit);
        }

        public void ReplaceUnit(IUnit unit, int position)
        {
            if (position >= _units.Count)
                throw new ArgumentOutOfRangeException();

            _units[position] = unit;
        }

        public void ReplaceUnit(IUnit newUnit, IUnit oldUnit)
        {
            for (int i = 0; i < _units.Count; i++)
                if (_units[i] == oldUnit)
                {
                    _units[i] = newUnit;
                    break;
                }
        }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public IArmy Clone()
        {
            List<IUnit> clonedArmy = new List<IUnit>();
            foreach (IUnit unit in _units)
                clonedArmy.Add(unit.Clone());

            StandardArmy cloned = new StandardArmy(clonedArmy);
            cloned.Name = Name;
            cloned._observers = _observers;
            cloned.Width = Width;

            return cloned;
        }

        public List<ActionResult> CollectKilledUnits()
        {
            List<ActionResult> results = new List<ActionResult>();

            int i = Count - 1;
            while (i >= 0)
            {
                if (!_units[i].IsAlive())
                {
                    Notify(_units[i].Name + " (" + Name + ") dead.");
                    results.Add(new ActionResult(_units[i], null, ActionResultType.Death));

                    int j = i + Width;
                    while (j < Count)
                    {
                        _units[j - Width] = _units[j];
                        j += Width;
                    }
                    _units.RemoveAt(j - Width);
                }
                i--;
            }

            return results;
        }

        public void Attach(IObserver observer)
        {
            if (observer != null)
                _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            if (observer != null)
                _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (IObserver observer in _observers)
                observer.Update(this, message);
        }
    }
}
