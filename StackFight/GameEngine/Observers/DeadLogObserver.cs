using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.GameEngine.Observers
{
    public class DeadLogObserver : IObserver
    {
        protected string Path;

        public DeadLogObserver(string path = "DeadLog.log")
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            Path = path;

            if (!File.Exists(Path))
                using (File.Create(Path)) { }
        }

        public void Update(ISubject sender, string message)
        {
            using (StreamWriter writer = new StreamWriter(Path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}
