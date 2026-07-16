namespace ConsoleRPG
{
    internal abstract class Character : Entity
    {
        public event Action? OnCharacterDeath;
        public Inventory Inventory { get; private set; } = new();
        public Weapon CurrentWeapon { get; private set; }
        public Armor? CurrentArmor { get; private set; }
        public Character(string name, int health, Weapon startWeapon) //First save initialize constructor
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            CurrentWeapon = startWeapon;
            Damage = CurrentWeapon.Damage;
            Inventory.AddItem(CurrentWeapon!);
        }

        public Character(string name, int health, int maxHealth, Weapon startWeapon, Armor startArmor) //Constructor for load from file
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            CurrentWeapon = startWeapon;
            CurrentArmor = startArmor;
            Damage = CurrentWeapon.Damage;
            DamageReduction = CurrentArmor.DamageReduction;
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
            Console.WriteLine($"{this.Name} was healed by {amount} and have {this.Health} health");
            Console.ResetColor();

            return true;
        }

        override protected void Die()
        {
            base.Die();

            OnCharacterDeath?.Invoke();
        }

        public bool EquipWeapon(Weapon newWeapon)
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
            Console.WriteLine($"{this.Name} equiped {newWeapon.Name}, and have {this.Damage} damage");
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
            Console.WriteLine($"{this.Name} equiped {newArmor.Name}, and have {this.Damage} damage");
            Console.ResetColor();
            return true;
        }
        public void ShowEquipedItems()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Equiped items:");
            Console.Write(
                $"Weapon: {this.CurrentWeapon.Name}\n" +
                $"Armor: {(CurrentArmor != null ? this.CurrentArmor.Name : "None")}\n" +
                $"Amulet: {/*this.EquipedAmulet.Name*/false}\n");
            Console.ResetColor();
        }
    }
}
