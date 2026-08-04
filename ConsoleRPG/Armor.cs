using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleRPG
{
    internal abstract class Armor(string name) : Item(name, 1, 1), IEquipable
    {
        /// <summary>
        /// In fraction percent
        /// </summary>
        public double DamageReduction { get; protected set; }
        public bool Equip(Character character)
        {
            return character.EquipArmor(this);
        }
        public override void ShowInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Damage reduction: {DamageReduction}");
            Console.ResetColor();
        }
    }
}
