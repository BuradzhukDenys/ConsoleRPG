namespace ConsoleRPG
{
    internal abstract class Character : Entity
    {
        public event Action? OnCharacterDeath;

        private int _currentHealingPotionReloadTime = HealingPotion.reloadTime;
        public struct HealingPotion
        {
            public bool canUse;
            public int healAmount;
            public static int reloadTime = 2;

            public HealingPotion()
            {
                canUse = true;
                healAmount = 30;
            }
        }

        private HealingPotion healingPotion = new();
        private int CurrentHealingPotionReloadTime
        {
            get { return _currentHealingPotionReloadTime; }
            set
            {
                if (value <= 0)
                {
                    _currentHealingPotionReloadTime = HealingPotion.reloadTime;
                    healingPotion.canUse = true;
                    return;
                }

                _currentHealingPotionReloadTime = value;
            }
        }
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

        public override void Attack(Entity entity)
        {
            base.Attack(entity);

            if (this.healingPotion.canUse)
            {
                return;
            }    
            CurrentHealingPotionReloadTime--;
        }

        public bool Heal()
        {
            if (!this.healingPotion.canUse)
            {
                Console.WriteLine($"You can't heal, wait {CurrentHealingPotionReloadTime} turns");
                return false;
            }
            if (Health == MaxHealth)
            {
                Console.WriteLine($"Your health is full");
                return false;
            }

            Health += healingPotion.healAmount;
            healingPotion.canUse = false;
            Console.WriteLine($"{this.Name} was healed by {this.healingPotion.healAmount} and have {this.Health} health");

            return true;
        }

        override public void ShowBattleInfo()
        {
            base.ShowBattleInfo();
            Console.WriteLine($"Can use healing potion: {(this.healingPotion.canUse ? "yes" : "no")}");
        }

        override protected void Die()
        {
            base.Die();

            OnCharacterDeath?.Invoke();
        }
    }
}
