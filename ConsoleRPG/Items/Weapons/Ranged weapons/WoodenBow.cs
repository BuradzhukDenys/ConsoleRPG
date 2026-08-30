using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Weapons.RangedWeapons;

internal class WoodenBow : Weapon, IRangedWeapon
{
    public int MeleeDamage => 10;
    public WoodenBow() : base("Wooden bow")
    {
        Damage = 30;
    }
}
