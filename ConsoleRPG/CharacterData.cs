namespace ConsoleRPG
{
    internal static class CharacterData
    {
        private static int _gold;
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
}
