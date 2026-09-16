using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Armor
{
    internal class BlackSteelArmor : Armor
    {
        public BlackSteelArmor() : base("Black steel armor")
        {
            DamageReduction = 0.35;
        }
    }
}
