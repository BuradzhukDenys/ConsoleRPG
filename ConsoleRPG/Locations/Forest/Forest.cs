using ConsoleRPG.Entities.Enemies;
using ConsoleRPG.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Locations.Forest;

internal class Forest : Location
{
    private readonly List<Enemy> _enemies =
        [
            new Slime(),
            new Goblin(),
            new Slime(),
            new Slime(),
            new Ent(),
            new Goblin(),
            new Slime(),
            new Goblin()
        ];
    public Forest() : base(6, [
            3, 0, 0, 1, 1, 2,
            1, 0, 1, 1, 0, 0,
            2, 0, 0, 1, 2, 1,
            1, 1, 9, 1, 0, 1,
            0, 0, 2, 0, 0, 2,
            0, 1, 1, 1, 2, 0,
            1, 2, 0, 0, 0, 0,
            0, 1, 1, 1, 2, 0
        ], new ForestShop())
    {
        InitEnemies(_enemies);
    }
}
