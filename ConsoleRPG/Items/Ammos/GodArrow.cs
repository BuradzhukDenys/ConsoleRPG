using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Ammos;

internal class GodArrow : Ammo
{
    public GodArrow() : base("God arrow")
    {
        DamageMultiplier = 3;
    }
}
