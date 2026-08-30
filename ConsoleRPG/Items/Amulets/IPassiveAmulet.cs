using ConsoleRPG.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Amulets;

internal interface IPassiveAmulet
{
    public void UnequipEffect(Character character);
}
