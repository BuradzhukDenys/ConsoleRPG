using ConsoleRPG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Amulets;

internal interface IAttackAmulet
{
    public enum EffectTarget
    {
        Character,
        Enemy
    }
    public EffectTarget AmuletEffectTarget { get; }
    public void AmuletAction(Entity entity);
}
