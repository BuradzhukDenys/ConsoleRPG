namespace ConsoleRPG
{
    internal abstract class Entity
    {
        private int _health;
        private string? _name;
        private int _damage = 0;
        private int _maxHealth;
        private double _damageReduction = 0.0;
        private readonly List<Effect> effects = [];

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
        public double DamageMultiplier { get; private set; } = 1.0f;
        public int TotalDamage
        {
            get { return (int)Math.Ceiling(Damage * DamageMultiplier); }
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
        public void AddDamageMultiplier(double multiplier)
        {
            if (multiplier < 0) throw new ArgumentException("Multiplier must be positive");
            DamageMultiplier += multiplier;
        }
        public void RemoveDamageMultiplier(double multiplier)
        {
            DamageMultiplier -= Math.Abs(multiplier);

            DamageMultiplier = Math.Max(1.0, DamageMultiplier);
        }
        virtual public void Attack(Entity entity)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} was attack {entity.Name} by {this.Damage} damage");
            entity.TakeDamage(this.TotalDamage);
            Console.ResetColor();
        }
        virtual public void Attack(Entity entity, int damage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} was attack {entity.Name} by {damage} damage");
            entity.TakeDamage(damage);
            Console.ResetColor();
        }
        public void TakeDamage(int damage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            int finaleDamage = (int)(damage - damage * DamageReduction);
            this.Health -= finaleDamage;
            Console.WriteLine($"{this.Name} was take damage by {finaleDamage} damage and have {this.Health} health");
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
            Console.ForegroundColor = ConsoleColor.Cyan;

            if (effects.Count > 0)
            {
                Console.WriteLine("Effects: ");
                for (int i = 0; i < effects.Count; i++)
                {
                    Console.Write($"{effects[i].Name} ({effects[i].Duration} turns)");

                    if (i < effects.Count - 1)
                    {
                        Console.Write(", ");
                    }

                    if ((i + 1) % 4 == 0)
                    {
                        Console.Write(Environment.NewLine);
                    }

                }

                if (effects.Count % 4 != 0)
                {
                    Console.Write(Environment.NewLine);
                }
            }
            else
            {
                Console.WriteLine("Effects: None");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Health: {this.Health}/{this.MaxHealth}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Damage: {this.TotalDamage}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Damage reduction: {this.DamageReduction * 100}%");
            Console.ResetColor();
        }
        public void AddEffect(Effect newEffect)
        {
            effects.Add(newEffect);

            newEffect.InitializeMessage(this);
        }
        //Walk through effects list copy, if effect will add new effect program don't crash
        public void ApplyEffects()
        {
            foreach (var effect in effects.ToList())
            {
                effect.Apply(this);
            }

            effects.RemoveAll(effect => effect.Duration <= 0);
        }
    }
}
