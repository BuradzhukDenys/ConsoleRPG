using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Inventory
    {
        private List<Item> inventory = [];
        private int currentPage = 1;
        private const int ItemsPerPage = 7;

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
                inventory.Add(item);
            }
        }
        private void CheckItemsExist()
        {
            inventory.RemoveAll(item => item.Count <= 0);
        }
        public void ShowInventory()
        {
            CheckItemsExist();

            int totalPages = (int)Math.Ceiling((double)inventory.Count / ItemsPerPage);
            if (totalPages == 0) totalPages = 1;

            if (currentPage > totalPages) currentPage = totalPages;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Inventory: (Page {currentPage}/{totalPages}):");

            var itemsOnPage = inventory
                .Skip((currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();

            for (int i = 0; i < itemsOnPage.Count; i++)
            {
                var item = itemsOnPage[i];
                int displaySlot = i + 1;

                Console.Write($"{displaySlot}. {item.Name}");

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
            if (int.TryParse(input, out int slot) && slot >= 1 && slot <= ItemsPerPage)
            {
                int realIndex = (currentPage - 1) * ItemsPerPage + (slot - 1);

                if (realIndex >= 0 && realIndex < inventory.Count)
                {
                    return inventory[realIndex];
                }
            }
            return null;
        }

        public void NextPage()
        {
            currentPage = Math.Clamp(currentPage + 1, 1, 999);
        }
        public void PreviousPage()
        {
            currentPage = Math.Clamp(currentPage - 1, 1, 999);
        }
        //public T? SelectItem<T>() where T : Item
        //{
        //    return inventory.OfType<T>().FirstOrDefault();
        //}
    }
}
