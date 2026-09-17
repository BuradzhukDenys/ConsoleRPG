using ConsoleRPG.Effects;

namespace ConsoleRPG.Entities.Enemies;

internal abstract class Enemy : Entity
{
    public int GoldReward { get; protected set; }
    public Enemy(string name, int health, int damage, int goldReward, double damageReduction)
    {
        Name = name;
        Damage = damage;

        ArgumentOutOfRangeException.ThrowIfNegative(goldReward);
        GoldReward = goldReward;

        MaxHealth = health;
        Health = health;
        DamageReduction = damageReduction;
    }
    private void GiveReward()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Name} give {GoldReward} gold");
        CharacterData.AddGold(GoldReward);
        Console.ResetColor();
    }
    protected void TryApplyEffect(Entity target, Effect effect, double chance)
    {
        var random = new Random();
        if (random.NextDouble() <= chance)
        {
            target.AddEffect(effect);
        }
    }

    protected void ShowEffectInfo(string effectName, double chance, int duration, ConsoleColor color, int damage = 0)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"{effectName} chance: {chance * 100}% ({duration} turns, {(damage == 0 ? "" : "damage")})");
        Console.ResetColor();
    }
    override protected void Die()
    {
        base.Die();
        GiveReward();
    }
}
