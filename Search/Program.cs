using System;
using System.Collections.Generic;
using Search.Models;
using Search.SearchExpressionParser;

namespace Search
{
    class Program
    {
        private static void PrintResult(IReadOnlyCollection<Laptop> laptops)
        {
            foreach (var laptop in laptops)
            {
                Console.WriteLine($"{laptop.Id} {laptop.Name}");
            }
        }

        static void Main(string[] args)
        {
            var laptops = DataParser.ParseLaptopJson("../../../goods_data.json");
            var dataStorage = new DataStorage();

            dataStorage.SetLaptops(laptops);
            var searchParser = new SearchQueryParser();

            var searcher = new Searcher(searchParser, dataStorage);
            while (true)
            {
                Console.WriteLine("type your query");
                var entered = Console.ReadLine();
                if (entered == "exit")
                {
                    break;
                }
                else
                {
                    var result = searcher.Search(entered);
                    if (string.IsNullOrWhiteSpace(result.Error))
                    {
                        if (result.Laptops.Count > 0)
                        {
                            Console.WriteLine("your result: ");
                            PrintResult(result.Laptops);
                        }
                        else
                        {
                            Console.WriteLine("nothing found");
                        }
                    }
                    else
                    {
                        Console.WriteLine(result.Error);
                    }
                }
            }
        }
    }
}
