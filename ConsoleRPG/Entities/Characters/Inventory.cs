using ConsoleRPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Entities.Characters;

internal class Inventory
{
    private List<Item> inventory = [];

    private Paginator<Item> _paginator = new(7);

    public void AddItem(Item item)
    {
        var searchItem = inventory.FirstOrDefault(
            i => i.GetType() == item.GetType() &&
            i.Count < i.MaxCount);

        if (searchItem != null)
        {
            searchItem.Count += item.Count;
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

        int totalPages = _paginator.GetTotalPages(inventory.Count);
        List<Item> itemsOnPage = _paginator.GetPage(inventory);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Inventory: (Page {_paginator.CurrentPage}/{totalPages}):");

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
        if (int.TryParse(input, out int slot) && slot >= 1 && slot <= _paginator.ItemsPerPage)
        {
            int realIndex = (_paginator.CurrentPage - 1) * _paginator.ItemsPerPage + (slot - 1);

            if (realIndex >= 0 && realIndex < inventory.Count)
            {
                return inventory[realIndex];
            }
        }
        return null;
    }
    public void NextPage()
    {
        _paginator.NextPage(inventory.Count);
    }
    public void PreviousPage()
    {
        _paginator.PreviousPage();
    }
    //public bool CheckAmmos(string AmmoName)
    //{
    //    Ammo? selectedAmmo = (Ammo?)inventory.FirstOrDefault(item => item.Name == AmmoName);

    //    return selectedAmmo != null;
    //}


    //public T? SelectItem<T>() where T : Item
    //{
    //    return inventory.OfType<T>().FirstOrDefault();
    //}
}
