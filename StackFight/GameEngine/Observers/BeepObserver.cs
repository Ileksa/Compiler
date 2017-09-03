using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.GameEngine.Observers
{
    public class BeepObserver : IObserver
    {
        private readonly Random _random = new Random();
        private int _min = 0;
        private int _max = 0;

        public BeepObserver(int min = 400, int max = 600)
        {
            _max = max;
            _min = min;
        }

        public void Update(ISubject sender, string message)
        {
            Console.Beep(_random.Next(_min, _max), 100);
        }
    }
}
