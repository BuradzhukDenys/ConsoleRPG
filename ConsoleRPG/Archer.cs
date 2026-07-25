using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Archer : Character
    {
        public Ammo? CurrentAmmo
        {
            get;
            private set;
        }
        public Archer(string name, int health) : base(name, health, new WoodenBow())
        {
            CurrentAmmo = new Arrow(15);
            Damage = (int)Math.Ceiling((double)CurrentWeapon.Damage * CurrentAmmo.DamageMultiplier);
            Inventory.AddItem(CurrentAmmo);
        }
        public Archer(string name, int health, int maxHealth, Weapon startWeapon, Armor startArmor, Ammo EquipedAmmo)
            : base(name, health, maxHealth, startWeapon, startArmor)
        {
            CurrentAmmo = EquipedAmmo;
            Damage = (int)Math.Ceiling((double)CurrentWeapon.Damage * CurrentAmmo.DamageMultiplier);
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

            CurrentAmmo = newAmmo;
            Damage = (int)Math.Ceiling((double)CurrentWeapon.Damage * CurrentAmmo.DamageMultiplier);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{this.Name} equiped {newAmmo.Name}, and have {this.Damage} damage");
            Console.ResetColor();
            return true;
        }
        public override void Attack(Entity entity)
        {
            if (CurrentAmmo == null || CurrentAmmo.Name == null)
            {
                return;
            }

            if (CurrentWeapon is IRangedWeapon rangedWeapon && rangedWeapon.CheckAmmos(this, CurrentAmmo.Name))
            {
                CurrentAmmo.Count--;
                base.Attack(entity);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"You don't have ammo");
                Console.ResetColor();
            }

            if (CurrentAmmo.Count <= 0)
            {
                CurrentAmmo = null;
            }
        }
        public override bool EquipWeapon(Weapon newWeapon)
        {
            if (newWeapon is IRangedWeapon)
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
    }
}
