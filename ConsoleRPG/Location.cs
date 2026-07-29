namespace ConsoleRPG
{
    internal abstract class Location(int width, List<int> mapArea)
    {
        public enum Direction
        {
            UP,
            RIGHT,
            DOWN,
            LEFT
        }
        private readonly List<int> _area = mapArea!;
        private readonly int _mapWidth = width;
        private void ShowMap()
        {
            for (int i = 0; i < _area.Count; i++)
            {
                if (_area[i] == 2)
                {
                    Console.Write("[X]");
                }
                else if (_area[i] == 1)
                {
                    Console.Write("[ ]");
                }
                else if (_area[i] == 0)
                {
                    Console.Write("   ");
                }

                if ((i + 1) % _mapWidth == 0)
                {
                    Console.Write(Environment.NewLine);
                }
            }
        }
        private void Move(Direction dir)
        {
            int playerIndex = _area.IndexOf(2);

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

            if (_area[nextStepIndex] == 0)
            {
                return;
            }

            _area[playerIndex] = 1;
            _area[nextStepIndex] = 2;
        }
        public void Start()
        {
            while (true)
            {
                Console.Clear();
                ShowMap();
                Console.Write(
                    "1. UP\n" +
                    "2. DOWN\n" +
                    "3. RIGHT\n" +
                    "4. LEFT\n");

                string input = Console.ReadLine()!;

                switch (input)
                {
                    case "1":
                        Move(Direction.UP);
                        break;
                    case "2":
                        Move(Direction.DOWN);
                        break;
                    case "3":
                        Move(Direction.RIGHT);
                        break;
                    case "4":
                        Move(Direction.LEFT);
                        break;
                    default:
                        Console.WriteLine("Unknown action");
                        break;
                }
            }
        }
    }
}
