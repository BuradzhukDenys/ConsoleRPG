namespace ConsoleRPG
{
    internal class Game
    {
        private readonly List<string> classes = [
            "1. Warrior",
            "2. Archer",
            "3. Wizzard"
            ];

        private Character? character = null;
        private Enemy? enemy = null;
        private string? playerInput;

        public void SelectCharacterClass()
        {
            Console.WriteLine("Select character class");

            while (true)
            {
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
                    break;
                }
            }

            Console.Clear();
            Battle();
        }
        public void Battle()
        {
            while (true)
            {
                if (enemy == null)
                {
                    enemy = new Slime("Slime", 60, 10, 10);
                }

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
                        break;
                    case "2":
                        if (!character.Heal())
                        {
                            continue;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid action");
                        break;
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
        }

        public void Start()
        {
            SelectCharacterClass();
        }

        public void GameOver()
        {
            Console.WriteLine("Game Over!");
            Environment.Exit(0);
        }
    }
}
