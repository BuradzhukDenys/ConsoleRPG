using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleRPG
{
    internal abstract class Amulet(string name) : Item(name, 1, 1), IEquipable
    {
        public virtual bool Equip(Character character)
        {
            return character.EquipAmulet(this);
        }
    }
}
