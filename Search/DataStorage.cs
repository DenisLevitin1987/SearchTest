using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Search.Models;

namespace Search
{
    public static class DataStorage
    {
        private static object obj = new object(); 

        private static object objCacheLock = new object();

        private static IReadOnlyCollection<Laptop> Laptops;

        private static Dictionary<string, Dictionary<object, Laptop>> Cache = new Dictionary<string, Dictionary<object, Laptop>>();

        private static Func<Laptop, object> GetKeySelector(string fieldName)
        {
            /// TODO: Realize properly

            /// Parse fieldName, use attributes
            return (Laptop laptop) => { return laptop.Producer; };
        }

        public static void SetLaptops(IReadOnlyCollection<Laptop> laptops)
        {
            if (Laptops == null)
            {
                lock (obj)
                {
                    if (Laptops == null)
                    {
                        Laptops = laptops;
                    }
                }
            }
        }

        public static IReadOnlyCollection<Laptop> GetLaptops()
        {
            return Laptops;
        }

        public static IReadOnlyDictionary<object, Laptop> GetDictionaryByFieldName(string fieldName)
        {
            if (Cache.TryGetValue(fieldName, out var cache))
            {
                return cache;
            }
            else
            {
                var keySelector = GetKeySelector(fieldName);
                var dict = Laptops.ToDictionary(keySelector, x => x);

                lock (objCacheLock)
                {
                    if (!Cache.TryGetValue(fieldName, out cache))
                    {
                        Cache.Add(fieldName, dict);
                    }
                }

                return cache;
            }
        }
    }
}
