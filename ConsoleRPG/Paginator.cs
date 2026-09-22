using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    /// <summary>
    /// Class paginator for make pages of items in shop and inventory
    /// </summary>
    /// <typeparam name="T">Type of list items per page</typeparam>
    /// <param name="ItemsPerPage"></param>
    internal class Paginator<T>(int ItemsPerPage)
    {
        public int CurrentPage { get; private set; } = 1;
        public int ItemsPerPage { get; } = ItemsPerPage;
        private const int _maxPages = 99;
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
