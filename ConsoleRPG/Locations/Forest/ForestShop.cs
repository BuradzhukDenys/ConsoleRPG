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
        Items.Add(new Offer(new LeatherArmor(), 10));
        Items.Add(new Offer(new IronArmor(), 18));
        Items.Add(new Offer(new HealthAmulet(), 15));
        Items.Add(new Offer(new HealingPotion { Count = 1 }, 5, 8));
        switch (CharacterData.CurrentCharacterClass)
        {
            case CharacterData.CharacterClass.Warrior:
                Items.Add(new Offer(new IronSword(), 15));
                break;
            case CharacterData.CharacterClass.Archer:
                Items.Add(new Offer(new CompositeBow(), 15));
                Items.Add(new Offer(new Arrow { Count = 5 }, 3, 6));
                break;
        }
    }
}
