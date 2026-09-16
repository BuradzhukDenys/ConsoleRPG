using ConsoleRPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Locations;

internal record struct Offer(Item Item, int Cost, int Count = 1);
internal abstract class Shop
{
    public List<Offer> Items { get; protected set; } = [];

    private readonly Paginator<Offer> _paginator = new(6);
    public void ShowShop()
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;

        int totalPages = _paginator.GetTotalPages(Items.Count);
        List<Offer> itemsOnPage = _paginator.GetPage(Items);

        Console.WriteLine($"Page {_paginator.CurrentPage}/{totalPages}");
        for (int i = 0; i < itemsOnPage.Count; i++)
        {
            var offer = itemsOnPage[i];

            if (offer.Count > 1)
            {
                Console.WriteLine($"{i + 1}. {offer.Item.Name} X{offer.Count} - {offer.Cost} gold");
            }
            else
            {
                Console.WriteLine($"{i + 1}. {offer.Item.Name} - {offer.Cost} gold");
            }
        }

        Console.WriteLine("----------------------------------");
        CharacterData.ShowGold();
        Console.WriteLine("----------------------------------");
        Console.Write(
            "7. Previous page\n" +
            "8. Next page\n" +
            "9. Inventory\n" +
            "0. Back\n");
    }
    //Fix, a problem, when item is the reference and when i change count the same item in shop count change too
    public bool TryBuyItem(string input, out Item? item)
    {
        if (int.TryParse(input, out int slot) && slot >= 1 && slot <= _paginator.ItemsPerPage)
        {
            int realIndex = (_paginator.CurrentPage - 1) * _paginator.ItemsPerPage + (slot - 1);

            if (realIndex >= 0 && realIndex < Items.Count)
            {
                Offer offer = Items[realIndex];

                if (offer.Item != null && CharacterData.SpendGold(offer.Cost))
                {
                    item = offer.Item.Clone();
                    Console.WriteLine($"{offer.Item.Name} is bought");

                    if (offer.Count > 1)
                    {
                        offer.Count--;
                        Items[realIndex] = offer;
                    }
                    else
                    {
                        Items.RemoveAt(realIndex);
                    }
                    return true;
                }
            }
        }

        item = null;
        return false;
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
