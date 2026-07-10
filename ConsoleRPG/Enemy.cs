namespace ConsoleRPG
{
    internal abstract class Enemy : Entity
    {
        public int GoldReward { get; protected set; }

        public Enemy(string name, int health, int damage, int goldReward)
        {
            Name = name;
            Damage = damage;

            if (goldReward < 0)
            {
                throw new ArgumentOutOfRangeException("Gold reward can't be negative");
            }
            GoldReward = goldReward;

            MaxHealth = health;
            Health = health;
        }

        private void GiveReward()
        {
            Console.WriteLine($"{this.Name} give {this.GoldReward} gold");
            CharacterData.AddGold(GoldReward);
        }

        override protected void Die()
        {
            base.Die();
            GiveReward();
        }
    }
}
