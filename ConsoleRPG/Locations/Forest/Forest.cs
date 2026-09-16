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
            new Slime(),
            new Slime(),
            new Slime()
        ];
    public Forest() : base(5, [
            3, 0, 0, 1, 1,
            1, 0, 1, 1, 0,
            2, 0, 0, 1, 2,
            1, 1, 9, 1, 0,
            0, 0, 2, 0, 0,
            0, 1, 1, 1, 0,
            1, 2, 0, 0, 0,
            0, 1, 1, 0, 0
        ], new ForestShop())
    {
        int k = 0;
        for (int i = 0; i < Area.Count; i++)
        {
            if (Area[i] == 2)
            {
                var enemyPos = new Vector2(i % MapWidth, i / MapWidth);
                EnemiesInfo.Add(enemyPos, _enemies[k]);
                k++;
            }
            else
            {
                Area[i] = 1;
            }
        }
    }
}
