using System;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Models;

namespace SearchEngine.Storage
{
    public class LaptopDataProvider : ILaptopDataProvider
    {
        private static object obj = new object();

        private static IReadOnlyDictionary<int, Laptop> Laptops;

        private static Dictionary<string, HashSet<int>> Cache = new Dictionary<string, HashSet<int>>();

        private void SetCache(IReadOnlyCollection<Laptop> laptops)
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

        public IReadOnlyDictionary<int, Laptop> GetLaptops()
        {
            if (Laptops == null)
            {
                lock (obj)
                {
                    if (Laptops == null)
                    {
                        var laptops = DataParser.ParseLaptopJson("goods_data.json");
                        /// load from file
                        Laptops = laptops.ToDictionary(x => x.Id, x => x);
                        SetCache(laptops);
                    }
                }
            }
            
            return Laptops;
        }

        public IReadOnlyCollection<int> GetIdsByEqualString(string equal)
        {
            return Cache.TryGetValue(equal, out var ids) ? ids : new HashSet<int>();
        }
    }
}
