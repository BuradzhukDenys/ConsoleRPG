using ConsoleRPG.Entities.Enemies;

namespace ConsoleRPG.Locations;

internal abstract class Location(int width, List<int> map, Shop shop)
{
    public event Action<Enemy>? StartBattle;
    public event Action? OpenShop;
    public enum Direction
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    protected List<int> Area { get; private set; } = map;
    protected int MapWidth { get; private set; } = width;
    protected Dictionary<Vector2, Enemy> _enemiesInfo = [];
    public Shop Shop { get; private set; } = shop;
    public void ShowMap()
    {
        for (int i = 0; i < Area.Count; i++)
        {
            switch (Area[i])
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
                case 3:
                    Console.Write("[S]");
                    break;
                case 9:
                    Console.Write("[P]");
                    break;
            }

            if ((i + 1) % MapWidth == 0)
            {
                Console.Write(Environment.NewLine);
            }
        }
    }
    public void Move(Direction dir)
    {
        int playerIndex = Area.IndexOf(9);

        int playerX = playerIndex % MapWidth;
        int playerY = playerIndex / MapWidth;

        int mapHeight = Area.Count / MapWidth;

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

        if (nextX < 0 || nextX >= MapWidth || nextY < 0 || nextY >= mapHeight)
        {
            return;
        }

        int nextStepIndex = nextY * MapWidth + nextX;
        int nextStepValue = Area[nextStepIndex];

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
        else if (nextStepValue == 3)
        {
            if (Shop != null) OpenShop?.Invoke();
            return;
        }

        Area[playerIndex] = 1;
        Area[nextStepIndex] = 9;
    }
    //When enemy is defeated, find this enemy in Dictionary and delete enemy from dictionary and remove in map to empty cell
    public void RemoveDefeatedEnemy(Enemy deadEnemy)
    {
        var enemyEntry = _enemiesInfo.FirstOrDefault(kV => kV.Value == deadEnemy);

        if (enemyEntry.Value != null)
        {
            Vector2 pos = enemyEntry.Key;

            _enemiesInfo.Remove(pos);

            int mapIndex = pos.Y * MapWidth + pos.X;
            Area[mapIndex] = 1;
        }
    }
}
