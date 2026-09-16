using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Armor
{
    internal class IronArmor : Armor
    {
        public IronArmor() : base("Iron armor")
        {
            DamageReduction = 0.2;
        }
    }
}
