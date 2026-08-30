using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Weapons.RangedWeapons;

internal class GodBow : Weapon, IRangedWeapon
{
    public int MeleeDamage => 250;
    public GodBow() : base("God bow")
    {
        Damage = 1000;
    }
}
