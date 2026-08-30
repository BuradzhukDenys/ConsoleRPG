using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class SaveManager
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };
        private const string _saveDataPath = "D:\\Codes\\C# training\\ConsoleRPG\\ConsoleRPG\\Save\\Save.json";
        public static void SaveData(GameSaveData saveData)
        {
            var JsonSaveData = JsonSerializer.Serialize<GameSaveData>(saveData, _options);
            File.WriteAllText(_saveDataPath, JsonSaveData);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Game saved");
            Console.ResetColor();
        }
        public static GameSaveData LoadData()
        {
            var jsonSaveData = File.ReadAllText(_saveDataPath);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Game loaded");
            Console.ResetColor();

            return JsonSerializer.Deserialize<GameSaveData>(jsonSaveData)!;
        }
    }
}
