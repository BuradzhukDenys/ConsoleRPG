using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Weapons;
using ConsoleRPG.Items.Weapons.MeleeWeapons;

namespace ConsoleRPG.Entities.Characters;

internal class Warrior : Character
{
    private const int _baseHealth = 120;
    public Warrior() : base("Warrior", _baseHealth, new Stick()) { }
    public Warrior(int health, int maxHealth, Weapon startWeapon, Armor startArmor, Amulet startAmulet, Inventory inventory)
        : base("Warrior", health, maxHealth, startWeapon, startArmor, startAmulet, inventory) { }
}
