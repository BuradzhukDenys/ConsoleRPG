using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class BurnEffect(int duration) : Effect("Burn", duration)
    {
        private readonly int _damage = 5;
        public override void InitializeMessage(Entity target)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{target.Name} was burned");
            Console.ResetColor();
        }
        public override void Action(Entity target)
        {
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"{target.Name} are burning, and take {_damage} damage");
            //Console.ResetColor();

            target.TakeDamage(_damage);
        }
    }
}
