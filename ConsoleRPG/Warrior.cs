namespace ConsoleRPG
{
    internal class Warrior : Character
    {
        private const int WarriorMaxHealth = 120;
        public Warrior(string name, int health, int damage) : base(name, health, damage) { }
        public Warrior(string name, int health, int damage, int maxHealth = WarriorMaxHealth) : base(name, health, maxHealth, damage) { }
    }
}
