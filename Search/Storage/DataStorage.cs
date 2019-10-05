using System;
using System.Collections.Generic;
using System.Linq;
using Search.Models;

namespace Search
{
    public static class DataStorage
    {
        private static object obj = new object();

        private static IReadOnlyDictionary<int, Laptop> Laptops;

        private static Dictionary<string, HashSet<int>> Cache = new Dictionary<string, HashSet<int>>();

        private static void SetCache(IReadOnlyCollection<Laptop> laptops)
        {
            foreach (var laptop in laptops)
            {
                foreach (var attribute in laptop.Attributes) 
                {
                    if (Cache.TryGetValue(attribute, out var ids))
                    {
                        if (!ids.Contains(laptop.Id))
                        {
                            ids.Add(laptop.Id);
                        }
                    }
                    else
                    {
                        Cache.Add(attribute, new HashSet<int>() { laptop.Id });
                    }
                }
            }
        }

        public static void SetLaptops(IReadOnlyCollection<Laptop> laptops)
        {
            if (Laptops == null)
            {
                lock (obj)
                {
                    if (Laptops == null)
                    {
                        Laptops = laptops.ToDictionary(x => x.Id, x => x);
                        SetCache(laptops);
                    }
                }
            }
        }

        public static IReadOnlyDictionary<int, Laptop> GetLaptops()
        {
            return Laptops;
        }

        public static IReadOnlyDictionary<int, Laptop> GetEquals(IReadOnlyDictionary<int, Laptop> laptops, string equal)
        {
            if (Cache.TryGetValue(equal, out var ids))
            {
                var res = new Dictionary<int, Laptop>();
                foreach (var id in ids)
                {
                    if (laptops.TryGetValue(id, out var laptop))
                    {
                        res.Add(id, laptop);
                    }
                }

                return res;
            }
            else
            {
                return new Dictionary<int, Laptop>();
            }
        }

        public static IReadOnlyDictionary<int, Laptop> GetNotEquals(IReadOnlyDictionary<int, Laptop> laptops, string equal)
        {
            var res = new Dictionary<int, Laptop>();
            if (Cache.TryGetValue(equal, out var ids))
            {
                foreach (var laptop in laptops)
                {
                    if (!ids.Contains(laptop.Key))
                    {
                        res.Add(laptop.Key, laptop.Value);
                    }
                }

                return res;
            }
            else
            {
                return laptops;
            }
        }
    }
}
