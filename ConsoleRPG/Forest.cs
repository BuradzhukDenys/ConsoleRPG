using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Forest : Location
    {
        private static readonly List<int> _map =
            [
                1, 0, 0, 1, 1,
                1, 0, 1, 1, 0,
                2, 0, 0, 1, 2,
                1, 1, 9, 1, 0,
                0, 0, 2, 0, 0,
                0, 1, 1, 1, 0,
                1, 2, 0, 0, 0,
                0, 1, 1, 0, 0
            ];
        private static readonly int _width = 5;
        private readonly List<Enemy> _enemies =
            [
                new Slime(),
                new Slime(),
                new Slime(),
                new Slime()
            ];
        public Forest() : base(_width, _map)
        {
            int k = 0;
            for (int i = 0; i < _map.Count; i++)
            {
                if (_map[i] == 2)
                {
                    int enemyX = i % _width;
                    int enemyY = i / _width;

                    var enemyPos = new Vector2(enemyX, enemyY);

                    _enemiesInfo.Add(enemyPos, _enemies[k++]);
                }
            }
        }
    }
}
