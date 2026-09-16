using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Ammos;

internal class Arrow : Ammo
{
    public Arrow() : base("Arrow")
    {
        DamageMultiplier = 1.05;
    }
}
