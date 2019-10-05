using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Search.Models;

namespace Search
{
    public static class DataParser
    {
        private class JsonItem
        {
            public int Id { get; set; }

            public string Description { get; set; }
        }

        public static IReadOnlyCollection<Laptop> ParseLaptopJson(string filePath)
        {
            var res = new List<Laptop>();
            var text = File.ReadAllText(filePath);

            var jsonItems = JsonConvert.DeserializeObject<JsonItem[]>(text);
            foreach (var jsonItem in jsonItems)
            {
                var laptop = new Laptop()
                {
                    Id = jsonItem.Id
                };

                var descriptioArray = jsonItem.Description.Split(new string[] {" ", ", ", ",", "\"," }, StringSplitOptions.RemoveEmptyEntries);
                if (descriptioArray.Length > 0)
                {
                    laptop.Producer = descriptioArray[0].ToLower();
                    laptop.Serial = descriptioArray[1].ToLower();
                    if (descriptioArray.Length == 3)
                    {
                        laptop.ModelCode = descriptioArray[2].ToLower();
                    }
                    /// TODO: Проблемы с парсингом цвета
                    
                    res.Add(laptop);
                }
            }

            return res;
        }
    }
}
