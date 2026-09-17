using ConsoleRPG.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal interface IEffectProvider
    {
        public double EffectChance { get; }
        public int EffectDuration { get; }
        public int EffectDamage
        {
            get { return 0; }
        }
        public Effect GetEffect();
    }
}
