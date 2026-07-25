using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class GodArrow : Ammo
    {
        public GodArrow(int count) : base("God arrow", count)
        {
            DamageMultiplier = 3;
        }
    }
}
