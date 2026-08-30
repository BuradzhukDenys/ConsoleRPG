namespace ConsoleRPG;

internal static class CharacterData
{
    private static int _gold = 99999;
    public enum CharacterClass
    {
        Warrior,
        Archer,
        Wizzard,
        None = -1
    }
    public static CharacterClass CurrentCharacterClass { get; private set; } = CharacterClass.None;
    public static void setCharacterClass(CharacterClass characterClass)
    {
        CurrentCharacterClass = characterClass;
    }
    public static void setGold(int amount)
    {
        Gold = amount;
    }
    public static int Gold
    {
        get { return _gold; }
        private set
        {
            _gold = Math.Clamp(value, 0, 99999);
        }
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
