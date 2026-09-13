using ConsoleRPG.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Consumables;

internal class HealingPotion : Item, IUsable
{
    public int HealAmount { get; private set; } = 30;
    public HealingPotion() : base("Healing potion", 5) { }
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
