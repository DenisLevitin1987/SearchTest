using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SearchEngine.Models;

namespace SearchEngine
{
    public static class DataParser
    {
        public static IReadOnlyCollection<Laptop> ParseLaptopJson(string filePath)
        {
            var text = File.ReadAllText(filePath);

            var laptops = JsonConvert.DeserializeObject<Laptop[]>(text);
            foreach (var laptop in laptops)
            {
                laptop.Name = laptop.Name.Replace(",", "").Replace(@"\", "").Replace("\"", "");
                var descriptioArray = laptop.Name?.Split(new string[] { " ", ", ", ",", "\"," }, StringSplitOptions.RemoveEmptyEntries);
                if (descriptioArray?.Length > 0)
                {
                    laptop.Attributes = descriptioArray.Select(x => x.ToLower()).ToArray();
                }
            }

            return laptops;
        }
    }
}
