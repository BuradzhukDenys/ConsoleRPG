using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Armor;

internal class GodArmor : Armor
{
    public GodArmor() : base("God armor")
    {
        DamageReduction = 100;
    }
}
