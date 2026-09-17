using ConsoleRPG.Items.Ammos;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Weapons;
using ConsoleRPG.Items.Weapons.RangedWeapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Entities.Characters;

internal class Archer : Character, IHasAmmo
{
    private const int _baseHealth = 100;
    public IRangedWeapon? ArcherWeapon => CurrentWeapon as IRangedWeapon;
    public Ammo? CurrentAmmo { get; internal set; }
    public Archer() : base("Archer", _baseHealth, new WoodenBow())
    {
        CurrentAmmo = new Arrow { Count = 15 };
        AddDamageMultiplier(CurrentAmmo.DamageMultiplier);
        Inventory.AddItem(CurrentAmmo);
    }
    public bool EquipAmmo(Ammo newAmmo)
    {
        if (CurrentAmmo != null && CurrentAmmo.GetType() == newAmmo.GetType())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{newAmmo.Name} already equiped!");
            Console.ResetColor();
            return false;
        }

        if (CurrentAmmo != null)
        {
            RemoveDamageMultiplier(CurrentAmmo.DamageMultiplier);
        }
        CurrentAmmo = newAmmo;

        AddDamageMultiplier(CurrentAmmo.DamageMultiplier);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Name} equiped {newAmmo.Name}, and have {Damage} damage");
        Console.ResetColor();
        return true;
    }
    public override void Attack(Entity entity)
    {
        if (CurrentWeapon is not IRangedWeapon || ArcherWeapon == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("You can't attack without ranged weapon!");
            Console.ResetColor();
            return;
        }

        if (CurrentAmmo == null || CurrentAmmo.Count <= 0)
        {
            base.Attack(entity, ArcherWeapon.MeleeDamage);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"You attack {entity.Name} by melee attack");
            Console.ResetColor();
            return;
        }

        CurrentAmmo.Count--;
        base.Attack(entity);

        var random = new Random();
        if (CurrentAmmo is IEffectProvider effectAmmo && random.NextDouble() <= effectAmmo.EffectChance)
        {
            entity.AddEffect(effectAmmo.GetEffect());
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"You consumed 1 {CurrentAmmo.Name}");

        if (CurrentAmmo.Count <= 0)
        {
            RemoveDamageMultiplier(CurrentAmmo.DamageMultiplier);
            CurrentAmmo = null;
            Console.WriteLine("You are out of ammo!");
        }
        Console.ResetColor();
    }
    public override bool EquipWeapon(Weapon newWeapon)
    {
        if (newWeapon is IRangedWeapon)
        {
            return base.EquipWeapon(newWeapon);
        }

        return false;
    }
    public override void ShowEquipedItems()
    {
        base.ShowEquipedItems();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Ammo: {(CurrentAmmo?.Name ?? "None")}");
        Console.ResetColor();
    }
    public override void ShowBattleInfo()
    {
        base.ShowBattleInfo();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Melee damage: {ArcherWeapon?.MeleeDamage}");
        Console.ResetColor();
    }
}
