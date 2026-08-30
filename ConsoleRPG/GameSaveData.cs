using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRPG.Entities.Characters;
using ConsoleRPG.Locations;

namespace ConsoleRPG
{
    internal class GameSaveData(Character character, CharacterData.CharacterClass characterClass, Location location, int gold)
    {
        public Character Character { get; private set; } = character;
        public CharacterData.CharacterClass CharacterClass { get; private set; } = characterClass;
        public Location Location { get; private set; } = location;
        public int Gold { get; private set; } = gold;
    }
}
