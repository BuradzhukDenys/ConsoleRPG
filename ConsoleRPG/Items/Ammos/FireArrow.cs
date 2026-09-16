using ConsoleRPG.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Ammos
{
    internal class FireArrow : Ammo, IEffectProvider
    {
        public double EffectChance => 0.3;
        public int EffectDuration => 2;
        public int EffectDamage => 4;
        public Effect GetEffect()
        {
            return new BurnEffect(EffectDuration, EffectDamage);
        }
        public FireArrow() : base("Fire arrow")
        {
            DamageMultiplier = 1.2;
        }
    }
}
