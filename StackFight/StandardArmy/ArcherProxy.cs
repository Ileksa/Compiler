using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackFight.StandardArmy
{
    public class ArcherProxy : /*Archer,*/ IUnit, ISpecialAbility, ICanBeHealed
    {
        public Archer ArcherUnit { get; protected set; }

        public string Path { get; protected set; }

        public ArcherProxy(Archer archer, string path = "archer.log")
        {
            if (archer == null)
                throw new ArgumentNullException(nameof(archer));

            ArcherUnit = archer;

            Path = path;
            if (!File.Exists(Path))
                using (File.Create(Path)) { }

            WriteMessage("Creation: " + ArcherUnit.ToString());
        }

        public string Name
        {
            get { return ArcherUnit.Name; }
            set
            {
                WriteMessage(Name + " changed name. Now: " + value);
                ArcherUnit.Name = value;
            }
        }

        public int Health
        {
            get { return ArcherUnit.Health; }
        }

        public int Power
        {
            get { return ArcherUnit.Power; }
            set { ArcherUnit.Power = value; }
        }

        public int Defence
        {
            get { return ArcherUnit.Defence; }
            set { ArcherUnit.Defence = value; }
        }

        public int Cost
        {
            get { return ArcherUnit.Cost; }
        }

        public IUnit TakeDamage(int damage)
        {
            IUnit resultUnit = ((IUnit)ArcherUnit).TakeDamage(damage);
            WriteMessage(Name + " taked damage. New state: " + ArcherUnit.ToString());
            return resultUnit;
        }

        public bool IsAlive()
        {
            return ArcherUnit.IsAlive();
        }

        public IUnit Clone()
        {
            return new ArcherProxy((Archer)ArcherUnit.Clone(), Path);
        }

        public ActionResult DoSpecialAction(int position, IArmy myArmy, IArmy enemyArmy, Random random = null)
        {
            return ArcherUnit.DoSpecialAction(position, myArmy, enemyArmy, random);
        }

        protected void WriteMessage(string message)
        {
            using (StreamWriter writer = new StreamWriter(Path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public void TakeHealth(int health)
        {
            ArcherUnit.TakeHealth(health);
        }
    }
}
