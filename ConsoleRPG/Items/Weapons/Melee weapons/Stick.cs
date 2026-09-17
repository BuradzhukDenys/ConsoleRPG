using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Weapons.MeleeWeapons;

internal class Stick : Weapon
{
    public Stick() : base("Stick")
    {
        Damage = 10;
    }
}
