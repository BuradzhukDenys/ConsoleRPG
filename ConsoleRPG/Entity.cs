namespace ConsoleRPG
{
    internal abstract class Entity
    {
        private int _health;
        private string? _name;
        private int _damage;
        private int _maxHealth;


        public bool IsDead { get; protected set; } = false;
        public string Name
        {
            get { return _name!; }
            set
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
        virtual public void Attack(Entity entity)
        {
            Console.WriteLine($"{this.Name} was attack {entity.Name} by {this.Damage} damage");
            entity.TakeDamage(Damage);
        }

        protected void TakeDamage(int damage)
        {
            this.Health -= damage;
            Console.WriteLine($"{this.Name} was take damage by {damage} damage and have {this.Health} health");
        }

        virtual protected void Die()
        {
            IsDead = true;
            Console.WriteLine($"{Name} died");
        }

        virtual public void ShowBattleInfo()
        {
            Console.Write(
                $"{this.Name}:\n" +
                $"Health: {this.Health}/{this.MaxHealth}\n" +
                $"Damage: {this.Damage}\n"
                );
        }
    }
}
