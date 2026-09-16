using ConsoleRPG.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Locations.Hell
{
    internal class Hell : Location
    {
        private readonly List<Enemy> _enemies =
        [
            new FireElemental(),
            new Devil(),
            new Devil(),
            new Imp(),
            new FireElemental(),
            new Imp(),
            new Imp(),
            new FireElemental(),
            new FireElemental()
        ];
        public Hell() : base(10, [
            0, 9, 0, 2, 3, 0, 1, 2, 0, 0,
            1, 1, 0, 1, 0, 1, 1, 0, 1, 2,
            2, 0, 0, 1, 1, 1, 0, 0, 1, 0,
            1, 1, 1, 2, 0, 1, 1, 1, 2, 0,
            2, 0, 0, 0, 0, 2, 0, 0, 1, 2
            ], new HellShop())
        {
            InitEnemies(_enemies);
        }
    }
}
