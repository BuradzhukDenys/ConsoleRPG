namespace ConsoleRPG
{
    internal abstract class Location(int width, List<int> mapArea)
    {
        public event Action<Enemy>? StartBattle;
        public enum Direction
        {
            UP,
            RIGHT,
            DOWN,
            LEFT
        }
        private readonly List<int> _area = mapArea;
        private readonly int _mapWidth = width;
        protected Dictionary<Vector2, Enemy> _enemiesInfo = [];
        public void ShowMap()
        {
            for (int i = 0; i < _area.Count; i++)
            {
                switch (_area[i])
                {
                    case 0:
                        Console.Write("   ");
                        break;
                    case 1:
                        Console.Write("[ ]");
                        break;
                    case 2:
                        Console.Write("[E]");
                        break;
                    case 9:
                        Console.Write("[P]");
                        break;
                }

                if ((i + 1) % _mapWidth == 0)
                {
                    Console.Write(Environment.NewLine);
                }
            }
        }
        public void Move(Direction dir)
        {
            int playerIndex = _area.IndexOf(9);

            int playerX = playerIndex % _mapWidth;
            int playerY = playerIndex / _mapWidth;

            int mapHeight = _area.Count / _mapWidth;

            int nextX = playerX;
            int nextY = playerY;
            switch (dir)
            {
                case Direction.UP: nextY--; break;
                case Direction.DOWN: nextY++; break;
                case Direction.LEFT: nextX--; break;
                case Direction.RIGHT: nextX++; break;
                default: return;
            }

            if (nextX < 0 || nextX >= _mapWidth || nextY < 0 || nextY >= mapHeight)
            {
                return;
            }

            int nextStepIndex = nextY * _mapWidth + nextX;
            int nextStepValue = _area[nextStepIndex];

            if (nextStepValue == 0)
            {
                return;
            }

            if (nextStepValue == 2)
            {
                var enemyPos = new Vector2(nextX, nextY);

                if (_enemiesInfo.TryGetValue(enemyPos, out Enemy? enemy) && enemy != null)
                {
                    StartBattle?.Invoke(enemy);
                }
                return;
            }

            _area[playerIndex] = 1;
            _area[nextStepIndex] = 9;
        }
        //When enemy is defeated, find this enemy in Dictionary and delete enemy from dictionary and remove in map to empty cell
        public void RemoveDefeatedEnemy(Enemy deadEnemy)
        {
            var enemyEntry = _enemiesInfo.FirstOrDefault(kV => kV.Value == deadEnemy);

            if (enemyEntry.Value != null)
            {
                Vector2 pos = enemyEntry.Key;

                _enemiesInfo.Remove(pos);

                int mapIndex = pos.Y * _mapWidth + pos.X;
                _area[mapIndex] = 1;
            }
        }
    }
}
