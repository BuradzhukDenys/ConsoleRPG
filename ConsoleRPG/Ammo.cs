using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal abstract class Ammo(string name, int count) : Item(name, 99, count), IEquipable
    {
        private double _damageMultiplier;
        public double DamageMultiplier
        {
            get { return _damageMultiplier; }
            protected set
            {
                _damageMultiplier = Math.Clamp(value, 1, 2);
            }
        }
        public bool Equip(Character character)
        {
            if (character is IHasAmmo hero)
            {
                return hero.EquipAmmo(this);
            }

            return false;
        }
    }
}
