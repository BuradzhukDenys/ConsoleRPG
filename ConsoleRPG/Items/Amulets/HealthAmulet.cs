using ConsoleRPG.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Items.Amulets
{
    internal class HealthAmulet() : Amulet("Health amulet"), IPassiveAmulet
    {
        private const int _maxHealthBonus = 25;
        public void UnequipEffect(Character character)
        {
            character.Health -= _maxHealthBonus;
            character.MaxHealth -= _maxHealthBonus;
        }
        public override bool Equip(Character character)
        {
            if (base.Equip(character))
            {
                character.MaxHealth += _maxHealthBonus;
                character.Health += _maxHealthBonus;
                return true;
            }

            return false;
        }
        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Add 25 max health");
            Console.ResetColor();
        }
    }
}
