using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Inventory
    {
        private List<Item> inventory = new();
        private int currentPage = 1;

        public void AddItem(Item item)
        {
            var searchItem = inventory.FirstOrDefault(
                i => i.GetType() == item.GetType() &&
                i.Count < i.MaxCount);

            if (searchItem != null)
            {
                searchItem.Count++;
            }
            else
            {
                item.InventorySlot = inventory.Count + 1;
                inventory.Add(item);
            }
        }
        private void CheckItemsExist()
        {
            foreach (var item in inventory)
            {
                if (item.Count <= 0)
                {
                    item.Count = 0;
                    inventory.Remove(item);
                    return;
                }
            }
        }
        private void ShowEquipebleItems()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(
                $"Weapon: ");
        }
        public void ShowInventory()
        {
            CheckItemsExist();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Inventory:");
            Console.WriteLine($"Page {currentPage}:");

            foreach (var item in inventory)
            {
                Console.Write($"{item.InventorySlot}. {item.Name}");

                if (item.CanStack)
                {
                    Console.Write($" - X{item.Count}\n");
                }
                else
                {
                    Console.Write(Environment.NewLine);
                }
            }
            Console.ResetColor();
        }

        public Item? SelectItem(string input)
        {
            return inventory.FirstOrDefault(i => i.InventorySlot == int.Parse(input));
        }
    }
}
