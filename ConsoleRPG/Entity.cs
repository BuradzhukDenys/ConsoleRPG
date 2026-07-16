namespace ConsoleRPG
{
    internal abstract class Entity
    {
        private int _health;
        private string? _name;
        private int _damage;
        private int _maxHealth;
        private double _damageReduction = 0.0;

        public bool IsDead { get; protected set; } = false;
        public string Name
        {
            get { return _name!; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name can't be empty or null");
                }

                _name = value;
            }
        }
        public int Health
        {
            get { return _health; }
            protected set
            {
                _health = Math.Clamp(value, 0, MaxHealth);

                if (_health == 0 && !IsDead)
                {
                    Die();
                }
            }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Max health can't be zero or less");
                }

                _maxHealth = value;

                if (Health > _maxHealth)
                {
                    Health = _maxHealth;
                }
            }
        }
        public int Damage
        {
            get { return _damage; }
            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Damage can't be zero or less");
                }

                _damage = value;
            }
        }

        /// <summary>
        /// In fraction percent
        /// </summary>
        public double DamageReduction
        {
            get { return _damageReduction; }
            set
            {
                _damageReduction = Math.Clamp(value, 0, 0.5);
            }
        }
        virtual public void Attack(Entity entity)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} was attack {entity.Name} by {this.Damage} damage");
            entity.TakeDamage(Damage);
            Console.ResetColor();

        }

        private void TakeDamage(int damage)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} was take damage by {damage} damage and have {this.Health - damage} health");
            int finaleDamage = (int)(damage - damage * DamageReduction);
            this.Health -= finaleDamage;
            Console.ResetColor();

        }

        virtual protected void Die()
        {
            IsDead = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Name} died");
            Console.ResetColor();
        }

        virtual public void ShowBattleInfo()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{this.Name}:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Health: {this.Health}/{this.MaxHealth}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Damage: {this.Damage}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Damage reduction: {this.DamageReduction * 100}%");
            Console.ResetColor();
        }
    }
}
