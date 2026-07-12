using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal abstract class Weapon(string name) : Item(name, 1, 1), IEquipable
    {
        public int Damage { get; protected set; }

        public bool Equip(Character character)
        {
            return character.EquipWeapon(this);
        }
    }
}
