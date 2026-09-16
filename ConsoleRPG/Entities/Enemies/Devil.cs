using ConsoleRPG.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Entities.Enemies
{
    internal class Devil() : Enemy("Devil", 95, 20, 40, 0.2)
    {
        private const double _fireChance = 0.30;
        private const int _fireDuration = 4;
        private const int _fireDamage = 8;
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
