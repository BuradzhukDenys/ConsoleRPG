using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Weapons.MeleeWeapons
{
    internal class BattleAxe : Weapon
    {
        public BattleAxe() : base("Battle axe")
        {
            Damage = 35;
        }
    }
}
