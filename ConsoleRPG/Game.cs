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
using ConsoleRPG.Items.Weapons;
using ConsoleRPG.Locations.Cave;
using ConsoleRPG.Locations.Hell;

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
        ShowStats,
        StartGame
    }
    enum BattleState
    {
        PlayerTurn,
        EnemyTurn
    }
    private Game()
    {
        UniversalFactory<Enemy>.Initialize();
        UniversalFactory<Item>.Initialize();
        UniversalFactory<Location>.Initialize();
        UniversalFactory<Character>.Initialize();

        InitLocationsOrder();
    }
    private static Game? _instance;
    private static bool SaveExist => File.Exists(SaveManager.SaveDataPath);
    public static Game? Instance
    {
        get
        {
            _instance ??= new Game();

            return _instance;
        }
    }

    private Queue<string> _locationsOrder = new();

    private GameSaveData? _saveData = null;
    private Character? _character = null;
    private Location? _location = null;
    private Enemy? _currentEnemy = null;
    private Shop? _currentShop = null;
    private bool _isLocationCompleted = false;
    private string? playerInput;

    private GameState currentGameState = GameState.StartGame;
    private GameState previousGameState = GameState.StartGame;
    private BattleState currentBattleState = BattleState.PlayerTurn;
    private bool isGameRunning = true;

    private void InitLocationsOrder()
    {
        _locationsOrder.Clear();

        _locationsOrder.Enqueue(typeof(Forest).Name);
        _locationsOrder.Enqueue(typeof(Cave).Name);
        _locationsOrder.Enqueue(typeof(Hell).Name);
    }
    private void ResetGame()
    {
        _character = null;
        _location = null;
        _currentShop = null;
        _currentEnemy = null;
        _isLocationCompleted = false;
        CharacterData.SetCharacterClass(CharacterData.CharacterClass.None);

        InitLocationsOrder();
    }
    private void SelectCharacterClass()
    {
        if (CharacterData.CurrentCharacterClass != CharacterData.CharacterClass.None) return;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Select character class");

        var availableClasses = Enum.GetValues<CharacterData.CharacterClass>()
            .Where(c => c != CharacterData.CharacterClass.None)
            .ToList();

        for (int i = 0; i < availableClasses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableClasses[i]}");
        }
        playerInput = Console.ReadLine()!;
        Console.Clear();

        if (int.TryParse(playerInput, out int choice) && choice > 0 && choice <= availableClasses.Count)
        {
            var selectedClass = availableClasses[choice - 1];

            _character = UniversalFactory<Character>.CreateObject(selectedClass.ToString());

            if (_character != null)
            {
                CharacterData.SetCharacterClass(selectedClass);
                _character.OnCharacterDeath += GameOver;

                previousGameState = GameState.SelectCharacterClass;
                currentGameState = GameState.Map;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Invalid choose");
            Console.ResetColor();
        }
    }
    private void SetupLocation(Location location)
    {
        if (_location != null)
        {
            _location.StartBattle -= InitiateBattle;
            _location.OpenShop -= InitiateShop;
            _location.LocationCompleted -= LocationCompleted;
        }

        _location = location;

        _location.StartBattle += InitiateBattle;
        _location.OpenShop += InitiateShop;
        _location.LocationCompleted += LocationCompleted;

        _currentShop = _location.Shop;
    }
    private void InitiateShop()
    {
        currentGameState = GameState.Shop;
    }
    private void Battle()
    {
        if (_currentEnemy == null || _character == null)
        {
            currentGameState = previousGameState;
            return;
        }

        previousGameState = GameState.Battle;

        Console.ResetColor();
        Console.WriteLine("------------------------------------------------------------");
        _character.ShowBattleInfo();
        Console.ResetColor();
        Console.WriteLine("------------------------------------------------------------");
        _currentEnemy.ShowBattleInfo();
        Console.ResetColor();
        Console.WriteLine("------------------------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Actions:");
        Console.Write(
            $"1. Attack\n" +
            $"2. Open inventory\n" +
            $"3. Wait\n"
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
                _currentEnemy.Attack(_character);

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
    private void TrySave()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Are you want to save game?");

        Console.Write(
            "1. Yes\n" +
            "2. No\n");

        playerInput = Console.ReadLine()!;

        switch (playerInput)
        {
            case "1":
                SaveGame();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unknown action");
                Console.ResetColor();
                break;
        }

        Console.ResetColor();
        Console.Clear();
    }
    private void LocationCompleted()
    {
        _isLocationCompleted = true;
    }
    private void NextLocation()
    {
        if (_locationsOrder.TryDequeue(out string? result) && result != null)
        {
            var newLocation = UniversalFactory<Location>.CreateObject(result);
            if (newLocation != null)
            {
                SetupLocation(newLocation);

                _isLocationCompleted = false;
            }
        }
    }
    private void MapExplore()
    {
        if (_isLocationCompleted && _locationsOrder.Count == 0)
        {
            Victory();
            return;
        }

        if (_location == null)
        {
            NextLocation();
        }

        _location?.ShowMap();
        Console.Write(
            "1. UP\n" +
            "2. DOWN\n" +
            "3. RIGHT\n" +
            "4. LEFT\n" +
            "5. Inventory\n" +
            "6. Stats\n" +
            "7. Save game\n" +
            "8. Load game\n" +
            "9. Main menu\n" +
            "0. Exit\n");

        if (_isLocationCompleted)
        {
            Console.WriteLine("11. Next location");
        }

        playerInput = Console.ReadLine()!;

        previousGameState = GameState.Map;
        Console.Clear();

        switch (playerInput)
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
            case "7":
                SaveGame();
                break;
            case "8":
                LoadSave();
                break;
            case "9":
                TrySave();
                currentGameState = GameState.StartGame;
                break;
            case "0":
                TrySave();
                isGameRunning = false;
                break;
            case "11":
                if (_isLocationCompleted)
                {
                    NextLocation();
                }
                break;
            default:
                Console.WriteLine("Unknown action");
                break;
        }
    }
    private void SaveGame()
    {
        if (_character == null || _location == null) return;

        List<ItemSaveData> savedInventory = [];
        List<OfferSaveData> offerSaveData = [];

        foreach (var item in _character.Inventory.Items)
        {
            savedInventory.Add(new ItemSaveData { Count = item.Count, Type = item.GetType().Name });
        }

        foreach (var offer in _currentShop?.Items ?? [])
        {
            ItemSaveData OfferItem = new() { Count = offer.Item.Count, Type = offer.Item.GetType().Name };

            offerSaveData.Add(new OfferSaveData() { AvailableCount = offer.Count, Cost = offer.Cost, Item = OfferItem });
        }

        var locationData = new LocationSaveData()
        {
            LocationType = _location.GetType().Name,
            Area = _location.Area,
            Enemies = _location.GetEnemiesForSave(),
            ShopOffers = offerSaveData,
            IsLocationCompleted = _isLocationCompleted
        };

        _saveData = new GameSaveData()
        {
            CurrentHealth = _character.Health,
            MaxHealth = _character.MaxHealth,
            Damage = _character.Damage,
            DamageMultiplayer = _character.DamageMultiplier,
            DamageReduction = _character.DamageReduction,
            CurrentWeaponType = _character.CurrentWeapon.GetType().Name,
            CurrentArmorType = _character?.CurrentArmor?.GetType().Name ?? "",
            CurrentAmuletType = _character?.CurrentAmulet?.GetType().Name ?? "",
            CurrentAmmoType = (_character as Archer)?.CurrentAmmo?.GetType().Name ?? "",
            CharacterClass = CharacterData.CurrentCharacterClass,
            Gold = CharacterData.Gold,

            Inventory = savedInventory,
            Location = locationData,
            LocationsOrder = _locationsOrder
        };

        SaveManager.SaveData(_saveData);
    }
    private void LoadSave()
    {
        if (!SaveExist) return;

        _saveData = SaveManager.LoadData();

        if (_saveData == null || _saveData.Location == null) return;

        CharacterData.SetCharacterClass(_saveData.CharacterClass);
        CharacterData.SetGold(_saveData.Gold);
        _isLocationCompleted = _saveData.Location.IsLocationCompleted;

        _character = RestoreLoadedData.LoadCharacter(_saveData);
        _locationsOrder = RestoreLoadedData.LoadLocationsOrder(_saveData);

        _location = RestoreLoadedData.LoadLocation(_saveData);
        if (_location != null)
        {
            SetupLocation(_location);
        }
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
        Console.WriteLine("0. Back");

        string input = Console.ReadLine()!;

        if (input == "0")
        {
            currentGameState = previousGameState;
        }
        Console.Clear();
    }
    private void ItemActions()
    {
        var item = _character?.Inventory.SelectItem(playerInput!);
        item?.ShowActions();

        playerInput = Console.ReadLine()!;

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
            $"0. Close\n"
            );

        playerInput = Console.ReadLine()!;

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
            default:
                var item = _character?.Inventory.SelectItem(playerInput);
                if (item != null)
                {
                    currentGameState = GameState.ItemAction;
                }
                break;
        }
        Console.ResetColor();
    }
    private void StartGame()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("1. New Game");

        if (SaveExist)
        {
            Console.WriteLine("2. Load game");
        }

        playerInput = Console.ReadLine()!;

        Console.Clear();
        switch (playerInput)
        {
            case "1":
                ResetGame();
                currentGameState = GameState.SelectCharacterClass;
                break;
            case "2":
                if (!SaveExist)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Unknown action!");
                    Console.ResetColor();
                    break;
                }

                LoadSave();
                currentGameState = GameState.Map;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Unknown action!");
                Console.ResetColor();
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
                case GameState.StartGame:
                    StartGame();
                    break;
                case GameState.SelectCharacterClass:
                    SelectCharacterClass();
                    break;
                //case GameState.SelectTestField:
                //    SelectTestField();
                //    break;
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
    private void Victory()
    {
        Console.Clear();
        Console.WriteLine("Victory!");
        TrySave();

        Console.ReadLine();

        currentGameState = GameState.StartGame;
        previousGameState = GameState.StartGame;
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

        ResetGame();

        Console.ReadLine();

        currentGameState = GameState.StartGame;
        previousGameState = GameState.StartGame;
    }
}
