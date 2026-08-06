using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class FireAmulet() : Amulet("Fire amulet"), IAttackAmulet
    {
        private readonly double _baseBurnChance = 0.3;
        private double _burnChance = 0.3;
        private readonly int _fireDurationTurns = 2;
        public double BurnChance
        {
            get { return _burnChance; }
            set
            {
                _burnChance = Math.Clamp(value, 0.01, 0.65);
            }
        }
        public IAttackAmulet.EffectTarget AmuletEffectTarget => IAttackAmulet.EffectTarget.Enemy;
        public void AmuletAction(Entity entity)
        {
            Random rand = new();

            double chance = rand.NextDouble();

            if (chance > 0 && chance <= BurnChance)
            {
                entity.AddEffect(new BurnEffect(2));
                BurnChance = _baseBurnChance;
            }
            else
            {
                BurnChance += 0.03;
            }
        }
        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Have chance in {BurnChance * 100}% to burn enemy for {_fireDurationTurns} {(_fireDurationTurns > 1 ? "turns" : "turn")}");
            Console.ResetColor();
        }
    }
}
