using ConsoleRPG.Entities.Characters;
using ConsoleRPG.Entities.Enemies;
using ConsoleRPG.Items;
using ConsoleRPG.Items.Ammos;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Weapons;
using ConsoleRPG.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal abstract class RestoreLoadedData
    {
        public static Character? LoadCharacter(GameSaveData SaveData)
        {
            if (SaveData == null) return null;

            var newCharacter = UniversalFactory<Character>.CreateObject(SaveData.CharacterClass.ToString());

            if (newCharacter == null) return null;

            newCharacter.Health = SaveData.CurrentHealth;
            newCharacter.MaxHealth = SaveData.MaxHealth;
            newCharacter.Damage = SaveData.Damage;
            newCharacter.DamageMultiplier = SaveData.DamageMultiplayer;
            newCharacter.DamageReduction = SaveData.DamageReduction;

            if (UniversalFactory<Item>.CreateObject(SaveData.CurrentWeaponType) is Weapon w)
            {
                newCharacter.CurrentWeapon = w;
            }
            var startArmor = (Armor?)UniversalFactory<Item>.CreateObject(SaveData.CurrentArmorType);
            var startAmulet = (Amulet?)UniversalFactory<Item>.CreateObject(SaveData.CurrentAmuletType);

            newCharacter.CurrentArmor = startArmor;
            newCharacter.CurrentAmulet = startAmulet;

            if (newCharacter is Archer archer)
            {
                var startAmmo = (Ammo?)UniversalFactory<Item>.CreateObject(SaveData.CurrentAmmoType);
                var savedAmmo = SaveData.Inventory.FirstOrDefault(it => it.Type == startAmmo?.GetType().Name);

                if (startAmmo != null && savedAmmo != null)
                {
                    startAmmo.Count = savedAmmo.Count;
                    archer.CurrentAmmo = startAmmo;
                }
            }

            newCharacter.Inventory.Items.Clear();

            foreach (var itemSaveData in SaveData.Inventory)
            {
                Item? newItem = UniversalFactory<Item>.CreateObject(itemSaveData.Type);

                if (newItem != null)
                {
                    if (newItem.CanStack)
                    {
                        newItem.Count = itemSaveData.Count;
                    }
                    newCharacter.Inventory.Items.Add(newItem);
                }
            }

            return newCharacter;
        }
        public static Queue<string> LoadLocationsOrder(GameSaveData SaveData)
        {
            if (SaveData == null) return [];

            var newLocationsOrder = new Queue<string>();

            foreach (var locationName in SaveData?.LocationsOrder ?? [])
            {
                newLocationsOrder.Enqueue(locationName);
            }

            return newLocationsOrder;
        }
        public static Location? LoadLocation(GameSaveData SaveData)
        {
            if (SaveData == null) return null;

            var newLocation = UniversalFactory<Location>.CreateObject(SaveData?.Location?.LocationType!)!;

            if (newLocation == null) return null;

            newLocation.Area = SaveData?.Location?.Area ?? [];
            newLocation.EnemiesInfo.Clear();

            foreach (var kv in SaveData?.Location?.Enemies ?? [])
            {
                int enemyX = kv.Key % newLocation.MapWidth;
                int enemyY = kv.Key / newLocation.MapWidth;

                var enemyPos = new Vector2(enemyX, enemyY);

                Enemy? newEnemy = UniversalFactory<Enemy>.CreateObject(kv.Value);

                if (newEnemy != null)
                {
                    newLocation.EnemiesInfo.Add(enemyPos, newEnemy);
                }
            }

            newLocation.Shop.Items.Clear();
            foreach (var offerData in SaveData?.Location?.ShopOffers ?? [])
            {
                if (offerData.Item == null) continue;

                var restoredItem = UniversalFactory<Item>.CreateObject(offerData.Item?.Type!);

                if (restoredItem != null)
                {
                    restoredItem.Count = offerData.Item!.Count;

                    Offer newOffer = new()
                    {
                        Item = restoredItem,
                        Cost = offerData.Cost,
                        Count = offerData.AvailableCount
                    };

                    newLocation.Shop.Items.Add(newOffer);
                }
            }

            return newLocation;
        }
    }
}
