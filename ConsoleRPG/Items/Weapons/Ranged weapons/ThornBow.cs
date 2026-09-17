using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Weapons.RangedWeapons
{
    internal class ThornBow : Weapon, IRangedWeapon
    {
        public int MeleeDamage => 35;
        public ThornBow() : base("Thorn bow")
        {
            Damage = 25;
        }
    }
}
