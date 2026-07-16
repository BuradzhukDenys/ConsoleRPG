namespace ConsoleRPG
{
    internal class Warrior : Character
    {
        private const int WarriorMaxHealth = 120;
        public Warrior(string name, int health) : base(name, health, new IronSword()) { }
        public Warrior(string name, int health, Weapon startWeapon, Armor startArmor) : base(name, health, WarriorMaxHealth, startWeapon, startArmor) { }
    }
}
