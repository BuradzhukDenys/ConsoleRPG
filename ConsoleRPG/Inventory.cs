using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Inventory
    {
        private HashSet<Item> inventory = new();

        public void AddItem(Item item)
        {
            if (inventory.Equals(item))
            {
                item.Count++;
            }
            else
            {
                inventory.Add(item);
            }
            inventory = inventory.OrderBy(i => i.Name).ToHashSet();
        }

        public void ShowInventory()
        {
            Console.WriteLine("Inventory:");
            
            foreach (var item in inventory)
            {
                Console.Write(item.Name);

                if (item.CanStack)
                {
                    Console.Write($" - X{item.Count}");
                }
            }
        }
    }
}
