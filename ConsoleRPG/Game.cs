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

    private readonly List<string> classes = [
        "1. Warrior",
        "2. Archer",
        "3. Wizzard"
        ];

    private readonly Queue<string> _locationsOrder = new();

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
        CharacterData.SetCharacterClass(CharacterData.CharacterClass.None);

        InitLocationsOrder();
    }
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
                CharacterData.SetCharacterClass(CharacterData.CharacterClass.Warrior);
                break;
            case "2":
                _character = new Archer();
                CharacterData.SetCharacterClass(CharacterData.CharacterClass.Archer);
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
            currentGameState = GameState.Map;
        }
        Console.ResetColor();
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
    //private void SelectTestField()
    //{
    //    Console.ForegroundColor = ConsoleColor.Cyan;
    //    Console.Write(
    //        $"Select test field:\n" +
    //        $"1. Battle\n" +
    //        $"2. Inventory test\n" +
    //        $"3. Map test\n"
    //        );

    //    playerInput = Console.ReadLine();

    //    previousGameState = GameState.SelectTestField;
    //    switch (playerInput)
    //    {
    //        case "1":
    //            currentGameState = GameState.Battle;
    //            break;
    //        case "2":
    //            currentGameState = GameState.Inventory;
    //            break;
    //        case "3":
    //            currentGameState = GameState.Map;
    //            break;
    //        default:
    //            Console.ForegroundColor = ConsoleColor.DarkRed;
    //            Console.WriteLine("Unknown test");
    //            Console.ResetColor();
    //            break;
    //    }
    //    Console.Clear();
    //    Console.ResetColor();
    //}
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
        if (_location == null)
        {
            var newLocation = UniversalFactory<Location>.CreateObject(_locationsOrder.Dequeue());
            if (newLocation != null)
            {
                SetupLocation(newLocation);
            }
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
        _saveData = SaveManager.LoadData();

        if (_saveData == null || _saveData.Location == null) return;

        CharacterData.SetCharacterClass(_saveData.CharacterClass);
        CharacterData.SetGold(_saveData.Gold);
        _isLocationCompleted = _saveData.Location.IsLocationCompleted;

        LoadCharacter();
        LoadLocation();
        LoadLocationsOrder();
    }
    private void LoadCharacter()
    {
        if (_saveData == null) return;

        _character = UniversalFactory<Character>.CreateObject(_saveData.CharacterClass.ToString());

        if (_character == null) return;

        _character.Health = _saveData.CurrentHealth;
        _character.MaxHealth = _saveData.MaxHealth;

        if (UniversalFactory<Item>.CreateObject(_saveData.CurrentWeaponType) is Weapon w)
        {
            _character.CurrentWeapon = w;
        }
        var startArmor = (Armor?)UniversalFactory<Item>.CreateObject(_saveData.CurrentArmorType);
        var startAmulet = (Amulet?)UniversalFactory<Item>.CreateObject(_saveData.CurrentAmuletType);

        _character.CurrentArmor = startArmor;
        _character.CurrentAmulet = startAmulet;

        if (_character is Archer archer)
        {
            var startAmmo = (Ammo?)UniversalFactory<Item>.CreateObject(_saveData.CurrentAmmoType);
            var savedAmmo = _saveData.Inventory.FirstOrDefault(it => it.Type == startAmmo?.GetType().Name);

            if (startAmmo != null && savedAmmo != null)
            {
                startAmmo.Count = savedAmmo.Count;
                archer.CurrentAmmo = startAmmo;
            }
        }

        _character.Inventory.Items.Clear();

        foreach (var itemSaveData in _saveData.Inventory)
        {
            Item? newItem = UniversalFactory<Item>.CreateObject(itemSaveData.Type);

            if (newItem != null)
            {
                if (newItem.CanStack)
                {
                    newItem.Count = itemSaveData.Count;
                }
                _character.Inventory.Items.Add(newItem);
            }
        }
    }
    private void LoadLocationsOrder()
    {
        _locationsOrder.Clear();

        foreach (var locationName in _saveData?.LocationsOrder ?? [])
        {
            _locationsOrder.Enqueue(locationName);
        }
    }
    private void LoadLocation()
    {
        if (_saveData == null) return;

        var newLocation = UniversalFactory<Location>.CreateObject(_saveData?.Location?.LocationType!)!;

        if (newLocation == null) return;

        newLocation.Area = _saveData?.Location?.Area ?? [];
        newLocation.EnemiesInfo.Clear();

        foreach (var kv in _saveData?.Location?.Enemies ?? [])
        {
            int enemyX = kv.Key % newLocation.MapWidth;
            int enemyY = kv.Key / newLocation.MapWidth;

            var enemyPos = new Vector2(enemyX, enemyY);

            Enemy? newEnemy = UniversalFactory<Enemy>.CreateObject(kv.Value);

            if (newEnemy != null)
            {
                newLocation.EnemiesInfo.Add(enemyPos, newEnemy);
            }
        }

        newLocation.Shop.Items.Clear();
        foreach (var offerData in _saveData?.Location?.ShopOffers ?? [])
        {
            if (offerData.Item == null) continue;

            var restoredItem = UniversalFactory<Item>.CreateObject(offerData.Item?.Type!);

            if (restoredItem != null)
            {
                restoredItem.Count = offerData.Item!.Count;

                Offer newOffer = new()
                {
                    Item = restoredItem,
                    Cost = offerData.Cost,
                    Count = offerData.AvailableCount
                };

                newLocation.Shop.Items.Add(newOffer);
            }
        }

        SetupLocation(newLocation);
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
