using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    /// <summary>
    /// A generic paginator to handle pagination for UI lists (shop, inventory)
    /// </summary>
    /// <typeparam name="T">The type of items being paginated.</typeparam>
    /// <param name="ItemsPerPage"></param>
    internal class Paginator<T>
    {
        public int CurrentPage { get; private set; } = 1;
        public int ItemsPerPage { get; }
        private const int _maxPages = 99;
        public Paginator(int ItemsPerPage)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ItemsPerPage);
            this.ItemsPerPage = ItemsPerPage;
        }
        public List<T> GetPage(List<T> allItems)
        {
            int totalPages = GetTotalPages(allItems.Count);

            if (CurrentPage > totalPages) CurrentPage = totalPages;

            return allItems
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();
        }
        public int GetTotalPages(int totalItemsCount)
        {
            int totalPages = (int)Math.Ceiling((double)totalItemsCount / ItemsPerPage);
            return totalPages == 0 ? 1 : totalPages;
        }
        public void NextPage(int totalItemsCount)
        {
            int totalPages = GetTotalPages(totalItemsCount);
            CurrentPage = Math.Clamp(CurrentPage + 1, 1, totalPages);
        }
        public void PreviousPage()
        {
            CurrentPage = Math.Clamp(CurrentPage - 1, 1, _maxPages);
        }
    }
}
