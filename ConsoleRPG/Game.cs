using ConsoleRPG.Entities.Characters;
using ConsoleRPG.Entities.Enemies;
using ConsoleRPG.Items.Ammos;
using ConsoleRPG.Items.Amulets;
using ConsoleRPG.Items.Armor;
using ConsoleRPG.Items.Consumables;
using ConsoleRPG.Items.Weapons.MeleeWeapons;
using ConsoleRPG.Items.Weapons.RangedWeapons;
using ConsoleRPG.Items;
using ConsoleRPG.Effects;
using ConsoleRPG.Locations.Forest;
using ConsoleRPG.Locations;
using static ConsoleRPG.Locations.Location;

namespace ConsoleRPG;

internal sealed class Game
{
    enum GameState
    {
        SelectCharacterClass,
        SelectTestField,
        Battle,
        Inventory,
        ItemAction,
        Map,
        Shop,
        ShowStats
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

    private GameSaveData _saveData;
    private Character? _character = null;
    private Location? _location = null;
    private Enemy? _currentEnemy = null;
    private Shop? _currentShop = null;
    private string? playerInput;

    private GameState currentGameState = GameState.SelectCharacterClass;
    private GameState previousGameState = GameState.SelectCharacterClass;
    private BattleState currentBattleState = BattleState.PlayerTurn;
    private bool isGameRunning = true;

    private void SelectCharacterClass()
    {
        if (CharacterData.CurrentCharacterClass != CharacterData.CharacterClass.None) return;

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
                _character = new Warrior();
                CharacterData.setCharacterClass(CharacterData.CharacterClass.Warrior);
                break;
            case "2":
                _character = new Archer();
                CharacterData.setCharacterClass(CharacterData.CharacterClass.Archer);
                break;
            case "3":
                //character = new Wizzard("Wizzard", 60, 35);
                //CharacterData.CurrentCharacterClass = CharacterData.CharacterClass.Wizzard;
                break;
            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid choose");
                Console.ResetColor();
                break;
        }

        if (CharacterData.CurrentCharacterClass == CharacterData.CharacterClass.None)
        {
            Console.WriteLine("Class don't choose");
        }
        else if (_character != null)
        {
            _character.OnCharacterDeath += GameOver;
            Console.Clear();
            previousGameState = GameState.SelectCharacterClass;
            currentGameState = GameState.SelectTestField; // Test
        }
        Console.ResetColor();
    }
    private void SetupLocation(Location location)
    {
        if (_location != null)
        {
            _location.StartBattle -= InitiateBattle;
            _location.OpenShop -= InitiateShop;
        }

        _location = location;

        _location.StartBattle += InitiateBattle;
        _location.OpenShop += InitiateShop;

        _currentShop = _location.Shop;
    }
    private void InitiateShop()
    {
        currentGameState = GameState.Shop;
    }
    private void Battle()
    {
        if (_currentEnemy == null)
        {
            currentGameState = previousGameState;
            return;
        }

        previousGameState = GameState.Battle;

        Console.ResetColor();
        Console.WriteLine("------------------------------------------------------------");
        _character!.ShowBattleInfo();
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
                _location?.RemoveDefeatedEnemy(_currentEnemy);

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

        previousGameState = GameState.SelectTestField;
        switch (playerInput)
        {
            case "1":
                currentGameState = GameState.Battle;
                break;
            case "2":
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
        if (_location == null)
        {
            SetupLocation(new Forest());
        }

        _location?.ShowMap();
        Console.Write(
            "1. UP\n" +
            "2. DOWN\n" +
            "3. RIGHT\n" +
            "4. LEFT\n" +
            "5. Inventory\n" +
            "6. Stats\n" +
            "9. Load game\n" +
            "0. Save game\n");

        string input = Console.ReadLine()!;

        previousGameState = GameState.Map;
        Console.Clear();

        switch (input)
        {
            case "1":
                _location?.Move(Direction.UP);
                break;
            case "2":
                _location?.Move(Direction.DOWN);
                break;
            case "3":
                _location?.Move(Direction.RIGHT);
                break;
            case "4":
                _location?.Move(Direction.LEFT);
                break;
            case "5":
                currentGameState = GameState.Inventory;
                break;
            case "6":
                currentGameState = GameState.ShowStats;
                break;
            case "9":
                LoadSave();
                break;
            case "0":
                SaveGame();
                break;
            default:
                Console.WriteLine("Unknown action");
                break;
        }
    }
    private void SaveGame()
    {
        SaveManager.SaveData(_saveData);
    }
    private void LoadSave()
    {
        _saveData = SaveManager.LoadData();

        _character = _saveData.Character;
        CharacterData.setCharacterClass(_saveData.CharacterClass);
        CharacterData.setGold(_saveData.Gold);
        _location = _saveData.Location;
    }
    private void Shop()
    {
        if (_currentShop == null) return;

        _currentShop.ShowShop();

        string input = Console.ReadLine()!;
        Console.Clear();

        switch (input)
        {
            case "7":
                _currentShop.PreviousPage();
                break;
            case "8":
                _currentShop.NextPage();
                break;
            case "9":
                previousGameState = currentGameState;
                currentGameState = GameState.Inventory;
                break;
            case "0":
                currentGameState = GameState.Map;
                break;
            default:
                if (_currentShop.TryBuyItem(input, out Item? item) && item != null)
                {
                    _character?.Inventory.AddItem(item);
                }
                break;
        }

        Console.ResetColor();
    }
    private void ShowStats()
    {
        _character?.ShowBattleInfo();

        Console.ResetColor();
        Console.WriteLine("-------------------------------");
        Console.WriteLine("1. Back");

        string input = Console.ReadLine()!;

        if (input == "1")
        {
            currentGameState = previousGameState;
        }
        Console.Clear();
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
                    currentGameState = previousGameState;
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
        CharacterData.ShowGold();
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
                currentGameState = previousGameState;
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
                case GameState.SelectTestField:
                    SelectTestField();
                    break;
                case GameState.Battle:
                    Battle();
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
                case GameState.ShowStats:
                    ShowStats();
                    break;
                case GameState.Shop:
                    Shop();
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
