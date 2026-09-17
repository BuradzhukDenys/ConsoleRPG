using ConsoleRPG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleRPG.Effects;
internal class BurnEffect(int duration, int damage) : Effect("Burn", duration)
{
    public override void InitializeMessage(Entity target)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{target.Name} was burned");
        Console.ResetColor();
    }
    public override void Action(Entity target)
    {
        target.TakeDamage(damage);
    }
}
