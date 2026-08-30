using ConsoleRPG.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items;

internal interface IEquipable
{
    public bool Equip(Character character);
}
