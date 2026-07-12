namespace ConsoleRPG
{
    internal class Warrior : Character
    {
        private const int WarriorMaxHealth = 120;
        public Warrior(string name, int health) : base(name, health) { }
        public Warrior(string name, int health, Weapon startWeapon) : base(name, health, WarriorMaxHealth, startWeapon) { }
    }
}
