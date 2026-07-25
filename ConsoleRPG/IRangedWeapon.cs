using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal interface IRangedWeapon
    {
        public bool CheckAmmos(Archer archer, string AmmoName)
        {
            return archer.Inventory.CheckAmmos(AmmoName);
        }
    }
}
