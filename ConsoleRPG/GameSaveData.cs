using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRPG.Entities.Characters;
using ConsoleRPG.Items;
using ConsoleRPG.Items.Ammos;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Weapons;
using ConsoleRPG.Locations;

namespace ConsoleRPG
{
    internal class GameSaveData
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public string CurrentWeaponType { get; set; } = "";
        public string CurrentArmorType { get; set; } = "";
        public string CurrentAmuletType { get; set; } = "";
        public string CurrentAmmoType { get; set; } = "";
        public List<ItemSaveData> Inventory { get; set; } = [];
        public LocationSaveData? Location { get; set; }
        public CharacterData.CharacterClass CharacterClass { get; set; }
        public int Gold { get; set; }
        public Queue<string> LocationsOrder { get; set; } = new();
    }
}
