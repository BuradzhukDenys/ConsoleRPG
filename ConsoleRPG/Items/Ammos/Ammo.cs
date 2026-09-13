using ConsoleRPG.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleRPG.Items.Ammos;

internal abstract class Ammo(string name, int count = 1) : Item(name, 99, count), IEquipable
{
    private double _damageMultiplier;
    public double DamageMultiplier
    {
        get { return _damageMultiplier; }
        protected set
        {
            _damageMultiplier = Math.Clamp(value, 1, 2);
        }
    }
    public bool Equip(Character character)
    {
        if (character is IHasAmmo hero)
        {
            return hero.EquipAmmo(this);
        }

        return false;
    }
    public override void ShowInfo()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Damage multiplier: {DamageMultiplier}");
        Console.ResetColor();
    }
}
