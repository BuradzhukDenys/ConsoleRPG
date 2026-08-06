using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class DamageAmulet() : Amulet("Amulet of damage"), IPassiveAmulet
    {
        private readonly double _damageMultiplier = 0.15;
        public void UnequipEffect(Character character)
        {
            character.RemoveDamageMultiplier(_damageMultiplier);
        }
        public override bool Equip(Character character)
        {
            if (base.Equip(character))
            {
                character.AddDamageMultiplier(_damageMultiplier);
                return true;
            }

            return false;
        }
        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Add 15% damage");
            Console.ResetColor();
        }
    }
}
