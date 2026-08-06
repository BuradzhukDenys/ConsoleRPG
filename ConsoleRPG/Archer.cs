using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Archer : Character, IHasAmmo
    {
        public IRangedWeapon? ArcherWeapon => CurrentWeapon as IRangedWeapon;
        public Ammo? CurrentAmmo { get; private set; }
        public Archer(string name, int health) : base(name, health, new WoodenBow())
        {
            CurrentAmmo = new Arrow(15);
            AddDamageMultiplier(CurrentAmmo.DamageMultiplier);
            Inventory.AddItem(CurrentAmmo);
        }
        public Archer(string name, int health, int maxHealth, Weapon startWeapon, Armor startArmor, Amulet startAmulet, Ammo EquipedAmmo)
            : base(name, health, maxHealth, startWeapon, startArmor, startAmulet)
        {
            CurrentAmmo = EquipedAmmo;
            AddDamageMultiplier(CurrentAmmo.DamageMultiplier);
        }
        public bool EquipAmmo(Ammo newAmmo)
        {
            if (CurrentAmmo != null && CurrentAmmo.GetType() == newAmmo.GetType())
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{newAmmo.Name} already equiped!");
                Console.ResetColor();
                return false;
            }

            RemoveDamageMultiplier(CurrentAmmo!.DamageMultiplier);
            CurrentAmmo = newAmmo;

            AddDamageMultiplier(CurrentAmmo.DamageMultiplier);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} equiped {newAmmo.Name}, and have {this.Damage} damage");
            Console.ResetColor();
            return true;
        }
        public override void Attack(Entity entity)
        {
            if (CurrentWeapon is not IRangedWeapon)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You can't attack without ranged weapon!");
                Console.ResetColor();
                return;
            }

            if (CurrentAmmo == null || CurrentAmmo.Count <= 0)
            {
                base.Attack(entity, ArcherWeapon!.MeleeDamage);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"You attack {entity.Name} by melee attack");
                Console.ResetColor();
                return;
                if (CurrentAmmo.Count > 0)
                {
                    
                }
            }

            CurrentAmmo.Count--;
            base.Attack(entity);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"You consumed 1 {CurrentAmmo.Name}");

            if (CurrentAmmo.Count <= 0)
            {
                RemoveDamageMultiplier(CurrentAmmo.DamageMultiplier);
                CurrentAmmo = null;
                Console.WriteLine("You are out of ammo!");
            }
            Console.ResetColor();
        }
        public override bool EquipWeapon(Weapon newWeapon)
        {
            if (newWeapon is IRangedWeapon rangedWeapon)
            {
                return base.EquipWeapon(newWeapon);
            }

            return false;
        }
        public override void ShowEquipedItems()
        {
            base.ShowEquipedItems();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Ammo: {(CurrentAmmo != null ? this.CurrentAmmo.Name : "None")}");
            Console.ResetColor();
        }
        public override void ShowBattleInfo()
        {
            base.ShowBattleInfo();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Melee damage: {ArcherWeapon?.MeleeDamage}");
            Console.ResetColor();
        }
    }
}
