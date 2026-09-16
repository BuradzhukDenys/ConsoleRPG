using ConsoleRPG.Items.Ammos;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Consumables;
using ConsoleRPG.Items.Weapons.MeleeWeapons;
using ConsoleRPG.Items.Weapons.RangedWeapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Locations.Hell
{
    internal class HellShop : Shop
    {
        public HellShop() : base()
        {
            Items.Add(new Offer(new BlackSteelArmor(), 45));
            Items.Add(new Offer(new DamageAmulet(), 38));
            Items.Add(new Offer(new GodArmor(), 65));
            Items.Add(new Offer(new HealingPotion { Count = 1 }, 15, 5));
            switch (CharacterData.CurrentCharacterClass)
            {
                case CharacterData.CharacterClass.Warrior:
                    Items.Add(new Offer(new UltraHammer(), 60));
                    break;
                case CharacterData.CharacterClass.Archer:
                    Items.Add(new Offer(new GodBow(), 65));
                    Items.Add(new Offer(new Arrow { Count = 5 }, 8, 5));
                    Items.Add(new Offer(new FireArrow { Count = 3 }, 10, 6));
                    Items.Add(new Offer(new GodArrow { Count = 2 }, 20, 3));
                    break;
            }
        }
    }
}
