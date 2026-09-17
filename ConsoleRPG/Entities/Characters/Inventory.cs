using ConsoleRPG.Items;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Entities.Characters;

internal class Inventory
{
    public List<Item> Items { get; private set; } = [];

    private Paginator<Item> _paginator = new(7);
    public Inventory()
    {
        Items = [];

        Items.Add(new HealthAmulet());
        Items.Add(new DamageAmulet());
        Items.Add(new LeatherArmor());
    }
    public Inventory(List<Item> items)
    {
        Items = items;
    }
    public void AddItem(Item item)
    {
        var searchItem = Items.FirstOrDefault(
            i => i.GetType() == item.GetType() &&
            i.Count < i.MaxCount);

        if (searchItem != null)
        {
            searchItem.Count += item.Count;
        }
        else
        {
            Items.Add(item);
        }
    }
    private void CheckItemsExist()
    {
        Items.RemoveAll(item => item.Count <= 0);
    }
    public void ShowInventory()
    {
        CheckItemsExist();

        int totalPages = _paginator.GetTotalPages(Items.Count);
        List<Item> itemsOnPage = _paginator.GetPage(Items);

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

            if (realIndex >= 0 && realIndex < Items.Count)
            {
                return Items[realIndex];
            }
        }
        return null;
    }
    public void NextPage()
    {
        _paginator.NextPage(Items.Count);
    }
    public void PreviousPage()
    {
        _paginator.PreviousPage();
    }
}
