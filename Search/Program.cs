using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Search.Models;

namespace Search
{
    class Program
    {
        private static void PrintResult(IReadOnlyCollection<Laptop> laptops)
        {
            foreach (var laptop in laptops)
            {
                Console.WriteLine($"{laptop.Id} {laptop.Producer} {laptop.Serial} {laptop.ModelCode} {laptop.ModelNumber} {laptop.Color}");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var laptops = DataParser.ParseLaptopJson("goods_data.json");
            DataStorage.SetLaptops(laptops);

            var searcher = new Searcher();
            while (true)
            {
                var entered = Console.ReadLine();
                if (entered == "exit")
                {
                    break;
                }
                else
                {
                    var result = searcher.Search(entered);
                    if (!string.IsNullOrWhiteSpace(result.Error))
                    {
                        PrintResult(result.Laptops);
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
