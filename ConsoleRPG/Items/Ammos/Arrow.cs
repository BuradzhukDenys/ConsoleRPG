using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Ammos;

internal class Arrow : Ammo
{
    public Arrow(int count) : base("Arrow", count)
    {
        DamageMultiplier = 1.05;
    }
}
