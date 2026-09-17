using ConsoleRPG.Entities;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Weapons;
using System;

namespace ConsoleRPG.Entities.Characters;

internal abstract class Character : Entity
{
    public event Action? OnCharacterDeath;
    public Inventory Inventory { get; internal set; } = new();
    public Weapon CurrentWeapon { get; internal set; }
    public Armor? CurrentArmor { get; internal set; }
    public Amulet? CurrentAmulet { get; internal set; }
    protected Character(string name, int health, Weapon startWeapon) //First save initialize constructor
    {
        Name = name;
        MaxHealth = health;
        Health = health;
        CurrentWeapon = startWeapon;
        Damage = CurrentWeapon.Damage;
        Inventory.AddItem(CurrentWeapon);
    }
    public bool Heal(int amount)
    {
        if (Health == MaxHealth)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Your health is full");
            Console.ResetColor();
            return false;
        }

        Health += amount;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{Name} was healed by {amount} and have {Health} health");
        Console.ResetColor();

        return true;
    }
    public override void Attack(Entity entity)
    {
        base.Attack(entity);

        if (CurrentWeapon is IEffectProvider effectProvider)
        {
            var random = new Random();
            
            if (random.NextDouble() <= effectProvider.EffectChance)
            {
                entity.AddEffect(effectProvider.GetEffect());
            }
        }

        if (CurrentAmulet is IAttackAmulet attackAmulet)
        {
            switch (attackAmulet.AmuletEffectTarget)
            {
                case IAttackAmulet.EffectTarget.Character:
                    attackAmulet.AmuletAction(this);
                    break;
                case IAttackAmulet.EffectTarget.Enemy:
                    attackAmulet.AmuletAction(entity);
                    break;
                default:
                    Console.WriteLine("Unknown target");
                    break;
            }
        }
    }
    override protected void Die()
    {
        base.Die();

        OnCharacterDeath?.Invoke();
    }
    virtual public bool EquipWeapon(Weapon newWeapon)
    {
        if (CurrentWeapon.GetType() == newWeapon.GetType())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{newWeapon.Name} already equiped!");
            Console.ResetColor();
            return false;
        }

        CurrentWeapon = newWeapon;
        Damage = CurrentWeapon.Damage;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Name} equiped {newWeapon.Name}, and have {Damage} damage");
        Console.ResetColor();
        return true;
    }
    public bool EquipArmor(Armor newArmor)
    {
        if (CurrentArmor != null && CurrentArmor.GetType() == newArmor.GetType())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{newArmor.Name} already equiped!");
            Console.ResetColor();
            return false;
        }

        CurrentArmor = newArmor;
        DamageReduction = CurrentArmor.DamageReduction;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Name} equiped {newArmor.Name}, and have {DamageReduction} damage reduction");
        Console.ResetColor();
        return true;
    }
    public bool EquipAmulet(Amulet newAmulet)
    {
        if (CurrentAmulet != null && CurrentAmulet.GetType() == newAmulet.GetType())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{newAmulet.Name} already equiped!");
            Console.ResetColor();
            return false;
        }

        if (CurrentAmulet is IPassiveAmulet passiveAmulet) passiveAmulet.UnequipEffect(this);

        CurrentAmulet = newAmulet;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Name} equiped {newAmulet.Name}");
        Console.ResetColor();
        return true;
    }
    virtual public void ShowEquipedItems()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Equiped items:");
        Console.Write(
            $"Weapon: {CurrentWeapon.Name}\n" +
            $"Armor: {(CurrentArmor != null ? CurrentArmor.Name : "None")}\n" +
            $"Amulet: {(CurrentAmulet != null ? CurrentAmulet.Name : "None")}\n");
        Console.ResetColor();
    }
}
