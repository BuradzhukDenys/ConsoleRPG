using ConsoleRPG.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Entities.Enemies
{
    internal class FireElemental() : Enemy("Fire elemental", 35, 5, 30, 0.1)
    {
        private const double _fireChance = 0.65;
        private const int _fireDuration = 2;
        private const int _fireDamage = 5;
        public override void Attack(Entity entity)
        {
            base.Attack(entity);

            TryApplyEffect(entity, new BurnEffect(_fireDuration, _fireDamage), _fireChance);
        }
        public override void ShowBattleInfo()
        {
            base.ShowBattleInfo();

            ShowEffectInfo("Fire", _fireChance, _fireDuration, ConsoleColor.Red, _fireDamage);
        }
    }
}
