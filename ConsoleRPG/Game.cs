using static ConsoleRPG.Location;

namespace ConsoleRPG
{
    internal sealed class Game
    {
        enum GameState
        {
            SelectCharacterClass,
            Battle,
            Inventory,
            ItemAction,
            Map,
            Waiting
        }
        enum BattleState
        {
            PlayerTurn,
            EnemyTurn
        }
        private Game()
        {
            _location.StartBattle += InitiateBattle;
        }
        private static Game? _instance;
        public static Game? Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game();
                }

                return _instance;
            }
        }

        private readonly List<string> classes = [
            "1. Warrior",
            "2. Archer",
            "3. Wizzard"
            ];

        private Character? _character = null;
        private Location _location = new Forest();
        private Enemy? _currentEnemy = null;
        private string? playerInput;

        private GameState currentGameState = GameState.SelectCharacterClass;
        private GameState previousInventoryState = GameState.Battle;
        private BattleState currentBattleState = BattleState.PlayerTurn;
        private bool isGameRunning = true;

        private void SelectCharacterClass()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Select character class");

            foreach (string characterClass in classes)
            {
                Console.WriteLine(characterClass);
            }
            playerInput = Console.ReadLine()!;

            switch (playerInput)
            {
                case "1":
                    _character = new Warrior("Warrior", 120);
                    break;
                case "2":
                    _character = new Archer("Archer", 100);
                    break;
                case "3":
                    //character = new Wizzard("Wizzard", 60, 35);
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid choose");
                    Console.ResetColor();
                    break;
            }

            if (_character != null)
            {
                _character.OnCharacterDeath += GameOver;
                Console.Clear();
                currentGameState = GameState.Waiting; // Test
            }
            Console.ResetColor();
        }
        private void Battle()
        {
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            _character!.ShowBattleInfo();
            CharacterData.ShowGold();
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            _currentEnemy!.ShowBattleInfo();
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Actions:");
            Console.Write(
                $"1. Attack\n" +
                $"2. Open inventory\n" +
                $"3. Give burn effect to enemy\n" +
                $"4. Wait\n"
                );

            playerInput = Console.ReadLine()!;
            Console.Clear();

            switch (playerInput)
            {
                case "1":
                    _character.Attack(_currentEnemy);
                    currentBattleState = BattleState.EnemyTurn;
                    break;
                case "2":
                    currentGameState = GameState.Inventory;
                    break;
                case "3":
                    _currentEnemy?.AddEffect(new BurnEffect(5));
                    break;
                case "4":
                    currentBattleState = BattleState.EnemyTurn;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid action");
                    Console.ResetColor();
                    break;
            }

            if (_currentEnemy != null && currentBattleState == BattleState.EnemyTurn)
            {
                _currentEnemy.ApplyEffects();
                if (_currentEnemy.IsDead)
                {
                    _location.RemoveDefeatedEnemy(_currentEnemy);

                    _currentEnemy = null;
                    currentGameState = GameState.Map;
                }
                else
                {
                    _currentEnemy.Attack(_character!);

                    _character?.ApplyEffects();
                    currentBattleState = BattleState.PlayerTurn;
                }
            }
            Console.ResetColor();
        }
        private void InitiateBattle(Enemy enemy)
        {
            Console.Clear();
            _currentEnemy = enemy;
            currentGameState = GameState.Battle;
            currentBattleState = BattleState.PlayerTurn;

            _character?.ApplyEffects();
        }
        private void SelectTestField()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(
                $"Select test field:\n" +
                $"1. Battle\n" +
                $"2. Inventory test\n" +
                $"3. Map test\n"
                );

            playerInput = Console.ReadLine();

            switch (playerInput)
            {
                case "1":
                    currentGameState = GameState.Battle;
                    break;
                case "2":
                    previousInventoryState = currentGameState;
                    currentGameState = GameState.Inventory;
                    break;
                case "3":
                    currentGameState = GameState.Map;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknown test");
                    Console.ResetColor();
                    break;
            }
            Console.Clear();
            Console.ResetColor();
        }
        private void MapExplore()
        {
            Console.Clear();
            _location.ShowMap();
            Console.Write(
                "1. UP\n" +
                "2. DOWN\n" +
                "3. RIGHT\n" +
                "4. LEFT\n");

            string input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    _location.Move(Direction.UP);
                    break;
                case "2":
                    _location.Move(Direction.DOWN);
                    break;
                case "3":
                    _location.Move(Direction.RIGHT);
                    break;
                case "4":
                    _location.Move(Direction.LEFT);
                    break;
                default:
                    Console.WriteLine("Unknown action");
                    break;
            }
        }
        private void ItemActions()
        {
            var item = _character?.Inventory.SelectItem(playerInput!);
            item?.ShowActions();

            playerInput = Console.ReadLine();

            Console.Clear();
            switch (playerInput)
            {
                case "1":
                    if (item != null && item.Action(_character!))
                    {
                        currentGameState = previousInventoryState;
                    }
                    else
                    {
                        currentGameState = GameState.Inventory;
                    }
                    break;
                case "0":
                    currentGameState = GameState.Inventory;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknown command");
                    Console.ResetColor();
                    break;
            }
            Console.ResetColor();
        }
        private void Inventory()
        {
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            _character?.ShowEquipedItems();
            Console.WriteLine("------------------------------------------------------------");
            _character?.Inventory.ShowInventory();
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(
                $"8. Previous page\n" +
                $"9. Next page\n" +
                $"0. Close\n" +
                $"20. Add leather armor\n" +
                $"21. Add god armor\n" +
                $"22. Add new weapon\n" +
                $"23. Add healing potion\n" +
                $"25. Add new bow\n" +
                $"24. Add arrow(X10)\n" +
                $"26. Add god arrow(X10)\n" +
                $"27. Add fire amulet\n" +
                $"28. Add stick\n" +
                $"29. Add amulet of damage\n"
                );

            playerInput = Console.ReadLine();

            Console.Clear();
            switch (playerInput)
            {
                case "8":
                    _character?.Inventory.PreviousPage();
                    break;
                case "9":
                    _character?.Inventory.NextPage();
                    break;
                case "0":
                    currentGameState = previousInventoryState;
                    break;
                case "20":
                    _character?.Inventory.AddItem(new LeatherArmor());
                    break;
                case "21":
                    _character?.Inventory.AddItem(new GodArmor());
                    break;
                case "22":
                    _character?.Inventory.AddItem(new UltraHammer());
                    break;
                case "23":
                    _character?.Inventory.AddItem(new HealingPotion(1));
                    break;
                case "24":
                    _character?.Inventory.AddItem(new Arrow(10));
                    break;
                case "25":
                    _character?.Inventory.AddItem(new GodBow());
                    break;
                case "26":
                    _character?.Inventory.AddItem(new GodArrow(10));
                    break;
                case "27":
                    _character?.Inventory.AddItem(new FireAmulet());
                    break;
                case "28":
                    _character?.Inventory.AddItem(new Stick());
                    break;
                case "29":
                    _character?.Inventory.AddItem(new DamageAmulet());
                    break;
                default:
                    var item = _character?.Inventory.SelectItem(playerInput!);
                    if (item != null)
                    {
                        currentGameState = GameState.ItemAction;
                    }
                    break;
            }
            Console.ResetColor();
        }

        public void Start()
        {
            while (isGameRunning)
            {
                switch (currentGameState)
                {
                    case GameState.SelectCharacterClass:
                        SelectCharacterClass();
                        break;
                    case GameState.Battle:
                        Battle();
                        break;
                    case GameState.Waiting:
                        SelectTestField();
                        break;
                    case GameState.Inventory:
                        Inventory();
                        break;
                    case GameState.ItemAction:
                        ItemActions();
                        break;
                    case GameState.Map:
                        MapExplore();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Unknown game state");
                        Console.ResetColor();
                        Environment.Exit(1);
                        break;
                }
            }
        }

        private void GameOver()
        {
            if (_character != null)
            {
                _character.OnCharacterDeath -= GameOver;
                _character = null;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Game Over!");
            Console.ResetColor();
            isGameRunning = false;
        }
    }
}
