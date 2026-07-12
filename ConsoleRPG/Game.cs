using static System.Net.Mime.MediaTypeNames;

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
            Waiting
        }
        enum BattleState
        {
            PlayerTurn,
            EnemyTurn
        }
        private Game() { }
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

        private Character? character = null;
        private Enemy? enemy = null;
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
                    character = new Warrior("Warrior", 120);
                    break;
                case "2":
                    //character = new Archer("Archer", 90, 25);
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

            if (character != null)
            {
                character.OnCharacterDeath += GameOver;
                Console.Clear();
                currentGameState = GameState.Waiting; // Test
                //currentGameState = GameState.Battle;
            }
            Console.ResetColor();
        }
        private void Battle()
        {
            currentBattleState = BattleState.PlayerTurn;

            if (enemy == null)
            {
                enemy = new Slime("Slime", 60, 10, 10);
            }

            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            character!.ShowBattleInfo();
            CharacterData.ShowGold();
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            enemy!.ShowBattleInfo();
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Actions:");
            Console.Write(
                $"1. Attack\n" +
                $"2. Open inventory\n"
                );

            playerInput = Console.ReadLine()!;
            Console.Clear();

            switch (playerInput)
            {
                case "1":
                    character.Attack(enemy);
                    currentBattleState = BattleState.EnemyTurn;
                    break;
                case "2":
                    currentGameState = GameState.Inventory;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid action");
                    Console.ResetColor();
                    break;
            }

            if (enemy != null && currentBattleState == BattleState.EnemyTurn)
            {
                if (enemy.IsDead)
                {
                    enemy = null;
                }
                else
                {
                    enemy.Attack(character!);
                }
            }
            Console.ResetColor();
        }

        private void SelectTestField()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(
                $"Select test field:\n" +
                $"1. Battle\n" +
                $"2. Inventory test\n"
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
                default:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknown test");
                    Console.ResetColor();
                    break;
            }
            Console.Clear();
            Console.ResetColor();
        }

        private void ItemActions()
        {
            var item = character?.Inventory.SelectItem(playerInput!);
            if (item != null)
            {
                item.ShowActions();
            }

            playerInput = Console.ReadLine();

            Console.Clear();
            switch (playerInput)
            {
                case "1":
                    if (item != null && item.Action(character!))
                    {
                        currentGameState = previousInventoryState;
                        currentBattleState = BattleState.EnemyTurn;
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
            character?.ShowEquipedItems();
            Console.WriteLine("------------------------------------------------------------");
            character?.Inventory.ShowInventory();
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(
                $"0. Close\n" +
                $"9. Add new weapon\n" +
                $"10. Add healing potion\n"
                );

            playerInput = Console.ReadLine();

            Console.Clear();
            switch (playerInput)
            {
                case "0":
                    currentGameState = previousInventoryState;
                    break;
                case "9":
                    character?.Inventory.AddItem(new UltraHammer());
                    break;
                case "10":
                    character?.Inventory.AddItem(new HealingPotion(1));
                    break;
                default:
                    var item = character?.Inventory.SelectItem(playerInput!);
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Game Over!");
            Console.ResetColor();
            isGameRunning = false;
        }
    }
}
