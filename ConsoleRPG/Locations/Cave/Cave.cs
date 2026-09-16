using ConsoleRPG.Entities.Enemies;
using ConsoleRPG.Locations.Forest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Locations.Cave
{
    internal class Cave : Location
    {
        private readonly List<Enemy> _enemies =
        [
            new Golem(),
            new Goblin(),
            new Skeleton(),
            new Skeleton(),
            new Skeleton(),
            new Skeleton(),
            new Goblin(),
            new Golem(),
            new Goblin()
        ];
        public Cave() : base(8, [
            2, 0, 0, 0, 1, 1, 2, 1,
            1, 2, 0, 0, 1, 0, 0, 1,
            0, 1, 1, 2, 1, 0, 1, 9,
            0, 0, 0, 0, 0, 0, 1, 0,
            1, 2, 1, 0, 1, 1, 2, 0,
            1, 0, 1, 1, 2, 0, 1, 0,
            2, 0, 0, 0, 1, 0, 3, 0,
            0, 0, 0, 0, 2, 0, 0, 0,
            ], new CaveShop())
        {
            InitEnemies(_enemies);
        }
    }
}
