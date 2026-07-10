namespace ConsoleRPG
{
    internal sealed class Game
    {
        enum GameState
        {
            SelectCharacterClass,
            Battle
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
        private bool isGameRunning = true;

        private void SelectCharacterClass()
        {
            Console.WriteLine("Select character class");

            foreach (string characterClass in classes)
            {
                Console.WriteLine(characterClass);
            }
            playerInput = Console.ReadLine()!;

            switch (playerInput)
            {
                case "1":
                    character = new Warrior("Warrior", 120, 15);
                    break;
                case "2":
                    //character = new Archer("Archer", 90, 25);
                    break;
                case "3":
                    //character = new Wizzard("Wizzard", 60, 35);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid choose");
                    break;
            }

            if (character != null)
            {
                character.OnCharacterDeath += GameOver;
                Console.Clear();
                currentGameState = GameState.Battle;
            }
        }
        private void Battle()
        {
            if (enemy == null)
            {
                enemy = new Slime("Slime", 60, 10, 10);
            }

            bool isPlayerInputValid = false;
            while (!isPlayerInputValid)
            {
                character!.ShowBattleInfo();
                CharacterData.ShowGold();
                Console.WriteLine("------------------------------------------------------------");
                enemy!.ShowBattleInfo();
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("Actions:");
                Console.Write(
                    $"1. Attack\n" +
                    $"2. Heal\n"
                    );

                playerInput = Console.ReadLine()!;
                Console.Clear();

                switch (playerInput)
                {
                    case "1":
                        character.Attack(enemy);
                        isPlayerInputValid = true;
                        break;
                    case "2":
                        if (!character.Heal())
                        {
                            continue;
                        }
                        isPlayerInputValid = true;
                        break;
                    default:
                        Console.WriteLine("Invalid action");
                        continue;
                }
            }

            if (enemy != null)
            {
                if (enemy.IsDead)
                {
                    enemy = null;
                }
                else
                {
                    enemy.Attack(character);
                }
            }
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
                    default:
                        Console.WriteLine("Unknown game state");
                        Environment.Exit(1);
                        break;
                }
            }
        }

        private void GameOver()
        {
            Console.WriteLine("Game Over!");
            isGameRunning = false;
        }
    }
}
