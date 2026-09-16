using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

//Ready
namespace ConsoleRPG
{
    internal static class SaveManager
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };
        public const string SaveDataPath = "D:\\Codes\\C# training\\ConsoleRPG\\ConsoleRPG\\Save\\Save.json";
        public static void SaveData(GameSaveData saveData)
        {
            var JsonSaveData = JsonSerializer.Serialize<GameSaveData>(saveData, _options);
            File.WriteAllText(SaveDataPath, JsonSaveData);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Game saved");
            Console.ResetColor();
        }
        public static GameSaveData? LoadData()
        {
            if (!File.Exists(SaveDataPath))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("No save file found!");
                Console.ResetColor();
                return null;
            }

            var jsonSaveData = File.ReadAllText(SaveDataPath);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Game loaded");
            Console.ResetColor();

            return JsonSerializer.Deserialize<GameSaveData>(jsonSaveData);
        }
    }
}
