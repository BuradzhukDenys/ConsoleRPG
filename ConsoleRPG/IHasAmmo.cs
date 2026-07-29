using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal interface IHasAmmo
    {
        public bool EquipAmmo(Ammo newAmmo);
    }
}
