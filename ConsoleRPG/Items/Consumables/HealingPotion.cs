using ConsoleRPG.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Consumables;

internal class HealingPotion(int count) : Item("Healing potion", 5, count), IUsable
{
    public int HealAmount { get; private set; } = 30;

    public bool Use(Character target)
    {
        if (!target.Heal(HealAmount))
        {
            return false;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Name} used");
        Console.ResetColor();
        Count--;
        return true;
    }
    public override void ShowInfo()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Heal {HealAmount} health");
        Console.ResetColor();
    }
}
