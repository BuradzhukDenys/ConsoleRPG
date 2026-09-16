using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Weapons.RangedWeapons
{
    internal class CompositeBow : Weapon, IRangedWeapon
    {
        public int MeleeDamage => 15;
        public CompositeBow() : base("Composite bow")
        {
            Damage = 20;
        }
    }
}
