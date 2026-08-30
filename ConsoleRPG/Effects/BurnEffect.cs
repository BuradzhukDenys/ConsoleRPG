using ConsoleRPG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Effects;
internal class BurnEffect(int duration) : Effect("Burn", duration)
{
    private readonly int _damage = 5;
    public override void InitializeMessage(Entity target)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{target.Name} was burned");
        Console.ResetColor();
    }
    public override void Action(Entity target)
    {
        target.TakeDamage(_damage);
    }
}
