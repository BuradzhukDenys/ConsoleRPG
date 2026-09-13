using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class OfferSaveData
    {
        public ItemSaveData? Item { get; set; }
        public int Cost { get; set; }
        public int AvailableCount { get; set; }
    }
}
