using ConsoleRPG.Entities.Enemies;
using ConsoleRPG.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class LocationSaveData
    {
        public List<int> Area { get; set; } = [];
        public string LocationType { get; set; } = "";
        public Dictionary<int, string> Enemies { get; set; } = [];
        public List<OfferSaveData> ShopOffers { get; set; } = [];
    }
}
