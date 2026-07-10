using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class HealingPotion(string name, int count) : Item(name, count, maxCountPotion), IUsable
    {
        private const int maxCountPotion = 5;
        private const int duration = 3;

        public void Use()
        {

        }
    }
}
