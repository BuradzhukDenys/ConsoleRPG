using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRPG.Effects;

namespace ConsoleRPG.Items.Weapons.MeleeWeapons
{
    internal class FireSword : Weapon, IEffectProvider
    {
        public FireSword() : base("Fire sword")
        {
            Damage = 16;
        }
        public double EffectChance => 0.3;
        public int EffectDuration => 3;
        public int EffectDamage => 5;
        public Effect GetEffect()
        {
            return new BurnEffect(EffectDuration, EffectDamage);
        }
    }
}
