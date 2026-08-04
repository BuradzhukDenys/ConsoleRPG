namespace ConsoleRPG
{
    internal class Warrior : Character
    {
        public Warrior(string name, int health) : base(name, health, new IronSword()) { }
        public Warrior(string name, int health, int maxHealth, Weapon startWeapon, Armor startArmor, Amulet startAmulet)
            : base(name, health, maxHealth, startWeapon, startArmor, startAmulet) { }
    }
}
