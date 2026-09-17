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

namespace ConsoleRPG.Locations.Cave
{
    internal class CaveShop : Shop
    {
        public CaveShop() : base()
        {
            Items.Add(new Offer(new MythrilArmor(), 34));
            Items.Add(new Offer(new FireAmulet(), 32));
            Items.Add(new Offer(new HealingPotion { Count = 1 }, 12, 5));
            switch (CharacterData.CurrentCharacterClass)
            {
                case CharacterData.CharacterClass.Warrior:
                    Items.Add(new Offer(new FireSword(), 25));
                    Items.Add(new Offer(new BattleAxe(), 40));
                    break;
                case CharacterData.CharacterClass.Archer:
                    Items.Add(new Offer(new Arrow { Count = 5 }, 6, 4));
                    Items.Add(new Offer(new ThornBow { Count = 5 }, 6, 4));
                    Items.Add(new Offer(new FireArrow { Count = 3 }, 10, 3));
                    Items.Add(new Offer(new GodArrow { Count = 1 }, 40, 2));
                    break;
            }
        }
    }
}
