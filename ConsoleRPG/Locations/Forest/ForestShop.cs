using ConsoleRPG.Items.Ammos;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Consumables;
using ConsoleRPG.Items.Weapons.MeleeWeapons;
using ConsoleRPG.Items.Weapons.RangedWeapons;
using ConsoleRPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Locations.Forest;

internal class ForestShop : Shop
{
    public ForestShop() : base()
    {
        Items.Add(new Offer(new LeatherArmor(), 15));
        Items.Add(new Offer(new GodArmor(), 60));
        Items.Add(new Offer(new FireAmulet(), 30));
        Items.Add(new Offer(new DamageAmulet(), 20));
        Items.Add(new Offer(new HealingPotion { Count = 1 }, 5, 5));
        switch (CharacterData.CurrentCharacterClass)
        {
            case CharacterData.CharacterClass.Warrior:
                Items.Add(new Offer(new UltraHammer(), 50));
                Items.Add(new Offer(new IronSword(), 15));
                Items.Add(new Offer(new Stick(), 5));
                break;
            case CharacterData.CharacterClass.Archer:
                Items.Add(new Offer(new GodBow(), 50));
                Items.Add(new Offer(new WoodenBow(), 15));
                Items.Add(new Offer(new Arrow { Count = 5 }, 3, 6));
                Items.Add(new Offer(new GodArrow { Count = 5 }, 8, 3));
                break;
        }
    }
}
