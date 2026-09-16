using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Armor
{
    internal class MythrilArmor : Armor
    {
        public MythrilArmor() : base("Mythril armor")
        {
            DamageReduction = 0.25;
        }
    }
}
