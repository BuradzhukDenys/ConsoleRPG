namespace ConsoleRPG
{
    internal abstract class Character : Entity
    {
        public event Action? OnCharacterDeath;

        //public Weapon CurrentWeapon { get; protected set; }

        public Character(string name, int health, int damage) //First save initialize constructor
        {
            Name = name;
            Damage = damage;
            MaxHealth = health;
            Health = health;
        }

        public Character(string name, int health, int maxHealth, int damage) //Constructor for load from file
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            Damage = damage;
        }

        public Inventory Inventory { get; private set; } = new();

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
            Console.WriteLine($"{this.Name} was healed by {amount} and have {this.Health} health");
            Console.ResetColor();

            return true;
        }

        override protected void Die()
        {
            base.Die();

            OnCharacterDeath?.Invoke();
        }
    }
}
