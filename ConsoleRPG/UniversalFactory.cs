using ConsoleRPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    /// <summary>
    /// A generic factory class to dynamically create object instances from save data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class UniversalFactory<T> where T : class
    {
        private static readonly Dictionary<string, Type> _dict = [];
        /// <summary>
        /// Dynamically finds and registers all non-abstract subclasses of T using Reflection, 
        /// eliminating the need for hardcoded switch-case statements.
        /// </summary>
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
