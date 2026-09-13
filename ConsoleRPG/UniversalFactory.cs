using ConsoleRPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal static class UniversalFactory<T> where T : class
    {
        private static readonly Dictionary<string, Type> _dict = [];
        public static void Initialize()
        {
            var itemTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);

            foreach (var itemType in itemTypes)
            {
                _dict.Add(itemType.Name, itemType);
            }
        }
        public static T? CreateObject(string objectID)
        {
            if (objectID != null && _dict.TryGetValue(objectID, out Type? objectType) && objectType != null)
            {
                var newObject = (T?)Activator.CreateInstance(objectType);

                return newObject;
            }

            return null;
        }
    }
}
