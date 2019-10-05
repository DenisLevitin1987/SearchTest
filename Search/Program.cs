﻿using System;
using System.Collections.Generic;
using Search.Models;

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
                    if (string.IsNullOrWhiteSpace(result.Error))
                    {
                        if (result.Laptops.Count > 0)
                        {
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