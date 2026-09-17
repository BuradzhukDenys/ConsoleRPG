namespace ConsoleRPG;

internal static class CharacterData
{
    public enum CharacterClass
    {
        Warrior,
        Archer,
        None = -1
    }
    public static CharacterClass CurrentCharacterClass { get; private set; } = CharacterClass.None;
    public static void SetCharacterClass(CharacterClass characterClass)
    {
        CurrentCharacterClass = characterClass;
    }
    private static int _gold = 99999;
    public static int Gold
    {
        get { return _gold; }
        private set
        {
            _gold = Math.Clamp(value, 0, 99999);
        }
    }
    public static void SetGold(int amount)
    {
        Gold = amount;
    }
    public static void AddGold(int amount)
    {
        Gold += Math.Abs(amount);
    }
    public static bool SpendGold(int amount)
    {
        int cost = Math.Abs(amount);
        
        if (Gold >= cost)
        {
            Gold -= cost;
            return true;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Not enough gold!");
            Console.ResetColor();
            return false;
        }
    }
    public static void ShowGold()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Gold: {Gold}");
        Console.ResetColor();
    }
}
