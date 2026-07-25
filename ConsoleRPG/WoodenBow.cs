using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class WoodenBow : Weapon, IRangedWeapon
    {
        public WoodenBow() : base("Wooden bow")
        {
            Damage = 30;
        }
    }
}
